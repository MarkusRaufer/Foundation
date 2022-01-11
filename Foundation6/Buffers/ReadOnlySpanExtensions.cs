namespace Foundation.Buffers;

public static class ReadOnlySpanExtensions
{
    public static IReadOnlyCollection<int> IndexesOf<T>(this ReadOnlySpan<T> span, T selector)
        where T : IEquatable<T>
    {
        var indices = new List<int>();

        var pos = -1;
        int index;
        while (-1 != (index = span.IndexOf(selector)))
        {
            if (-1 == pos) pos = index;
            else pos += index + 1;

            indices.Add(pos);
            span = span.Slice(index + 1);
        }

        return indices;
    }

    public static IReadOnlyCollection<int> IndexesOf<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> selectors)
        where T : IEquatable<T>
    {
        var indices = new List<int>();

        var pos = -1;
        int index;
        while (-1 != (index = span.IndexOfAny(selectors)))
        {
            if (-1 == pos) pos = index;
            else pos += index + 1;

            indices.Add(pos);
            span = span.Slice(index + 1);
        }

        return indices;
    }

    public static IReadOnlyCollection<int> IndexesOf<T>(this ReadOnlySpan<T> span, params T[] selectors)
        where T : IEquatable<T>
    {
        if (0 == selectors.Length) throw new ArgumentNullException(nameof(selectors));

        return IndexesOf<T>(span, selectors.AsSpan());
    }

    public static IReadOnlyCollection<(int, int)> IndexLengthTuples<T>(this ReadOnlySpan<T> span, T separator)
        where T : IEquatable<T>
    {
        var tuples = new List<(int, int)>();

        var index = 0;
        foreach (var chunk in span.Split(separator))
        {
            tuples.Add((index, chunk.Length));
            index += chunk.Length + 1;
        }

        return tuples;
    }

    public static IReadOnlyCollection<(int, int)> IndexLengthTuples<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> separators)
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

    public static IReadOnlyCollection<(int, int)> IndexLengthTuples<T>(this ReadOnlySpan<T> span, params T[] separators)
            where T : IEquatable<T>
    {
        if (0 == separators.Length) throw new ArgumentNullException(nameof(separators));

        return IndexLengthTuples(span, separators.AsSpan());
    }

    public static SplitEnumerator<T> Split<T>(
       this ReadOnlySpan<T> span,
       ReadOnlySpan<T> separators,
       bool notFoundReturnsNothing = true)
       where T : IEquatable<T>
    {
        return new SplitEnumerator<T>(span, separators, notFoundReturnsNothing);
    }

    public static SplitEnumerator<T> Split<T>(this ReadOnlySpan<T> span, params T[] separators)
        where T : IEquatable<T>
    {
        if (0 == separators.Length) throw new ArgumentNullException(nameof(separators));

        return new SplitEnumerator<T>(span, separators);
    }

    public static SplitEnumerator<T> Split<T>(this ReadOnlySpan<T> span, bool notFoundReturnsNothing, params T[] separators)
        where T : IEquatable<T>
    {
        if (0 == separators.Length) throw new ArgumentNullException(nameof(separators));

        return new SplitEnumerator<T>(span, notFoundReturnsNothing, separators);
    }

    public static CharSplitEnumerator Split(this ReadOnlySpan<char> span, char separator, bool notFoundReturnsNothing = true)
    {
        return new CharSplitEnumerator(span, separator, notFoundReturnsNothing);
    }
}

