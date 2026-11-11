window.OS1_TYPES = window.OS1_TYPES || {};
window.OS1_TYPES["OpenAiClientSystemOneExtensions"] = {
  "name": "OpenAiClientSystemOneExtensions",
  "namespace": "OpenAI",
  "kind": "Class",
  "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
  "declaration": "public class OpenAiClientSystemOneExtensions",
  "overline": "Namespace: OpenAI \u00B7 Assembly: OpenAI.SystemOne",
  "summary": "Provides extension methods for creating System One clients from OpenAI clients.",
  "sections": [
    {
      "heading": "Remarks",
      "html": "\u003Cp\u003EProvides extension methods for creating System One clients from OpenAI clients.\u003C/p\u003E"
    }
  ],
  "properties": [],
  "members": [
    {
      "name": "GetSystemOneClient(OpenAIClient, string)",
      "kind": "Method",
      "since": "1.0.0\u002Bccbad9bde937c4b8ed4f8ca759a9d89b2784d406",
      "declaration": "GetSystemOneClient(OpenAIClient openAiClient, string model)",
      "desc": "Creates a System One client that reuses the pipeline and endpoint of an OpenAI client.",
      "return": "ISystemOneClient",
      "href": "getsystemoneclient-openaiclient-string.html",
      "overline": "Returns: ISystemOneClient",
      "sections": [
        {
          "heading": "Definition",
          "html": "\u003Cp\u003ECreates a System One client that reuses the pipeline and endpoint of an OpenAI client.\u003C/p\u003E"
        },
        {
          "heading": "Parameters",
          "html": "\u003Ctable\u003E\u003Ctr\u003E\u003Cth\u003EName\u003C/th\u003E\u003Cth\u003EType\u003C/th\u003E\u003Cth\u003EDescription\u003C/th\u003E\u003C/tr\u003E\n\u003Ctr\u003E\u003Ctd\u003E\u003Ccode\u003EopenAiClient\u003C/code\u003E\u003C/td\u003E\u003Ctd\u003EOpenAIClient\u003C/td\u003E\u003Ctd\u003EThe OpenAI client whose pipeline and endpoint are reused.\u003C/td\u003E\u003C/tr\u003E\n\u003Ctr\u003E\u003Ctd\u003E\u003Ccode\u003Emodel\u003C/code\u003E\u003C/td\u003E\u003Ctd\u003Estring\u003C/td\u003E\u003Ctd\u003EThe model name to use for System One requests.\u003C/td\u003E\u003C/tr\u003E\n\u003C/table\u003E"
        },
        {
          "heading": "Returns",
          "html": "\u003Cp\u003EA System One client.\u003C/p\u003E"
        }
      ]
    }
  ]
};
