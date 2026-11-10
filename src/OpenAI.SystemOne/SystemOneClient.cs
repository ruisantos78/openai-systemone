using System.ClientModel;
using System.ClientModel.Primitives;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace OpenAI.SystemOne;

public class SystemOneClient : ISystemOneClient
{
    private const string SystemOnePath = "v1/systemone";

    private readonly ClientPipeline _pipeline;
    private readonly Uri _endpoint;
    private readonly Uri _requestUri;
    private readonly string _model;
    private readonly bool _ownsPipeline;

    public string Model => _model;

    public SystemOneClient(string model, Uri endpoint, string? apiKey = null, HttpClient? httpClient = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model);
        ArgumentNullException.ThrowIfNull(endpoint);

        _model = model;
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

    public SystemOneClient(string model, ClientPipeline pipeline, Uri endpoint)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(model);
        ArgumentNullException.ThrowIfNull(pipeline);
        ArgumentNullException.ThrowIfNull(endpoint);

        _model = model;
        _pipeline = pipeline;
        _endpoint = endpoint;
        _requestUri = ResolveRequestUri(endpoint);
        _ownsPipeline = false;
    }

    public virtual async Task<SystemOneResponse> GenerateAsync(
        object state,
        IDictionary<string, SystemOneQuestion> questions,
        SystemOneOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(state);
        ArgumentNullException.ThrowIfNull(questions);

        WireSystemOneRequest request = new()
        {
            Model = _model,
            State = state as JsonNode ?? JsonSerializer.SerializeToNode(state)!,
            Questions = questions,
            KeepAlive = options?.KeepAlive,
            Images = options?.Images,
        };

        byte[] utf8Bytes;
        if (options?.AdditionalProperties is { Count: > 0 } additionalProperties)
        {
            JsonObject json = (JsonSerializer.SerializeToNode(request, SystemOneJsonContext.Default.WireSystemOneRequest) ??
                throw new InvalidOperationException("The System One request could not be serialized.")).AsObject();

            foreach ((string key, object? value) in additionalProperties)
            {
                json[key] = value as JsonNode ?? JsonSerializer.SerializeToNode(value);
            }

            utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(json);
        }
        else
        {
            utf8Bytes = JsonSerializer.SerializeToUtf8Bytes(request, SystemOneJsonContext.Default.WireSystemOneRequest);
        }

        using BinaryContent content = BinaryContent.Create(BinaryData.FromBytes(utf8Bytes));

        using PipelineMessage message = _pipeline.CreateMessage(_requestUri, "POST", SuccessClassifier.Shared);
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

        PipelineResponse response = message.Response
            ?? throw new SystemOneException(0, "The pipeline did not produce a response.");

        if (response.IsError || response.Status >= 400)
        {
            string errorText = response.ReasonPhrase ?? string.Empty;
            try
            {
                BinaryData body = await response.BufferContentAsync(cancellationToken).ConfigureAwait(false);
                errorText = body.ToString();
            }
            catch { }

            throw new SystemOneException(response.Status, $"The System One endpoint returned status {response.Status}: {errorText}");
        }

        Stream? stream = response.ContentStream;
        if (stream is null)
        {
            throw new SystemOneException(response.Status, "The System One endpoint returned no content.");
        }

        SystemOneResponse? result = await JsonSerializer
            .DeserializeAsync(stream, SystemOneJsonContext.Default.SystemOneResponse, cancellationToken)
            .ConfigureAwait(false);

        if (result is null)
        {
            throw new SystemOneException(response.Status, "The System One endpoint returned a response that could not be parsed.");
        }

        return result;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
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
        string path = endpoint.AbsolutePath.TrimEnd('/');
        if (path.EndsWith("/" + SystemOnePath, StringComparison.OrdinalIgnoreCase))
        {
            return endpoint;
        }

        string firstSegment = SystemOnePath.Split('/')[0];
        if (path.EndsWith("/" + firstSegment, StringComparison.OrdinalIgnoreCase))
        {
            path = path[..^(firstSegment.Length + 1)];
        }

        string fullPath = string.IsNullOrEmpty(path) ? "/" + SystemOnePath : path + "/" + SystemOnePath;
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
