using OpenAI.SystemOne;

namespace OpenAI;

public static class OpenAIClientSystemOneExtensions
{
    public static ISystemOneClient GetSystemOneClient(this OpenAIClient openAIClient, string model)
    {
        ArgumentNullException.ThrowIfNull(openAIClient);
        ArgumentException.ThrowIfNullOrWhiteSpace(model);

        return new SystemOneClient(model, openAIClient.Pipeline, openAIClient.Endpoint);
    }
}
