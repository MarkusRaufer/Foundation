using Foundation.Collections.Generic;

namespace Foundation;

public static class RandomHelper
{
    /// <summary>
    /// Returns a Guid at a specific <paramref name="index"/>. Same <paramref name="index"/> returns always the same Guid.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Guid GetRandomOrdinalGuid(int index)
    {
        if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "negative indices are not allowed");

        return GetRandomOrdinalGuids(new int[] { index }).FirstOrDefault();
    }

    /// <summary>
    /// Returns random values at specific <paramref name="indices"/>. The same index returns always the same value.
    /// </summary>
    /// <param name="indices">Indices from which the values are returned.</param>
    /// <returns></returns>
    public static IEnumerable<Guid> GetRandomOrdinalGuids(int[] indices)
    {
        indices.ThrowIfNull();
        indices.ThrowIf(() => indices.Any(index => index < 0), "negative indices are not allowed");

        if (0 == indices.Length) yield break;

        var random = new Random(0);

        Array.Sort(indices);

        var maxIndex = indices.Last() + 1;
        var buffer = new byte[16];

        foreach (var index in Enumerable.Range(0, maxIndex))
        {
            var value = random.NextGuid(buffer);
            if (indices.Contains(index)) yield return value;
        }
    }

    /// <summary>
    /// Returns random values at specific <paramref name="indices"/>. The same index returns always the same value.
    /// </summary>
    /// <param name="indices">Indices from which the values are returned.</param>
    /// <param name="seed">Seed for the random generator. null returns always different values.</param>
    /// <returns>Contains only values from valueSet.</returns>
    public static IEnumerable<DateTime> GetRandomOrdinalValues(int[] indices, DateTime min, DateTime max)
    {
        indices.ThrowIfNull();
        indices.ThrowIf(() => indices.Any(index => index < 0), "negative indices are not allowed");

        if (0 == indices.Length) yield break;

        var random = new Random(0);

        Array.Sort(indices);

        var maxIndex = indices.Last() + 1;

        foreach (var index in Enumerable.Range(0, maxIndex))
        {
            var value = random.NextDateTime(min, max);
            if (indices.Contains(index)) yield return value;
        }
    }

    /// <summary>
    /// Returns random values at specific <paramref name="indices"/>. The same index returns always the same value.
    /// </summary>
    /// <param name="indices">Indices from which the values are returned.</param>
    /// <param name="seed">Seed for the random generator. null returns always different values.</param>
    /// <returns>Contains only values from valueSet.</returns>
    public static IEnumerable<double> GetRandomOrdinalValues(int[] indices, double min, double max)
    {
        indices.ThrowIfNull();
        indices.ThrowIf(() => indices.Any(index => index < 0), "negative indices are not allowed");

        if (0 == indices.Length) yield break;

        var random = new Random(0);

        Array.Sort(indices);

        var maxIndex = indices.Last() + 1;

        foreach (var index in Enumerable.Range(0, maxIndex))
        {
            var value = random.NextDouble(min, max);
            if (indices.Contains(index)) yield return value;
        }
    }

    /// <summary>
    /// Returns random values at specific <paramref name="indices"/>. The same index returns always the same value.
    /// </summary>
    /// <param name="indices">Indices from which the values are returned.</param>
    /// <param name="seed">Seed for the random generator. null returns always different values.</param>
    /// <returns>Contains only values from valueSet.</returns>
    public static IEnumerable<int> GetRandomOrdinalValues(int[] indices, int min, int max)
    {
        indices.ThrowIfNull();
        indices.ThrowIf(() => indices.Any(index => index < 0), "negative indices are not allowed");

        if (0 == indices.Length) yield break;

        var random = new Random(0);

        Array.Sort(indices);

        var maxIndex = indices.Last() + 1;

        foreach (var index in Enumerable.Range(0, maxIndex))
        {
            var value = random.Next(min, max);
            if (indices.Contains(index)) yield return value;
        }
    }

    /// <summary>
    /// Returns random values at specific <paramref name="indices"/>. The same index returns always the same value.
    /// </summary>
    /// <param name="indices">Indices from which the values are returned.</param>
    /// <param name="seed">Seed for the random generator. null returns always different values.</param>
    /// <returns>Contains only values from valueSet.</returns>
    public static IEnumerable<long> GetRandomOrdinalValues(int[] indices, long min, long max)
    {
        indices.ThrowIfNull();
        indices.ThrowIf(() => indices.Any(index => index < 0), "negative indices are not allowed");

        if (0 == indices.Length) yield break;

        var random = new Random(0);

        Array.Sort(indices);

        var maxIndex = indices.Last() + 1;

        foreach (var index in Enumerable.Range(0, maxIndex))
        {
            var value = random.NextInt64(min, max);
            if (indices.Contains(index)) yield return value;
        }
    }

    /// <summary>
    /// Generates random values from a set of values.
    /// </summary>
    /// <param name="valueSet">set of values.</param>
    /// <param name="epsilon">The deviation of the result values.</param>
    /// <param name="seed">Seed for the random generator. null returns always different values.</param>
    /// <returns>Contains only values from valueSet.</returns>
    public static IEnumerable<double> GetRandomValues(double[] valueSet, double epsilon = double.Epsilon, int? seed = null)
    {
        valueSet.ThrowIfNull();

        if (0 == valueSet.Length) yield break;

        if (!seed.HasValue) seed = (int)DateTime.Now.Ticks;

        var random = new Random(seed.Value);

        Array.Sort(valueSet);

        var min = valueSet.First();
        var max = valueSet.Last();

        while (true)
        {
            var value = random.NextDouble(min, max);
            foreach (var setValue in valueSet)
            {
                if (setValue.Equal(value, epsilon))
                {
                    yield return value;
                    break;
                }

            }
        }
    }

    /// <summary>
    /// Generates random values from a set of values.
    /// </summary>
    /// <param name="valueSet">set of values.</param>
    /// <param name="seed">Seed for the random generator. null returns always different values.</param>
    /// <returns>Contains only values from valueSet.</returns>
    public static IEnumerable<int> GetRandomValues(int[] valueSet, int? seed = null)
    {
        valueSet.ThrowIfNull();
        valueSet.ThrowIf(() => valueSet.Any(index => index < 0), "negative indices are not allowed");

        if (0 == valueSet.Length) yield break;

        if (!seed.HasValue) seed = (int)DateTime.Now.Ticks;

        var random = new Random(seed.Value);

        Array.Sort(valueSet);

        var min = valueSet.First();
        var max = valueSet.Last() + 1;

        while (true)
        {
            var value = random.Next(min, max);
            if (valueSet.Contains(value)) yield return value;
        }
    }

    /// <summary>
    /// Generates random values from a set of values.
    /// </summary>
    /// <param name="valueSet">set of values.</param>
    /// <param name="seed">Seed for the random generator. null returns always different values.</param>
    /// <returns>Contains only values from valueSet.</returns>
    public static IEnumerable<long> GetRandomValues(long[] valueSet, int? seed = null)
    {
        valueSet.ThrowIfNull();
        valueSet.ThrowIf(() => valueSet.Any(index => index < 0), "negative indices are not allowed");

        if (0 == valueSet.Length) yield break;

        if(!seed.HasValue) seed = (int)DateTime.Now.Ticks;

        var random = new Random(seed.Value);

        Array.Sort(valueSet);

        var min = valueSet.First();
        var max = valueSet.Last() + 1;

        while(true)
        {
            var value = random.NextInt64(min, max);
            if (valueSet.Contains(value)) yield return value;
        }
    }

    
}
