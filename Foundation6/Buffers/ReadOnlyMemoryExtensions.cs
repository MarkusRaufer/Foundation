namespace Foundation.Buffers;
using System.Diagnostics.CodeAnalysis;

public static class ReadOnlyMemoryExtensions
{
    /// <summary>
    /// returns the indexes of the found selectors in array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="memory"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IEnumerable<int> IndexesOfAny<T>(
        this ReadOnlyMemory<T> memory, 
        [DisallowNull] IEnumerable<T> selectors,
        int stopAfterNumberOfHits = -1)
    {
        var numberOfHits = 0;
        for (var i = 0; i < memory.Length; i++)
        {
            if (-1 < stopAfterNumberOfHits && numberOfHits >= stopAfterNumberOfHits) break;

            if (selectors.Any(selector => selector.EqualsNullable(memory.Span[i])))
            {
                numberOfHits++;
                yield return i;
            }
        }
    }

    public static IEnumerable<int> IndexesOf<T>(this ReadOnlyMemory<T>[] memories, ReadOnlyMemory<T>[] selectors)
        where T : IEquatable<T>
    {
        var i = 0;
        foreach (var memory in memories)
        {
            if (selectors.Any(s => s.SequenceEqual(memory))) yield return i;

            i++;
        }
    }

    /// <summary>
    /// checks if size, values and position are same.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool IsSameAs<T>(this ReadOnlyMemory<T> lhs, ReadOnlyMemory<T> rhs)
    {
        if (lhs.Length != rhs.Length) return false;

        var lhsSpan = lhs.Span;
        var rhsSpan = rhs.Span;

        for (var i = 0; i < lhs.Length; i++)
        {
            var left = lhsSpan[i];
            var right = rhsSpan[i];
            if (null == left)
            {
                if (null != right) return false;
                continue;
            }

            if (!left.Equals(right)) return false;
        }

        return true;
    }

    public static IEnumerable<ReadOnlyMemory<T>> SelectByIndex<T>(
        this ReadOnlyMemory<T>[] memories,
        params int[] indices)
    {
        foreach (var index in indices)
        {
            if (0 <= index && index < memories.Length) yield return memories[index];
        }
    }

    public static bool SequenceEqual<T>(this IEnumerable<ReadOnlyMemory<T>> lhs, [DisallowNull] IEnumerable<ReadOnlyMemory<T>> rhs)
        where T : IEquatable<T>
    {
        var itLhs = lhs.GetEnumerator();
        var itRhs = rhs.GetEnumerator();
        var nextLhs = itLhs.MoveNext();
        if (!nextLhs) return !itRhs.MoveNext();

        while (nextLhs)
        {
            if(!itRhs.MoveNext()) return false;
            if(!itLhs.Current.Equals(itRhs.Current)) return false;

            nextLhs = itLhs.MoveNext();
        }
        if (itRhs.MoveNext()) return false;

        return true;
    }

    public static bool SequenceEqual<T>(this ReadOnlyMemory<T> lhs, ReadOnlyMemory<T> rhs)
        where T : IEquatable<T>
    {
        return lhs.Span.SequenceEqual(rhs.Span);
    }

    //public static IEnumerable<ReadOnlyMemory<T>> Split<T>(this ReadOnlyMemory<T> memory, T separator)
    //    where T : IEquatable<T>
    //{
    //    foreach (var (index, length) in memory.Span.IndexLengthTuples(separator))
    //    {
    //        yield return memory.Slice(index, length);
    //    }
    //}

    //public static IEnumerable<ReadOnlyMemory<T>> Split<T>(this ReadOnlyMemory<T> memory, params T[] separators)
    //    where T : IEquatable<T>
    //{
    //    foreach (var (index, length) in memory.Span.IndexLengthTuples(separators))
    //    {
    //        yield return memory.Slice(index, length);
    //    }
    //}
}

