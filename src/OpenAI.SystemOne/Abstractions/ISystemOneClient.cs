namespace OpenAI.SystemOne;

/// <summary>
/// Provides an abstraction for a client that answers choice, yes/no (noul), and scoring questions
/// about a shared state using a System One endpoint.
/// </summary>
public interface ISystemOneClient : IDisposable
{
    /// <summary>Gets the model name configured for this client.</summary>
    string Model { get; }

    /// <summary>Answers the given questions about the given state.</summary>
    /// <param name="state">The shared state that all questions are asked about (text, object, or array).</param>
    /// <param name="questions">The named questions about the shared state.</param>
    /// <param name="options">Optional parameters for generation.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task containing the <see cref="SystemOneResponse"/>.</returns>
    Task<SystemOneResponse> DecideAsync(
        SystemOneState state,
        IDictionary<string, SystemOneQuestion> questions,
        SystemOneOptions? options = null,
        CancellationToken cancellationToken = default);
}
