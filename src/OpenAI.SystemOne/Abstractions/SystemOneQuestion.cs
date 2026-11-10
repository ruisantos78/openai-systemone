using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace OpenAI.SystemOne;

/// <summary>
/// A single named question asked in a System One request. Use <see cref="SystemOneChoiceQuestion"/>,
/// <see cref="SystemOneNoulQuestion"/>, or <see cref="SystemOneScoreQuestion"/> to create one.
/// </summary>
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(SystemOneChoiceQuestion), SystemOneChoiceQuestion.ChoiceTypeName)]
[JsonDerivedType(typeof(SystemOneNoulQuestion), SystemOneNoulQuestion.NoulTypeName)]
[JsonDerivedType(typeof(SystemOneScoreQuestion), SystemOneScoreQuestion.ScoreTypeName)]
public abstract class SystemOneQuestion
{
    /// <summary>Gets the type discriminator of this question.</summary>
    [JsonIgnore]
    public abstract string Type { get; }

    /// <summary>
    /// Gets or sets the instructions shown to the model for this question.
    /// </summary>
    [JsonPropertyName("instructions")]
    public required JsonNode Instructions { get; set; }
}

/// <summary>A question that picks one option out of a set of named criteria.</summary>
public sealed class SystemOneChoiceQuestion : SystemOneQuestion
{
    public const string ChoiceTypeName = "choice";
    public const int MinCriteriaCount = 2;
    public const int MaxCriteriaCount = 255;

    [JsonIgnore]
    public override string Type => ChoiceTypeName;

    [JsonPropertyName("criteria")]
    public required IDictionary<string, string?> Criteria { get; set; }
}

/// <summary>A yes/no (noul) question.</summary>
public sealed class SystemOneNoulQuestion : SystemOneQuestion
{
    public const string NoulTypeName = "noul";

    [JsonIgnore]
    public override string Type => NoulTypeName;

    [JsonPropertyName("criteria")]
    public SystemOneNoulCriteria? Criteria { get; set; }
}

/// <summary>Descriptions for the <c>false</c> and <c>true</c> outcomes of a <see cref="SystemOneNoulQuestion"/>.</summary>
public sealed class SystemOneNoulCriteria
{
    [JsonPropertyName("false")]
    public string? No { get; set; }

    [JsonPropertyName("true")]
    public string? Yes { get; set; }
}

/// <summary>A question that scores the state on a scale defined by an ordered list of criteria.</summary>
public sealed class SystemOneScoreQuestion : SystemOneQuestion
{
    public const string ScoreTypeName = "score";
    public const int MinCriteriaCount = 2;
    public const int MaxCriteriaCount = 10;

    [JsonIgnore]
    public override string Type => ScoreTypeName;

    [JsonPropertyName("criteria")]
    public required IReadOnlyList<string> Criteria { get; set; }
}
