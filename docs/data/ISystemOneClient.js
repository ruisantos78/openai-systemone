window.OS1_TYPES = window.OS1_TYPES || {};
window.OS1_TYPES["ISystemOneClient"] = {
  "name": "ISystemOneClient",
  "namespace": "OpenAI.SystemOne",
  "kind": "Interface",
  "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
  "declaration": "public interface ISystemOneClient",
  "overline": "Namespace: OpenAI.SystemOne \u00B7 Assembly: OpenAI.SystemOne",
  "summary": "Provides an abstraction for a client that answers choice, yes/no (noul), and scoring questions about a shared state using a System One endpoint.",
  "sections": [
    {
      "heading": "Remarks",
      "html": "\u003Cp\u003EProvides an abstraction for a client that answers choice, yes/no (noul), and scoring questions about a shared state using a System One endpoint.\u003C/p\u003E"
    }
  ],
  "properties": [
    {
      "name": "Model",
      "kind": "Property",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "string Model { get; }",
      "desc": "Gets the model name configured for this client.",
      "return": "string",
      "href": "model.html"
    }
  ],
  "members": [
    {
      "name": "DecideAsync(SystemOneState, IDictionary\u003Cstring, SystemOneQuestion\u003E, SystemOneOptions, CancellationToken)",
      "kind": "Method",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "ISystemOneClient.DecideAsync(SystemOneState state, IDictionary\u003Cstring, SystemOneQuestion\u003E questions, SystemOneOptions options = default, CancellationToken cancellationToken = default)",
      "desc": "Answers the given questions about the given state.",
      "return": "Task\u003CSystemOneResponse\u003E",
      "href": "decideasync-systemonestate-idictionary-string-systemonequestion-systemoneoptions-cancellationtoken.html",
      "overline": "Returns: Task\u003CSystemOneResponse\u003E",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003EAnswers the given questions about the given state.\u003C/p\u003E"
        },
        {
          "heading": "Parameters",
          "html": "\u003Ctable\u003E\u003Ctr\u003E\u003Cth\u003EName\u003C/th\u003E\u003Cth\u003EType\u003C/th\u003E\u003Cth\u003EDescription\u003C/th\u003E\u003C/tr\u003E\n\u003Ctr\u003E\u003Ctd\u003E\u003Ccode\u003Estate\u003C/code\u003E\u003C/td\u003E\u003Ctd\u003ESystemOneState\u003C/td\u003E\u003Ctd\u003EThe shared state that all questions are asked about (text, object, or array).\u003C/td\u003E\u003C/tr\u003E\n\u003Ctr\u003E\u003Ctd\u003E\u003Ccode\u003Equestions\u003C/code\u003E\u003C/td\u003E\u003Ctd\u003EIDictionary\u0026lt;string, SystemOneQuestion\u0026gt;\u003C/td\u003E\u003Ctd\u003EThe named questions about the shared state.\u003C/td\u003E\u003C/tr\u003E\n\u003Ctr\u003E\u003Ctd\u003E\u003Ccode\u003Eoptions\u003C/code\u003E\u003C/td\u003E\u003Ctd\u003ESystemOneOptions\u003C/td\u003E\u003Ctd\u003EOptional parameters for generation.\u003C/td\u003E\u003C/tr\u003E\n\u003Ctr\u003E\u003Ctd\u003E\u003Ccode\u003EcancellationToken\u003C/code\u003E\u003C/td\u003E\u003Ctd\u003ECancellationToken\u003C/td\u003E\u003Ctd\u003ECancellation token.\u003C/td\u003E\u003C/tr\u003E\n\u003C/table\u003E"
        },
        {
          "heading": "Returns",
          "html": "\u003Cp\u003EA task containing the SystemOneResponse.\u003C/p\u003E"
        }
      ]
    }
  ]
};
