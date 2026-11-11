# OpenAI.SystemOne

.NET client for the System One API, with integration with `OpenAIClient` and .NET Aspire hosting. Send a state and a set of named questions to receive choice, yes/no (noul), or score answers.

## Installation

Install the NuGet package in your application project:

```bash
dotnet add package OpenAI.SystemOne
```

The package supports .NET 10 and .NET 11 and depends on `OpenAI` and `Aspire.OpenAI`.

## .NET Aspire integration

Register an OpenAI client with the `decision` key in the AppHost and chain `.AddSystemOneClient()`:

```csharp
var decision = builder.AddKeyedOpenAIClient("decision")
    .AddSystemOneClient();
```

Configure the `decision` connection string with the endpoint, credentials, and model settings supported by `Aspire.OpenAI`. The extension uses the `OpenAIClient` registered for that key and registers an `ISystemOneClient` with the same key. The model is read from `Model=` or `Deployment=` in the connection string. You can also specify it explicitly:

```csharp
builder.AddKeyedOpenAIClient("decision")
    .AddSystemOneClient(model: "my-model");
```

In the application service, inject the client with the same key:

```csharp
using OpenAI.SystemOne;
using Microsoft.Extensions.DependencyInjection;

public sealed class DecisionService(
    [FromKeyedServices("decision")] ISystemOneClient client)
{
    public Task<SystemOneResponse> DecideAsync(CancellationToken cancellationToken) =>
        client.DecideAsync(
            state: new { text = "Does the proposal meet the criteria?" },
            questions: new Dictionary<string, SystemOneQuestion>
            {
                ["approved"] = new SystemOneNoulQuestion
                {
                    Instructions = System.Text.Json.Nodes.JsonValue.Create(
                        "Determine whether the proposal should be approved."),
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

The extension also provides `AddKeyedSystemOneClient` to choose a System One client key different from the `OpenAIClient` key.

## Usage without Aspire

You can also create the client directly:

```csharp
using OpenAI.SystemOne;

using var client = new SystemOneClient(
    model: "my-model",
    endpoint: new Uri("http://localhost:11434"),
    apiKey: "optional-key");

SystemOneResponse response = await client.DecideAsync(
    state: new { text = "Content to analyze" },
    questions: new Dictionary<string, SystemOneQuestion>
    {
        ["category"] = new SystemOneChoiceQuestion
        {
            Instructions = System.Text.Json.Nodes.JsonValue.Create("Choose the most appropriate category."),
            Criteria = new Dictionary<string, string?>
            {
                ["support"] = "Support request",
                ["sales"] = "Sales request"
            }
        }
    },
    cancellationToken: cancellationToken);
```

The client sends a `POST` to `v1/systemone`, appended to the base address if it is not already present. `DecideAsync` accepts text or any serializable object as the state. Use `SystemOneOptions` to send additional parameters, images, or the `KeepAlive` option.

## Question types

- `SystemOneChoiceQuestion`: selects one item from named criteria.
- `SystemOneNoulQuestion`: produces a yes/no answer.
- `SystemOneScoreQuestion`: scores the state using an ordered scale of criteria.

The corresponding answers are `SystemOneChoiceAnswer`, `SystemOneNoulAnswer`, and `SystemOneScoreAnswer`, accessible in `SystemOneResponse.Answers` by question name.
