using System.Text.Json.Nodes;
using TUnit.Mocks.Http;

namespace OpenAI.SystemOne.Test.Client;

public class SystemOneChoiceQuestionTests: IAsyncDisposable
{
    private readonly MockHttpClient _httpClient;
    private readonly SystemOneClient _systemOneClient;

    public SystemOneChoiceQuestionTests()
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
    public async Task DecideAsync_WithChoiceQuestion_ReturnsExpectedAnswer()
    {
        // Arrange
        _httpClient.Handler.OnPost("/v1/systemone").RespondWithJson("""
        {
          "model": "nimble",
          "answers": {
            "label": {
              "type": "choice",
              "choice": "bug",
              "probabilities": {
                "billing": 0.0125,
                "bug": 0.9781,
                "account": 0.0093
              },
              "confidence": 0.8906
            }
          },
          "usage": {
            "input_tokens": 174,
            "output_tokens": 1
          }
        }
        """);
        
        const string state = "Our checkout has returned 500 errors since 9am.";

        var questions = new Dictionary<string, SystemOneQuestion>
        {
            ["label"] = new SystemOneChoiceQuestion
            {
                Instructions = "Which label fits this ticket?",
                Criteria = new Dictionary<string, string?>
                {
                    ["billing"] = "Payments and refunds",
                    ["bug"] = "Software errors",
                    ["account"] = "Login and account access"
                }
            }
        };
        
        // Act
        var response = await _systemOneClient.DecideAsync(state, questions);

        // Assert
        await Assert.That(response.Model).IsEqualTo("nimble");

        var answer = (SystemOneChoiceAnswer?)await Assert.That(response.Answers["label"]).IsAssignableFrom<SystemOneChoiceAnswer>();
        await Assert.That(answer!.Choice).IsEqualTo("bug");
        await Assert.That(answer.Confidence).IsEqualTo(0.8906);
        await Assert.That(answer.Probabilities["bug"]).IsEqualTo(0.9781);
        await Assert.That(answer.Probabilities["billing"]).IsEqualTo(0.0125);
        await Assert.That(answer.Probabilities["account"]).IsEqualTo(0.0093);
        
        await Assert.That(response.Usage.InputTokens).IsEqualTo(174);
        await Assert.That(response.Usage.OutputTokens).IsEqualTo(1);
        
        await Assert.That(JsonNode.Parse(_httpClient.Handler.Requests[0].Body!)).IsDeepEqualTo(JsonNode.Parse("""
            {
              "model": "nimble",
              "state": "Our checkout has returned 500 errors since 9am.",
              "questions": {
                "label": {
                  "type": "choice",
                  "instructions": "Which label fits this ticket?",
                  "criteria": {
                    "billing": "Payments and refunds",
                    "bug": "Software errors",
                    "account": "Login and account access"
                  }
                }
              }
            }
            """));
    }
}
