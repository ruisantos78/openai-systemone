window.OS1_TYPES = window.OS1_TYPES || {};
window.OS1_TYPES["SystemOneScoreAnswer"] = {
  "name": "SystemOneScoreAnswer",
  "namespace": "OpenAI.SystemOne",
  "kind": "Class",
  "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
  "declaration": "public class SystemOneScoreAnswer",
  "overline": "Namespace: OpenAI.SystemOne \u00B7 Assembly: OpenAI.SystemOne",
  "summary": "An answer that scores the state on a scale defined by a score question.",
  "sections": [
    {
      "heading": "Remarks",
      "html": "\u003Cp\u003EAn answer that scores the state on a scale defined by a score question.\u003C/p\u003E"
    }
  ],
  "properties": [
    {
      "name": "Type",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "string Type { get; }",
      "desc": "Gets the type discriminator of this answer.",
      "return": "string",
      "href": "type.html"
    },
    {
      "name": "Score",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "double Score { get; set; }",
      "desc": "Gets or sets the score assigned by the model.",
      "return": "double",
      "href": "score.html"
    },
    {
      "name": "Legend",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "Dictionary\u003Cstring, string\u003E Legend { get; set; }",
      "desc": "Gets or sets the descriptions associated with score values.",
      "return": "Dictionary\u003Cstring, string\u003E",
      "href": "legend.html"
    },
    {
      "name": "Probabilities",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "Dictionary\u003Cstring, double\u003E Probabilities { get; set; }",
      "desc": "Gets or sets the probability assigned to each possible score.",
      "return": "Dictionary\u003Cstring, double\u003E",
      "href": "probabilities.html"
    },
    {
      "name": "Confidence",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "double Confidence { get; set; }",
      "desc": "Gets or sets the model\u0027s confidence in the score.",
      "return": "double",
      "href": "confidence.html"
    },
    {
      "name": "ScoreTypeName",
      "kind": "Constant",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public const string ScoreTypeName = \u0022score\u0022",
      "desc": "The JSON type discriminator for score answers.",
      "return": "string",
      "href": "scoretypename.html"
    }
  ],
  "members": [
    {
      "name": "SystemOneScoreAnswer",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneScoreAnswer()",
      "desc": "Initializes a new instance of the SystemOneScoreAnswer class.",
      "return": "SystemOneScoreAnswer",
      "href": "systemonescoreanswer.html",
      "overline": "Returns: SystemOneScoreAnswer",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneScoreAnswer class.\u003C/p\u003E"
        }
      ]
    }
  ]
};
