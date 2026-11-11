window.OS1_TYPES = window.OS1_TYPES || {};
window.OS1_TYPES["SystemOneException"] = {
  "name": "SystemOneException",
  "namespace": "OpenAI.SystemOne",
  "kind": "Exception",
  "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
  "declaration": "public class SystemOneException",
  "overline": "Namespace: OpenAI.SystemOne \u00B7 Assembly: OpenAI.SystemOne",
  "summary": "Represents an error returned by a System One endpoint or a failure while invoking it.",
  "sections": [
    {
      "heading": "Remarks",
      "html": "\u003Cp\u003ERepresents an error returned by a System One endpoint or a failure while invoking it.\u003C/p\u003E"
    }
  ],
  "properties": [
    {
      "name": "StatusCode",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "int? StatusCode { get; }",
      "desc": "Gets the HTTP status code associated with the error, if available.",
      "return": "int?",
      "href": "statuscode.html"
    }
  ],
  "members": [
    {
      "name": "SystemOneException",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneException()",
      "desc": "Initializes a new instance of the SystemOneException class.",
      "return": "SystemOneException",
      "href": "systemoneexception.html",
      "overline": "Returns: SystemOneException",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneException class.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "SystemOneException(string)",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneException(string message)",
      "desc": "Initializes a new instance of the SystemOneException class.",
      "return": "SystemOneException",
      "href": "systemoneexception-string.html",
      "overline": "Returns: SystemOneException",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneException class.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "SystemOneException(string, Exception)",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneException(string message, Exception innerException)",
      "desc": "Initializes a new instance of the SystemOneException class.",
      "return": "SystemOneException",
      "href": "systemoneexception-string-exception.html",
      "overline": "Returns: SystemOneException",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneException class.\u003C/p\u003E"
        }
      ]
    },
    {
      "name": "SystemOneException(int, string, Exception)",
      "kind": "Constructor",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "SystemOneException(int statusCode, string message, Exception innerException = default)",
      "desc": "Initializes a new instance of the SystemOneException class.",
      "return": "SystemOneException",
      "href": "systemoneexception-int-string-exception.html",
      "overline": "Returns: SystemOneException",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EInitializes a new instance of the SystemOneException class.\u003C/p\u003E"
        }
      ]
    }
  ]
};
