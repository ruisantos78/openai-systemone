# OpenAI.SystemOne

Biblioteca .NET para chamar um endpoint compatível com System One e obter respostas a perguntas estruturadas sobre um estado partilhado.

## Requisitos

- .NET 10 SDK (o projeto também declara um target para .NET 11).
- Um endpoint System One acessível pela aplicação.

## Instalação

Adicione o projeto à sua solução:

```bash
dotnet add reference caminho/para/OpenAI.SystemOne.csproj
```
O projeto usa os pacotes `OpenAI` e `Aspire.OpenAI`, definidos no ficheiro `.csproj`.

## Utilização

Crie o cliente com o nome do modelo e o endereço base do serviço. A chave API é opcional:

```csharp
using OpenAI.SystemOne;

using var client = new SystemOneClient(
    model: "meu-modelo",
    endpoint: new Uri("http://localhost:11434"),
    apiKey: "chave-opcional");

var questions = new Dictionary<string, SystemOneQuestion>
{
    // Configure aqui as perguntas suportadas pelo serviço.
};

SystemOneResponse response = await client.GenerateAsync(
    state: new { text = "Conteúdo a analisar" },
    questions: questions,
    cancellationToken: cancellationToken);
```

O cliente envia um `POST` para `v1/systemone` (acrescentado ao endereço base, caso ainda não esteja presente). Os tipos `SystemOneQuestion`, `SystemOneOptions` e `SystemOneResponse` descrevem as perguntas, opções e resposta. `GenerateAsync` aceita também um estado como texto ou objeto serializável.

## Aspire

O projeto inclui extensões de hosting para integrar o cliente com `Aspire.OpenAI`. Consulte `Hosting/AspireOpenAIClientBuilderSystemOneExtensions.cs` para as opções de configuração disponíveis.

## Compilação

```bash
dotnet build OpenAI.SystemOne.slnx
```
