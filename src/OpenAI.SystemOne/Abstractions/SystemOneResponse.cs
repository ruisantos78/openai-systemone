using System.Text.Json.Serialization;

namespace OpenAI.SystemOne;

/// <summary>The response from a System One generation operation.</summary>
public sealed class SystemOneResponse
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("answers")]
    public Dictionary<string, SystemOneAnswer> Answers { get; set; } = new();

    [JsonPropertyName("usage")]
    public SystemOneUsage Usage { get; set; } = new();
}

public sealed class SystemOneUsage
{
    [JsonPropertyName("input_tokens")]
    public int InputTokens { get; set; }

    [JsonPropertyName("output_tokens")]
    public int OutputTokens { get; set; }
}
