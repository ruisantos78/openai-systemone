window.OS1_TYPES = window.OS1_TYPES || {};
window.OS1_TYPES["SystemOneClientMetadata"] = {
  "name": "SystemOneClientMetadata",
  "namespace": "OpenAI.SystemOne",
  "kind": "Class",
  "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
  "declaration": "public class SystemOneClientMetadata",
  "overline": "Namespace: OpenAI.SystemOne \u00B7 Assembly: OpenAI.SystemOne",
  "summary": "Describes an ISystemOneClient, for use in logging and diagnostics.",
  "sections": [
    {
      "heading": "Remarks",
      "html": "\u003Cp\u003EDescribes an ISystemOneClient, for use in logging and diagnostics.\u003C/p\u003E"
    }
  ],
  "properties": [
    {
      "name": "ModelName",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "string ModelName { get; set; }",
      "desc": "Gets the name of the System One model used by the client, such as nimble or clef:cloud.",
      "return": "string",
      "href": "modelname.html"
    },
    {
      "name": "ProviderName",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "string ProviderName { get; set; }",
      "desc": "Gets the name of the service provider, such as ollama.",
      "return": "string",
      "href": "providername.html"
    },
    {
      "name": "Endpoint",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "Uri Endpoint { get; set; }",
      "desc": "Gets the service endpoint the client sends requests to, if known.",
      "return": "Uri",
      "href": "endpoint.html"
    },
    {
      "name": "AdditionalProperties",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "IDictionary\u003Cstring, object\u003E AdditionalProperties { get; set; }",
      "desc": "Gets any additional information about the client, if available.",
      "return": "IDictionary\u003Cstring, object\u003E",
      "href": "additionalproperties.html"
    }
  ],
  "members": [
    {
      "name": "SystemOneClientMetadata",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneClientMetadata()",
      "desc": "Initializes a new instance of the SystemOneClientMetadata class.",
      "return": "SystemOneClientMetadata",
      "href": "systemoneclientmetadata.html",
      "overline": "Returns: SystemOneClientMetadata",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneClientMetadata class.\u003C/p\u003E"
        }
      ]
    }
  ]
};
