using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace OpenAI.SystemOne;

internal sealed class WireSystemOneRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public JsonNode State { get; set; } = null!;

    [JsonPropertyName("questions")]
    public IDictionary<string, SystemOneQuestion> Questions { get; set; } = new Dictionary<string, SystemOneQuestion>();

    [JsonPropertyName("keep_alive")]
    public JsonNode? KeepAlive { get; set; }

    [JsonPropertyName("images")]
    public IList<string>? Images { get; set; }
}
