window.OS1_TYPES = window.OS1_TYPES || {};
window.OS1_TYPES["SystemOneState"] = {
  "name": "SystemOneState",
  "namespace": "OpenAI.SystemOne",
  "kind": "Union",
  "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
  "declaration": "public union SystemOneState",
  "overline": "Namespace: OpenAI.SystemOne \u00B7 Assembly: OpenAI.SystemOne",
  "summary": "Represents the shared state that questions are asked about which can be text, an object, or an array.",
  "sections": [
    {
      "heading": "Remarks",
      "html": "\u003Cp\u003ERepresents the shared state that questions are asked about which can be text, an object, or an array.\u003C/p\u003E"
    }
  ],
  "properties": [
    {
      "name": "Value",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "object Value { get; }",
      "desc": "",
      "return": "object",
      "href": "value.html"
    }
  ],
  "members": [
    {
      "name": "SystemOneState(string)",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneState(string value)",
      "desc": "Initializes a new instance of the SystemOneState class.",
      "return": "SystemOneState",
      "href": "systemonestate-string.html",
      "overline": "Returns: SystemOneState",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneState class.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "SystemOneState(JsonObject)",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneState(JsonObject value)",
      "desc": "Initializes a new instance of the SystemOneState class.",
      "return": "SystemOneState",
      "href": "systemonestate-jsonobject.html",
      "overline": "Returns: SystemOneState",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneState class.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "SystemOneState(JsonArray)",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneState(JsonArray value)",
      "desc": "Initializes a new instance of the SystemOneState class.",
      "return": "SystemOneState",
      "href": "systemonestate-jsonarray.html",
      "overline": "Returns: SystemOneState",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneState class.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "FromObject(object)",
      "kind": "Method",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "FromObject(object value)",
      "desc": "Creates a SystemOneState from any serializable object.",
      "return": "SystemOneState",
      "href": "fromobject-object.html",
      "overline": "Returns: SystemOneState",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003ECreates a SystemOneState from any serializable object.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "implicit operator SystemOneState",
      "kind": "Operator",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public static implicit operator SystemOneState(string value)",
      "desc": "Creates a SystemOneState from a text string.",
      "return": "SystemOneState",
      "href": "implicit-operator-systemonestate.html",
      "overline": "Returns: SystemOneState",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003ECreates a SystemOneState from a text string.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "implicit operator SystemOneState",
      "kind": "Operator",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public static implicit operator SystemOneState(JsonObject value)",
      "desc": "Creates a SystemOneState from a JsonObject.",
      "return": "SystemOneState",
      "href": "implicit-operator-systemonestate.html",
      "overline": "Returns: SystemOneState",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003ECreates a SystemOneState from a JsonObject.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "implicit operator SystemOneState",
      "kind": "Operator",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public static implicit operator SystemOneState(JsonArray value)",
      "desc": "Creates a SystemOneState from a JsonArray.",
      "return": "SystemOneState",
      "href": "implicit-operator-systemonestate.html",
      "overline": "Returns: SystemOneState",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003ECreates a SystemOneState from a JsonArray.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "implicit operator SystemOneState",
      "kind": "Operator",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public static implicit operator SystemOneState(JsonNode value)",
      "desc": "Creates a SystemOneState from a JsonNode.",
      "return": "SystemOneState",
      "href": "implicit-operator-systemonestate.html",
      "overline": "Returns: SystemOneState",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003ECreates a SystemOneState from a JsonNode.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "implicit operator SystemOneState",
      "kind": "Operator",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public static implicit operator SystemOneState(string[] value)",
      "desc": "Creates a SystemOneState from a string array.",
      "return": "SystemOneState",
      "href": "implicit-operator-systemonestate.html",
      "overline": "Returns: SystemOneState",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003ECreates a SystemOneState from a string array.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "implicit operator SystemOneState",
      "kind": "Operator",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public static implicit operator SystemOneState(SystemOneContent value)",
      "desc": "Implicitly converts a SystemOneContent to a SystemOneState.",
      "return": "SystemOneState",
      "href": "implicit-operator-systemonestate.html",
      "overline": "Returns: SystemOneState",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EImplicitly converts a SystemOneContent to a SystemOneState.\u003C/p\u003E"
        }
      ]
    }
  ]
};
