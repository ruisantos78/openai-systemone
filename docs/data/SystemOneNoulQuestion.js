window.OS1_TYPES = window.OS1_TYPES || {};
window.OS1_TYPES["SystemOneNoulQuestion"] = {
  "name": "SystemOneNoulQuestion",
  "namespace": "OpenAI.SystemOne",
  "kind": "Class",
  "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
  "declaration": "public class SystemOneNoulQuestion",
  "overline": "Namespace: OpenAI.SystemOne \u00B7 Assembly: OpenAI.SystemOne",
  "summary": "A yes/no (noul) question.",
  "sections": [
    {
      "heading": "Remarks",
      "html": "\u003Cp\u003EA yes/no (noul) question.\u003C/p\u003E"
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
      "declaration": "SystemOneNoulCriteria Criteria { get; set; }",
      "desc": "Gets or sets the optional descriptions for the no and yes outcomes.",
      "return": "SystemOneNoulCriteria",
      "href": "criteria.html"
    },
    {
      "name": "NoulTypeName",
      "kind": "Constant",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public const string NoulTypeName = \u0022noul\u0022",
      "desc": "The JSON type discriminator for noul questions.",
      "return": "string",
      "href": "noultypename.html"
    }
  ],
  "members": [
    {
      "name": "SystemOneNoulQuestion",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneNoulQuestion()",
      "desc": "Initializes a new instance of the SystemOneNoulQuestion class.",
      "return": "SystemOneNoulQuestion",
      "href": "systemonenoulquestion.html",
      "overline": "Returns: SystemOneNoulQuestion",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneNoulQuestion class.\u003C/p\u003E"
        }
      ]
    }
  ]
};
