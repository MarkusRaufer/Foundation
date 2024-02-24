namespace Foundation.Collections.Generic;

public static class EnumeratorExtensions
{
    /// <summary>
    /// Creates an enumerable from an enumerator.
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    /// <param name="enumerator"></param>
    /// <returns>IEnumerable<typeparamref name="T"/></returns>
    public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> enumerator)
    {
        ArgumentNullException.ThrowIfNull(enumerator);

        while(enumerator.MoveNext())
        {
            yield return enumerator.Current;
        }
    }
}
