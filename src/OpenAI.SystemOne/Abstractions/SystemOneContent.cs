using System.Text.Json;
using System.Text.Json.Nodes;

namespace OpenAI.SystemOne;

/// <summary>
/// Content representing a nonempty string, or an object or array serialized as JSON text.
/// Used for question instructions and shared state.
/// </summary>
public union SystemOneContent(string, JsonObject, JsonArray)
{
    /// <summary>Creates a <see cref="SystemOneContent"/> from a text string.</summary>
    public static implicit operator SystemOneContent(string text) => new(text);

    /// <summary>Creates a <see cref="SystemOneContent"/> from a <see cref="JsonObject"/>.</summary>
    public static implicit operator SystemOneContent(JsonObject obj) => new(obj);

    /// <summary>Creates a <see cref="SystemOneContent"/> from a <see cref="JsonArray"/>.</summary>
    public static implicit operator SystemOneContent(JsonArray arr) => new(arr);

    /// <summary>Creates a <see cref="SystemOneContent"/> from a <see cref="JsonNode"/>.</summary>
    public static implicit operator SystemOneContent(JsonNode node) => node switch
    {
        JsonObject obj => obj,
        JsonArray arr => arr,
        _ => node.ToJsonString(),
    };

    /// <summary>Creates a <see cref="SystemOneContent"/> from a string array.</summary>
    public static implicit operator SystemOneContent(string[] items)
    {
        var array = new JsonArray();
        foreach (var item in items)
        {
            array.Add((JsonNode?)item);
        }
        return array;
    }

    /// <summary>Implicitly converts a <see cref="SystemOneState"/> to a <see cref="SystemOneContent"/>.</summary>
    public static implicit operator SystemOneContent(SystemOneState state) => state.Value switch
    {
        string s => s,
        JsonObject o => o,
        JsonArray a => a,
        _ => throw new InvalidOperationException("Unsupported SystemOneState case.")
    };

    /// <summary>Creates a <see cref="SystemOneContent"/> from any serializable object.</summary>
    public static SystemOneContent FromObject(object value)
    {
        ArgumentNullException.ThrowIfNull(value);
        if (value is string s) return s;
        if (value is JsonObject jo) return jo;
        if (value is JsonArray ja) return ja;
        if (value is JsonNode jn) return (SystemOneContent)jn;

        var node = JsonSerializer.SerializeToNode(value)
                   ?? throw new ArgumentException("The value could not be serialized to a JSON node.", nameof(value));
        return (SystemOneContent)node;
    }
}