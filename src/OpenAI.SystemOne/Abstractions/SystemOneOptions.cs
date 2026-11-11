namespace OpenAI.SystemOne;

/// <summary>Represents parameters for a System One decision request.</summary>
public class SystemOneOptions
{
    /// <summary>Gets or sets the duration for which the model should remain loaded.</summary>
    public SystemOneKeepAlive? KeepAlive { get; set; }

    /// <summary>Gets or sets the images to include in the generation request.</summary>
    public IList<string>? Images { get; set; }

    /// <summary>Gets or sets additional provider-specific properties.</summary>
    public IDictionary<string, object?>? AdditionalProperties { get; set; }
}
