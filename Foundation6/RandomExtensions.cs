namespace Foundation;

using Foundation.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

public static class RandomExtensions
{
    public static IEnumerable<int> IntegersWithoutDuplicates([DisallowNull] this Random random, int min, int max)
    {
        if (null == random) throw new ArgumentNullException(nameof(random));
        var numbers = Enumerable.Range(min, 1 + max - min).ToArray();
        return numbers.Shuffle(random);
    }

    public static bool NextBoolean([DisallowNull] this Random random)
    {
        return 0 != random.Next(0, 1);
    }

    public static double NextDouble([DisallowNull] this Random random, double max)
    {
        return random.NextDouble() * max;
    }

    public static double NextDouble([DisallowNull] this Random random, double min, double max)
    {
        return random.NextDouble() * (max - min) + min;
    }

    /// <summary>
    /// Creates deterministic guids often needed for tests. Duplicates are possible.
    /// </summary>
    /// <param name="random"></param>
    /// <returns></returns>
    public static Guid NextGuid([DisallowNull] this Random random)
    {
        return NextGuid(random, new byte[16]);
    }

    /// <summary>
    /// Creates deterministic guids often needed for tests. Duplicates are possible.
    /// </summary>
    /// <param name="random"></param>
    /// <param name="guidBuffer">A reusable buffer for creating Guids.</param>
    /// <returns></returns>
    public static Guid NextGuid([DisallowNull] this Random random, [DisallowNull] byte[] guidBuffer)
    {
        random.ThrowIfNull(nameof(random));
        guidBuffer.ThrowIfNull(nameof(guidBuffer));
        guidBuffer.ThrowIf(() => 16 != guidBuffer.Length, nameof(guidBuffer), "buffer for guid must have the size of 16");

        random.NextBytes(guidBuffer);
        return new Guid(guidBuffer);
    }
}

