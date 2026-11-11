# OpenAI.SystemOne

A .NET client library for the System One (Decision API) typed question-answering endpoint (`/v1/systemone`), designed to integrate seamlessly with the official `OpenAI` .NET SDK client pipeline and .NET Aspire (`Aspire.OpenAI`).

## Overview

System One answers choice, boolean (noul), and scoring questions using high-speed decision models (such as `kev-4b`, `clef-flash`, or `laya-multilingual`). This library models the typed requests and polymorphic answers, and integrates with the `OpenAIClient` pipeline to handle authentication, telemetry, logging, and Aspire service discovery.

## File Inventory

| File | Type | Description |
|------|------|-------------|
| `SystemOneClient.cs` | Client | Main implementation of `ISystemOneClient` using `ClientPipeline` |
| `Abstractions/ISystemOneClient.cs` | Interface | Public contract for System One decision inference |
| `Abstractions/SystemOneAnswer.cs` | Model | Polymorphic answers: `SystemOneChoiceAnswer`, `SystemOneNoulAnswer`, `SystemOneScoreAnswer` |
| `Abstractions/SystemOneClientMetadata.cs` | Metadata | Component metadata and version information |
| `Abstractions/SystemOneException.cs` | Exception | Custom exception carrying HTTP status and response payload |
| `Abstractions/SystemOneOptions.cs` | Options | Execution options for System One decision calls |
| `Abstractions/SystemOneQuestion.cs` | Model | Polymorphic questions: `SystemOneChoiceQuestion`, `SystemOneNoulQuestion`, `SystemOneScoreQuestion` |
| `Abstractions/SystemOneResponse.cs` | Model | Response payload containing model identifier and dictionary of answers |
| `Hosting/AspireOpenAIClientBuilderSystemOneExtensions.cs` | Extension | Integrates `ISystemOneClient` into .NET Aspire `OpenAIClient` builders |
| `Hosting/OpenAIClientSystemOneExtensions.cs` | Extension | Adds `GetSystemOneClient()` extension method to `OpenAIClient` |
| `Schemas/SystemOneJsonContext.cs` | JSON | Source-generated `JsonSerializerContext` for Native AOT and trimmed serialization |
| `Schemas/WireSystemOneRequest.cs` | Wire Model | Internal request serialization shape for `/v1/systemone` |

## Directory Structure

```text
Abstractions/
  ISystemOneClient.cs
  SystemOneAnswer.cs
  SystemOneClientMetadata.cs
  SystemOneException.cs
  SystemOneOptions.cs
  SystemOneQuestion.cs
  SystemOneResponse.cs
Hosting/
  AspireOpenAIClientBuilderSystemOneExtensions.cs
  OpenAIClientSystemOneExtensions.cs
Schemas/
  SystemOneJsonContext.cs
  WireSystemOneRequest.cs
OpenAI.SystemOne.csproj
SystemOneClient.cs
```
