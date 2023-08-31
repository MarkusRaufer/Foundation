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

    /// <summary>
    /// Generates a sequence of characters between a minimum and maximum. The number of items is not needed. E.g. min == 'A' and max == 'Z'.
    /// </summary>
    /// <param name="min">The start value.</param>
    /// <param name="max">The end value.</param>
    /// <returns></returns>
    public static IEnumerable<char> Range(char min, char max)
    {
        for (var i = min; i <= max; i++)
            yield return i;
    }

    /// <summary>
    /// Generates a sequence of DateOnly values between a minimum and maximum increased by increment.
    /// The number of items is not needed. E.g. min == 23.4 and max == 76.3
    /// </summary>
    /// <param name="min">The start value.</param>
    /// <param name="max">The end value.</param>
    /// <param name="increment">The TimeSpan of the increment.</param>
    /// <returns></returns>
    public static IEnumerable<DateOnly> Range(DateOnly min, DateOnly max, TimeSpan increment)
    {
        DateOnly inc(DateOnly date) => date.Add(increment);
        return Range(min, max, inc);
    }

    /// <summary>
    /// Generates a sequence of DateTime values between a minimum and maximum increased by increment.
    /// The number of items is not needed. E.g. min == 23.4 and max == 76.3
    /// </summary>
    /// <param name="min">The start value.</param>
    /// <param name="max">The end value.</param>
    /// <param name="increment">The TimeSpan of the increment.</param>
    /// <returns></returns>
    public static IEnumerable<DateTime> Range(DateTime min, DateTime max, TimeSpan increment)
    {
        return Range(min, max, x => x + increment);
    }

    /// <summary>
    /// Generates a sequence of decimal values between a minimum and maximum increased by increment.
    /// The number of items is not needed. E.g. min == 23.4 and max == 76.3
    /// </summary>
    /// <param name="min">The start value.</param>
    /// <param name="max">The end value.</param>
    /// <param name="increment">The size of the increment.</param>
    /// <returns></returns>
    public static IEnumerable<decimal> Range(decimal min, decimal max, decimal increment)
    {
        for (var i = min; i <= max; i += increment)
            yield return i;
    }

    /// <summary>
    /// Generates a sequence of double values between a minimum and maximum increased by increment.
    /// The number of items is not needed. E.g. min == 23.4 and max == 76.3
    /// </summary>
    /// <param name="min">The start value.</param>
    /// <param name="max">The end value.</param>
    /// <param name="increment">The size of the increment.</param>
    /// <returns></returns>
    public static IEnumerable<double> Range(double min, double max, double increment)
    {
        for (var i = min; i <= max; i += increment)
            yield return i;
    }

    /// <summary>
    /// Generates a sequence of integers between a minimum and maximum. The number of items is not needed. E.g. min == 234 and max == 763
    /// </summary>
    /// <param name="min">The start value.</param>
    /// <param name="max">The end value.</param>
    /// <returns></returns>
    public static IEnumerable<int> Range(int min, int max)
    {
        for (var i = min; i <= max; i++)
            yield return i;
    }

    /// <summary>
    /// Generates a sequence of long values between a minimum and maximum. The number of items is not needed. E.g. min == 234 and max == 763
    /// </summary>
    /// <param name="min">The start value.</param>
    /// <param name="max">The end value.</param>
    /// <returns></returns>
    public static IEnumerable<long> Range(long min, long max)
    {
        for (var i = min; i <= max; i++)
            yield return i;
    }
    
    
    /// <summary>
    /// Generates a sequence of values between a minimum and maximum increased by increment.
    /// The number of items is not needed. E.g. min == 23.4 and max == 76.3
    /// </summary>
    /// <param name="min">The start value.</param>
    /// <param name="max">The end value.</param>
    /// <param name="increment">The new incremented value.</param>
    /// <returns></returns>
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
}

