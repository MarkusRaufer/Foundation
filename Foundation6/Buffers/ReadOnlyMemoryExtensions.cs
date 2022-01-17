namespace Foundation.Buffers;

using Foundation.Collections.Generic;
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
    public static IEnumerable<int> IndexesOf<T>(this ReadOnlyMemory<T> memory, [DisallowNull] IEnumerable<T> selectors)
    {
        for (var i = 0; i < memory.Length; i++)
        {
            if (selectors.Any(selector => selector.EqualsNullable(memory.Span[i]))) yield return i;
        }
    }

    public static IEnumerable<int> IndexesOf<T>(this ReadOnlyMemory<T>[] memories, params ReadOnlyMemory<T>[] selectors)
        where T : IEquatable<T>
    {
        var i = 0;
        foreach (var memory in memories)
        {
            if (selectors.Any(s => s.SequenceEqual(memory))) yield return i;

            i++;
        }
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
        return lhs.Zip(rhs, (l, r) => l.SequenceEqual(r)).All(x => x);
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

