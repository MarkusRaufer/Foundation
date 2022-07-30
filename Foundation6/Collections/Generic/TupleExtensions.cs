namespace Foundation.Collections.Generic;

public static class TupleExtensions
{
    public static (T[], T[]) ToArrays<T>(this (IEnumerable<T>, IEnumerable<T>) tuple)
    {
        return (tuple.Item1.ToArray(), tuple.Item2.ToArray());
    }

    public static (IList<T>, IList<T>) ToLists<T>(this (IEnumerable<T>, IEnumerable<T>) tuple)
    {
        return (tuple.Item1.ToList(), tuple.Item2.ToList());
    }
}
