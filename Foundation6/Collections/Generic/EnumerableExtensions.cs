namespace Foundation.Collections.Generic;

using Foundation;
using Foundation.ComponentModel;
using System;
using System.ComponentModel;
using System.Data;
using System.Net.Http.Headers;
//using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

public static class EnumerableExtensions
{
    /// <summary>
    /// Aggregates elements like standard LINQ.
    /// The first element is taken as seed and can be transformed.
    /// </summary>
    /// <typeparam name="T">Type of the elements</typeparam>
    /// <typeparam name="TAccumulate">The transformation format</typeparam>
    /// <param name="items">Elements</param>
    /// <param name="seed">A functor transforms the first element in the list.</param>
    /// <param name="func">The aggregation function.</param>
    /// <returns></returns>
    public static Option<TAccumulate> Aggregate<T, TAccumulate>(
        this IEnumerable<T> items,
        Func<T, TAccumulate> seed,
        Func<TAccumulate, T, TAccumulate> func)
    {
        seed.ThrowIfNull();
        func.ThrowIfNull();

        TAccumulate? acc = default;

        foreach (var item in items.OnFirst(x => acc = seed(x))
                                  .Skip(1))
        {
            acc = func(acc!, item);
        }

        return Option.Maybe(acc);
    }

    /// <summary>
    /// checks if all items are equal.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TSelector"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static bool AllEqual<T>(this IEnumerable<T> items)
    {
        return AllEqual(items, x => x);
    }

    /// <summary>
    /// checks if all values of the selector are equal.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TSelector"></typeparam>
    /// <param name="items"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static bool AllEqual<T, TSelector>(this IEnumerable<T> items, Func<T, TSelector> selector)
    {
        selector.ThrowIfNull(nameof(selector));
        var it = items.ThrowIfNull(nameof(items)).GetEnumerator();

        if (!it.MoveNext()) return true;

        var first = selector(it.Current);

        while (it.MoveNext())
        {
            if (!EqualityComparer<TSelector>.Default.Equals(selector(it.Current), first))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Returns the median of all values returned by the converter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="converter"></param>
    /// <returns></returns>
    public static decimal AverageMedian<T>(this IEnumerable<T> items, Func<T, decimal>? converter = null)
    {
        var (opt1, opt2) = AverageMedianValues(items);
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

    /// <summary>
    /// Returns the real values instead of a division of the median values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static (Option<T> value1, Option<T> value2) AverageMedianValues<T>(this IEnumerable<T> items)
    {
        var sorted = items.OrderBy(x => x);
        var count = sorted.Count();
        int halfIndex = count / 2;

        return (count % 2 == 0)
            ? (Option.Some(sorted.ElementAt(halfIndex - 1)), Option.Some(sorted.ElementAt(halfIndex)))
            : (Option.Some(sorted.ElementAt(halfIndex)), Option.None<T>());
    }

    /// <summary>
    /// Creates a cartesian product from the lists lhs and rhs.
    /// </summary>
    /// <typeparam name="T">Type of the list.</typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static IEnumerable<TResult> CartesianProduct<T, TResult>(
        this IEnumerable<T> lhs,
        IEnumerable<T> rhs,
        Func<T, T, TResult> selector)
    {
        return from l in lhs
               from r in rhs
               select selector(l, r);
    }

    /// <summary>
    /// Checks if lhs contains at least one element of rhs.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool Contains<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs)
    {
        var search = new HashSet<T>(rhs);
        return search.Overlaps(lhs);
    }

    /// <summary>
    /// compares to lists of integers.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static int CompareTo(this IEnumerable<int> lhs, IEnumerable<int> rhs)
    {
        var lhsArray = lhs.ToArray();
        var rhsArray = rhs.ToArray();
        Array.Sort(lhsArray);
        Array.Sort(rhsArray);

        var i = 0;
        var j = 0;
        for(; i < lhsArray.Length && j < rhsArray.Length; i++, j++)
        {
            var cmp = lhsArray[i].CompareTo(rhsArray[j]);
            if (0 != cmp) return cmp;
        }

        return i.CompareTo(j);
    }

    /// <summary>
    /// Correlates to lists by a selector.
    /// </summary>
    /// <typeparam name="T">Type of the items</typeparam>
    /// <typeparam name="TKey">Type of the selector value.</typeparam>
    /// <param name="lhs">Left list.</param>
    /// <param name="rhs">Right list.</param>
    /// <param name="selector">Selector funciton.</param>
    /// <returns>List of correlating items</returns>
    public static IEnumerable<T> Correlate<T, TKey>(this IEnumerable<T> lhs, IEnumerable<T> rhs, Func<T, TKey> selector)
    {
        foreach (var (x, y) in lhs.Join(rhs, selector, selector, (x, y) => (x, y)))
        {
            yield return x;
            yield return y;
        }
    }

    /// <summary>
    /// Creates an endless list of items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> Cycle<T>(this IEnumerable<T> items)
    {
        return new CyclicEnumerable<T>(items);
    }

    /// <summary>
    /// Cycles a counter between min and max. If the counter reaches max it starts with min.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    public static IEnumerable<(T, int)> CycleEnumerate<T>(this IEnumerable<T> items, System.Range range)
    {
        return new CyclicEnumerable<T, int>(items, range.Start.Value, range.End.Value, idx => idx + 1);
    }

    /// <summary>
    /// Cycles a counter between min and max. If the counter reaches max it starts with min.
    /// This allows negative values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns>A tuple containing the item and a counter.</returns>
    public static IEnumerable<(T, int)> CycleEnumerate<T>(this IEnumerable<T> items, int min, int max)
    {
        return new CyclicEnumerable<T, int>(items, min, max, idx => idx + 1);
    }

    /// <summary>
    /// Cycles a counter between min and max. If the counter reaches max it starts with min.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TCount"></typeparam>
    /// <param name="items"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="increment">The counter will be increased by this function.</param>
    /// <returns></returns>
    public static IEnumerable<(T, TCount)> CycleEnumerate<T, TCount>(
        this IEnumerable<T> items
        , TCount min
        , TCount max
        , Func<TCount, TCount> increment)
        where TCount : IComparable<TCount>
    {
        return new CyclicEnumerable<T, TCount>(items, min, max, increment);
    }

    /// <summary>
    /// Removes all duplicates from a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="comparer">a compare function to compare the items.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<T> DistinctBy<T>(this IEnumerable<T> items, Func<T?, T?, bool> comparer)
    {
        comparer.ThrowIfNull();

        return DistinctBy(items, comparer, null);
    }

    /// <summary>
    /// Removes all duplicates from a list. With an additional hash function you can change the sorting order.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="comparer"></param>
    /// <param name="hashFunc">The hash function for each single item. Example: you want all null items at the end of the list. Default: null is -1.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<T> DistinctBy<T>(
        this IEnumerable<T> items,
        Func<T?, T?, bool> comparer,
        Func<T?, int>? hashFunc)
    {
        comparer.ThrowIfNull();

        return items.Distinct(new LambdaEqualityComparer<T>(comparer, hashFunc));
    }

    /// <summary>
    /// This Distinct preserves the ordinal position.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> DistinctPreserveOrdinalPosition<T>(this IEnumerable<T> items)
    {
        return items.Enumerate()
                    .DistinctBy(tuple => tuple.item)
                    .OrderBy(tuple => tuple.counter)
                    .Select(tuple => tuple.item);
    }

    /// <summary>
    /// Returns doublets of a list. If there are e.g. three of an item, 2 will returned.
    /// </summary>
    /// <typeparam name="T">type of elements</typeparam>
    /// <param name="items">list of elements</param>
    /// <returns>all doublets</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> items)
    {
        if (null == items) throw new ArgumentNullException(nameof(items));

        var set = new HashSet<T>();
        foreach (var item in items)
        {
            if (!set.Add(item)) yield return item;
        }
    }

    /// <summary>
    /// returns doublets of a list. If there are e.g. three of an item, 2 will returned.
    /// </summary>
    /// <typeparam name="T">type of elements</typeparam>
    /// <typeparam name="TSelector">type of selector value</typeparam>
    /// <param name="items">list of elements</param>
    /// <param name="selector">selector delegate</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<T> Duplicates<T, TSelector>(
        this IEnumerable<T> items,
        Func<T, TSelector> selector)
    {
        if (null == items) throw new ArgumentNullException(nameof(items));

        var set = new HashSet<TSelector>();
        foreach (var item in items)
        {
            if (!set.Add(selector(item))) yield return item;
        }
    }

    /// <summary>
    /// returns doublets of a list. If there are e.g. three of an item, 2 will returned.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="items">list of items</param>
    /// <param name="comparer">comparer used for equality</param>
    /// <returns>all doublets</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> items, IEqualityComparer<T> comparer)
    {
        var set = new HashSet<T>(comparer.ThrowIfNull());
        foreach (var item in items.ThrowIfNull())
        {
            if (!set.Add(item)) yield return item;
        }
    }

    /// <summary>
    /// Enumerates items starting from seed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="seed"></param>
    /// <returns>Returns tuples (item, counter).</returns>
    public static IEnumerable<(int counter, T item)> Enumerate<T>(this IEnumerable<T> items, int seed = 0)
    {
        var i = seed;
        return Enumerate(items, (item) => i++);
    }

    /// <summary>
    /// Enumerates items. createValue is called on every item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="items"></param>
    /// <param name="createValue"></param>
    /// <returns>Returns a tuple (item, counter).</returns>
    public static IEnumerable<(TValue counter, T item)> Enumerate<T, TValue>(this IEnumerable<T> items, Func<T, TValue> createValue)
    {
        createValue.ThrowIfNull();

        foreach (var item in items)
            yield return (createValue(item), item);
    }

    /// <summary>
    /// Enumerates items. Starting from min until max. If the counter reaches max it starts again from min.
    /// This allows also negative values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="minMax">Allows also negative numbers.</param>
    /// <returns></returns>
    public static IEnumerable<(int counter, T item)> Enumerate<T>(this IEnumerable<T> items, int min, int max)
    {
        var i = min;
        foreach (var item in items)
        {
            if (i > max) i = min;

            yield return (i, item);
            i++;
        }
    }

    /// <summary>
    /// Enumerates items. Starting from Min until Max. If the counter reaches Max it starts again from Min.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="minMax"></param>
    /// <returns></returns>
    public static IEnumerable<(int counter, T item)> Enumerate<T>(this IEnumerable<T> items, System.Range range)
    {
        if (range.End.IsFromEnd) throw new ArgumentException($"{range.End}.IsFromEnd is not allowed");

        var i = range.Start.IsFromEnd ? 0 : range.Start.Value;
        foreach (var item in items)
        {
            if (!range.End.IsFromEnd && range.End.Value < i) i = range.Start.Value;

            yield return (i, item);
            i++;
        }
    }

    /// <summary>
    /// Enumerates items. Starting from Min until Max. If the counter reaches Max it starts again from Min.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TValue1"></typeparam>
    /// <typeparam name="TValue2"></typeparam>
    /// <param name="items"></param>
    /// <param name="createValue1"></param>
    /// <param name="createValue2"></param>
    /// <returns></returns>
    public static IEnumerable<(TValue1, TValue2, T)> Enumerate<T, TValue1, TValue2>(
        this IEnumerable<T> items,
        Func<T, TValue1> createValue1,
        Func<T, TValue2> createValue2)
    {
        foreach (var item in items)
            yield return (createValue1(item), createValue2(item), item);
    }

    /// <summary>
    /// Returns true, if all elements of lhs appear in rhs and the number of items and their occurrency are same.
    /// Positions can be different.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <typeparam name="T"></typeparam>
    public static bool EqualsCollection<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs)
    {
        if (null == lhs) return null == rhs;
        if (null == rhs) return false;

        return !lhs.SymmetricDifference(rhs, retainDuplicates: true).Any();
    }

    public static bool EqualsCollection<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs, Func<T, T, bool> equal)
    {
        if (null == lhs) return null == rhs;
        if (null == rhs) return false;

        return !lhs.SymmetricDifference(rhs, retainDuplicates: true).Any();
    }

    /// <summary>
    /// Returns all lhs elements which are not in rhs. The comparision is made between the TKey values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="keySelector"></param>
    /// <param name="resultSelector"></param>
    /// <returns></returns>
    public static IEnumerable<TResult> ExceptBy<T, TKey, TResult>(
        this IEnumerable<T> lhs,
        IEnumerable<T> rhs,
        Func<T, TKey> keySelector,
        Func<T?, TResult> resultSelector)
    {
        rhs.ThrowIfNull();
        keySelector.ThrowIfNull();
        resultSelector.ThrowIfNull();

        return lhs.Except(rhs, new LambdaEqualityComparer<T>((T? a, T? b) =>
        {
            if (null == a) return null == b;
            if (null == b) return true;

            var selectedA = keySelector(a);
            var selectedB = keySelector(b);
            if (null == selectedA) return null == selectedB;
            if (null == selectedB) return false;

            return selectedA.Equals(selectedB);
        })).Select(resultSelector);
    }

    /// <summary>
    /// Returns all elements of first which are not in second.
    /// </summary>
    /// <typeparam name="T1">Type of elements of first.</typeparam>
    /// <typeparam name="T2">Type of elements of second.</typeparam>
    /// <typeparam name="TKey">Type of the compare value.</typeparam>
    /// <typeparam name="TResult">Value which will be returned of the first list.</typeparam>
    /// <param name="lhs">First list.</param>
    /// <param name="rhs">Second list.</param>
    /// <param name="lhsKeySelector">Compare value of the elements of the first list. Must be of type TKey.</param>
    /// <param name="rhsKeySelector">Compare value of the elements of the second list. Must be of type TKey.</param>
    /// <param name="resultSelector">Rückgabe-Wert. Hier kann auch ein neuer Typ erzeugt werden.</param>
    /// <returns></returns>
    public static IEnumerable<TResult> ExceptBy<T1, T2, TKey, TResult>(
        this IEnumerable<T1> lhs,
        IEnumerable<T2> rhs,
        Func<T1, TKey> lhsKeySelector,
        Func<T2, TKey> rhsKeySelector,
        Func<T1, TResult> resultSelector)
    {
        rhs.ThrowIfNull();
        lhsKeySelector.ThrowIfNull();
        rhsKeySelector.ThrowIfNull();
        resultSelector.ThrowIfNull();

        var hashedRhs = new HashSet<TKey>(rhs.Select(rhsKeySelector));
        return lhs.Where(l => !hashedRhs.Contains(lhsKeySelector(l))).Select(resultSelector);
    }

    /// <summary>
    /// Returns all elements from <paramref name="lhs"/> which are not in <paramref name="rhs"/> including duplicates.
    /// If T implements <see cref="=IComparable<typeparamref name="T"/>"/> use <see cref="=ExceptOrderedWithDuplicates"/>, because it is more efficient.
    /// The smaller list should be rhs.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static IEnumerable<T> ExceptWithDuplicates<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs)
    {
        var right = new Dictionary<NullableKey<T>, (int l, int r)>();
        foreach (var r in rhs)
        {
            var key = NullableKey.New(r);
            if (right.TryGetValue(key, out var tuple))
            {
                tuple.r++;
                right[key] = tuple;
                continue;
            }

            right[key] = (0, 1);
        }

        foreach (var left in lhs)
        {
            var key = NullableKey.New(left);
            if (right.TryGetValue(key, out var tuple))
            {
                tuple.l++;
                right[key] = tuple;

                if (tuple.l <= tuple.r) continue;
            }

            yield return left;
        }
    }

    /// <summary>
    /// Returns all elements from <paramref name="lhs"/> which are not in <paramref name="rhs"/> including duplicates.
    /// The smaller list should be rhs.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    /// <typeparam name="TKey">The type of the selector value which is used for comparison.</typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IEnumerable<T> ExceptWithDuplicates<T, TKey>(
        this IEnumerable<T> lhs,
        IEnumerable<T> rhs,
        Func<T, TKey?> selector)
    {
        TKey? nullableSelector(NullableKey<T> x) => null == x.Value ? default : selector(x.Value);

        var right = new Dictionary<NullableKey<T>, (int l, int r)>(new LambdaEqualityComparer<NullableKey<T>, TKey>(nullableSelector));
        foreach (var r in rhs)
        {
            var key = NullableKey.New(r);
            if (right.TryGetValue(key, out var tuple))
            {
                tuple.r++;
                right[key] = tuple;
                continue;
            }

            right[key] = (0, 1);
        }

        foreach (var left in lhs)
        {
            var key = NullableKey.New(left);
            if (right.TryGetValue(key, out var tuple))
            {
                tuple.l++;
                right[key] = tuple;

                if (tuple.l <= tuple.r) continue;
            }

            yield return left;
        }
    }

    /// <summary>
    /// Returns all elements from <paramref name="lhs"/> which are not in <paramref name="rhs"/> including duplicates.
    /// First lhs and rhs are sorted and then a <see cref="=Array.BinarySearch"/> is made.
    /// The smaller list should be rhs.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static IEnumerable<T> ExceptWithDuplicatesSorted<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs)
        where T : IComparable<T>
    {
        var lhsArray = lhs.ToArray();
        var rhsArray = rhs.ToArray();
        Array.Sort(lhsArray);
        Array.Sort(rhsArray);
        
        var rhsMaxIndex = rhsArray.Length - 1;
        var rhsIndex = 0;
        for(var i = 0; i < lhsArray.Length; i++)
        {
            var left = lhsArray[i];
            if(rhsIndex > rhsMaxIndex)
            {
                yield return left;
                continue;
            }

            var right = rhsArray[rhsIndex];
            
            var compare = left.CompareTo(right);
            if(-1 == compare)
            {
                yield return left;
                continue;
            }
            if(0 == compare)
            {
                rhsIndex++;
                continue;
            }

            do
            {
                rhsIndex++;
                if (rhsIndex > rhsMaxIndex) break;

                right = rhsArray[rhsIndex];

                compare = left.CompareTo(right);
                if (0 == compare) rhsIndex++;

            } while (1 > compare);
        }
    }

    /// <summary>
    /// Filters and transform items. It returns only Option.Some values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IEnumerable<TResult> FilterMap<T, TResult>(
        this IEnumerable<T> items,
        Func<T, Option<TResult>> selector)
    {
        selector.ThrowIfNull();

        foreach (var item in items)
        {
            var optional = selector(item);
            if (optional.TryGet(out TResult? value)) yield return value;
        }
    }

    /// <summary>
    /// Filters and transform items. It returns only Option.Some values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IEnumerable<TResult> FilterMap<T, TResult>(
        this IEnumerable<T> items, 
        Func<T, bool> predicate,
        Func<T, TResult> selector)
    {
        selector.ThrowIfNull();

        foreach (var item in items)
        {
            if (predicate(item)) yield return selector(item);
        }
    }

    /// <summary>
    /// Returns first item as Option. If the list is empty None is returned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static Option<T> FirstAsOption<T>(this IEnumerable<T> items)
    {
        return items.FirstAsOption(x => true);
    }

    /// <summary>
    /// Returns first item exists and is of type TResult Some(TResult) is return otherwise None. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static Option<TResult> FirstAsOption<T, TResult>(this IEnumerable<T> items)
    {
        return items.FirstAsOption(x => true)
                    .Either(x => x.ToOption<TResult>(),
                          () => Option.None<TResult>());
    }

    /// <summary>
    /// Returns the first item as <typeparamref name="TResult"/> optional.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="onSome"></param>
    /// <returns></returns>
    public static Option<TResult> FirstAsOption<T, TResult>(this IEnumerable<T> items, Func<T, TResult> onSome)
    {
        return items.FirstAsOption(x => true, onSome);
    }

    /// <summary>
    /// Returns the first item that matches the predicate or None.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static Option<T> FirstAsOption<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        return items.FirstAsOption(predicate, x => x);
    }

    public static Option<TResult> FirstAsOption<T, TResult>(
        this IEnumerable<T> items,
        Func<T, bool> predicate,
        Func<T, TResult> onSome)
    {
        if (null == items) return Option.None<TResult>();
        predicate.ThrowIfNull();
        onSome.ThrowIfNull();

        foreach (var item in items.Where(predicate))
            return Option.Some(onSome(item));

        return Option.None<TResult>();
    }

    /// <summary>
    /// Returns the first item that matches the predicate or None.
    /// </summary>
    /// <typeparam name="TOk"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="items">a list of results.</param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static Option<TOk> FirstOk<TOk, TError>(
        this IEnumerable<Result<TOk, TError>> items, 
        Func<TOk, bool> predicate)
    {
        predicate.ThrowIfNull();

        return items.SelectOk().FirstAsOption(predicate);
    }

    /// <summary>
    /// Executes action for every item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="action"></param>
    /// <returns>The number of executed actions.</returns>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        action.ThrowIfNull();

        foreach (var item in source)
        {
            action(item);
        }
    }

    /// <summary>
    /// Calls <paramref name="action"/> for every item. If the list is empty <paramref name="emptyAction"/> is called.
    /// </summary>
    /// <typeparam name="T">Type of the item.</typeparam>
    /// <param name="source"></param>
    /// <param name="action">Will be executed for every item.</param>
    /// <param name="emptyAction">Will be executed if source is empty.</param>
    /// <returns>The number of executed actions.</returns>
    public static void ForEach<T>(
        this IEnumerable<T> source,
        Action<T> action,
        Action emptyAction)
    {
        action.ThrowIfNull();
        emptyAction.ThrowIfNull();

        var it = source.GetEnumerator();
        if(!it.MoveNext())
        {
            emptyAction();
            return;
        }

        var item = it.Current;
        action(item);
        
        while(it.MoveNext())
        {
            item = it.Current;
            action(item);
        }
    }

    public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(
        this IEnumerable<TSource> source,
        Func<TSource, TKey> keySelector,
        Func<TKey?, TKey?, bool> predicate)
    {
        return source.GroupBy(keySelector.ThrowIfNull(),
                              new LambdaEqualityComparer<TKey>(predicate.ThrowIfNull()));
    }

    /// <summary>
    /// Returns if the list has at least numberOfElements.
    /// </summary>
    /// <typeparam name="T">Type of list elements.</typeparam>
    /// <param name="items">List of items.</param>
    /// <param name="numberOfElements">The number of elements</param>
    /// <param name="action">action which is called if numberOfElements is reached.</param>
    /// <returns></returns>
    public static bool HasAtLeast<T>(this IEnumerable<T> items, int numberOfElements)
    {
        if (0 > numberOfElements) return false;

        int counter = 0;
        var it = items.ThrowIfNull().GetEnumerator();
        while(it.MoveNext())
        {
            counter++;

            if (counter >= numberOfElements) break;
        }

        return counter >= numberOfElements;
    }

    /// <summary>
    /// Returns a list of indices of found items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> items, T? item)
    {
        bool equals(T? x)
        {
            if (item is null) return x is null;
            if (x is null) return false;

            return item.Equals(x);
        }
        
        var i = 0;
        foreach (var itm in items)
        {
            if (equals(itm)) yield return i;

            i++;
        }
    }

    /// <summary>
    /// Returns a list of indices of found items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate">If true the index of the item will be returned.</param>
    /// <returns></returns>
    public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        predicate.ThrowIfNull();

        var i = 0;
        foreach (var item in items)
        {
            if (predicate(item))
                yield return i;

            i++;
        }
    }

    /// <summary>
    /// Returns the index of the first found item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static int IndexOf<T>(this IEnumerable<T> items, T? item)
    {
        bool equals(T? x)
        {
            if (item is null) return x is null;
            if (x is null) return false;

            return item.Equals(x);
        }

        var index = 0;
        foreach (var itm in items)
        {
            if (equals(itm)) return index;

            index++;
        }
        return -1;
    }

    /// <summary>
    /// Returns the index of the first item found with a specific predicate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static int IndexOf<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        predicate.ThrowIfNull();

        var i = 0;
        foreach (var item in items)
        {
            if (predicate(item)) return i;

            i++;
        }
        return -1;
    }

    /// <summary>
    /// Returns the index of the first item found after start with a specific predicate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="start"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static int IndexOf<T>(this IEnumerable<T> items, int start, Func<T, bool> predicate)
    {
        predicate.ThrowIfNull();

        var i = start;
        foreach (var item in items.Skip(start))
        {
            if (predicate(item)) return i;

            i++;
        }
        return -1;
    }


    /// <summary>
    /// returns a list of tuples including the item and its ordinal index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<(T item, int index)> IndexTuplesOf<T>(
        this IEnumerable<T> items, 
        Func<T, bool> predicate)
    {
        predicate.ThrowIfNull();

        var i = 0;
        foreach (var elem in items)
        {
            if (predicate(elem)) yield return (item: elem, index: i);

            i++;
        }
    }

    /// <summary>
    /// The ignored item does not appear in the result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="ignoredItem">The item which is filtered.</param>
    /// <returns></returns>
    public static IEnumerable<T> Ignore<T>(this IEnumerable<T> items, T ignoredItem)
    {
        foreach (var item in items.ThrowIfNull())
        {
            if (item.EqualsNullable(ignoredItem)) continue;

            yield return item;
        }
    }

    /// <summary>
    /// Only items withing the range are returned. If the position is greater than the range end it stops.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="range"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IEnumerable<T> Ignore<T>(this IEnumerable<T> items, System.Range range)
    {
        if (range.Start.Value < 0) throw new ArgumentOutOfRangeException(nameof(range));

        int i = 0;
        foreach (var item in items.ThrowIfNull())
        {
            if(range.Start.Value >= i && range.End.Value <= i) yield return item;
            if (range.End.Value < i) yield break;
            i++;
        }
    }

    /// <summary>
    /// Items for which the predicate applies are not returned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> Ignore<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        predicate.ThrowIfNull();

        foreach (var item in items)
        {
            if (predicate(item)) continue;

            yield return item;
        }
    }

    public static IEnumerable<T> Ignore<T>(this IEnumerable<T> items, Func<T, bool> predicate, Action<T> action)
    {
        predicate.ThrowIfNull();

        foreach (var item in items)
        {
            if (predicate(item))
            {
                action(item);
                continue;
            }

            yield return item;
        }
    }

    /// <summary>
    /// Items at indicies are not returned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="indices"></param>
    /// <returns></returns>
    public static IEnumerable<T> Ignore<T>(this IEnumerable<T> items, params int[] indices)
    {
        if (0 == indices.Length) yield break;

        var i = 0;
        foreach (var item in items)
        {
            if (!indices.Contains(i))
                yield return item;

            i++;
        }
    }

    /// <summary>
    /// inserts an item before the equal item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="item"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> Insert<T>(this IEnumerable<T> items, T item, Func<T, bool> predicate)
    {
        predicate.ThrowIfNull();

        var inserted = false;
        foreach (var i in items.ThrowIfNull())
        {
            if (!inserted && predicate(i))
            {
                yield return item;
                inserted = true;
            }
            yield return i;
        }

        if (!inserted) yield return item;
    }

    /// <summary>
    /// inserts an item before the equal item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="item"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<T> Insert<T>(
        this IEnumerable<T> items,
        T item,
        IComparer<T> comparer)
    {
        comparer.ThrowIfNull();

        var inserted = false;
        foreach (var i in items.ThrowIfNull())
        {
            if (!inserted && comparer.Compare(item, i) < 1)
            {
                yield return item;
                inserted = true;
            }
            yield return i;
        }

        if (!inserted) yield return item;
    }

    /// <summary>
    /// Intersects a list of collections.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collections"></param>
    /// <returns>Only items that are in all collections.</returns>
    public static IEnumerable<T> Intersect<T>(this IEnumerable<IEnumerable<T>> collections)
    {
        var itCollections = collections.GetEnumerator();
        if (!itCollections.MoveNext()) yield break;

        var intersected = itCollections.Current;
        while (itCollections.MoveNext())
        {
            intersected = intersected.Intersect(itCollections.Current);

            if (!intersected.Any()) break;
        }

        foreach (var item in intersected)
        {
            yield return item;
        }
    }

    /// <summary>
    /// Intersects all collections.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collections"></param>
    /// <param name="compare"></param>
    /// <returns></returns>
    public static IEnumerable<T> IntersectBy<T>(this IEnumerable<IEnumerable<T>> collections, Func<T?, T?, bool> compare)
    {
        var itCollections = collections.GetEnumerator();
        if (!itCollections.MoveNext()) yield break;

        var intersected = itCollections.Current;
        while (itCollections.MoveNext())
        {
            intersected = intersected.Intersect(itCollections.Current, new LambdaEqualityComparer<T>(compare));

            if (!intersected.Any()) break;
        }

        foreach (var item in intersected)
        {
            yield return item;
        }
    }

    /// <summary>
    /// Iterates to all items. Can be used to iterate all items without using memory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    public static void Iterate<T>(this IEnumerable<T> items)
    {
        var enumerator = items.ThrowIfNull()
                              .GetEnumerator();

        while (enumerator.MoveNext())
        {
        }
    }

    /// <summary>
    /// Returns a list of k-combinations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> KCombinations<T>(this IEnumerable<T> items, int length)
        where T : IComparable
    {
        items.ThrowIfNull();

        if (length == 1) return items.Select(t => new T[] { t });

        return KCombinations(items, length - 1)
                    .SelectMany(t => items.Where(o => o.CompareTo(t.Last()) > 0),
                               (t1, t2) => t1.Concat(new T[] { t2 }));
    }

    /// <summary>
    /// Returns a list of k-combinations with repetitions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> KCombinationsWithRepetition<T>(this IEnumerable<T> items, int length) 
        where T : IComparable
    {
        items.ThrowIfNull();

        if (length == 1) return items.Select(t => new T[] { t });

        return KCombinationsWithRepetition(items, length - 1)
                    .SelectMany(t => items.Where(o => o.CompareTo(t.Last()) >= 0),
                               (t1, t2) => t1.Concat(new T[] { t2 }));
    }

    /// <summary>
    /// Returns the last item of items if not empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static Option<T> LastAsOption<T>(this IEnumerable<T> items)
    {
        var it = items.ThrowIfNull().GetEnumerator();

        var last = Option.None<T>();

        while (it.MoveNext())
        {
            last = Option.Some(it.Current);
        }

        return last;
    }

    /// <summary>
    /// Returns all matching items of both lists as a tuple of two lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey">Key that is used for matching.</typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="keySelector"></param>
    /// <param name="unique"></param>
    /// <returns>matching items as a tuple of two lists.</returns>
    public static (IEnumerable<T> lhs, IEnumerable<T> rhs) Match<T, TKey>(
        this IEnumerable<T> lhs,
        IEnumerable<T> rhs,
        Func<T, TKey> keySelector)
        where TKey : notnull
    {
        lhs.ThrowIfNull();
        rhs.ThrowIfNull();
        keySelector.ThrowIfNull();

        var lhsTuples = lhs.Enumerate().ToArray();
        var rhsTuples = rhs.Enumerate().ToArray();

        var tuples = from left in lhsTuples
                     join right in rhsTuples on keySelector(left.item) equals keySelector(right.item)
                     select (left, right);

        var lhsMap = new MultiValueMap<TKey, (int, T)>();
        var rhsMap = new MultiValueMap<TKey, (int, T)>();

        foreach (var (left, right) in tuples)
        {
            lhsMap.AddUnique(keySelector(left.item), left);
            rhsMap.AddUnique(keySelector(right.item), right);
        }

        return (lhsMap.Values.Select(t => t.Item2), rhsMap.Values.Select(t => t.Item2));
    }

    /// <summary>
    /// Returns all matching items between two lists. The items are returned as tuples with their occurrencies.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns>matching items with their occurrencies.</returns>
    public static IEnumerable<((int counter, T? item) lhs, (int counter, T? item) rhs)> MatchWithOccurrencies<T>(
        this IEnumerable<T?> lhs,
        IEnumerable<T?> rhs)
    {
        lhs.ThrowIfNull();
        rhs.ThrowIfNull();
        
        var enumeratedLhs = lhs.Select(l => NullableKey.New(l))
                               .Enumerate();

        var enumeratedRhs = rhs.Select(r => NullableKey.New(r))
                               .Enumerate();

        var tuples = enumeratedLhs
                    .Join(enumeratedRhs,
                     left => left.item,
                     right => right.item,
                     (left, right) => (left, right)).ToArray();

        
        var lhsMap = new MultiValueMap<NullableKey<T>, (int, T?)>();
        var rhsMap = new MultiValueMap<NullableKey<T>, (int, T?)>();

        foreach (var (left, right) in tuples)
        {
            lhsMap.AddUnique(left.item, (left.counter, left.item.Value));
            rhsMap.AddUnique(right.item, (right.counter, right.item.Value));
        }

        foreach (var pair in lhsMap)
        {
            var lhsTuples = lhsMap.GetValues(new[] { pair.Key });
            var rhsTuples = rhsMap.GetValues(new[] { pair.Key });

            var leftTuple = (lhsTuples.Count(), pair.Key.Value);
            var rightTuple = (rhsTuples.Count(), pair.Key.Value);

            yield return (leftTuple, rightTuple);
        }
    }

    /// <summary>
    /// Returns matching items of both lists with their occurrencies.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="keySelector"></param>
    /// <returns></returns>
    public static IEnumerable<((int counter, T item) lhs, (int counter, T item) rhs)> MatchWithOccurrencies<T, TKey>(
        this IEnumerable<T> lhs,
        IEnumerable<T> rhs,
        Func<T, TKey> keySelector)
        where TKey : notnull
    {
        lhs.ThrowIfNull();
        rhs.ThrowIfNull();
        keySelector.ThrowIfNull();
        
        var enumeratedLhs = lhs.Enumerate();
        var enumeratedRhs = rhs.Enumerate();

        var tuples = from left in enumeratedLhs
                     join right in enumeratedRhs on keySelector(left.item) equals keySelector(right.item)
                     select (left, right);

        var lhsMap = new MultiValueMap<TKey, (int, T)>();
        var rhsMap = new MultiValueMap<TKey, (int, T)>();

        foreach (var (left, right) in tuples)
        {
            var key = keySelector(left.item);

            lhsMap.AddUnique(key, left);
            rhsMap.AddUnique(key, right);
        }

        foreach(var pair in lhsMap)
        {
            var lhsTuples = lhsMap.GetValues(new[] { pair.Key });
            var rhsTuples = rhsMap.GetValues(new[] { pair.Key });

            var leftTuple = (lhsTuples.Count(), pair.Value.Item2);
            var rightTuple = (rhsTuples.Count(), pair.Value.Item2);

            yield return (leftTuple, rightTuple);
        }
    }

    /// <summary>
    /// Returns the greatest item selected by the selector.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="comparer">Returns the value to compare.</param>
    /// <returns></returns>
    public static T MaxBy<T>(this IEnumerable<T> items, Func<T, T, int> comparer)
    {
        items.ThrowIfNull();
        comparer.ThrowIfNull();
        
        return items.Aggregate((a, b) => comparer(a, b) == 1 ? a : b);
    }

    /// <summary>
    /// Returns the min item selected by the selector.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TSelector"></typeparam>
    /// <param name="items"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static T? MinBy<T>(this IEnumerable<T> items, Func<T, T, int> comparer)
    {
        items.ThrowIfNull();
        comparer.ThrowIfNull();

        return items.Aggregate((a, b) => comparer(a, b) == -1 ? a : b);

    }

    /// <summary>
    /// Returns the min and max value.
    /// </summary>
    /// <typeparam name="T">T must implement IComparable<T></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static Option<(T min, T max)> MinMax<T>(this IEnumerable<T> items)
        where T : IComparable<T>
    {
        items.ThrowIfNull();
        T? min = default;
        T? max = default;

        foreach (var item in items
        .OnFirst(i =>
        {
            min = i;
            max = i;
        })
        .Skip(1))
        {
            if (-1 == item.CompareTo(min))
            {
                min = item;
                continue;
            }

            if (1 == item.CompareTo(max))
                max = item;
        }

        return (null != min && null != max)
            ? Option.Some((min, max))
            : Option.None<(T, T)>();
    }

    /// <summary>
    /// Returns the min and max selected by the selector.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TSelector"></typeparam>
    /// <param name="items"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static Option<(T min, T max)> MinMax<T, TSelector>(
        this IEnumerable<T> items,
        Func<T, TSelector> selector)
        where TSelector : IComparable
    {
        T? min = default;
        T? max = default;

        foreach (var item in items
            .OnFirst(x =>
        {
            min = x;
            max = x;
        })
        .Skip(1))
        {
            var selectorValue = selector(item);
            if (null == selectorValue) continue;

            if (-1 == selectorValue.CompareTo(selector(min!)))
            {
                min = item;
                continue;
            }

            if (1 == selectorValue.CompareTo(selector(max!)))
                max = item;
        }

        return null != min && null != max
            ? Option.Some((min, max))
            : Option.None<(T, T)>();
    }

    /// <summary>
    /// Returns the elements that occure most frequently.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey"></typeparam>
    /// <param name="items"></param>
    /// <param name="keySelector"></param>
    /// <returns></returns>
    public static (IEnumerable<T> items, int count) MostFrequent<T, TKey>(
        this IEnumerable<T> items,
        Func<T, TKey> keySelector)
    {
        var grouped = items.ThrowIfNull()
                           .GroupBy(keySelector);

        var mostFrequent = new List<IGrouping<TKey, T>>();
        var itemCount = 0;

        foreach (var grp in grouped.OnFirst(g =>
        {
            mostFrequent.Add(g);
            itemCount = mostFrequent.Count;
        }).Skip(1))
        {
            var count = grp.Count();
            if (itemCount > count) continue;

            if (count > itemCount) mostFrequent.Clear();

            mostFrequent.Add(grp);
            itemCount = count;
        }

        if (null == mostFrequent) return (Enumerable.Empty<T>(), 0);

        return (mostFrequent.Select(g => g.First()), itemCount);
    }

    /// <summary>
    /// Returns the item on a specific index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="index"></param>
    /// <returns>An optional.</returns>
    public static Option<T> Nth<T>(this IEnumerable<T> items, int index)
    {
        if (0 > index) return Option.None<T>();

        var pos = 0;
        foreach (var item in items.ThrowIfNull())
        {
            if (pos > index) break;

            if (index == pos)
                return Option.Some(item);

            pos++;
        }
        return Option.None<T>();
    }


    /// <summary>
    /// Returns all items within the range of indices.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="range">range of indices. Indices from end are not supported</param>
    /// <returns>all items within the range of indices</returns>
    public static IEnumerable<T> Nths<T>(this IEnumerable<T> items, System.Range range)
    {
        int i = 0;
        foreach (var item in items.ThrowIfNull())
        {
            if (range.Includes(i)) yield return item;

            i++;
        }
    }

    /// <summary>
    /// gets items on the certain indexes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="indexes"></param>
    /// <returns></returns>
    public static IEnumerable<T> Nths<T>(this IEnumerable<T> items, IEnumerable<int> indexes)
    {
        var pos = 0;
        foreach (var item in items.ThrowIfNull())
        {
            if (indexes.Contains(pos))
                yield return item;

            pos++;
        }
    }

    /// <summary>
    /// Returns a list of items. The predicate allows filtering items by index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> Nths<T>(this IEnumerable<T> items, Func<long, bool> predicate)
    {
        predicate.ThrowIfNull();

        long i = 0;
        return items.Where(item => predicate(i++));
    }

    /// <summary>
    /// Returns all items with their occurrencies.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="items">list of items</param>
    /// <returns>Returns all items with their occurrencies.</returns>
    public static IEnumerable<(T value, int quantity)> Occurrencies<T>(this IEnumerable<T> items)
    {
        foreach(var group in items.GroupBy(x => x))
        {
            yield return (value: group.Key, quantity: group.Count());
        }
    }

    /// <summary>
    /// Returns all items with their occurrencies.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="items">List of items</param>
    /// <param name="comparer">custom comparer.</param>
    /// <returns>Returns all items with their occurrencies.</returns>
    public static IEnumerable<(T? value, int quantity)> Occurrencies<T>(
        this IEnumerable<T> items,
        IEqualityComparer<T> comparer)
    {
        var set = new CountedHashSet<T>(new HashSet<Countable<T>>(new NullableEqualityComparer<Countable<T>>(
                        (x, y) => comparer.Equals(x.Value, y.Value),
                        x => comparer.GetHashCode(x.Value!))));

        foreach (var item in items)
        {
            set.Add(item);
        }

        foreach (var (item, count) in set.GetCountedElements())
        {
            yield return (value: item, quantity: count);
        }
    }

    /// <summary>
    /// Returns all items whose type matches with a list of types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static IEnumerable<T> OfType<T>(this IEnumerable<T> items, params Type[] types)
    {
        foreach (var item in items.ThrowIfNull())
        {
            if (null == item) continue;

            var type = item.GetType();
            if (types.Any(t => t.Equals(type) || t.IsAssignableFrom(type)))
                yield return item;
        }
    }

    /// <summary>
    /// Returns all items of type <paramref name="types"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="selector"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static IEnumerable<TResult> OfType<T, TResult>(
        this IEnumerable<T> items,
        Func<T, TResult> selector, 
        params Type[] types)
    {
        return items.OfType(types).Select(selector);
    }

    /// <summary>
    /// Returns adjacent items as tuples.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="selector">First T is lhs item and second T is the rhs item and so on.</param>
    /// <returns></returns>
    public static IEnumerable<(T lhs, T rhs)> Pairs<T>(this IEnumerable<T> items)
    {
        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext()) yield break;

        var lhs = it.Current;
        while (it.MoveNext())
        {
            yield return (lhs, it.Current);

            lhs = it.Current;
        }
    }

    /// <summary>
    /// Calls action on all adjacent elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action">Contains the previous and the current item.</param>
    /// <returns></returns>
    public static IEnumerable<(T lhs, T rhs)> Pairs<T>(this IEnumerable<T> items, Action<T, T> action)
    {
        action.ThrowIfNull();

        foreach (var tuple in items.Pairs())
        {
            action(tuple.lhs, tuple.rhs);
            yield return tuple;
        }
    }

    /// <summary>
    /// Calls selector on all adjacent elements and returns all elements of the tuple.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IEnumerable<T> Pairs<T>(this IEnumerable<T> items, Func<T, T, (T, T)> selector)
    {
        selector.ThrowIfNull();

        foreach (var (lhs, rhs) in items.Pairs())
        {
            var (l, r) = selector(lhs, rhs);
            yield return l;
            yield return r;
        }
    }

    /// <summary>
    /// Calls selector on all adjacent elements and transforms each element into TReturn.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action">Contains the previous and the current item.</param>
    /// <returns></returns>
    public static IEnumerable<TReturn> Pairs<T, TReturn>(this IEnumerable<T> items, Func<T, T, TReturn> selector)
    {
        selector.ThrowIfNull();

        foreach (var (lhs, rhs) in items.Pairs())
            yield return selector(lhs, rhs);
    }

    /// <summary>
    /// Partitions items into two lists. If predicate is true the item is added to matching otherwise to notMatching.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate">Discriminator of the two lists.</param>
    /// <returns></returns>
    public static (IEnumerable<T> matching, IEnumerable<T> notMatching) Partition<T>(
        this IEnumerable<T> items, 
        Func<T, bool> predicate)
    {
        items.ThrowIfNull();
        predicate.ThrowIfNull();

        var groups = items.GroupBy(predicate);

        return (groups.Where(grp => grp.Key).SelectMany(x => x), groups.Where(grp => !grp.Key).SelectMany(x => x));
    }

    /// <summary>
    /// Partitions items into two lists. If predicate is true the item is added to matching otherwise notMatching.
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    /// <typeparam name="TResult">Type of the result.</typeparam>
    /// <param name="items">List of items.</param>
    /// <param name="predicate">Predicate to partition.</param>
    /// <param name="match">projection to TResult.</param>
    /// <param name="noMatch">projection to TResult.</param>
    /// <returns></returns>
    /// 
    public static (TMatch matching, TNoMatch notMatching) Partition<T, TMatch, TNoMatch>(
        this IEnumerable<T> items,
        Func<T, bool> predicate,
        Func<IEnumerable<T>, TMatch> match,
        Func<IEnumerable<T>, TNoMatch> noMatch)
    {
        predicate.ThrowIfNull();
        match.ThrowIfNull();
        noMatch.ThrowIfNull();

        var (matching, notMatching) = Partition(items, predicate);

        return (match(matching), noMatch(notMatching));
    }

    /// <summary>
    /// Creates permutations of a list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items">items must be sorted</param>
    /// <param name="length">This is the permutation size.</param>
    /// <param name="repetitions">If true, it contains repetitions.</param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> Permutations<T>(
        this IEnumerable<T> items,
        int length,
        bool repetitions = true)
    {
        items.ThrowIfNull();

        if (length == 1) return items.Select(t => new T[] { t });

        if (repetitions)
        {
            return Permutations(items, length - 1, repetitions)
                        .SelectMany(t => items,
                                   (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        return Permutations(items, length - 1)
                    .SelectMany(t => items.Where(o => !t.Contains(o)),
                               (t1, t2) => t1.Concat(new T[] { t2 }));
    }

    /// <summary>
    /// Creates permutations of two lists.
    /// </summary>
    /// <typeparam name="T1">Type of elements of first list.</typeparam>
    /// <typeparam name="T2">Type of elements of second list.</typeparam>
    /// <param name="lhs">first list.</param>
    /// <param name="rhs">second list.</param>
    /// <param name="repetitions">if true the permutations can include repetitions otherwise not.</param>
    /// <returns></returns>
    public static IEnumerable<(T1, T2)> Permutations<T1, T2>(
        this IEnumerable<T1> lhs, 
        IEnumerable<T2> rhs, 
        bool repetitions = true)
    {
        var tuples = lhs.SelectMany(l => rhs, (l, r) => (l, r));
        return repetitions ? tuples : tuples.Distinct();
    }

    /// <summary>
    /// Creates permutations of multiple lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lists"></param>
    /// <returns></returns>
    public static IEnumerable<T[]> Permutations<T>(this IEnumerable<IEnumerable<T>> lists)
    {
        if (!lists.Any())
        {
            yield return Array.Empty<T>();
            yield break;
        }

        var firstList = lists.First();
        var remainingLists = lists.Skip(1);

        var withoutFirstList = Permutations(remainingLists);

        foreach (var lhs in firstList)
        {
            foreach (var rhs in withoutFirstList)
            {
                var permutation = new T[1 + rhs.Length];
                permutation[0] = lhs;

                if (rhs.Length > 0) Array.Copy(rhs, 0, permutation, 1, rhs.Length);

                yield return permutation;
            }
        }
    }

    /// <summary>
    /// Inserts an item at the beginning only if the list is not empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static IEnumerable<T> PrependIfNotEmpty<T>(this IEnumerable<T> items, Func<T> factory)
    {
        factory.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext()) yield break;

        yield return factory();
        yield return it.Current;

        while (it.MoveNext()) yield return it.Current;
    }

    /// <summary>
    /// Returns a random subset of elems.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="elems"></param>
    /// <param name="numberOfSubSetElements">the size of the subset.</param>
    /// <param name="random">random number generator. If it is null a number generator is created.</param>
    /// <returns></returns>
    public static IEnumerable<T> RandomSubset<T>(
        this IEnumerable<T> elems,
        int numberOfSubSetElements,
        Random? random = null)
    {
        elems.ThrowIfNull();

        if (0 >= numberOfSubSetElements) return Enumerable.Empty<T>();

        if (null == random) random = new Random();

        var itemCount = elems.Count();
        if (numberOfSubSetElements > itemCount) 
            numberOfSubSetElements = itemCount;

        var indexes = Enumerable.Range(0, itemCount).ToList();
        var shuffledIndexes = indexes.Shuffle(random);

        return elems.Nths(shuffledIndexes
                    .Take(numberOfSubSetElements)
                    .ToArray());
    }

    /// <summary>
    /// Removes the last element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static IEnumerable<T> RemoveTail<T>(this IEnumerable<T> items)
    {
        var it = items.ThrowIfNull()
                      .GetEnumerator();

        while (true)
        {
            if (!it.MoveNext()) yield break;
            var prev = it.Current;

            if (!it.MoveNext()) yield break;

            yield return prev;
            yield return it.Current;
        }
    }

    /// <summary>
    /// Replaces an item at a specified index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="index">The position in the list.</param>
    /// <param name="item">Item that replaces the existing item at a specific index.</param>
    /// <returns></returns>
    public static IEnumerable<T> Replace<T>(this IEnumerable<T> items, int index, T item)
    {
        items.ThrowIfNull();
        item.ThrowIfNull();

        return Replace(items, new[] {(index, item) }, item => item);
    }

    /// <summary>
    /// Replaces the items from a list at specified indexes with the specified values of replaceTuples.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="replaceTuples"></param>
    /// <returns></returns>
    public static IEnumerable<T> Replace<T>(
        this IEnumerable<T> items, 
        IEnumerable<(int index, T item)> replaceTuples)
    {
        items.ThrowIfNull();
        replaceTuples.ThrowIfNull();

        return Replace(items, replaceTuples, item => item);
    }

    /// <summary>
    /// Replaces the items from a list with the values from project.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="project">Gets each item with index.</param>
    /// <returns></returns>
    public static IEnumerable<TResult> Replace<T, TResult>(
        this IEnumerable<T> items, 
        Func<int, T, TResult> project)
    {
        items.ThrowIfNull();
        project.ThrowIfNull();
        
        foreach (var (counter, item) in items.Enumerate())
        {
            yield return project(counter, item);
        }
    }

    /// <summary>
    /// Replaces number of items from a list with items from replace method.
    /// </summary>
    /// <typeparam name="T">Type of the elements of items.</typeparam>
    /// <param name="items">List of items.</param>
    /// <param name="predicate">The selector for replacements.</param>
    /// <param name="replace">Returns items which replace the existing ones. The int is the current number of replacements.</param>
    /// <param name="numberOfReplacements">Number of replaced items.</param>
    /// <returns></returns>
    public static IEnumerable<T> Replace<T>(
        this IEnumerable<T> items,
        Func<T, bool> predicate,
        Func<int, T> replace,
        int numberOfReplacements)
    {
        items.ThrowIfNull();
        replace.ThrowIfNull();

        if (1 > numberOfReplacements) yield break;

        var replaced = 0;

        var it = items.GetEnumerator();
        bool hasNext;

        while(hasNext = it.MoveNext())
        {
            if (replaced >= numberOfReplacements)
            {
                yield return it.Current;
                break;
            }
            if (predicate(it.Current))
            {
                yield return replace(replaced);
                replaced++;
                continue;
            }
            yield return it.Current;
        }

        if(!hasNext) yield break;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Replaces the items from a list with specified <paramref name="replaceTuples"/> including values with their indices.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="replaceTuples">A list of tuples which include the replacement items and indexes.</param>
    /// <param name="project">Projects each item to TResult.</param>
    /// <returns>A list of TResult items.</returns>
    public static IEnumerable<TResult> Replace<T, TResult>(
        this IEnumerable<T> items,
        IEnumerable<(int index, T item)> replaceTuples,
        Func<T, TResult> project)
    {
        items.ThrowIfNull();
        replaceTuples.ThrowIfNull();
        project.ThrowIfNull();

        var orderedTuples = replaceTuples.Where(tuple => 0 <= tuple.index)
                                         .OrderBy(tuple => tuple.index);

        return items.Enumerate()
                    .ZipLeft(orderedTuples,
                            (lhs, rhs) => lhs.Item1 == rhs.Item1 ? BinarySelection.Right : BinarySelection.Left,
                            tuple => project(tuple.item));
    }

    /// <summary>
    /// Enables you to do folds like <see cref="=Aggregate"/>, while collecting the intermediate results.
    /// This is equivalent to the Scala scanLeft function.
    /// </summary>
    /// <typeparam name="T">Type of an item</typeparam>
    /// <param name="items">List of items.</param>
    /// <param name="seed">The initial value.</param>
    /// <param name="func">The scan function</param>
    /// <returns></returns>
    public static IEnumerable<T> ScanLeft<T>(this IEnumerable<T> items, T seed, Func<T, T, T> func)
    {
        var it = items.GetEnumerator();
        var result = seed;

        yield return result;

        while (it.MoveNext())
        {
            result = func(result, it.Current);
            yield return result;
        }
    }

    /// <summary>
    /// Enables you to do folds like <see cref="=Aggregate"/>, while collecting the intermediate results.
    /// This is equivalent to the Scala scanRight function.
    /// </summary>
    /// <typeparam name="T">Type of an item</typeparam>
    /// <param name="items">List of items.</param>
    /// <param name="seed">The initial value.</param>
    /// <param name="func">The scan function</param>
    /// <returns></returns>
    public static IEnumerable<T> ScanRight<T>(this IEnumerable<T> items, T seed, Func<T, T, T> func)
    {
        var it = items.Reverse().GetEnumerator();
        var result = seed;
        var sequence = Enumerable.Empty<T>();
        
        sequence = sequence.Prepend(result);

        while (it.MoveNext())
        {
            result = func(result, it.Current);
            sequence= sequence.Prepend(result);
        }

        return sequence;
    }

    /// <summary>
    /// Returns items only if all items have a valid predicate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="project"></param>
    /// <returns>Returns an empty list if not all items have a valid predicate.</returns>
    public static IEnumerable<TResult> SelectAll<T, TResult>(
        this IEnumerable<T> items,
        Func<T, bool> predicate,
        Func<T, TResult> project)
    {
        predicate.ThrowIfNull();
        project.ThrowIfNull();

        var all = true;
        foreach (var item in items.ThrowIfNull())
        {
            if(!predicate(item))
            {
                all = false;
                break;
            }
        }

        if (!all) yield break;

        foreach (var item in items)
        {
            yield return project(item);
        }
    }

    /// <summary>
    /// Returns only the Error values.
    /// </summary>
    /// <typeparam name="TOk"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<TError> SelectError<TOk, TError>(this IEnumerable<Result<TOk, TError>> items)
    {
        return items.ThrowIfNull()
                    .Where(item => !item.IsOk)
                    .Select(result => result.ToError());
    }


    /// <summary>
    /// Filters all items that are null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> SelectNotNull<T>(this IEnumerable<T?> items)
    {
        foreach (var item in items.ThrowIfNull())
        {
            if (item is T result) yield return result;
        }
    }

    /// <summary>
    /// Returns only the Ok values.
    /// </summary>
    /// <typeparam name="TOk"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<TOk> SelectOk<TOk, TError>(this IEnumerable<Result<TOk, TError>> items)
    {
        return items.ThrowIfNull()
                    .Where(item => item.IsOk)
                    .Select(result => result.ToOk());
    }

    /// <summary>
    /// Returns all Some values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> SelectSome<T>(this IEnumerable<Option<T>> items)
    {
        foreach(var item in items)
        {
            if (item.TryGet(out var value)) yield return value;
        }
    }

    /// <summary>
    /// Returns true if lhs and rhs has the same size and all values in the same order.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="equals"></param>
    /// <returns></returns>
    public static bool SequenceEqual<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs, Func<T, T, bool> equals)
    {
        var itLhs = lhs.GetEnumerator();
        var itRhs = rhs.GetEnumerator();

        while(itLhs.MoveNext())
        {
            var hasNext = itRhs.MoveNext();
            if (!hasNext) return false;

            var l = itLhs.Current;
            var r = itRhs.Current;
            if(!equals(l, r)) return false;
        }

        return !itRhs.MoveNext();
    }

    /// <summary>
    /// Returns Some if exactly one item exists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static Option<T> SingleAsOption<T>(this IEnumerable<T> items)
    {
        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext()) return Option.None<T>();

        var first = it.Current;

        if (it.MoveNext()) return Option.None<T>();

        return Option.Maybe(first);
    }

    /// <summary>
    /// Returns Some if exactly one item exists with this predicate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static Option<T> SingleAsOption<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        items.ThrowIfNull();
        predicate.ThrowIfNull();

        var found = Option.None<T>();
        foreach (var item in items)
        {
            if (predicate(item))
            {
                if (found.IsSome) return Option.None<T>();
                found = Option.Some(item);
            }
        }
        return found;
    }

    /// <summary>
    /// Skips items until all predicates match exactly one time, and then returns the remaining elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicates"></param>
    /// <returns></returns>
    public static IEnumerable<T> SkipUntilSatisfied<T>(this IEnumerable<T> items, params Func<T, bool>[] predicates)
    {
        items.ThrowIfNull();

        var invasivePredicates = new InvasiveVerification<T>(predicates);
        var isNone = new TriState();
        var isTrue = new TriState(true);

        foreach (var item in items)
        {
            var triState = invasivePredicates.Verify(item);
            if (isNone != triState) continue;

            yield return item;
        }
    }

    /// <summary>
    /// Creates a list of enumerables of size length.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="length">this is the chop size.</param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> Slice<T>(this IEnumerable<T> items, int length)
    {
        items.ThrowIfNull();
        var sliced = Enumerable.Empty<T>();

        var itemCounter = 0;
        foreach (var item in items)
        {
            sliced = sliced.Append(item);
            itemCounter++;
            if (itemCounter == length)
            {
                itemCounter = 0;
                yield return sliced;

                sliced = Enumerable.Empty<T>();
            }
        }

        if (0 < itemCounter) yield return sliced;
    }

    /// <summary>
    /// Creates a list of enumerables. For each predicate an enumerable.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicates"></param>
    /// <returns></returns>
    public static IEnumerable<IEnumerable<T>> Slice<T>(
        this IEnumerable<T> items,
        params Func<T, bool>[] predicates)
    {
        items.ThrowIfNull();
        if (0 == predicates.Length) return Enumerable.Empty<IEnumerable<T>>();

        var splittedItems = new Dictionary<int, IEnumerable<T>>();

        for (var i = 0; i < predicates.Length; i++)
            splittedItems.Add(i, Enumerable.Empty<T>());

        foreach (var item in items)
        {
            for (var i = 0; i < predicates.Length; i++)
                if (predicates[i](item))
                    splittedItems[i] = splittedItems[i].Append(item);
        }

        return splittedItems.Select(x => x.Value);
    }

    /// <summary>
    /// Returns the object with the smallest selector value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="selector">Returns the value to compare.</param>
    /// <returns></returns>
    public static T Smallest<T>(this IEnumerable<T> items, Func<T, IComparable> selector)
    {
        selector.ThrowIfNull();

        return items.Aggregate((a, b) => selector(a).CompareTo(selector(b)) == -1 ? a : b);
    }

    /// <summary>
    /// Returns the object with the smallest selector value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TSelector"></typeparam>
    /// <param name="items"></param>
    /// <param name="selector">Returns the value to compare.</param>
    /// <returns></returns>
    public static T Smallest<T, TSelector>(this IEnumerable<T> items, Func<T, TSelector> selector)
        where TSelector : IComparable<TSelector>
    {
        selector.ThrowIfNull();

        return items.Aggregate((a, b) => selector(a).CompareTo(selector(b)) == -1 ? a : b);
    }

    /// <summary>
    /// This method implements the Fisher-Yates Shuffle.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="random"></param>
    /// <returns></returns>
    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> items, Random? random = null)
    {
        if (null == random) random = new Random();

        return items.ThrowIfEmpty()
                    .ToArray()
                    .Shuffle(random);
    }

    public static IEnumerable<T> Swap<T>(this IEnumerable<T> items, int lhsIndex, int rhsIndex)
    {
        lhsIndex.ThrowIfOutOfRange(() => 0 > lhsIndex);
        rhsIndex.ThrowIfOutOfRange(() => 0 > rhsIndex);

        var (min, max) = MathExt.MinMax(lhsIndex, rhsIndex);

        var swappedItems = Enumerable.Empty<T>();

        var minItem = Option.None<T>();

        var lhs = Enumerable.Empty<T>();

        var enumerated = items.Enumerate();

        var it = enumerated.GetEnumerator();
        bool swapped = false;
        while(it.MoveNext())
        {
            var (counter, item) = it.Current;

            if(!swapped)
            {
                if (minItem.IsNone)
                {
                    if (counter == min)
                    {
                        minItem = Option.Some(item);
                        lhs = swappedItems;
                        swappedItems = Enumerable.Empty<T>();
                        continue;
                    }
                }
                else
                {
                    if (counter == max)
                    {
                        var rhs = swappedItems.Append(minItem.OrThrow());
                        lhs = lhs.Append(item);
                        swappedItems = lhs.Concat(rhs);
                        swapped = true;
                        continue;
                    }
                }
            }

            swappedItems = swappedItems.Append(item);
        }

        foreach(var item in swappedItems)
        {
            yield return item;
        }
    }

    /// <summary>
    /// Returns the symmetric difference of two lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="retainDuplicates">If true then duplicates are taken into account.</param>
    /// <returns></returns>
    public static IEnumerable<T> SymmetricDifference<T>(
        this IEnumerable<T> lhs,
        IEnumerable<T> rhs,
        bool retainDuplicates = false)
    {
        if (!retainDuplicates)
        {
            var set = new HashSet<T>(lhs);
            set.SymmetricExceptWith(rhs);
            return set;
        }

        return lhs.ExceptWithDuplicates(rhs).Concat(rhs.ExceptWithDuplicates(lhs));
    }

    /// <summary>
    /// Returns the symmetric difference of two lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TKey">The type of the selector value.</typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="selector">Selects the value of each item to compare.</param>
    /// <param name="retainDuplicates"></param>
    /// <returns></returns>
    public static IEnumerable<T> SymmetricDifference<T, TKey>(
        this IEnumerable<T> lhs,
        IEnumerable<T> rhs,
        Func<T, TKey?> selector,
        bool retainDuplicates = false)
    {
        if (!retainDuplicates)
        {
            var set = new HashSet<T>(lhs, new LambdaEqualityComparer<T, TKey>(selector));
            set.SymmetricExceptWith(rhs);
            return set;
        }

        return lhs.ExceptWithDuplicates(rhs, selector).Concat(rhs.ExceptWithDuplicates(lhs, selector));
    }

    public static IEnumerable<T> SymmetricDifferenceSorted<T>(
        this IEnumerable<T> lhs,
        IEnumerable<T> rhs,
        bool retainDuplicates = false)
        where T : IComparable<T>

    {
        if (!retainDuplicates)
        {
            var set = new HashSet<T>(lhs);
            set.SymmetricExceptWith(rhs);
            return set;
        }

        return lhs.ExceptWithDuplicatesSorted(rhs).Concat(rhs.ExceptWithDuplicatesSorted(lhs));
    }

    /// <summary>
    /// Returns a tuple of taken elements and remaining elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TTaken"></typeparam>
    /// <param name="items"></param>
    /// <param name="numberOfElements"></param>
    /// <param name="projectTaken">Projection of taken elements.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static (TTaken taken, IEnumerable<T> remaining) Take<T, TTaken>(
        this IEnumerable<T> items, 
        int numberOfElements,
        Func<IEnumerable<T>, TTaken> projectTaken)
    {
        if (0 > numberOfElements) throw new ArgumentOutOfRangeException(nameof(numberOfElements), "cannot be negative");

        var it = items.GetEnumerator();
        var itemCounter = 0;

        var takenElements = Enumerable.Empty<T>();

        while(it.MoveNext())
        {
            itemCounter++;

            if (itemCounter >= numberOfElements) break;
            
            takenElements = takenElements.Append(it.Current);
        }

        var remaining = Enumerable.Empty<T>();

        while (it.MoveNext())
        {
            remaining = remaining.Append(it.Current);
        }

        return (projectTaken(takenElements), remaining);
    }

    /// <summary>
    /// Returns a tuple of taken elements and remaining elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TTaken"></typeparam>
    /// <typeparam name="TRemaining"></typeparam>
    /// <param name="items"></param>
    /// <param name="numberOfElements"></param>
    /// <param name="projectTaken">Projection of taken elements.</param>
    /// <param name="projectRemaining">Projection of remaining elements.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static (TTaken taken, TRemaining remaining) Take<T, TTaken, TRemaining>(
        this IEnumerable<T> items,
        int numberOfElements,
        Func<IEnumerable<T>, TTaken> projectTaken,
        Func<IEnumerable<T>, TRemaining> projectRemaining)
    {
        if (0 > numberOfElements) throw new ArgumentOutOfRangeException(nameof(numberOfElements), "cannot be negative");

        var it = items.GetEnumerator();
        var itemCounter = 0;

        var takenElements = Enumerable.Empty<T>();
        var remaining = Enumerable.Empty<T>();

        while (it.MoveNext())
        {
            if (itemCounter >= numberOfElements)
            {
                remaining = remaining.Append(it.Current);
                break;
            }

            takenElements = takenElements.Append(it.Current);
            itemCounter++;
        }


        while (it.MoveNext())
        {
            remaining = remaining.Append(it.Current);
        }

        return (projectTaken(takenElements), projectRemaining(remaining));
    }

    /// <summary>
    /// Returns at least numberOfElements elements. If the number of elements is smaller, an empty enumerable is returned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items">Elements of the list.</param>
    /// <param name="numberOfElements"></param>
    /// <returns></returns>
    public static IEnumerable<T> TakeAtLeast<T>(this IEnumerable<T> items, int numberOfElements)
    {
        if (0 > numberOfElements) throw new ArgumentOutOfRangeException(nameof(numberOfElements), "cannot be negative");

        var it = items.GetEnumerator();

        var required = new List<T>();
        while (it.MoveNext())
        {
            required.Add(it.Current);
            if (required.Count == numberOfElements) break;
        }

        if (required.Count != numberOfElements) yield break;

        foreach (var item in required)
        {
            yield return item;
        }

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Returns at least numberOfElements elements. If the number of elements is smaller, an empty enumerable is returned.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="numberOfElements"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> TakeAtLeast<T>(this IEnumerable<T> items, int numberOfElements, Func<T, bool> predicate)
    {
        if (0 > numberOfElements) throw new ArgumentOutOfRangeException(nameof(numberOfElements), "cannot be negative");
        predicate.ThrowIfNull();

        var it = items.GetEnumerator();

        var required = new List<T>();
        while (it.MoveNext())
        {
            if (required.Count == numberOfElements) break;

            if (predicate(it.Current))
                required.Add(it.Current);
        }

        if (required.Count != numberOfElements) yield break;

        foreach (var item in required)
        {
            yield return item;
        }

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Returns <paramref name="numberOfElements"/> if items contains exactly <paramref name="numberOfElements"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="numberOfElements"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static IEnumerable<T> TakeExact<T>(this IEnumerable<T> items, int numberOfElements)
    {
        if (0 > numberOfElements)
            throw new ArgumentOutOfRangeException(nameof(numberOfElements), "cannot be negative");

        var required = new List<T>();

        var it = items.GetEnumerator();
        while (it.MoveNext())
        {
            required.Add(it.Current);
            if (required.Count == numberOfElements) break;
        }

        if (required.Count != numberOfElements) yield break;
        if (it.MoveNext()) yield break;

        foreach (var item in required)
        {
            yield return item;
        }
    }

    /// <summary>
    /// Returns elements from a sequence as long as a specified condition is false, and then skips the remaining elements.
    /// This is the counterpart to TakeWhile.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <param name="inclusive">if true the matching item is included.</param>
    /// <returns></returns>
    public static IEnumerable<T> TakeUntil<T>(
        this IEnumerable<T> items, 
        Func<T, bool> predicate, 
        bool inclusive = false)
    {
        foreach (var item in items)
        {
            if (predicate(item))
            {
                if (inclusive) yield return item;
                yield break;
            }
            yield return item;
        }
    }

    /// <summary>
    /// Returns items until all predicates match exactly one time.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicates"></param>
    /// <returns></returns>
    public static IEnumerable<T> TakeUntilSatisfied<T>(this IEnumerable<T> items, params Func<T, bool>[] predicates)
    {
        items.ThrowIfNull();

        var invasivePredicates = new InvasiveVerification<T>(predicates);
        var isNone = new TriState();
        var isTrue = new TriState(true);

        foreach (var item in items)
        {
            var triState = invasivePredicates.Verify(item);
            if (isNone == triState) yield break;
            if (isTrue == triState) yield return item;
        }
    }

    /// <summary>
    /// Returns true if all predicates are true.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="elems"></param>
    /// <param name="predicates"></param>
    /// <returns></returns>
    public static bool TryWhere<T>(this IEnumerable<T> items, out IEnumerable<T> elems, params Func<T, bool>[] predicates)
    {
        items.ThrowIfNull();
        elems = Enumerable.Empty<T>();

        var allFulfilled = new bool[predicates.Length];
        foreach (var item in items)
        {
            var index = 0;

            var fulfilled = false;
            foreach (var predicate in predicates)
            {
                if (predicate(item))
                {
                    allFulfilled[index] = true;
                    fulfilled = true;
                }

                index++;
            }

            if (fulfilled)
                elems.Append(item);
        }

        return allFulfilled.All(x => x == true);
    }

    /// <summary>
    /// Returns true if all elements fulfilling the predicate. Stops immediately if predicate returns false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <param name="elems"></param>
    /// <returns></returns>
    public static bool TryWhereAll<T>(
        this IEnumerable<T> items, 
        Func<T, bool> predicate,
        out IEnumerable<T> elems)
    {
        items.ThrowIfNull();
        predicate.ThrowIfNull();

        elems = Enumerable.Empty<T>();

        foreach (var item in items)
        {
            if (!predicate(item))
            {
                return false;
            }

            elems.Append(item);
        }

        return true;
    }

    /// <summary>
    /// Checks if all items fulfill the predicate. If there is an item not matching the predicate an exception is thrown.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> WhereAll<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        predicate.ThrowIfNull();

        foreach (var item in items.ThrowIfNull())
        {
            if (!predicate(item))
                throw new ArgumentOutOfRangeException($"{item}");

            yield return item;
        }
    }

    /// <summary>
    /// Returns all optional values that are some.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> WhereSome<T>(this IEnumerable<Option<T>> items)
    {
        return items.Where(item => item.IsSome).Select(opt => opt.OrThrow());
    }

    /// <summary>
    /// Works like Zip with comparer. Maps only maching items.
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="comparer"></param>
    /// <param name="resultSelector"></param>
    /// <returns></returns>
    public static IEnumerable<TResult> Zip<T1, T2, TResult>(
        this IEnumerable<T1> first,
        IEnumerable<T2> second,
        Func<T1, T2, bool> comparer,
        Func<T1, T2, TResult> resultSelector)
    {
        second.ThrowIfNull();
        comparer.ThrowIfNull();
        resultSelector.ThrowIfNull();

        return from firstItem in first
               from secondItem in second
               where comparer(firstItem, secondItem)
               select resultSelector(firstItem, secondItem);
    }

    /// <summary>
    /// Merges two lists. If lhs has more elements than rhs, the lhs elements are returned until the end.
    /// If rhs has more elements than lhs, the rhs elements are skipped.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="selector"></param>
    /// <param name="projection"></param>
    /// <returns></returns>
    public static IEnumerable<TResult> ZipLeft<T, TResult>(
        this IEnumerable<T> lhs,
        IEnumerable<T> rhs,
        Func<T, T, BinarySelection> selector,
        Func<T, TResult> projection)
    {
        var itRhs = rhs.GetEnumerator();

        var hasNext = Fused.Value(true).BlowIfChanged();

        var selection = BinarySelection.Right;
        foreach (var item in lhs)
        {
            if(hasNext.Value)
            {
                if(BinarySelection.Right == selection) hasNext.Value = itRhs.MoveNext();

                if(hasNext.Value)
                {
                    selection = selector(item, itRhs.Current);
                    
                    if (BinarySelection.Right == selection)
                    {
                        yield return projection(itRhs.Current);
                        continue;
                    }
                }
            }

            yield return projection(item);
        }
    }
}
