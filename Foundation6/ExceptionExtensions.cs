namespace Foundation;

using Foundation.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

public static class ExceptionExtensions
{
    /// <summary>
    /// Returns the exception and all inner exceptions.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static IEnumerable<Exception> Flatten(this Exception? e)
    {
        if (null == e) yield break;

        yield return e;
        foreach (var ex in Flatten(e.InnerException))
        {
            yield return ex;
        }
    }

    /// <summary>
    /// Returns the exception message and all inner exception messages.
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="separator">The separator for each exception message.</param>
    /// <returns></returns>
    public static string FlattenedMessages(this Exception? exception, string separator = ", \n")
    {
        return Flatten(exception).Select(e => e.Message)
                                 .ToReadableString(separator);
    }
}

