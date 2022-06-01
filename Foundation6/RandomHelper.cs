namespace Foundation;

public static class RandomHelper
{
    public static double GetRandomOrdinalDouble(int index, double min, double max)
    {
        if(index < 0) throw new ArgumentOutOfRangeException("negative indices are not allowed", nameof(index));

        return GetRandomOrdinalDouble(new int[] { index }, min, max).FirstOrDefault();
    }

    public static IEnumerable<double> GetRandomOrdinalDouble(int[] indices, double min, double max)
    {
        indices.ThrowIfNull();
        indices.ThrowIf(() => indices.Any(index => index < 0), "negative indices are not allowed");

        if (0 == indices.Length) yield break;

        var random = new Random(0);

        Array.Sort(indices);

        foreach (var index in Enumerable.Range(0, indices.Last() + 1))
        {
            var value = random.NextDouble(min, max);
            if (indices.Contains(index)) yield return value;
        }
    }

    public static Guid GetRandomOrdinalGuid(int index)
    {
        if (index < 0) throw new ArgumentOutOfRangeException("negative indices are not allowed", nameof(index));

        return GetRandomOrdinalGuid(new int[] { index }).FirstOrDefault();
    }

    public static IEnumerable<Guid> GetRandomOrdinalGuid(int[] indices)
    {
        indices.ThrowIfNull();
        indices.ThrowIf(() => indices.Any(index => index < 0), "negative indices are not allowed");

        if (0 == indices.Length) yield break;

        var random = new Random(0);

        Array.Sort(indices);

        var buffer = new byte[16];
        foreach (var index in Enumerable.Range(0, indices.Last() + 1))
        {
            var value = random.NextGuid(buffer);
            if (indices.Contains(index)) yield return value;
        }
    }

    public static long GetRandomOrdinalInt64(int index, long min, long max)
    {
        if (index < 0) throw new ArgumentOutOfRangeException("negative indices are not allowed", nameof(index));

        return GetRandomOrdinalInt64(new int[] { index }, min, max).FirstOrDefault();
    }

    public static IEnumerable<long> GetRandomOrdinalInt64(int[] indices, long min, long max)
    {
        indices.ThrowIfNull();
        indices.ThrowIf(() => indices.Any(index => index < 0), "negative indices are not allowed");

        if (0 == indices.Length) yield break;

        var random = new Random(0);

        Array.Sort(indices);

        foreach (var index in Enumerable.Range(0, indices.Last() + 1))
        {
            var value = random.NextInt64(min, max);
            if (indices.Contains(index)) yield return value;
        }
    }
}
