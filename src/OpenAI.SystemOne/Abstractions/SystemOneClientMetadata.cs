namespace OpenAI.SystemOne;

/// <summary>Describes an <see cref="ISystemOneClient"/>, for use in logging and diagnostics.</summary>
public sealed class SystemOneClientMetadata
{
    /// <summary>Gets the name of the System One model used by the client, such as <c>nimble</c> or <c>clef:cloud</c>.</summary>
    public required string ModelName { get; init; }

    /// <summary>Gets the name of the service provider, such as <c>ollama</c>.</summary>
    public required string ProviderName { get; init; }

    /// <summary>Gets the service endpoint the client sends requests to, if known.</summary>
    public Uri? Endpoint { get; init; }

    /// <summary>Gets any additional information about the client, if available.</summary>
    public IDictionary<string, object?>? AdditionalProperties { get; init; }
}
