using OpenAI.SystemOne;

namespace OpenAI;

/// <summary>Provides extension methods for creating System One clients from OpenAI clients.</summary>
public static class OpenAiClientSystemOneExtensions
{
    /// <summary>Creates a System One client that reuses the pipeline and endpoint of an OpenAI client.</summary>
    /// <param name="openAiClient">The OpenAI client whose pipeline and endpoint are reused.</param>
    /// <param name="model">The model name to use for System One requests.</param>
    /// <returns>A System One client.</returns>
    public static ISystemOneClient GetSystemOneClient(this OpenAIClient openAiClient, string model)
    {
        ArgumentNullException.ThrowIfNull(openAiClient);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        return new SystemOneClient(model, openAiClient.Pipeline, openAiClient.Endpoint);
    }
}
