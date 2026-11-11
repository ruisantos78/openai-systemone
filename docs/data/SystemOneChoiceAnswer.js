window.OS1_TYPES = window.OS1_TYPES || {};
window.OS1_TYPES["SystemOneChoiceAnswer"] = {
  "name": "SystemOneChoiceAnswer",
  "namespace": "OpenAI.SystemOne",
  "kind": "Class",
  "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
  "declaration": "public class SystemOneChoiceAnswer",
  "overline": "Namespace: OpenAI.SystemOne \u00B7 Assembly: OpenAI.SystemOne",
  "summary": "An answer that selects one of the named criteria from a choice question.",
  "sections": [
    {
      "heading": "Remarks",
      "html": "\u003Cp\u003EAn answer that selects one of the named criteria from a choice question.\u003C/p\u003E"
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
      "name": "Choice",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "string Choice { get; set; }",
      "desc": "Gets or sets the selected choice.",
      "return": "string",
      "href": "choice.html"
    },
    {
      "name": "Probabilities",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "Dictionary\u003Cstring, double\u003E Probabilities { get; set; }",
      "desc": "Gets or sets the probability assigned to each choice.",
      "return": "Dictionary\u003Cstring, double\u003E",
      "href": "probabilities.html"
    },
    {
      "name": "Confidence",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "double Confidence { get; set; }",
      "desc": "Gets or sets the model\u0027s confidence in the selected choice.",
      "return": "double",
      "href": "confidence.html"
    },
    {
      "name": "ChoiceTypeName",
      "kind": "Constant",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public const string ChoiceTypeName = \u0022choice\u0022",
      "desc": "The JSON type discriminator for choice answers.",
      "return": "string",
      "href": "choicetypename.html"
    }
  ],
  "members": [
    {
      "name": "SystemOneChoiceAnswer",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneChoiceAnswer()",
      "desc": "Initializes a new instance of the SystemOneChoiceAnswer class.",
      "return": "SystemOneChoiceAnswer",
      "href": "systemonechoiceanswer.html",
      "overline": "Returns: SystemOneChoiceAnswer",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneChoiceAnswer class.\u003C/p\u003E"
        }
      ]
    }
  ]
};
