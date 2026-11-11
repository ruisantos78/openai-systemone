namespace OpenAI.SystemOne;

/// <summary>Represents an error returned by a System One endpoint or a failure while invoking it.</summary>
public class SystemOneException : Exception
{
    /// <summary>Initializes a new instance of the <see cref="SystemOneException"/> class.</summary>
    public SystemOneException()
    {
    }

    /// <summary>Initializes a new instance of the <see cref="SystemOneException"/> class with a specified error message.</summary>
    /// <param name="message">The message that describes the error.</param>
    public SystemOneException(string message)
        : base(message)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="SystemOneException"/> class with a specified error message and inner exception.</summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public SystemOneException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }

    /// <summary>Initializes a new instance of the <see cref="SystemOneException"/> class with an HTTP status code, error message, and optional inner exception.</summary>
    /// <param name="statusCode">The HTTP status code associated with the error.</param>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that caused the current exception.</param>
    public SystemOneException(int statusCode, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    /// <summary>Gets the HTTP status code associated with the error, if available.</summary>
    public int? StatusCode { get; }
}
