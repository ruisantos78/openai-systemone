using System.ClientModel;
using Aspire.OpenAI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OpenAI.SystemOne.Test.Hostings;

public class AspireOpenAiClientBuilderSystemOneExtensionsTests
{
    private const string ConnectionName = "systemone";
    private const string BuilderServiceKey = "aspire-openai";
    private const string ClientServiceKey = "systemone-client";

    private static HostApplicationBuilder CreateHost(string? connectionString = null, string? openAiServiceKey = null)
    {
        var hostBuilder = Host.CreateApplicationBuilder();

        if (connectionString is not null)
        {
            hostBuilder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                [$"ConnectionStrings:{ConnectionName}"] = connectionString
            });
        }

        var openAiClient = new OpenAIClient(
            new ApiKeyCredential("test-key"),
            new OpenAIClientOptions
            {
                Endpoint = new Uri("http://localhost:11434/")
            });

        if (openAiServiceKey is null)
        {
            hostBuilder.Services.AddSingleton(openAiClient);
        }
        else
        {
            hostBuilder.Services.AddKeyedSingleton(openAiServiceKey, openAiClient);
        }

        return hostBuilder;
    }

    private static AspireOpenAIClientBuilder CreateBuilder(HostApplicationBuilder hostBuilder, string? serviceKey = null)
        => new(hostBuilder, ConnectionName, serviceKey, disableTracing: true, enableSensitiveTelemetryData: false);

    private static async Task<SystemOneClient> ResolveModelAsync(ISystemOneClient client)
    {
        var systemOneClient = (SystemOneClient?)await Assert.That(client).IsAssignableFrom<SystemOneClient>();
        return systemOneClient!;
    }

    // AddSystemOneClient

    [Test]
    public async Task AddSystemOneClient_WithNullBuilder_ThrowsArgumentNullException()
    {
        AspireOpenAIClientBuilder builder = null!;

        await Assert.That(() => builder.AddSystemOneClient()).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task AddSystemOneClient_WithExplicitModel_ReturnsBuilder()
    {
        var hostBuilder = CreateHost();
        var builder = CreateBuilder(hostBuilder);

        var result = builder.AddSystemOneClient("nimble");

        await Assert.That(result).IsSameReferenceAs(builder);
    }

    [Test]
    public async Task AddSystemOneClient_WithExplicitModel_RegistersClientWithModel()
    {
        var hostBuilder = CreateHost();
        var builder = CreateBuilder(hostBuilder);
        builder.AddSystemOneClient("nimble");

        await using var provider = hostBuilder.Services.BuildServiceProvider();
        var client = provider.GetRequiredService<ISystemOneClient>();

        var systemOneClient = await ResolveModelAsync(client);
        await Assert.That(systemOneClient.Model).IsEqualTo("nimble");
    }

    [Test]
    public async Task AddSystemOneClient_WithoutModel_ReadsModelFromConnectionString()
    {
        var hostBuilder = CreateHost("Endpoint=http://localhost:11434/;Key=test-key;Model=nimble");
        var builder = CreateBuilder(hostBuilder);
        builder.AddSystemOneClient();

        await using var provider = hostBuilder.Services.BuildServiceProvider();
        var client = provider.GetRequiredService<ISystemOneClient>();

        var systemOneClient = await ResolveModelAsync(client);
        await Assert.That(systemOneClient.Model).IsEqualTo("nimble");
    }

    [Test]
    public async Task AddSystemOneClient_WithoutModel_ReadsDeploymentFromConnectionString()
    {
        var hostBuilder = CreateHost("Endpoint=http://localhost:11434/;Key=test-key;Deployment=orca");
        var builder = CreateBuilder(hostBuilder);
        builder.AddSystemOneClient();

        await using var provider = hostBuilder.Services.BuildServiceProvider();
        var client = provider.GetRequiredService<ISystemOneClient>();

        var systemOneClient = await ResolveModelAsync(client);
        await Assert.That(systemOneClient.Model).IsEqualTo("orca");
    }

    [Test]
    public async Task AddSystemOneClient_WithoutModel_TrimsModelFromConnectionString()
    {
        var hostBuilder = CreateHost("Endpoint=http://localhost:11434/;Key=test-key;Model= nimble ");
        var builder = CreateBuilder(hostBuilder);
        builder.AddSystemOneClient();

        await using var provider = hostBuilder.Services.BuildServiceProvider();
        var client = provider.GetRequiredService<ISystemOneClient>();

        var systemOneClient = await ResolveModelAsync(client);
        await Assert.That(systemOneClient.Model).IsEqualTo("nimble");
    }

    [Test]
    public async Task AddSystemOneClient_WithoutModelOrConnectionString_ThrowsInvalidOperationException()
    {
        var hostBuilder = CreateHost();
        var builder = CreateBuilder(hostBuilder);
        builder.AddSystemOneClient();

        async Task Action()
        {
            await using var provider = hostBuilder.Services.BuildServiceProvider();
            provider.GetRequiredService<ISystemOneClient>();
        }

        await Assert.That(Action).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task AddSystemOneClient_WithConnectionStringWithoutModel_ThrowsInvalidOperationException()
    {
        var hostBuilder = CreateHost("Endpoint=http://localhost:11434/;Key=test-key");
        var builder = CreateBuilder(hostBuilder);
        builder.AddSystemOneClient();

        async Task Action()
        {
            await using var provider = hostBuilder.Services.BuildServiceProvider();
            provider.GetRequiredService<ISystemOneClient>();
        }

        await Assert.That(Action).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task AddSystemOneClient_RegistersClientAsSingleton()
    {
        var hostBuilder = CreateHost();
        var builder = CreateBuilder(hostBuilder);
        builder.AddSystemOneClient("nimble");

        await using var provider = hostBuilder.Services.BuildServiceProvider();
        var first = provider.GetRequiredService<ISystemOneClient>();
        var second = provider.GetRequiredService<ISystemOneClient>();

        await Assert.That(first).IsSameReferenceAs(second);
    }

    [Test]
    public async Task AddSystemOneClient_WithBuilderServiceKey_ResolvesKeyedAndNonKeyedClient()
    {
        var hostBuilder = CreateHost(openAiServiceKey: BuilderServiceKey);
        var builder = CreateBuilder(hostBuilder, serviceKey: BuilderServiceKey);
        builder.AddSystemOneClient("nimble");

        await using var provider = hostBuilder.Services.BuildServiceProvider();
        var keyed = provider.GetRequiredKeyedService<ISystemOneClient>(BuilderServiceKey);
        var nonKeyed = provider.GetRequiredService<ISystemOneClient>();

        var systemOneClient = await ResolveModelAsync(keyed);
        await Assert.That(systemOneClient.Model).IsEqualTo("nimble");
        await Assert.That(nonKeyed).IsSameReferenceAs(keyed);
    }

    // AddKeyedSystemOneClient

    [Test]
    public async Task AddKeyedSystemOneClient_WithNullBuilder_ThrowsArgumentNullException()
    {
        AspireOpenAIClientBuilder builder = null!;

        await Assert.That(() => builder.AddKeyedSystemOneClient(ClientServiceKey)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task AddKeyedSystemOneClient_WithNullServiceKey_ThrowsArgumentNullException()
    {
        var hostBuilder = CreateHost();
        var builder = CreateBuilder(hostBuilder);

        await Assert.That(() => builder.AddKeyedSystemOneClient(null!)).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task AddKeyedSystemOneClient_WithEmptyServiceKey_ThrowsArgumentException()
    {
        var hostBuilder = CreateHost();
        var builder = CreateBuilder(hostBuilder);

        await Assert.That(() => builder.AddKeyedSystemOneClient(string.Empty)).Throws<ArgumentException>();
    }

    [Test]
    public async Task AddKeyedSystemOneClient_WithWhitespaceServiceKey_ThrowsArgumentException()
    {
        var hostBuilder = CreateHost();
        var builder = CreateBuilder(hostBuilder);

        await Assert.That(() => builder.AddKeyedSystemOneClient("   ")).Throws<ArgumentException>();
    }

    [Test]
    public async Task AddKeyedSystemOneClient_WithExplicitModel_ReturnsBuilder()
    {
        var hostBuilder = CreateHost();
        var builder = CreateBuilder(hostBuilder);

        var result = builder.AddKeyedSystemOneClient(ClientServiceKey, "nimble");

        await Assert.That(result).IsSameReferenceAs(builder);
    }

    [Test]
    public async Task AddKeyedSystemOneClient_WithExplicitModel_RegistersKeyedAndNonKeyedClient()
    {
        var hostBuilder = CreateHost();
        var builder = CreateBuilder(hostBuilder);
        builder.AddKeyedSystemOneClient(ClientServiceKey, "nimble");

        await using var provider = hostBuilder.Services.BuildServiceProvider();
        var keyed = provider.GetRequiredKeyedService<ISystemOneClient>(ClientServiceKey);
        var nonKeyed = provider.GetRequiredService<ISystemOneClient>();

        var systemOneClient = await ResolveModelAsync(keyed);
        await Assert.That(systemOneClient.Model).IsEqualTo("nimble");
        await Assert.That(nonKeyed).IsSameReferenceAs(keyed);
    }

    [Test]
    public async Task AddKeyedSystemOneClient_WithoutModel_ReadsModelFromConnectionString()
    {
        var hostBuilder = CreateHost("Endpoint=http://localhost:11434/;Key=test-key;Model=nimble");
        var builder = CreateBuilder(hostBuilder);
        builder.AddKeyedSystemOneClient(ClientServiceKey);

        await using var provider = hostBuilder.Services.BuildServiceProvider();
        var keyed = provider.GetRequiredKeyedService<ISystemOneClient>(ClientServiceKey);

        var systemOneClient = await ResolveModelAsync(keyed);
        await Assert.That(systemOneClient.Model).IsEqualTo("nimble");
    }

    [Test]
    public async Task AddKeyedSystemOneClient_WithoutModelOrConnectionString_ThrowsInvalidOperationException()
    {
        var hostBuilder = CreateHost();
        var builder = CreateBuilder(hostBuilder);
        builder.AddKeyedSystemOneClient(ClientServiceKey);

        async Task Action()
        {
            await using var provider = hostBuilder.Services.BuildServiceProvider();
            provider.GetRequiredKeyedService<ISystemOneClient>(ClientServiceKey);
        }
        
        await Assert.That(Action()).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task AddKeyedSystemOneClient_RegistersKeyedClientAsSingleton()
    {
        var hostBuilder = CreateHost();
        var builder = CreateBuilder(hostBuilder);
        builder.AddKeyedSystemOneClient(ClientServiceKey, "nimble");

        await using var provider = hostBuilder.Services.BuildServiceProvider();
        var first = provider.GetRequiredKeyedService<ISystemOneClient>(ClientServiceKey);
        var second = provider.GetRequiredKeyedService<ISystemOneClient>(ClientServiceKey);

        await Assert.That(first).IsSameReferenceAs(second);
    }

    [Test]
    public async Task AddKeyedSystemOneClient_WithBuilderServiceKey_UsesKeyedOpenAIClient()
    {
        var hostBuilder = CreateHost(openAiServiceKey: BuilderServiceKey);
        var builder = CreateBuilder(hostBuilder, serviceKey: BuilderServiceKey);
        builder.AddKeyedSystemOneClient(ClientServiceKey, "nimble");

        await using var provider = hostBuilder.Services.BuildServiceProvider();
        var keyed = provider.GetRequiredKeyedService<ISystemOneClient>(ClientServiceKey);

        var systemOneClient = await ResolveModelAsync(keyed);
        await Assert.That(systemOneClient.Model).IsEqualTo("nimble");
    }
}
