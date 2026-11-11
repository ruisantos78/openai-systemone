# OpenAI.SystemOne - Agent Guidelines & Repository Context

## Project Overview

`OpenAI.SystemOne` is a .NET client library for the System One API. It integrates with
`OpenAIClient` and .NET Aspire and supports choice, yes/no (noul), and scoring questions.

The library targets .NET 10 and .NET 11 and is published as the `OpenAI.SystemOne` NuGet
package.

## Repository Layout

- **`src/OpenAI.SystemOne/`**: Main library source code.
  - **`Abstractions/`**: Public client contracts, request models, response models, options,
    and exceptions.
  - **`Hosting/`**: .NET Aspire and `OpenAIClient` integration extensions.
  - **`Schemas/`**: JSON source-generation context and wire request models.
  - **`SystemOneClient.cs`**: HTTP client implementation for the System One endpoint.
  - **`icon.png`**: NuGet package icon.
- **`README.md`**: Package and repository documentation. It is included in the NuGet package.
- **`OpenAI.SystemOne.slnx`**: Solution file.
- **`src/OpenAI.SystemOne/OpenAI.SystemOne.csproj`**: Project, package metadata, target
  frameworks, and dependency versions.

## Documentation Duty

Keep public API documentation in sync with the source code. Whenever a public class,
interface, constructor, property, method, extension method, or constant is added or changed:

1. Add or update its XML documentation in the source file.
2. Update `README.md` when the public usage, configuration, supported question types, or
   deployment process changes.
3. Preserve the package README's code examples and ensure they use valid API signatures.

The project enables `<GenerateDocumentationFile>true</GenerateDocumentationFile>`, so public
members should not be left undocumented.

## Build, Test & Package Commands

- **Restore dependencies**:
  ```bash
  dotnet restore OpenAI.SystemOne.slnx
  ```
- **Build the solution**:
  ```bash
  dotnet build OpenAI.SystemOne.slnx
  ```
- **Build in Release mode**:
  ```bash
  dotnet build OpenAI.SystemOne.slnx -c Release
  ```
- **Create the NuGet package**:
  ```bash
  dotnet pack src/OpenAI.SystemOne/OpenAI.SystemOne.csproj -c Release
  ```

There is currently no test project in this repository. If tests are added, run `dotnet test`
before packaging and releasing.

## Coding & Architectural Conventions

1. Target both `net10.0` and `net11.0`; keep code compatible with both target frameworks.
2. Preserve nullable reference type annotations and the existing implicit-using configuration.
3. Keep `ISystemOneClient` as the abstraction used for dependency injection and application
   code.
4. Keep `SystemOneClient` responsible for request serialization, endpoint resolution, HTTP
   execution, response deserialization, and conversion to `SystemOneException`.
5. Keep OpenAI and Aspire integration in extension classes under `Hosting/`.
6. Do not hard-code deployment-specific ports or credentials. Read endpoint, credentials, and
   model configuration from the supplied client or connection string.
7. Preserve the System One endpoint path (`v1/systemone`) and the JSON property names used by
   the wire contract unless the API contract is intentionally changed.
8. Public API changes require XML documentation and a corresponding README update when the
   usage changes.

## Configuration and Usage Rules

- `AddSystemOneClient()` uses the configured `OpenAIClient` and reads the model from the
  `Model=` or `Deployment=` connection-string setting when no model is supplied.
- `AddKeyedSystemOneClient()` is used when the System One service key must differ from the
  `OpenAIClient` service key.
- `GetSystemOneClient()` reuses the OpenAI client's pipeline and endpoint.
- Direct construction with `SystemOneClient` accepts a model, endpoint, optional bearer token,
  and optional `HttpClient`.
- `SystemOneOptions` is the extension point for `KeepAlive`, images, and provider-specific
  properties.
- Do not expose API keys in source code, README examples, package metadata, or committed
  configuration files.

## Packaging & Deployment

The project currently defines the following package metadata in
`src/OpenAI.SystemOne/OpenAI.SystemOne.csproj`:

- Package ID: `OpenAI.SystemOne`
- Version: `1.0.0` (the version on the current branch is the release version until it is bumped)
- License: MIT
- Package README: repository `README.md`
- Package icon: `src/OpenAI.SystemOne/icon.png`
- Repository: `https://github.com/ruisantos78/openai-systemone`

Before publishing a release:

1. Review the working tree and confirm that only intended changes are present:
   ```bash
   git status --short
   git diff --check
   ```
2. Validate the package by building and packing in Release mode:
   ```bash
   dotnet build OpenAI.SystemOne.slnx -c Release
   dotnet pack src/OpenAI.SystemOne/OpenAI.SystemOne.csproj -c Release
   ```
3. Inspect the generated `.nupkg` and `.snupkg` under
   `src/OpenAI.SystemOne/bin/Release/` and confirm that the README, icon, XML documentation,
   and both target frameworks are present.
4. Publish only after configuring credentials for the intended NuGet source. For nuget.org:
   ```bash
   dotnet nuget push src/OpenAI.SystemOne/bin/Release/OpenAI.SystemOne.<VERSION>.nupkg \
     --api-key <NUGET_API_KEY> \
     --source https://api.nuget.org/v3/index.json \
     --skip-duplicate
   ```
5. Create and push the release tag after the package has been validated:
   ```bash
   git tag v<VERSION> -m "Release v<VERSION>"
   git push origin main --tags
   ```
6. After a release, bump `<Version>` in `src/OpenAI.SystemOne/OpenAI.SystemOne.csproj` to the
   next unreleased version and commit that change separately.

There is no checked-in GitHub Actions publishing workflow at present. Do not assume that
pushing a tag publishes the package automatically. If automated deployment is introduced,
document its trigger, registry, required secret names, and artifact validation here.

## Release Versioning Rules

- The `<Version>` in the project file is the version to package and publish.
- Do not publish the same version twice unless the registry explicitly supports and requires
  replacement; prefer incrementing the version.
- Update the README and package metadata before creating the release artifact.
- Keep the release tag (`v<VERSION>`) and NuGet package version (`<VERSION>`) aligned.
- Bump the project version only after the release artifact has been built, validated, and
  published or intentionally handed off for publication.

## Safe Deployment Notes

- Never commit NuGet API keys, OpenAI API keys, connection strings containing secrets, or local
  credentials.
- Use environment variables or the local NuGet credential provider for authentication.
- Confirm the package source before running `dotnet nuget push`.
- Treat a successful local build as insufficient evidence that deployment succeeded; verify the
  package is visible in the target registry and that its metadata and README render correctly.
