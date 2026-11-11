window.OS1_TYPES = window.OS1_TYPES || {};
window.OS1_TYPES["SystemOneChoiceQuestion"] = {
  "name": "SystemOneChoiceQuestion",
  "namespace": "OpenAI.SystemOne",
  "kind": "Class",
  "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
  "declaration": "public class SystemOneChoiceQuestion",
  "overline": "Namespace: OpenAI.SystemOne \u00B7 Assembly: OpenAI.SystemOne",
  "summary": "A question that picks one option out of a set of named criteria.",
  "sections": [
    {
      "heading": "Remarks",
      "html": "\u003Cp\u003EA question that picks one option out of a set of named criteria.\u003C/p\u003E"
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
      "declaration": "IDictionary\u003Cstring, string\u003E Criteria { get; set; }",
      "desc": "Gets or sets the named criteria from which the model selects.",
      "return": "IDictionary\u003Cstring, string\u003E",
      "href": "criteria.html"
    },
    {
      "name": "ChoiceTypeName",
      "kind": "Constant",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public const string ChoiceTypeName = \u0022choice\u0022",
      "desc": "The JSON type discriminator for choice questions.",
      "return": "string",
      "href": "choicetypename.html"
    },
    {
      "name": "MinCriteriaCount",
      "kind": "Constant",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public const int MinCriteriaCount = \u00222\u0022",
      "desc": "The minimum number of criteria supported by a choice question.",
      "return": "int",
      "href": "mincriteriacount.html"
    },
    {
      "name": "MaxCriteriaCount",
      "kind": "Constant",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public const int MaxCriteriaCount = \u0022255\u0022",
      "desc": "The maximum number of criteria supported by a choice question.",
      "return": "int",
      "href": "maxcriteriacount.html"
    }
  ],
  "members": [
    {
      "name": "SystemOneChoiceQuestion",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneChoiceQuestion()",
      "desc": "Initializes a new instance of the SystemOneChoiceQuestion class.",
      "return": "SystemOneChoiceQuestion",
      "href": "systemonechoicequestion.html",
      "overline": "Returns: SystemOneChoiceQuestion",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneChoiceQuestion class.\u003C/p\u003E"
        }
      ]
    }
  ]
};
