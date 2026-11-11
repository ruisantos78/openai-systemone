namespace OpenAI.SystemOne;

/// <summary>
/// Specifies how long to keep the model loaded after a request, as a duration string (such as <c>5m</c>) or seconds.
/// </summary>
public union SystemOneKeepAlive(string, int)
{
    /// <summary>Creates a <see cref="SystemOneKeepAlive"/> from a duration string.</summary>
    public static implicit operator SystemOneKeepAlive(string duration) => new(duration);

    /// <summary>Creates a <see cref="SystemOneKeepAlive"/> from seconds.</summary>
    public static implicit operator SystemOneKeepAlive(int seconds) => new(seconds);

    /// <summary>Creates a <see cref="SystemOneKeepAlive"/> from a <see cref="TimeSpan"/>.</summary>
    public static implicit operator SystemOneKeepAlive(TimeSpan duration) => (int)duration.TotalSeconds;

    /// <summary>Creates a <see cref="SystemOneKeepAlive"/> from seconds represented as a double.</summary>
    public static implicit operator SystemOneKeepAlive(double seconds) => (int)seconds;
}