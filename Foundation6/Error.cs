namespace Foundation;

/// <summary>
/// This is a generic Error type. Use this if a method can return different kind of errors like exceptions, error messages...
/// </summary>
/// <param name="Id"></param>
/// <param name="Message"></param>
/// <param name="InnerErrors"></param>
public sealed record Error(string Id, string Message, Error[]? InnerErrors = null)
{
    /// <summary>
    /// Creates an Error instance from an exception.
    /// </summary>
    /// <param name="exception">The exception and all inner exceptions are fetched and created as a list of Errors.</param>
    /// <returns>An Error where the EntityId is the exception type.</returns>
    public static Error FromException(Exception exception)
    {
        var id = exception.GetType().Name;

        var innerErrors = exception.Flatten()
                                   .Select(x => FromException(x))
                                   .ToArray();

        return new Error(id, exception.Message, innerErrors);
    }

    /// <summary>
    /// Creates an Error instance from a specific type.
    /// </summary>
    /// <typeparam name="T">The type of the error.</typeparam>
    /// <param name="error">The error instance.</param>
    /// <param name="selector">A selector for the error message.</param>
    /// <returns>An Error where the EntityId is the error type.</returns>
    public static Error FromType<T>(T error, Func<T, string> selector)
    {
        var id = error.ThrowIfNull().GetType().Name;

        return new Error(id, selector(error));
    }
}
