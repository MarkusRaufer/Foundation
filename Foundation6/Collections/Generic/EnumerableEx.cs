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

    public static IEnumerable<char> Range(char min, char max)
    {
        for (var i = min; i <= max; i++)
            yield return i;
    }

    public static IEnumerable<int> Range(int min, int max)
    {
        for (var i = min; i <= max; i++)
            yield return i;
    }

    public static IEnumerable<long> Range(long min, long max)
    {
        for (var i = min; i <= max; i++)
            yield return i;
    }

    public static IEnumerable<decimal> Range(decimal min, decimal max, decimal increment)
    {
        for (var i = min; i <= max; i += increment)
            yield return i;
    }

    public static IEnumerable<double> Range(double min, double max, double increment)
    {
        for (var i = min; i <= max; i += increment)
            yield return i;
    }

    public static IEnumerable<DateOnly> Range(DateOnly min, DateOnly max, TimeSpan increment)
    {
        DateOnly inc(DateOnly date) => date.Add(increment);
        return Range(min, max, inc);
    }

    public static IEnumerable<DateTime> Range(DateTime min, DateTime max, TimeSpan increment)
    {
        return Range(min, max, x => x + increment);
    }

    public static IEnumerable<T> Range<T>(T min, T max, Func<T, T> increment)
        where T : notnull, IComparable<T>
    {
        var current = min;
        while (current.CompareTo(max) < 1)
        {
            yield return current;
            current = increment(current);
        }
    }

    public static IEnumerable<T> Range<T>(T min, T max, Func<T, T, int> compare, Func<T, T> increment)
        where T : notnull
    {
        var current = min;
        while (compare(current, max) < 1)
        {
            yield return current;
            current = increment(current);
        }
    }
}

