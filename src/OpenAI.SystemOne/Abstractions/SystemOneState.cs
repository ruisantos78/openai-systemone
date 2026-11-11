using System.Text.Json;
using System.Text.Json.Nodes;

namespace OpenAI.SystemOne;

/// <summary>
/// Represents the shared state that questions are asked about which can be text, an object, or an array.
/// </summary>
public union SystemOneState(string, JsonObject, JsonArray)
{
    /// <summary>Creates a <see cref="SystemOneState"/> from a text string.</summary>
    public static implicit operator SystemOneState(string text) => new(text);

    /// <summary>Creates a <see cref="SystemOneState"/> from a <see cref="JsonObject"/>.</summary>
    public static implicit operator SystemOneState(JsonObject obj) => new(obj);

    /// <summary>Creates a <see cref="SystemOneState"/> from a <see cref="JsonArray"/>.</summary>
    public static implicit operator SystemOneState(JsonArray arr) => new(arr);

    /// <summary>Creates a <see cref="SystemOneState"/> from a <see cref="JsonNode"/>.</summary>
    public static implicit operator SystemOneState(JsonNode node) => node switch
    {
        JsonObject obj => obj,
        JsonArray arr => arr,
        _ => node.ToJsonString(),
    };

    /// <summary>Creates a <see cref="SystemOneState"/> from a string array.</summary>
    public static implicit operator SystemOneState(string[] items)
    {
        var array = new JsonArray();
        foreach (var item in items)
        {
            array.Add((JsonNode?)item);
        }
        return array;
    }

    /// <summary>Implicitly converts a <see cref="SystemOneContent"/> to a <see cref="SystemOneState"/>.</summary>
    public static implicit operator SystemOneState(SystemOneContent content) => content.Value switch
    {
        string s => s,
        JsonObject o => o,
        JsonArray a => a,
        _ => throw new InvalidOperationException("Unsupported SystemOneContent case.")
    };

    /// <summary>Creates a <see cref="SystemOneState"/> from any serializable object.</summary>
    public static SystemOneState FromObject(object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        switch (value)
        {
            case string s:
                return s;
            case JsonObject jo:
                return jo;
            case JsonArray ja:
                return ja;
            case JsonNode jn:
                return (SystemOneState)jn;
            
            default:
            {
                var node = JsonSerializer.SerializeToNode(value)
                           ?? throw new ArgumentException("The value could not be serialized to a JSON node.", nameof(value));
                return (SystemOneState)node;
            }
        }
    }
}