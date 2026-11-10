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
    [JsonIgnore]
    public abstract string Type { get; }
}

public sealed class SystemOneChoiceAnswer : SystemOneAnswer
{
    public const string ChoiceTypeName = "choice";

    [JsonIgnore]
    public override string Type => ChoiceTypeName;

    [JsonPropertyName("choice")]
    public string Choice { get; set; } = string.Empty;

    [JsonPropertyName("probabilities")]
    public Dictionary<string, double> Probabilities { get; set; } = new();

    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }
}

public sealed class SystemOneNoulAnswer : SystemOneAnswer
{
    public const string NoulTypeName = "noul";

    [JsonIgnore]
    public override string Type => NoulTypeName;

    [JsonPropertyName("noul")]
    public double Noul { get; set; }
}

public sealed class SystemOneScoreAnswer : SystemOneAnswer
{
    public const string ScoreTypeName = "score";

    [JsonIgnore]
    public override string Type => ScoreTypeName;

    [JsonPropertyName("score")]
    public double Score { get; set; }

    [JsonPropertyName("legend")]
    public Dictionary<string, string> Legend { get; set; } = new();

    [JsonPropertyName("probabilities")]
    public Dictionary<string, double> Probabilities { get; set; } = new();

    [JsonPropertyName("confidence")]
    public double Confidence { get; set; }
}
