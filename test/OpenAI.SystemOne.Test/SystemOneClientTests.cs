using System.Net;
using System.Text.Json.Nodes;
using TUnit.Mocks.Http;

namespace OpenAI.SystemOne.Test;

public class SystemOneClientTests : IAsyncDisposable
{
    private const string MinimalResponseJson = """
    {
      "model": "nimble",
      "answers": {
        "urgent": {
          "type": "noul",
          "noul": 0.9
        }
      },
      "usage": {
        "input_tokens": 1,
        "output_tokens": 1
      }
    }
    """;

    private readonly MockHttpClient _httpClient;

    public SystemOneClientTests()
    {
        _httpClient = Mock.HttpClient();
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    private SystemOneClient CreateClient(Uri? endpoint = null, string? apiKey = null, string model = "nimble") =>
        new(model, endpoint ?? new Uri("http://localhost:11434/"), apiKey: apiKey, httpClient: _httpClient);

    private static Dictionary<string, SystemOneQuestion> NoulQuestions(
        string name = "urgent",
        string instructions = "Does this need attention?") =>
        new()
        {
            [name] = new SystemOneNoulQuestion { Instructions = instructions }
        };

    // Constructor validation

    [Test]
    public async Task Constructor_WithNullModel_ThrowsArgumentNullException()
    {
        await Assert.That(() => new SystemOneClient(null!, new Uri("http://localhost:11434/")))
            .Throws<ArgumentNullException>();
    }

    [Test]
    public async Task Constructor_WithEmptyModel_ThrowsArgumentException()
    {
        await Assert.That(() => new SystemOneClient(string.Empty, new Uri("http://localhost:11434/")))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task Constructor_WithWhitespaceModel_ThrowsArgumentException()
    {
        await Assert.That(() => new SystemOneClient("   ", new Uri("http://localhost:11434/")))
            .Throws<ArgumentException>();
    }

    [Test]
    public async Task Constructor_WithNullEndpoint_ThrowsArgumentNullException()
    {
        await Assert.That(() => new SystemOneClient("nimble", null!))
            .Throws<ArgumentNullException>();
    }

    // DecideAsync validation

    [Test]
    public async Task DecideAsync_WithNullQuestions_ThrowsArgumentNullException()
    {
        var client = CreateClient();

        await Assert.That(async () => await client.DecideAsync("state", null!))
            .Throws<ArgumentNullException>();
    }

    // Endpoint resolution

    [Test]
    public async Task DecideAsync_SendsPostToSystemOnePath()
    {
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson(MinimalResponseJson);
        var client = CreateClient();

        await client.DecideAsync("state", NoulQuestions());

        var request = _httpClient.Handler.Requests[0];
        await Assert.That(request.Method).IsEqualTo(HttpMethod.Post);
        await Assert.That(request.RequestUri).IsEqualTo(new Uri("http://localhost:11434/v1/systemone"));
        await Assert.That(request.Headers["Content-Type"]!.Single()).IsEqualTo("application/json");
    }

    [Test]
    public async Task DecideAsync_WithEndpointAlreadyEndingInSystemOnePath_ReusesEndpointAsIs()
    {
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson(MinimalResponseJson);
        var client = CreateClient(endpoint: new Uri("http://localhost:11434/v1/systemone"));

        await client.DecideAsync("state", NoulQuestions());

        await Assert.That(_httpClient.Handler.Requests[0].RequestUri)
            .IsEqualTo(new Uri("http://localhost:11434/v1/systemone"));
    }

    [Test]
    public async Task DecideAsync_WithEndpointEndingInV1_NormalizesToSystemOnePath()
    {
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson(MinimalResponseJson);
        var client = CreateClient(endpoint: new Uri("http://localhost:11434/v1"));

        await client.DecideAsync("state", NoulQuestions());

        await Assert.That(_httpClient.Handler.Requests[0].RequestUri)
            .IsEqualTo(new Uri("http://localhost:11434/v1/systemone"));
    }

    [Test]
    public async Task DecideAsync_WithEndpointSubPath_AppendsSystemOnePath()
    {
        _httpClient.Handler.OnPost("/api/v1/systemone").RespondWithJson(MinimalResponseJson);
        var client = CreateClient(endpoint: new Uri("http://localhost:11434/api/"));

        await client.DecideAsync("state", NoulQuestions());

        await Assert.That(_httpClient.Handler.Requests[0].RequestUri)
            .IsEqualTo(new Uri("http://localhost:11434/api/v1/systemone"));
    }

    // Authentication

    [Test]
    public async Task DecideAsync_WithApiKey_SendsBearerAuthorization()
    {
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson(MinimalResponseJson);
        var client = CreateClient(apiKey: "test-key");

        await client.DecideAsync("state", NoulQuestions());

        var request = _httpClient.Handler.Requests[0];
        await Assert.That(request.Headers["Authorization"]!.Single()).IsEqualTo("Bearer test-key");
    }

    [Test]
    public async Task DecideAsync_WithoutApiKey_DoesNotSendAuthorization()
    {
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson(MinimalResponseJson);
        var client = CreateClient();

        await client.DecideAsync("state", NoulQuestions());

        await Assert.That(_httpClient.Handler.Requests[0].Headers.ContainsKey("Authorization")).IsFalse();
    }

    // Error handling

    [Test]
    public async Task DecideAsync_WithHttp400_CarriesStatusCodeAndResponseBody()
    {
        _httpClient.Handler.OnPost("/v1/systemone")
            .RespondWithJson("""{"error": "invalid question"}""", HttpStatusCode.BadRequest);
        var client = CreateClient();

        SystemOneException? exception = null;
        try
        {
            await client.DecideAsync("state", NoulQuestions());
        }
        catch (SystemOneException ex)
        {
            exception = ex;
        }

        await Assert.That(exception).IsNotNull();
        await Assert.That(exception!.StatusCode).IsEqualTo(400);
        await Assert.That(exception.Message).Contains("invalid question");
    }

    [Test]
    public async Task DecideAsync_WithHttp500_CarriesStatusCode()
    {
        _httpClient.Handler.OnPost("/v1/systemone")
            .RespondWithJson("""{"error": "internal failure"}""", HttpStatusCode.InternalServerError);
        var client = CreateClient();

        SystemOneException? exception = null;
        try
        {
            await client.DecideAsync("state", NoulQuestions());
        }
        catch (SystemOneException ex)
        {
            exception = ex;
        }

        await Assert.That(exception).IsNotNull();
        await Assert.That(exception!.StatusCode).IsEqualTo(500);
    }

    [Test]
    public async Task DecideAsync_WithEmptyBody_ThrowsSystemOneException()
    {
        _httpClient.Handler.OnPost("/v1/systemone").Respond(HttpStatusCode.OK);
        var client = CreateClient();

        await Assert.That(async () => await client.DecideAsync("state", NoulQuestions()))
            .Throws<SystemOneException>();
    }

    // KeepAlive serialization

    [Test]
    public async Task DecideAsync_WithStringKeepAlive_SendsKeepAliveDuration()
    {
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson(MinimalResponseJson);
        var client = CreateClient();

        await client.DecideAsync("state", NoulQuestions(), new SystemOneOptions { KeepAlive = "10m" });

        await Assert.That(JsonNode.Parse(_httpClient.Handler.Requests[0].Body!)).IsDeepEqualTo(JsonNode.Parse("""
            {
              "model": "nimble",
              "state": "state",
              "questions": {
                "urgent": {
                  "type": "noul",
                  "instructions": "Does this need attention?"
                }
              },
              "keep_alive": "10m"
            }
            """));
    }

    [Test]
    public async Task DecideAsync_WithSecondsKeepAlive_SendsKeepAliveSeconds()
    {
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson(MinimalResponseJson);
        var client = CreateClient();

        await client.DecideAsync("state", NoulQuestions(), new SystemOneOptions { KeepAlive = 600 });

        await Assert.That(JsonNode.Parse(_httpClient.Handler.Requests[0].Body!)).IsDeepEqualTo(JsonNode.Parse("""
            {
              "model": "nimble",
              "state": "state",
              "questions": {
                "urgent": {
                  "type": "noul",
                  "instructions": "Does this need attention?"
                }
              },
              "keep_alive": 600
            }
            """));
    }

    [Test]
    public async Task DecideAsync_WithTimeSpanKeepAlive_SendsKeepAliveSeconds()
    {
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson(MinimalResponseJson);
        var client = CreateClient();

        await client.DecideAsync("state", NoulQuestions(), new SystemOneOptions { KeepAlive = TimeSpan.FromMinutes(5) });

        await Assert.That(JsonNode.Parse(_httpClient.Handler.Requests[0].Body!)).IsDeepEqualTo(JsonNode.Parse("""
            {
              "model": "nimble",
              "state": "state",
              "questions": {
                "urgent": {
                  "type": "noul",
                  "instructions": "Does this need attention?"
                }
              },
              "keep_alive": 300
            }
            """));
    }

    // State serialization

    [Test]
    public async Task DecideAsync_WithJsonArrayState_SendsArrayState()
    {
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson(MinimalResponseJson);
        var client = CreateClient();

        await client.DecideAsync(new JsonArray("alpha", "beta"), NoulQuestions());

        await Assert.That(JsonNode.Parse(_httpClient.Handler.Requests[0].Body!)).IsDeepEqualTo(JsonNode.Parse("""
            {
              "model": "nimble",
              "state": ["alpha", "beta"],
              "questions": {
                "urgent": {
                  "type": "noul",
                  "instructions": "Does this need attention?"
                }
              }
            }
            """));
    }

    [Test]
    public async Task DecideAsync_WithStringArrayState_SendsArrayState()
    {
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson(MinimalResponseJson);
        var client = CreateClient();

        await client.DecideAsync(new[] { "alpha", "beta" }, NoulQuestions());

        await Assert.That(JsonNode.Parse(_httpClient.Handler.Requests[0].Body!)).IsDeepEqualTo(JsonNode.Parse("""
            {
              "model": "nimble",
              "state": ["alpha", "beta"],
              "questions": {
                "urgent": {
                  "type": "noul",
                  "instructions": "Does this need attention?"
                }
              }
            }
            """));
    }

    [Test]
    public async Task DecideAsync_WithFromObjectState_SendsObjectState()
    {
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson(MinimalResponseJson);
        var client = CreateClient();

        await client.DecideAsync(SystemOneState.FromObject(new { ticket = "T-1", priority = 2 }), NoulQuestions());

        await Assert.That(JsonNode.Parse(_httpClient.Handler.Requests[0].Body!)).IsDeepEqualTo(JsonNode.Parse("""
            {
              "model": "nimble",
              "state": {
                "ticket": "T-1",
                "priority": 2
              },
              "questions": {
                "urgent": {
                  "type": "noul",
                  "instructions": "Does this need attention?"
                }
              }
            }
            """));
    }

    // Instructions serialization

    [Test]
    public async Task DecideAsync_WithJsonObjectInstructions_SendsObjectInstructions()
    {
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson(MinimalResponseJson);
        var client = CreateClient();

        var questions = new Dictionary<string, SystemOneQuestion>
        {
            ["urgent"] = new SystemOneNoulQuestion
            {
                Instructions = new JsonObject { ["text"] = "Does this need attention?" }
            }
        };

        await client.DecideAsync("state", questions);

        await Assert.That(JsonNode.Parse(_httpClient.Handler.Requests[0].Body!)).IsDeepEqualTo(JsonNode.Parse("""
            {
              "model": "nimble",
              "state": "state",
              "questions": {
                "urgent": {
                  "type": "noul",
                  "instructions": {
                    "text": "Does this need attention?"
                  }
                }
              }
            }
            """));
    }

    // AdditionalProperties

    [Test]
    public async Task DecideAsync_WithAdditionalProperties_MergesIntoRequestBody()
    {
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson(MinimalResponseJson);
        var client = CreateClient();

        var options = new SystemOneOptions
        {
            AdditionalProperties = new Dictionary<string, object?>
            {
                ["temperature"] = 0.2
            }
        };

        await client.DecideAsync("state", NoulQuestions(), options);

        await Assert.That(JsonNode.Parse(_httpClient.Handler.Requests[0].Body!)).IsDeepEqualTo(JsonNode.Parse("""
            {
              "model": "nimble",
              "state": "state",
              "questions": {
                "urgent": {
                  "type": "noul",
                  "instructions": "Does this need attention?"
                }
              },
              "temperature": 0.2
            }
            """));
    }

    // Cancellation

    [Test]
    public async Task DecideAsync_WithCancelledToken_ThrowsOperationCanceledException()
    {
        var client = CreateClient();
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        await Assert.That(async () => await client.DecideAsync("state", NoulQuestions(), cancellationToken: cts.Token))
            .Throws<OperationCanceledException>();
    }
}
