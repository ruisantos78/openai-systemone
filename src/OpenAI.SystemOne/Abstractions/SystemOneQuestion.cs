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
    public required SystemOneContent Instructions { get; set; }
}

/// <summary>A question that picks one option out of a set of named criteria.</summary>
public sealed class SystemOneChoiceQuestion : SystemOneQuestion
{
    /// <summary>The JSON type discriminator for choice questions.</summary>
    public const string ChoiceTypeName = "choice";

    /// <summary>The minimum number of criteria supported by a choice question.</summary>
    public const int MinCriteriaCount = 2;

    /// <summary>The maximum number of criteria supported by a choice question.</summary>
    public const int MaxCriteriaCount = 255;

    /// <inheritdoc />
    [JsonIgnore]
    public override string Type => ChoiceTypeName;

    /// <summary>Gets or sets the named criteria from which the model selects.</summary>
    [JsonPropertyName("criteria")]
    public required IDictionary<string, string?> Criteria { get; set; }
}

/// <summary>A yes/no (noul) question.</summary>
public sealed class SystemOneNoulQuestion : SystemOneQuestion
{
    /// <summary>The JSON type discriminator for noul questions.</summary>
    public const string NoulTypeName = "noul";

    /// <inheritdoc />
    [JsonIgnore]
    public override string Type => NoulTypeName;

    /// <summary>Gets or sets the optional descriptions for the no and yes outcomes.</summary>
    [JsonPropertyName("criteria")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public SystemOneNoulCriteria? Criteria { get; set; }
}

/// <summary>Descriptions for the <c>false</c> and <c>true</c> outcomes of a <see cref="SystemOneNoulQuestion"/>.</summary>
public sealed class SystemOneNoulCriteria
{
    /// <summary>Gets or sets the description of the no outcome.</summary>
    [JsonPropertyName("false")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? No { get; set; }

    /// <summary>Gets or sets the description of the yes outcome.</summary>
    [JsonPropertyName("true")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Yes { get; set; }
}

/// <summary>A question that scores the state on a scale defined by an ordered list of criteria.</summary>
public sealed class SystemOneScoreQuestion : SystemOneQuestion
{
    /// <summary>The JSON type discriminator for score questions.</summary>
    public const string ScoreTypeName = "score";

    /// <summary>The minimum number of criteria supported by a score question.</summary>
    public const int MinCriteriaCount = 2;

    /// <summary>The maximum number of criteria supported by a score question.</summary>
    public const int MaxCriteriaCount = 10;

    /// <inheritdoc />
    [JsonIgnore]
    public override string Type => ScoreTypeName;

    /// <summary>Gets or sets the ordered criteria defining the scoring scale.</summary>
    [JsonPropertyName("criteria")]
    public required IReadOnlyList<string> Criteria { get; set; }
}
