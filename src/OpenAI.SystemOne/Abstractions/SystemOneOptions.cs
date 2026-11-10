using System.Text.Json.Nodes;

namespace OpenAI.SystemOne;

/// <summary>Provides options for a <see cref="ISystemOneClient"/> generation operation.</summary>
public class SystemOneOptions
{
    public JsonNode? KeepAlive { get; set; }
    public IList<string>? Images { get; set; }
    public IDictionary<string, object?>? AdditionalProperties { get; set; }
}
