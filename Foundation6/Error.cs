namespace Foundation;


public sealed record Error(string Id, string Message, Error[]? InnerErrors = null)
{
    public static Error FromType<T>(T error, Func<T, string> selector)
    {
        var id = error.ThrowIfNull().GetType().Name;

        return new Error(id, selector(error));
    }

    public static Error FromException(Exception exception)
    {
        var id = exception.GetType().Name;

        var innerErrors = exception.Flatten()
                                   .Select(x => FromException(x))
                                   .ToArray();

        return new Error(id, exception.Message, innerErrors);
    }
}
