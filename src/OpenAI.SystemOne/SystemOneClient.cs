using System.ClientModel;
using System.ClientModel.Primitives;
using System.Text.Json;
using System.Text.Json.Nodes;
// ReSharper disable All

namespace OpenAI.SystemOne;

/// <summary>Provides access to a System One endpoint.</summary>
public sealed class SystemOneClient : ISystemOneClient
{
    private const string SystemOnePath = "v1/systemone";

    private readonly ClientPipeline _pipeline;
    private readonly Uri _endpoint;
    private readonly Uri _requestUri;
    private readonly bool _ownsPipeline;

    /// <summary>Gets the model name used by this client.</summary>
    public string Model { get; }

    /// <summary>Initializes a client that sends requests through a new pipeline.</summary>
    /// <param name="model">The model name to use.</param>
    /// <param name="endpoint">The System One service endpoint.</param>
    /// <param name="apiKey">The optional bearer token used to authenticate requests.</param>
    /// <param name="httpClient">The optional HTTP client used for transport.</param>
    public SystemOneClient(string model, Uri endpoint, string? apiKey = null, HttpClient? httpClient = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model);
        ArgumentNullException.ThrowIfNull(endpoint);

        Model = model;
        _endpoint = endpoint;
        _requestUri = ResolveRequestUri(endpoint);

        ClientPipelineOptions pipelineOptions = new()
        {
            Transport = new HttpClientPipelineTransport(httpClient ?? new HttpClient()),
        };

        List<PipelinePolicy> perTryPolicies = [];
        if (!string.IsNullOrWhiteSpace(apiKey))
        {
            perTryPolicies.Add(ApiKeyAuthenticationPolicy.CreateBearerAuthorizationPolicy(new ApiKeyCredential(apiKey)));
        }

        _pipeline = ClientPipeline.Create(pipelineOptions, [], perTryPolicies.ToArray(), []);
        _ownsPipeline = true;
    }

    /// <summary>Initializes a client using an existing pipeline.</summary>
    /// <param name="model">The model name to use.</param>
    /// <param name="pipeline">The pipeline used to send requests.</param>
    /// <param name="endpoint">The System One service endpoint.</param>
    public SystemOneClient(string model, ClientPipeline pipeline, Uri endpoint)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model);
        ArgumentNullException.ThrowIfNull(pipeline);
        ArgumentNullException.ThrowIfNull(endpoint);

        Model = model;
        _pipeline = pipeline;
        _endpoint = endpoint;
        _requestUri = ResolveRequestUri(endpoint);
        _ownsPipeline = false;
    }

    /// <summary>Answers the given questions about the given state.</summary>
    /// <param name="state">The shared state that all questions are asked about.</param>
    /// <param name="questions">The named questions about the shared state.</param>
    /// <param name="options">Optional parameters for generation.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task containing the System One response.</returns>
    public async Task<SystemOneResponse> DecideAsync(
        SystemOneState state,
        IDictionary<string, SystemOneQuestion> questions,
        SystemOneOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(questions);

        WireSystemOneRequest request = new()
        {
            Model = Model,
            State = state,
            Questions = questions,
            KeepAlive = options?.KeepAlive,
            Images = options?.Images,
        };

        byte[] utf8Bytes;
        if (options?.AdditionalProperties is { Count: > 0 } additionalProperties)
        {
            var json = (
                JsonSerializer.SerializeToNode(request, SystemOneJsonContext.Default.WireSystemOneRequest) 
                    ?? throw new InvalidOperationException("The System One request could not be serialized." )
            ).AsObject();

            foreach (var (key, value) in additionalProperties)
            {
                json[key] = value as JsonNode ?? JsonSerializer.SerializeToNode(value);
            }

            utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(json);
        }
        else
        {
            utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(request, SystemOneJsonContext.Default.WireSystemOneRequest);
        }

        using var content = BinaryContent.Create(BinaryData.FromBytes(utf8Bytes));

        using var message = _pipeline.CreateMessage(_requestUri, "POST", SuccessClassifier.Shared);
        message.Request.Content = content;
        message.Request.Headers.Set("Accept", "application/json");
        message.Request.Headers.Set("Content-Type", "application/json");

        message.Apply(new RequestOptions { CancellationToken = cancellationToken });

        try
        {
            await _pipeline.SendAsync(message).ConfigureAwait(false);
        }
        catch (ClientResultException ex)
        {
            throw new SystemOneException(ex.Status, ex.Message, ex);
        }

        var response = message.Response ?? throw new SystemOneException(0, "The pipeline did not produce a response.");

        if (response.IsError || response.Status >= 400)
        {
            var errorText = response.ReasonPhrase ?? string.Empty;
            try
            {
                var body = await response.BufferContentAsync(cancellationToken).ConfigureAwait(false);
                errorText = body.ToString();
            }
            catch
            {
                // ignored
            }

            throw new SystemOneException(response.Status, $"The System One endpoint returned status {response.Status}: {errorText}");
        }

        var stream = response.ContentStream;
        if (stream is null)
        {
            throw new SystemOneException(response.Status, "The System One endpoint returned no content.");
        }

        try
        {
            var result = await JsonSerializer
                .DeserializeAsync(stream, SystemOneJsonContext.Default.SystemOneResponse, cancellationToken)
                .ConfigureAwait(false);

            return result ?? throw new SystemOneException(response.Status, "The System One endpoint returned a response that could not be parsed.");
        }
        catch (JsonException ex)
        {
            throw new SystemOneException(response.Status, "The System One endpoint returned a response that could not be parsed.", ex);
        }
    }

    /// <summary>Releases resources owned by this client.</summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (disposing && _ownsPipeline)
        {
            if ((_pipeline as object) is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

    private static Uri ResolveRequestUri(Uri endpoint)
    {
        var path = endpoint.AbsolutePath.TrimEnd('/');
        if (path.EndsWith("/" + SystemOnePath, StringComparison.OrdinalIgnoreCase))
        {
            return endpoint;
        }

        var firstSegment = SystemOnePath.Split('/')[0];
        if (path.EndsWith("/" + firstSegment, StringComparison.OrdinalIgnoreCase))
        {
            path = path[..^(firstSegment.Length + 1)];
        }

        var fullPath = string.IsNullOrEmpty(path) ? $"/{SystemOnePath}" : $"{path}/{SystemOnePath}";
        return new UriBuilder(endpoint) { Path = fullPath }.Uri;
    }

    private sealed class SuccessClassifier : PipelineMessageClassifier
    {
        internal static readonly SuccessClassifier Shared = new();

        public override bool TryClassify(PipelineMessage message, out bool isError)
        {
            isError = false;
            return true;
        }

        public override bool TryClassify(PipelineMessage message, Exception? exception, out bool isRetriable)
        {
            isRetriable = false;
            return true;
        }
    }
}
