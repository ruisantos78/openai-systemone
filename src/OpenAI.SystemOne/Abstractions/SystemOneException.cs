namespace OpenAI.SystemOne;

/// <summary>Represents an error returned by a System One endpoint or a failure while invoking it.</summary>
public class SystemOneException : Exception
{
    public SystemOneException()
    {
    }

    public SystemOneException(string message)
        : base(message)
    {
    }

    public SystemOneException(string message, Exception? innerException)
        : base(message, innerException)
    {
    }

    public SystemOneException(int statusCode, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        StatusCode = statusCode;
    }

    public int? StatusCode { get; }
}
