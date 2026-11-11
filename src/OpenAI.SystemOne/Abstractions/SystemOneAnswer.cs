using System.Text.Json.Serialization;

namespace OpenAI.SystemOne;

/// <summary>
/// The answer to a single named question.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(SystemOneChoiceAnswer), SystemOneChoiceAnswer.ChoiceTypeName)]
[JsonDerivedType(typeof(SystemOneNoulAnswer), SystemOneNoulAnswer.NoulTypeName)]
[JsonDerivedType(typeof(SystemOneScoreAnswer), SystemOneScoreAnswer.ScoreTypeName)]
public abstract class SystemOneAnswer
{
    /// <summary>Gets the type discriminator of this answer.</summary>
    [JsonIgnore]
    public abstract string Type { get; }
}

/// <summary>An answer that selects one of the named criteria from a choice question.</summary>
public sealed class SystemOneChoiceAnswer : SystemOneAnswer
{
    /// <summary>The JSON type discriminator for choice answers.</summary>
    public const string ChoiceTypeName = "choice";

    /// <inheritdoc />
    [JsonIgnore]
    public override string Type => ChoiceTypeName;

    /// <summary>Gets or sets the selected choice.</summary>
    [JsonPropertyName("choice")]
    public string Choice { get; set; } = string.Empty;

    /// <summary>Gets or sets the probability assigned to each choice.</summary>
    [JsonPropertyName("probabilities")]
    public Dictionary<string, double> Probabilities { get; set; } = new();

    /// <summary>Gets or sets the model's confidence in the selected choice.</summary>
    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }
}

/// <summary>A yes/no (noul) answer.</summary>
public sealed class SystemOneNoulAnswer : SystemOneAnswer
{
    /// <summary>The JSON type discriminator for noul answers.</summary>
    public const string NoulTypeName = "noul";

    /// <inheritdoc />
    [JsonIgnore]
    public override string Type => NoulTypeName;

    /// <summary>Gets or sets the answer, represented as a numeric value.</summary>
    [JsonPropertyName("noul")]
    public double Noul { get; set; }
}

/// <summary>An answer that scores the state on a scale defined by a score question.</summary>
public sealed class SystemOneScoreAnswer : SystemOneAnswer
{
    /// <summary>The JSON type discriminator for score answers.</summary>
    public const string ScoreTypeName = "score";

    /// <inheritdoc />
    [JsonIgnore]
    public override string Type => ScoreTypeName;

    /// <summary>Gets or sets the score assigned by the model.</summary>
    [JsonPropertyName("score")]
    public double Score { get; set; }

    /// <summary>Gets or sets the descriptions associated with score values.</summary>
    [JsonPropertyName("legend")]
    public Dictionary<string, string> Legend { get; set; } = new();

    /// <summary>Gets or sets the probability assigned to each possible score.</summary>
    [JsonPropertyName("probabilities")]
    public Dictionary<string, double> Probabilities { get; set; } = new();

    /// <summary>Gets or sets the model's confidence in the score.</summary>
    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }
}
