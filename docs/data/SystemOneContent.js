window.OS1_TYPES = window.OS1_TYPES || {};
window.OS1_TYPES["SystemOneContent"] = {
  "name": "SystemOneContent",
  "namespace": "OpenAI.SystemOne",
  "kind": "Union",
  "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
  "declaration": "public union SystemOneContent",
  "overline": "Namespace: OpenAI.SystemOne \u00B7 Assembly: OpenAI.SystemOne",
  "summary": "Content representing a nonempty string, or an object or array serialized as JSON text. Used for question instructions and shared state.",
  "sections": [
    {
      "heading": "Remarks",
      "html": "\u003Cp\u003EContent representing a nonempty string, or an object or array serialized as JSON text. Used for question instructions and shared state.\u003C/p\u003E"
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
      "name": "SystemOneContent(string)",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneContent(string value)",
      "desc": "Initializes a new instance of the SystemOneContent class.",
      "return": "SystemOneContent",
      "href": "systemonecontent-string.html",
      "overline": "Returns: SystemOneContent",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneContent class.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "SystemOneContent(JsonObject)",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneContent(JsonObject value)",
      "desc": "Initializes a new instance of the SystemOneContent class.",
      "return": "SystemOneContent",
      "href": "systemonecontent-jsonobject.html",
      "overline": "Returns: SystemOneContent",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneContent class.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "SystemOneContent(JsonArray)",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneContent(JsonArray value)",
      "desc": "Initializes a new instance of the SystemOneContent class.",
      "return": "SystemOneContent",
      "href": "systemonecontent-jsonarray.html",
      "overline": "Returns: SystemOneContent",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneContent class.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "FromObject(object)",
      "kind": "Method",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "FromObject(object value)",
      "desc": "Creates a SystemOneContent from any serializable object.",
      "return": "SystemOneContent",
      "href": "fromobject-object.html",
      "overline": "Returns: SystemOneContent",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003ECreates a SystemOneContent from any serializable object.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "implicit operator SystemOneContent",
      "kind": "Operator",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public static implicit operator SystemOneContent(string value)",
      "desc": "Creates a SystemOneContent from a text string.",
      "return": "SystemOneContent",
      "href": "implicit-operator-systemonecontent.html",
      "overline": "Returns: SystemOneContent",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003ECreates a SystemOneContent from a text string.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "implicit operator SystemOneContent",
      "kind": "Operator",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public static implicit operator SystemOneContent(JsonObject value)",
      "desc": "Creates a SystemOneContent from a JsonObject.",
      "return": "SystemOneContent",
      "href": "implicit-operator-systemonecontent.html",
      "overline": "Returns: SystemOneContent",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003ECreates a SystemOneContent from a JsonObject.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "implicit operator SystemOneContent",
      "kind": "Operator",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public static implicit operator SystemOneContent(JsonArray value)",
      "desc": "Creates a SystemOneContent from a JsonArray.",
      "return": "SystemOneContent",
      "href": "implicit-operator-systemonecontent.html",
      "overline": "Returns: SystemOneContent",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003ECreates a SystemOneContent from a JsonArray.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "implicit operator SystemOneContent",
      "kind": "Operator",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public static implicit operator SystemOneContent(JsonNode value)",
      "desc": "Creates a SystemOneContent from a JsonNode.",
      "return": "SystemOneContent",
      "href": "implicit-operator-systemonecontent.html",
      "overline": "Returns: SystemOneContent",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003ECreates a SystemOneContent from a JsonNode.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "implicit operator SystemOneContent",
      "kind": "Operator",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public static implicit operator SystemOneContent(string[] value)",
      "desc": "Creates a SystemOneContent from a string array.",
      "return": "SystemOneContent",
      "href": "implicit-operator-systemonecontent.html",
      "overline": "Returns: SystemOneContent",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003ECreates a SystemOneContent from a string array.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "implicit operator SystemOneContent",
      "kind": "Operator",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "public static implicit operator SystemOneContent(SystemOneState value)",
      "desc": "Implicitly converts a SystemOneState to a SystemOneContent.",
      "return": "SystemOneContent",
      "href": "implicit-operator-systemonecontent.html",
      "overline": "Returns: SystemOneContent",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EImplicitly converts a SystemOneState to a SystemOneContent.\u003C/p\u003E"
        }
      ]
    }
  ]
};
