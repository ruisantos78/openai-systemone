using System.Text.Json.Serialization;

namespace OpenAI.SystemOne;

/// <summary>The response from a System One generation operation.</summary>
public sealed class SystemOneResponse
{
    /// <summary>Gets or sets the model that generated the response.</summary>
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    /// <summary>Gets or sets the answers keyed by question name.</summary>
    [JsonPropertyName("answers")]
    public Dictionary<string, SystemOneAnswer> Answers { get; set; } = new();

    /// <summary>Gets or sets token usage information for the generation operation.</summary>
    [JsonPropertyName("usage")]
    public SystemOneUsage Usage { get; set; } = new();
}

/// <summary>Describes token usage for a System One generation operation.</summary>
public sealed class SystemOneUsage
{
    /// <summary>Gets or sets the number of tokens in the input.</summary>
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; set; }

    /// <summary>Gets or sets the number of tokens in the output.</summary>
    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; set; }
}
