# OpenAI.SystemOne

Cliente .NET para a API System One, com integração ao `OpenAIClient` e ao hosting do .NET Aspire. Envie um estado e um conjunto de perguntas nomeadas para receber respostas de escolha, sim/não (noul) ou pontuação.

## Instalação

Instale o pacote NuGet no projeto da aplicação:

```bash
dotnet add package OpenAI.SystemOne
```

O pacote suporta .NET 10 e .NET 11 e depende de `OpenAI` e `Aspire.OpenAI`.

## Integração com .NET Aspire

Registe um cliente OpenAI com a chave `decision` no AppHost e encadeie `.AddSystemOneClient()`:

```csharp
var decision = builder.AddKeyedOpenAIClient("decision")
    .AddSystemOneClient();
```

Configure a connection string `decision` com as definições de endpoint, credenciais e modelo suportadas por `Aspire.OpenAI`. A extensão usa o `OpenAIClient` registado para essa chave e regista um `ISystemOneClient` com a mesma chave. O modelo é lido de `Model=` ou `Deployment=` na connection string. Também pode informá-lo explicitamente:

```csharp
builder.AddKeyedOpenAIClient("decision")
    .AddSystemOneClient(model: "meu-modelo");
```

No serviço da aplicação, injete o cliente com a mesma chave:

```csharp
using OpenAI.SystemOne;
using Microsoft.Extensions.DependencyInjection;

public sealed class DecisionService(
    [FromKeyedServices("decision")] ISystemOneClient client)
{
    public Task<SystemOneResponse> DecideAsync(CancellationToken cancellationToken) =>
        client.GenerateAsync(
            state: new { text = "A proposta cumpre os critérios?" },
            questions: new Dictionary<string, SystemOneQuestion>
            {
                ["approved"] = new SystemOneNoulQuestion
                {
                    Instructions = System.Text.Json.Nodes.JsonValue.Create(
                        "Determine se a proposta deve ser aprovada."),
                    Criteria = new SystemOneNoulCriteria
                    {
                        Yes = "Cumpre os critérios definidos.",
                        No = "Não cumpre os critérios definidos."
                    }
                }
            },
            cancellationToken: cancellationToken);
}
```

A extensão também oferece `AddKeyedSystemOneClient` para escolher uma chave de cliente System One diferente da chave do `OpenAIClient`.

## Uso sem Aspire

Também pode criar o cliente diretamente:

```csharp
using OpenAI.SystemOne;

using var client = new SystemOneClient(
    model: "meu-modelo",
    endpoint: new Uri("http://localhost:11434"),
    apiKey: "chave-opcional");

SystemOneResponse response = await client.GenerateAsync(
    state: new { text = "Conteúdo a analisar" },
    questions: new Dictionary<string, SystemOneQuestion>
    {
        ["category"] = new SystemOneChoiceQuestion
        {
            Instructions = System.Text.Json.Nodes.JsonValue.Create("Escolha a categoria mais adequada."),
            Criteria = new Dictionary<string, string?>
            {
                ["support"] = "Pedido de suporte",
                ["sales"] = "Pedido comercial"
            }
        }
    },
    cancellationToken: cancellationToken);
```

O cliente envia um `POST` para `v1/systemone`, acrescentado ao endereço base se ainda não estiver presente. `GenerateAsync` aceita texto ou qualquer objeto serializável como estado. Use `SystemOneOptions` para enviar parâmetros adicionais, imagens ou a opção `KeepAlive`.

## Tipos de pergunta

- `SystemOneChoiceQuestion`: escolhe um item entre critérios nomeados.
- `SystemOneNoulQuestion`: produz uma resposta sim/não.
- `SystemOneScoreQuestion`: pontua o estado segundo uma escala de critérios ordenada.

As respostas correspondentes são `SystemOneChoiceAnswer`, `SystemOneNoulAnswer` e `SystemOneScoreAnswer`, acessíveis em `SystemOneResponse.Answers` pelo nome da pergunta.
