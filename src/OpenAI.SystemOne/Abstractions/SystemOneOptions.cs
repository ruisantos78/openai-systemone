using System.Text.Json.Nodes;

namespace OpenAI.SystemOne;

/// <summary>Provides options for a <see cref="ISystemOneClient"/> generation operation.</summary>
public class SystemOneOptions
{
    /// <summary>Gets or sets the duration for which the model should remain loaded.</summary>
    public JsonNode? KeepAlive { get; set; }

    /// <summary>Gets or sets the images to include in the generation request.</summary>
    public IList<string>? Images { get; set; }

    /// <summary>Gets or sets additional provider-specific properties.</summary>
    public IDictionary<string, object?>? AdditionalProperties { get; set; }
}
