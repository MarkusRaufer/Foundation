using System.Runtime.CompilerServices;

namespace Foundation.Collections.Generic;

public static class ArrayExtensions
{
    public static decimal AverageMedian<T>(this T[] array, Func<T, decimal>? converter = null)
    {
        var (opt1, opt2) = AverageMedianPosition(array);
        if (opt1.IsNone) return 0;

        var value1 = (null == converter)
            ? Convert.ToDecimal(opt1.OrThrow())
            : converter(opt1.OrThrow());

        if (opt2.IsNone) return value1;

        var value2 = (null == converter)
            ? Convert.ToDecimal(opt2.OrThrow())
            : converter(opt2.OrThrow());

        return (value1 + value2) / 2M;
    }

    public static (Option<T> pos1, Option<T> pos2) AverageMedianPosition<T>(
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
            ? (Option.Some(sorted[halfIndex - 1]), Option.Some(sorted[halfIndex]))
            : (Option.Some(sorted[halfIndex]), Option.None<T>());
    }

    /// <summary>
    /// returns the indexes of the found selectors in array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IEnumerable<int> IndexesOf<T>(this T[] array, IEnumerable<T> selectors)
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

            (array[i], array[r]) = (array[r], array[i]);
        }

        return array;
    }

    public static T[] ThrowIfEmpty<T>(this T[] arr, [CallerArgumentExpression("arr")] string paramName = "")
        => 0 == arr.Length ? throw new ArgumentException($"{paramName} must not be empty") : arr;


    public static T[] ThrowIfNull<T>(this T[] arr, [CallerArgumentExpression("arr")] string paramName = "")
            => arr ?? throw new ArgumentException($"{paramName} must not be empty");

    public static T[] ThrowIfNullOrEmpty<T>(this T[] arr, [CallerArgumentExpression("arr")] string paramName = "")
            => ThrowIfNull(arr, paramName).ThrowIfEmpty(paramName);
}

