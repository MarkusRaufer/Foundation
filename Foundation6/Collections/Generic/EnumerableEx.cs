using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

public static class EnumerableEx
{
    /// <summary>
    /// Creates an endless number of elements.
    /// </summary>
    /// <typeparam name="T">Type of each element.</typeparam>
    /// <param name="factory">Factory of elements of type T.</param>
    /// <param name="seed">This is the seed value which is the first element.</param>
    /// <returns></returns>
    public static IEnumerable<T> Generator<T>(Func<T, T> factory, T seed)
    {
        factory.ThrowIfNull();

        var value = seed;
        yield return value;
        while (true)
        {
            value = factory(value);
            yield return value;
        }
    }
    public static IEnumerable<int> Range(MinMax<int> minmax)
    {
        for (var i = minmax.Min; i <= minmax.Max; i++)
            yield return i;
    }

    public static IEnumerable<decimal> Range(MinMax<decimal> minmax, decimal increment)
    {
        for (var i = minmax.Min; i <= minmax.Max; i += increment)
            yield return i;
    }

    public static IEnumerable<double> Range(MinMax<double> minmax, double increment)
    {
        for (var i = minmax.Min; i <= minmax.Max; i += increment)
            yield return i;
    }

    public static IEnumerable<T> Range<T>(MinMax<T> minmax, Func<T, T> increment)
        where T : notnull
    {
        for (var i = minmax.Min; i.EqualsNullable(minmax.Max); increment(i))
            yield return i;
    }
}

