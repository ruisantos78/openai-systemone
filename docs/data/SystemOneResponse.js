window.OS1_TYPES = window.OS1_TYPES || {};
window.OS1_TYPES["SystemOneResponse"] = {
  "name": "SystemOneResponse",
  "namespace": "OpenAI.SystemOne",
  "kind": "Class",
  "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
  "declaration": "public class SystemOneResponse",
  "overline": "Namespace: OpenAI.SystemOne \u00B7 Assembly: OpenAI.SystemOne",
  "summary": "The response from a System One generation operation.",
  "sections": [
    {
      "heading": "Remarks",
      "html": "\u003Cp\u003EThe response from a System One generation operation.\u003C/p\u003E"
    }
  ],
  "properties": [
    {
      "name": "Model",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "string Model { get; set; }",
      "desc": "Gets or sets the model that generated the response.",
      "return": "string",
      "href": "model.html"
    },
    {
      "name": "Answers",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "Dictionary\u003Cstring, SystemOneAnswer\u003E Answers { get; set; }",
      "desc": "Gets or sets the answers keyed by question name.",
      "return": "Dictionary\u003Cstring, SystemOneAnswer\u003E",
      "href": "answers.html"
    },
    {
      "name": "Usage",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneUsage Usage { get; set; }",
      "desc": "Gets or sets token usage information for the generation operation.",
      "return": "SystemOneUsage",
      "href": "usage.html"
    }
  ],
  "members": [
    {
      "name": "SystemOneResponse",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneResponse()",
      "desc": "Initializes a new instance of the SystemOneResponse class.",
      "return": "SystemOneResponse",
      "href": "systemoneresponse.html",
      "overline": "Returns: SystemOneResponse",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneResponse class.\u003C/p\u003E"
        }
      ]
    }
  ]
};
