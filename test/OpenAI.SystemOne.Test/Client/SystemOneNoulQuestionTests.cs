using System.Text.Json.Nodes;
using TUnit.Mocks.Http;

namespace OpenAI.SystemOne.Test.Client;

public class SystemOneNoulQuestionTests : IAsyncDisposable
{
    private readonly MockHttpClient _httpClient;
    private readonly SystemOneClient _systemOneClient;

    public SystemOneNoulQuestionTests()
    {
        _httpClient = Mock.HttpClient();
        _systemOneClient = new SystemOneClient("nimble", new Uri("http://localhost:11434/"), httpClient: _httpClient);
    }

    public ValueTask DisposeAsync()
    {
        _systemOneClient.Dispose();

        GC.SuppressFinalize(this);
        return ValueTask.CompletedTask;
    }

    [Test]
    public async Task DecideAsync_WithNoulQuestionWithoutCriteria_ReturnsExpectedAnswer()
    {
        // Arrange
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson("""
        {
          "model": "nimble",
          "answers": {
            "urgent": {
              "type": "noul",
              "noul": 0.959
            }
          },
          "usage": {
            "input_tokens": 115,
            "output_tokens": 1
          }
        }
        """);

        const string state = "Our checkout has returned 500 errors since 9am.";

        var questions = new Dictionary<string, SystemOneQuestion>
        {
            ["urgent"] = new SystemOneNoulQuestion
            {
                Instructions = "Does this ticket need attention within the hour?"
            }
        };

        // Act
        var response = await _systemOneClient.DecideAsync(state, questions);

        // Assert
        await Assert.That(response.Model).IsEqualTo("nimble");

        var answer = (SystemOneNoulAnswer?)await Assert.That(response.Answers["urgent"]).IsAssignableFrom<SystemOneNoulAnswer>();
        await Assert.That(answer!.Type).IsEqualTo("noul");
        await Assert.That(answer.Noul).IsEqualTo(0.959);

        await Assert.That(response.Usage.InputTokens).IsEqualTo(115);
        await Assert.That(response.Usage.OutputTokens).IsEqualTo(1);

        await Assert.That(JsonNode.Parse(_httpClient.Handler.Requests[0].Body!)).IsDeepEqualTo(JsonNode.Parse("""
            {
              "model": "nimble",
              "state": "Our checkout has returned 500 errors since 9am.",
              "questions": {
                "urgent": {
                  "type": "noul",
                  "instructions": "Does this ticket need attention within the hour?"
                }
              }
            }
            """));
    }

    [Test]
    public async Task DecideAsync_WithNoulQuestionWithCriteria_ReturnsExpectedAnswer()
    {
        // Arrange
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson("""
        {
          "model": "nimble",
          "answers": {
            "refund": {
              "type": "noul",
              "noul": 0.9989
            }
          },
          "usage": {
            "input_tokens": 150,
            "output_tokens": 1
          }
        }
        """);

        const string state = "I was charged twice. Please refund the extra payment.";

        var questions = new Dictionary<string, SystemOneQuestion>
        {
            ["refund"] = new SystemOneNoulQuestion
            {
                Instructions = "Is the customer requesting a refund?",
                Criteria = new SystemOneNoulCriteria
                {
                    No = "No refund is requested",
                    Yes = "The customer requests a refund"
                }
            }
        };

        // Act
        var response = await _systemOneClient.DecideAsync(state, questions);

        // Assert
        await Assert.That(response.Model).IsEqualTo("nimble");

        var answer = (SystemOneNoulAnswer?)await Assert.That(response.Answers["refund"]).IsAssignableFrom<SystemOneNoulAnswer>();
        await Assert.That(answer!.Type).IsEqualTo("noul");
        await Assert.That(answer.Noul).IsEqualTo(0.9989);

        await Assert.That(response.Usage.InputTokens).IsEqualTo(150);
        await Assert.That(response.Usage.OutputTokens).IsEqualTo(1);

        await Assert.That(JsonNode.Parse(_httpClient.Handler.Requests[0].Body!)).IsDeepEqualTo(JsonNode.Parse("""
            {
              "model": "nimble",
              "state": "I was charged twice. Please refund the extra payment.",
              "questions": {
                "refund": {
                  "type": "noul",
                  "instructions": "Is the customer requesting a refund?",
                  "criteria": {
                    "false": "No refund is requested",
                    "true": "The customer requests a refund"
                  }
                }
              }
            }
            """));
    }

    [Test]
    public async Task DecideAsync_WithNoulQuestionAndImages_ReturnsExpectedAnswer()
    {
        // Arrange
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson("""
        {
          "model": "clef-flash",
          "answers": {
            "has_ollama": {
              "type": "noul",
              "noul": 0.975
            }
          },
          "usage": {
            "input_tokens": 677,
            "output_tokens": 0
          }
        }
        """);

        const string state = "A user took this screenshot and wants to know what it shows.";

        var options = new SystemOneOptions
        {
            Images = ["iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg=="]
        };

        var questions = new Dictionary<string, SystemOneQuestion>
        {
            ["has_ollama"] = new SystemOneNoulQuestion
            {
                Instructions = "Does this image contain Ollama?"
            }
        };

        // Act
        var response = await _systemOneClient.DecideAsync(state, questions, options);

        // Assert
        await Assert.That(response.Model).IsEqualTo("clef-flash");

        var answer = (SystemOneNoulAnswer?)await Assert.That(response.Answers["has_ollama"]).IsAssignableFrom<SystemOneNoulAnswer>();
        await Assert.That(answer!.Type).IsEqualTo("noul");
        await Assert.That(answer.Noul).IsEqualTo(0.975);

        await Assert.That(response.Usage.InputTokens).IsEqualTo(677);
        await Assert.That(response.Usage.OutputTokens).IsEqualTo(0);

        await Assert.That(JsonNode.Parse(_httpClient.Handler.Requests[0].Body!)).IsDeepEqualTo(JsonNode.Parse("""
            {
              "model": "nimble",
              "state": "A user took this screenshot and wants to know what it shows.",
              "questions": {
                "has_ollama": {
                  "type": "noul",
                  "instructions": "Does this image contain Ollama?"
                }
              },
              "images": [
                "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg=="
              ]
            }
            """));
    }
}
