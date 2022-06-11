namespace Foundation.Collections.Generic;

public static class EnumerableHelper
{
    public static IEnumerable<T> AsEnumerable<T>(params T[] items)
    {
        return items;
    }

    /// <summary>
    /// creates a cartesian product of all lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sequences"></param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences)
    {
        IEnumerable<IEnumerable<T>> emptySequence = new[] { Enumerable.Empty<T>() };
        return sequences.Aggregate(
            emptySequence,
            (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence
                select accseq.Concat(new[] { item })
            );
    }
}

