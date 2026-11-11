using System.Text.Json.Nodes;
using TUnit.Mocks.Http;

namespace OpenAI.SystemOne.Test.Client;

public class SystemOneScoreQuestionTests : IAsyncDisposable
{
    private readonly MockHttpClient _httpClient;
    private readonly SystemOneClient _systemOneClient;

    public SystemOneScoreQuestionTests()
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
    public async Task DecideAsync_WithScoreQuestion_ReturnsExpectedAnswer()
    {
        // Arrange
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson("""
        {
          "model": "nimble",
          "answers": {
            "urgency": {
              "type": "score",
              "score": 0.8308,
              "legend": {
                "0": "Routine: no time pressure",
                "1": "Soon: a customer is inconvenienced",
                "2": "Immediate: a critical service is unavailable"
              },
              "probabilities": {
                "0": 0.2014,
                "1": 0.7664,
                "2": 0.0322
              },
              "confidence": 0.6582
            }
          },
          "usage": {
            "input_tokens": 120,
            "output_tokens": 1
          }
        }
        """);

        const string state = "Our checkout has returned 500 errors since 9am.";

        var questions = new Dictionary<string, SystemOneQuestion>
        {
            ["urgency"] = new SystemOneScoreQuestion
            {
                Instructions = "How urgently does this ticket need a response?",
                Criteria =
                [
                    "Routine: no time pressure",
                    "Soon: a customer is inconvenienced",
                    "Immediate: a critical service is unavailable"
                ]
            }
        };

        // Act
        var response = await _systemOneClient.DecideAsync(state, questions);

        // Assert
        await Assert.That(response.Model).IsEqualTo("nimble");

        var answer = (SystemOneScoreAnswer?)await Assert.That(response.Answers["urgency"]).IsAssignableFrom<SystemOneScoreAnswer>();
        await Assert.That(answer!.Score).IsEqualTo(0.8308);
        await Assert.That(answer.Confidence).IsEqualTo(0.6582);

        await Assert.That(answer.Legend["0"]).IsEqualTo("Routine: no time pressure");
        await Assert.That(answer.Legend["1"]).IsEqualTo("Soon: a customer is inconvenienced");
        await Assert.That(answer.Legend["2"]).IsEqualTo("Immediate: a critical service is unavailable");

        await Assert.That(answer.Probabilities["0"]).IsEqualTo(0.2014);
        await Assert.That(answer.Probabilities["1"]).IsEqualTo(0.7664);
        await Assert.That(answer.Probabilities["2"]).IsEqualTo(0.0322);

        await Assert.That(response.Usage.InputTokens).IsEqualTo(120);
        await Assert.That(response.Usage.OutputTokens).IsEqualTo(1);

        await Assert.That(JsonNode.Parse(_httpClient.Handler.Requests[0].Body!)).IsDeepEqualTo(JsonNode.Parse("""
            {
              "model": "nimble",
              "state": "Our checkout has returned 500 errors since 9am.",
              "questions": {
                "urgency": {
                  "type": "score",
                  "instructions": "How urgently does this ticket need a response?",
                  "criteria": [
                    "Routine: no time pressure",
                    "Soon: a customer is inconvenienced",
                    "Immediate: a critical service is unavailable"
                  ]
                }
              }
            }
            """));
    }

    [Test]
    public async Task DecideAsync_WithStructuredStateAndKeepAlive_ReturnsExpectedAnswer()
    {
        // Arrange
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson("""
        {
          "model": "nimble",
          "answers": {
            "satisfaction": {
              "type": "score",
              "score": 3.42,
              "legend": {
                "0": "Very Dissatisfied",
                "1": "Dissatisfied",
                "2": "Neutral",
                "3": "Satisfied",
                "4": "Very Satisfied"
              },
              "probabilities": {
                "0": 0.01,
                "1": 0.04,
                "2": 0.08,
                "3": 0.28,
                "4": 0.59
              },
              "confidence": 0.725
            }
          },
          "usage": {
            "input_tokens": 210,
            "output_tokens": 1
          }
        }
        """);

        var state = new JsonObject
        {
            ["review"] = "The product arrived on time and exceeded expectations!",
            ["stars"] = 5
        };

        var options = new SystemOneOptions
        {
            KeepAlive = "10m"
        };

        var questions = new Dictionary<string, SystemOneQuestion>
        {
            ["satisfaction"] = new SystemOneScoreQuestion
            {
                Instructions = "Score the customer satisfaction level based on the review.",
                Criteria =
                [
                    "Very Dissatisfied",
                    "Dissatisfied",
                    "Neutral",
                    "Satisfied",
                    "Very Satisfied"
                ]
            }
        };

        // Act
        var response = await _systemOneClient.DecideAsync(state, questions, options);

        // Assert
        await Assert.That(response.Model).IsEqualTo("nimble");

        var answer = (SystemOneScoreAnswer?)await Assert.That(response.Answers["satisfaction"]).IsAssignableFrom<SystemOneScoreAnswer>();
        await Assert.That(answer!.Score).IsEqualTo(3.42);
        await Assert.That(answer.Confidence).IsEqualTo(0.725);
        await Assert.That(answer.Legend.Count).IsEqualTo(5);
        await Assert.That(answer.Legend["4"]).IsEqualTo("Very Satisfied");
        await Assert.That(answer.Probabilities["4"]).IsEqualTo(0.59);

        await Assert.That(response.Usage.InputTokens).IsEqualTo(210);
        await Assert.That(response.Usage.OutputTokens).IsEqualTo(1);

        await Assert.That(JsonNode.Parse(_httpClient.Handler.Requests[0].Body!)).IsDeepEqualTo(JsonNode.Parse("""
            {
              "model": "nimble",
              "state": {
                "review": "The product arrived on time and exceeded expectations!",
                "stars": 5
              },
              "questions": {
                "satisfaction": {
                  "type": "score",
                  "instructions": "Score the customer satisfaction level based on the review.",
                  "criteria": [
                    "Very Dissatisfied",
                    "Dissatisfied",
                    "Neutral",
                    "Satisfied",
                    "Very Satisfied"
                  ]
                }
              },
              "keep_alive": "10m"
            }
            """));
    }
}
