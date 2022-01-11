namespace Foundation;

using Foundation.Collections.Generic;

public static class RandomExtensions
{
    public static IEnumerable<int> IntegersWithoutDuplicates(
        this Random random,
        int min,
        int max)
    {
        if (null == random) throw new ArgumentNullException(nameof(random));
        var numbers = Enumerable.Range(min, 1 + max - min).ToArray();
        return numbers.Shuffle(random);
    }

    public static bool NextBoolean(this Random random)
    {
        return 0 != random.Next(0, 1);
    }

    public static double NextDouble(this Random random, double min, double max)
    {
        return random.NextDouble() * (max - min) + min;
    }
}

