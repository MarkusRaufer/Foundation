using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

public static class ArrayExtensions
{
    public static IEnumerable<T> AsEnumerable<T>(params T[] items)
    {
        return items;
    }

    public static decimal AverageMedian<T>(this T[] array, Func<T, decimal>? converter = null)
    {
        var (opt1, opt2) = AverageMedianPosition(array);
        if (opt1.IsNone) return 0;

        var value1 = (null == converter)
            ? Convert.ToDecimal(opt1.ValueOrThrow())
            : converter(opt1.ValueOrThrow());

        if (opt2.IsNone) return value1;

        var value2 = (null == converter)
            ? Convert.ToDecimal(opt2.ValueOrThrow())
            : converter(opt2.ValueOrThrow());

        return (value1 + value2) / 2M;
    }

    public static (Opt<T> pos1, Opt<T> pos2) AverageMedianPosition<T>(
        this T[] array,
        IComparer<T>? comparer = null)
    {
        var sorted = new T[array.Length];
        Array.Copy(array, sorted, array.Length);

        if (null == comparer)
            Array.Sort(sorted);
        else
            Array.Sort(sorted, comparer);

        int halfIndex = sorted.Length / 2;

        return (sorted.Length % 2 == 0)
            ? (Opt.Some(sorted[halfIndex - 1]), Opt.Some(sorted[halfIndex]))
            : (Opt.Some(sorted[halfIndex]), Opt.None<T>());
    }

    /// <summary>
    /// returns the indexes of the found selectors in array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IEnumerable<int> IndexesOf<T>(this T[] array, [DisallowNull] IEnumerable<T> selectors)
    {
        var selectorArray = selectors.ToArray();

        for (var i = 0; i < array.Length; i++)
        {
            if (selectorArray.Contains(array[i])) yield return i;
        }
    }

    public static IEnumerable<int> IndexesOf<T>(this T[] array, params T[] selectors)
    {
        return IndexesOf<T>(array, (IEnumerable<T>)selectors);
    }

    /// <summary>
    /// This method implements the Fisher-Yates Shuffle.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="random"></param>
    /// <returns></returns>
    public static T[] Shuffle<T>(this T[] array, Random? random = null)
    {
        if (null == random) random = new Random();

        var n = array.Length;
        for (int i = 0; i < (n - 1); i++)
        {
            var r = i + random.Next(n - i);
            var t = array[r];
            array[r] = array[i];
            array[i] = t;
        }

        return array;
    }
}

