# OpenAI.SystemOne

.NET client for the System One API, with integration with `OpenAIClient` and .NET Aspire hosting. Send a state and a set of named questions to receive choice, yes/no (noul), or score answers.

## Installation

Install the NuGet package in your application project:

```bash
dotnet add package OpenAI.SystemOne
```

The package targets .NET 11 and depends on `OpenAI` and `Aspire.OpenAI`.

## .NET Aspire integration

Register an OpenAI client with the `decision` key in the AppHost and chain `.AddSystemOneClient()`:

```csharp
var decision = builder.AddKeyedOpenAIClient("decision")
    .AddSystemOneClient();
```

Configure the `decision` connection string with the endpoint, credentials, and model settings supported by `Aspire.OpenAI`. The extension uses the `OpenAIClient` registered for that key and registers an `ISystemOneClient` with the same key, plus an unkeyed `ISystemOneClient` registration. The model is read from `Model=` or `Deployment=` in the connection string. You can also specify it explicitly:

```csharp
builder.AddKeyedOpenAIClient("decision")
    .AddSystemOneClient(model: "my-model");
```

In the application service, inject the client with the same key — or without a key, using the unkeyed registration:

```csharp
using OpenAI.SystemOne;
using Microsoft.Extensions.DependencyInjection;

public sealed class DecisionService(
    [FromKeyedServices("decision")] ISystemOneClient client)
{
    public Task<SystemOneResponse> DecideAsync(CancellationToken cancellationToken) =>
        client.DecideAsync(
            state: "Does the proposal meet the criteria?",
            questions: new Dictionary<string, SystemOneQuestion>
            {
                ["approved"] = new SystemOneNoulQuestion
                {
                    Instructions = "Determine whether the proposal should be approved.",
                    Criteria = new SystemOneNoulCriteria
                    {
                        Yes = "Meets the defined criteria.",
                        No = "Does not meet the defined criteria."
                    }
                }
            },
            cancellationToken: cancellationToken);
}
```

The extension also provides `AddKeyedSystemOneClient` to register the System One client under a key different from the `OpenAIClient` key:

```csharp
builder.AddKeyedOpenAIClient("decision")
    .AddKeyedSystemOneClient("systemone");
```

The `OpenAIClient` key is used to resolve the OpenAI client; `ISystemOneClient` is registered under `systemone`.

## Usage without Aspire

You can also create the client directly:

```csharp
using OpenAI.SystemOne;

using var client = new SystemOneClient(
    model: "my-model",
    endpoint: new Uri("http://localhost:11434"),
    apiKey: "optional-key");

SystemOneResponse response = await client.DecideAsync(
    state: "Content to analyze",
    questions: new Dictionary<string, SystemOneQuestion>
    {
        ["category"] = new SystemOneChoiceQuestion
        {
            Instructions = "Choose the most appropriate category.",
            Criteria = new Dictionary<string, string?>
            {
                ["support"] = "Support request",
                ["sales"] = "Sales request"
            }
        }
    },
    cancellationToken: cancellationToken);
```

The client sends a `POST` to `v1/systemone`, appended to the endpoint unless the address already ends with that path (an address ending in `v1` is normalized as well).

### State

`DecideAsync` accepts a `SystemOneState`, which is implicitly created from:

- A text string: `client.DecideAsync("plain text state", questions)`
- A `JsonObject` or `JsonArray`
- A `string[]` (converted to a JSON array of strings)
- Any serializable object via `SystemOneState.FromObject(value)`

### Options

Use `SystemOneOptions` to send additional parameters, images, or the `KeepAlive` option:

```csharp
var options = new SystemOneOptions
{
    KeepAlive = "10m",                 // or seconds (int), a TimeSpan, or 10m-style durations
    Images = ["<base64-encoded image>"],
    AdditionalProperties = new Dictionary<string, object?>
    {
        ["temperature"] = 0.2
    }
};
```

`KeepAlive` specifies how long the model stays loaded after the request and is serialized as `keep_alive`. `Images` is serialized as `images`. `AdditionalProperties` merges provider-specific properties into the request body at the root level.

### Response

`DecideAsync` returns a `SystemOneResponse` with `Model`, `Answers` keyed by question name, and `Usage`. Each answer is deserialized according to the question type, so cast it to the matching answer class to read its data:

```csharp
SystemOneResponse response = await client.DecideAsync(state, questions);

// Choice answer: the selected option, its probabilities, and the model's confidence.
var category = (SystemOneChoiceAnswer)response.Answers["category"];
Console.WriteLine(category.Choice);              // "support"
Console.WriteLine(category.Confidence);          // 0.8906
Console.WriteLine(category.Probabilities["bug"]); // 0.9781

// Noul answer: the yes/no answer as a numeric value.
var urgent = (SystemOneNoulAnswer)response.Answers["urgent"];
Console.WriteLine(urgent.Noul);                  // 0.959

// Score answer: the score, the legend describing each level, and probabilities.
var urgency = (SystemOneScoreAnswer)response.Answers["urgency"];
Console.WriteLine(urgency.Score);                // 0.8308
Console.WriteLine(urgency.Legend["2"]);          // "Immediate: a critical service is unavailable"

// Token usage of the generation operation.
Console.WriteLine(response.Usage.InputTokens);   // 174
Console.WriteLine(response.Usage.OutputTokens);  // 1
```

`response.Model` contains the model that generated the response, as reported by the endpoint.

### Reusing an OpenAIClient

You can also create a System One client from an existing `OpenAIClient`, reusing its pipeline and endpoint:

```csharp
using OpenAI;
using OpenAI.SystemOne;

ISystemOneClient client = openAiClient.GetSystemOneClient("my-model");
```

## Question types

- `SystemOneChoiceQuestion`: selects one item from named criteria (2 to 255 criteria).
- `SystemOneNoulQuestion`: produces a yes/no answer. The criteria are optional and describe the `false` and `true` outcomes.
- `SystemOneScoreQuestion`: scores the state using an ordered scale of criteria (2 to 10 criteria).

```csharp
var questions = new Dictionary<string, SystemOneQuestion>
{
    ["label"] = new SystemOneChoiceQuestion
    {
        Instructions = "Which label fits this ticket?",
        Criteria = new Dictionary<string, string?>
        {
            ["billing"] = "Payments and refunds",
            ["bug"] = "Software errors",
            ["account"] = "Login and account access"
        }
    },
    ["urgent"] = new SystemOneNoulQuestion
    {
        Instructions = "Does this ticket need attention within the hour?",
        Criteria = new SystemOneNoulCriteria
        {
            No = "No attention is needed now",
            Yes = "Needs attention within the hour"
        }
    },
    ["urgency"] = new SystemOneScoreQuestion
    {
        Instructions = "How urgently does this ticket need a response?",
        Criteria =
        [
            "Routine: no time pressure",
            "Soon: a customer is inconvenienced",
            "Immediate: a critical service is unavailable"
        ]
    }
};
```

The corresponding answers are `SystemOneChoiceAnswer`, `SystemOneNoulAnswer`, and `SystemOneScoreAnswer`, accessible in `SystemOneResponse.Answers` by question name:

- `SystemOneChoiceAnswer`: `Choice`, `Probabilities`, and `Confidence`.
- `SystemOneNoulAnswer`: `Noul` (the yes/no answer as a numeric value).
- `SystemOneScoreAnswer`: `Score`, `Legend`, `Probabilities`, and `Confidence`.

`SystemOneResponse` also exposes `Model` and `Usage` (`InputTokens` and `OutputTokens`).

## Error handling

Failed requests throw a `SystemOneException` carrying the HTTP status code and the error message returned by the endpoint.
