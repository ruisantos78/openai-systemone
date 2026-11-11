using System.Text.Json.Serialization;

namespace OpenAI.SystemOne;

internal sealed class WireSystemOneRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public SystemOneState State { get; set; }

    [JsonPropertyName("questions")]
    public IDictionary<string, SystemOneQuestion> Questions { get; set; } = new Dictionary<string, SystemOneQuestion>();

    [JsonPropertyName("keep_alive")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SystemOneKeepAlive? KeepAlive { get; set; }

    [JsonPropertyName("images")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<string>? Images { get; set; }
}
