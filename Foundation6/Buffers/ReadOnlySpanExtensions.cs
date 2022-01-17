namespace Foundation.Buffers;

public static class ReadOnlySpanExtensions
{
    public static IReadOnlyCollection<int> IndexesOf(
        this ReadOnlySpan<char> span,
        ReadOnlySpan<char> search, 
        StringComparison comparisonType = StringComparison.InvariantCulture,
        int stopAfterNumberOfHits = -1)
    {
        var indices = new List<int>();
        var numberOfHits = 0;
        var pos = -1;
        int index;
        while (-1 != (index = span.IndexOf(search, comparisonType)))
        {
            if (-1 == pos) pos = index;
            else pos += index + search.Length;

            if (-1 < stopAfterNumberOfHits && numberOfHits >= stopAfterNumberOfHits) break;

            indices.Add(pos);
            numberOfHits++;

            span = span.Slice(index + search.Length);
        }

        return indices;
    }

    public static IReadOnlyCollection<int> IndexesOf<T>(this ReadOnlySpan<T> span, T selector, int stopAfterNumberOfHits = -1)
        where T : IEquatable<T>
    {
        var indices = new List<int>();
        var numberOfHits = 0;
        var pos = -1;
        int index;
        while (-1 != (index = span.IndexOf(selector)))
        {
            if (-1 < stopAfterNumberOfHits && numberOfHits >= stopAfterNumberOfHits) break;

            if (-1 == pos) pos = index;
            else pos += index + 1;

            indices.Add(pos);
            numberOfHits++;
            span = span.Slice(index + 1);
        }

        return indices;
    }

    public static IReadOnlyCollection<int> IndexesOfAny<T>(
        this ReadOnlySpan<T> span, 
        ReadOnlySpan<T> selectors, 
        int stopAfterNumberOfHits = -1)
        where T : IEquatable<T>
    {
        var indices = new List<int>();

        var numberOfHits = 0;
        var pos = -1;
        int index;
        while (-1 != (index = span.IndexOfAny(selectors)))
        {
            if (-1 < stopAfterNumberOfHits && numberOfHits >= stopAfterNumberOfHits) break;

            if (-1 == pos) pos = index;
            else pos += index + 1;

            indices.Add(pos);
            numberOfHits++;
            span = span.Slice(index + 1);
        }

        return indices;
    }

    /// <summary>
    /// returns tuples (index, tokenLength).
    /// IReadOnlyCollection is used because IEnumerable is not allowed on ref structures (ReadOnlySpan)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static IReadOnlyCollection<(int, int)> IndexLengthTuples<T>(this ReadOnlySpan<T> span, T separator)
        where T : IEquatable<T>
    {
        var tuples = new List<(int, int)>();
        var sep = new [] { separator }.AsSpan();
        var index = 0;
        foreach (var chunk in span.Split(sep))
        {
            tuples.Add((index, chunk.Length));
            index += chunk.Length + 1;
        }

        return tuples;
    }

    public static IReadOnlyCollection<(int, int)> IndexLengthTuplesAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> separators)
        where T : IEquatable<T>
    {
        var tuples = new List<(int, int)>();

        var index = 0;
        foreach (var chunk in span.Split(separators))
        {
            tuples.Add((index, chunk.Length));
            index += chunk.Length + 1;
        }

        return tuples;
    } 

    /// <summary>
    /// returns a SplitEnumerator which can be iterated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span"></param>
    /// <param name="separators">Any of the separators will split the span.</param>
    /// <param name="notFoundReturnsNothing"></param>
    /// <returns></returns>
    public static SplitEnumerator<T> Split<T>(
       this ReadOnlySpan<T> span,
       ReadOnlySpan<T> separators,
       bool notFoundReturnsNothing = true)
       where T : IEquatable<T>
    {
        return new SplitEnumerator<T>(span, separators, notFoundReturnsNothing);
    }
}

