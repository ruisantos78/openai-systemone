window.OS1_TYPES = window.OS1_TYPES || {};
window.OS1_TYPES["SystemOneQuestion"] = {
  "name": "SystemOneQuestion",
  "namespace": "OpenAI.SystemOne",
  "kind": "Class",
  "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
  "declaration": "public class SystemOneQuestion",
  "overline": "Namespace: OpenAI.SystemOne \u00B7 Assembly: OpenAI.SystemOne",
  "summary": "A single named question asked in a System One request. Use SystemOneChoiceQuestion, SystemOneNoulQuestion, or SystemOneScoreQuestion to create one.",
  "sections": [
    {
      "heading": "Remarks",
      "html": "\u003Cp\u003EA single named question asked in a System One request. Use SystemOneChoiceQuestion, SystemOneNoulQuestion, or SystemOneScoreQuestion to create one.\u003C/p\u003E"
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
      "name": "Instructions",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneContent Instructions { get; set; }",
      "desc": "Gets or sets the instructions shown to the model for this question.",
      "return": "SystemOneContent",
      "href": "instructions.html"
    }
  ],
  "members": []
};
