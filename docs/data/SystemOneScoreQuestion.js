window.OS1_TYPES = window.OS1_TYPES || {};
window.OS1_TYPES["SystemOneScoreQuestion"] = {
  "name": "SystemOneScoreQuestion",
  "namespace": "OpenAI.SystemOne",
  "kind": "Class",
  "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
  "declaration": "public class SystemOneScoreQuestion",
  "overline": "Namespace: OpenAI.SystemOne \u00B7 Assembly: OpenAI.SystemOne",
  "summary": "A question that scores the state on a scale defined by an ordered list of criteria.",
  "sections": [
    {
      "heading": "Remarks",
      "html": "\u003Cp\u003EA question that scores the state on a scale defined by an ordered list of criteria.\u003C/p\u003E"
    }
  ],
  "properties": [
    {
      "name": "Type",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "string Type { get; }",
      "desc": "Gets the type discriminator of this question.",
      "return": "string",
      "href": "type.html"
    },
    {
      "name": "Criteria",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "IReadOnlyList\u003Cstring\u003E Criteria { get; set; }",
      "desc": "Gets or sets the ordered criteria defining the scoring scale.",
      "return": "IReadOnlyList\u003Cstring\u003E",
      "href": "criteria.html"
    },
    {
      "name": "ScoreTypeName",
      "kind": "Constant",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public const string ScoreTypeName = \u0022score\u0022",
      "desc": "The JSON type discriminator for score questions.",
      "return": "string",
      "href": "scoretypename.html"
    },
    {
      "name": "MinCriteriaCount",
      "kind": "Constant",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public const int MinCriteriaCount = \u00222\u0022",
      "desc": "The minimum number of criteria supported by a score question.",
      "return": "int",
      "href": "mincriteriacount.html"
    },
    {
      "name": "MaxCriteriaCount",
      "kind": "Constant",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public const int MaxCriteriaCount = \u002210\u0022",
      "desc": "The maximum number of criteria supported by a score question.",
      "return": "int",
      "href": "maxcriteriacount.html"
    }
  ],
  "members": [
    {
      "name": "SystemOneScoreQuestion",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneScoreQuestion()",
      "desc": "Initializes a new instance of the SystemOneScoreQuestion class.",
      "return": "SystemOneScoreQuestion",
      "href": "systemonescorequestion.html",
      "overline": "Returns: SystemOneScoreQuestion",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneScoreQuestion class.\u003C/p\u003E"
        }
      ]
    }
  ]
};
