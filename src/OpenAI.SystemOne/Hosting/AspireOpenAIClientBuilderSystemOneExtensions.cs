using System.Text.RegularExpressions;
using Aspire.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using OpenAI.SystemOne;

namespace Microsoft.Extensions.Hosting;

public static class AspireOpenAIClientBuilderSystemOneExtensions
{
    public static AspireOpenAIClientBuilder AddSystemOneClient(
        this AspireOpenAIClientBuilder builder,
        string? model = null)
    {
        ArgumentNullException.ThrowIfNull(builder);

        if (builder.ServiceKey is not null)
        {
            builder.HostBuilder.Services.AddKeyedSingleton<ISystemOneClient>(builder.ServiceKey, (sp, _) =>
            {
                var openAiClient = sp.GetRequiredKeyedService<OpenAIClient>(builder.ServiceKey);
                var resolvedModel = model ?? GetModelFromConnectionString(builder);
                return openAiClient.GetSystemOneClient(resolvedModel);
            });

            builder.HostBuilder.Services.AddSingleton<ISystemOneClient>(sp =>
                sp.GetRequiredKeyedService<ISystemOneClient>(builder.ServiceKey));
        }
        else
        {
            builder.HostBuilder.Services.AddSingleton<ISystemOneClient>(sp =>
            {
                var openAiClient = sp.GetRequiredService<OpenAIClient>();
                var resolvedModel = model ?? GetModelFromConnectionString(builder);
                return openAiClient.GetSystemOneClient(resolvedModel);
            });
        }

        return builder;
    }

    public static AspireOpenAIClientBuilder AddKeyedSystemOneClient(
        this AspireOpenAIClientBuilder builder,
        string serviceKey,
        string? model = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentException.ThrowIfNullOrWhiteSpace(serviceKey);

        builder.HostBuilder.Services.AddKeyedSingleton<ISystemOneClient>(serviceKey, (sp, _) =>
        {
            var openAiClient = builder.ServiceKey is not null
                ? sp.GetRequiredKeyedService<OpenAIClient>(builder.ServiceKey)
                : sp.GetRequiredService<OpenAIClient>();

            var resolvedModel = model ?? GetModelFromConnectionString(builder);
            return openAiClient.GetSystemOneClient(resolvedModel);
        });

        builder.HostBuilder.Services.AddSingleton<ISystemOneClient>(sp =>
            sp.GetRequiredKeyedService<ISystemOneClient>(serviceKey));

        return builder;
    }

    private static string GetModelFromConnectionString(AspireOpenAIClientBuilder builder)
    {
        var connectionString = builder.HostBuilder.Configuration.GetConnectionString(builder.ConnectionName);
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            var match = Regex.Match(connectionString, @"(?:Model|Deployment)=([^;]+)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups[1].Value.Trim();
            }
        }

        throw new InvalidOperationException($"A System One model name is required for connection '{builder.ConnectionName}'.");
    }
}
