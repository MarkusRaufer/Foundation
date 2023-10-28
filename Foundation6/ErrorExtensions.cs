namespace Foundation;

public static class ErrorExtensions
{
    public static IEnumerable<Error> Flatten(this Error error)
    {
        if (error is null) yield break;

        yield return error;

        if (null == error.InnerErrors) yield break;

        foreach (var innerError in error.InnerErrors)
        {
            foreach (var err in Flatten(innerError))
                yield return err;
        }
    }

    public static IEnumerable<string> FlattenMessages(this Error error)
    {
        foreach (var err in error.Flatten())
        {
            yield return $"{err.Id}: {err.Message}";
        }
    }
}
