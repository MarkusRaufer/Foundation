namespace Foundation.Collections.Generic;

using Foundation;
using Foundation.ComponentModel;
//using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

public static class EnumerableExtensions
{
    private class ElseIf<T> : IElseIf<T>
    {
        private readonly IEnumerable<T> _items;

        public ElseIf(IEnumerable<T> items)
        {
            _items = items.ThrowIfNull();
        }

        public IEnumerable<T> Else() => _items;

        public IEnumerable<T> Else(Action<T> action)
        {
            foreach (var item in _items)
            {
                action(item);
                yield return item;
            }
        }

        public void EndIf()
        {
            foreach (var _ in Else())
            {
            }
        }

        IElseIf<T> IElseIf<T>.ElseIf(Func<T, bool> condition, Action<T> action)
        {
            return _items.If(condition, action);
        }
    }

    private class ElseResult<T, TResult> : IElse<T, TResult>
    {
        private readonly IEnumerable<T> _items;
        private readonly Func<T, bool> _predicate;
        private readonly Func<T, TResult> _mapIf;

        public ElseResult(
            IEnumerable<T> items,
            Func<T, bool> predicate,
            Func<T, TResult> mapIf)
        {
            _items = items.ThrowIfNull();
            _predicate = predicate.ThrowIfNull();
            _mapIf = mapIf.ThrowIfNull();
        }

        public IEnumerable<TResult> Else(Func<T, TResult> map)
        {
            foreach (var item in _items)
            {
                yield return _predicate(item) ? _mapIf(item) : map(item);
            }
        }
    }

    /// <summary>
    /// Adds an item if the list is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static IEnumerable<T> AddIfEmpty<T>(this IEnumerable<T> items, Func<T> factory)
    {
        factory.ThrowIfNull();

        var it = items.GetEnumerator();
        if (!it.MoveNext())
        {
            yield return factory();
        }
        else
        {
            yield return it.Current;

            while (it.MoveNext()) yield return it.Current;
        }
    }

    /// <summary>
    /// Executes action after every item except the last one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<T> AfterEach<T>(this IEnumerable<T> items, Action action)
    {
        action.ThrowIfNull();

        var it = items.GetEnumerator();
        var next = it.MoveNext();
        while (next)
        {
            yield return it.Current;

            next = it.MoveNext();
            if (next)
                action();
        }
    }

    /// <summary>
    /// Executes action after every item except the last one.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action">The parameter is the item before the action.</param>
    /// <returns>List of items</returns>
    public static IEnumerable<T> AfterEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        action.ThrowIfNull();

        var it = items.GetEnumerator();
        var next = it.MoveNext();
        while (next)
        {
            var prevItem = it.Current;
            yield return prevItem;

            next = it.MoveNext();
            if (next)
                action(prevItem);
        }
    }

    /// <summary>
    /// Executes action after first item except there is no second item.
    /// </summary>
    /// <typeparam name="T">The type of the items</typeparam>
    /// <param name="items">List of items</param>
    /// <param name="action">The action which is executed after the first item.</param>
    /// <returns></returns>
    public static IEnumerable<T> AfterFirst<T>(this IEnumerable<T> items, Action action)
    {
        action.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext()) yield break;
        var first = it.Current;
        yield return first;

        if (!it.MoveNext()) yield break;
        action();
        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Executes action after first item only if there is a second item.
    /// </summary>
    /// <typeparam name="T">Type of item</typeparam>
    /// <param name="items">List of items</param>
    /// <param name="action">The action which is executed after the first item.
    /// The argument is the first item.</param>
    /// <returns></returns>
    public static IEnumerable<T> AfterFirst<T>(this IEnumerable<T> items, Action<T> action)
    {
        action.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext()) yield break;
        var first = it.Current;
        yield return first;

        if (!it.MoveNext()) yield break;
        action(first);
        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

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
    /// returns doublets of a list. If there are e.g. three of an item, 2 will returned.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="items">the list of items</param>
    /// <returns>all doublets</returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> items)
    {
        if (null == items) throw new ArgumentNullException(nameof(items));

        var set = new HashSet<T>();
        foreach(var item in items)
        {
            if (!set.Add(item)) yield return item;
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
    /// Returns an empty enumerable if items is null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> items)
    {
        return items ?? Enumerable.Empty<T>();
    }

    /// <summary>
    /// Enumerates items starting from seed.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="seed"></param>
    /// <returns>Returns tuples (item, counter).</returns>
    public static IEnumerable<(T item, int counter)> Enumerate<T>(this IEnumerable<T> items, int seed = 0)
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
    public static IEnumerable<(T item, TValue counter)> Enumerate<T, TValue>(this IEnumerable<T> items, Func<T, TValue> createValue)
    {
        createValue.ThrowIfNull();

        foreach (var item in items)
            yield return (item, createValue(item));
    }

    /// <summary>
    /// Enumerates items. Starting from Min until Max. If the counter reaches Max it starts again from Min.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="minMax">Allows also negative numbers.</param>
    /// <returns></returns>
    public static IEnumerable<(T item, int counter)> Enumerate<T>(this IEnumerable<T> items, int min, int max)
    {
        var i = min;
        foreach (var item in items)
        {
            if (i > max) i = min;

            yield return (item, i);
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
    public static IEnumerable<(T item, int counter)> Enumerate<T>(this IEnumerable<T> items, System.Range range)
    {
        if (range.End.IsFromEnd) throw new ArgumentException($"{range.End}.IsFromEnd is not allowed");

        var i = range.Start.IsFromEnd ? 0 : range.Start.Value;
        foreach (var item in items)
        {
            if (!range.End.IsFromEnd && range.End.Value < i) i = range.Start.Value;

            yield return (item, i);
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
    public static IEnumerable<(T, TValue1, TValue2)> Enumerate<T, TValue1, TValue2>(
        this IEnumerable<T> items,
        Func<T, TValue1> createValue1,
        Func<T, TValue2> createValue2)
    {
        foreach (var item in items)
            yield return (item, createValue1(item), createValue2(item));
    }

    /// <summary>
    /// Returns all items except item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static IEnumerable<T> Except<T>(this IEnumerable<T> items, T? item)
    {
        Func<T, bool> equals = null == item 
            ? x => null == x || x.Equals(item)
            : x => null != x && x.Equals(item);

        foreach (var elem in items)
        {
            if (equals(elem)) continue;

            yield return elem;
        }
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
        var lhsMap = new MultiValueMap<NullableKey<T>, T>();
        lhs.ForEach(x => lhsMap.Add(NullableKey.New(x), x));

        var rhsMap = new MultiValueMap<NullableKey<T>, T>();
        rhs.ForEach(x => rhsMap.Add(NullableKey.New(x), x));

        foreach (var left in lhsMap)
        {
            if (rhsMap.TryGetValue(left.Key, out T? right))
            {
                rhsMap.Remove(left.Key, right!);
                continue;
            }

            yield return left.Value;
        }
    }

    /// <summary>
    /// Returns all elements from <paramref name="lhs"/> which are not in <paramref name="rhs"/> including duplicates.
    /// The smaller list should be rhs.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static IEnumerable<T> ExceptWithDuplicatesSorted<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs)
        where T : IComparable<T>
    {
        var lhsList = lhs.ToArray();
        var rhsList = rhs.ToArray();
        Array.Sort(lhsList);
        Array.Sort(rhsList);

        var index = 0;
        var length = rhsList.Length;
        var notInRhs = false;
        T? prev = default;

        foreach (var left in lhsList.OnFirst(x => prev = x))
        {
            if (!prev.EqualsNullable(left))
            {
                notInRhs = false;
                index = 0;
                length = rhsList.Length;
            }

            if (!notInRhs)
            {
                var foundIndex = Array.BinarySearch(rhsList, index, length, left);
                if (-1 < foundIndex)
                {
                    index = foundIndex + 1;
                    length = rhsList.Length - index;

                    prev = left;
                    continue;
                }
                notInRhs = true;
            }
            prev = left;
            yield return left;
        }
    }

    /// <summary>
    /// Returns all elements from <paramref name="lhs"/> which are not in <paramref name="rhs"/> including duplicates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static IEnumerable<T> ExceptWithDuplicatesSorted<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs, IComparer<T> comparer)
    {
        var lhsList = lhs.ToArray();
        var rhsList = rhs.ToArray();
        Array.Sort(lhsList, comparer);
        Array.Sort(rhsList, comparer);

        var index = 0;
        var length = rhsList.Length;
        var notInRhs = false;
        T? prev = default;

        foreach (var left in lhsList.OnFirst(x => prev = x))
        {
            if (!prev.EqualsNullable(left))
            {
                notInRhs = false;
                index = 0;
                length = rhsList.Length;
            }

            if (!notInRhs)
            {
                var foundIndex = Array.BinarySearch(rhsList, index, length, left);
                if (-1 < foundIndex)
                {
                    index = foundIndex + 1;
                    length = rhsList.Length - index;

                    prev = left;
                    continue;
                }
                notInRhs = true;
            }
            prev = left;
            yield return left;
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
            var option = selector(item);
            if (option.IsSome) yield return option.OrThrow();
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
                    .Match(x => x.ToOption<TResult>(),
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

    /// <summary>
    /// Returns a list of items. The predicate allows filtering items by index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> FromIndex<T>(this IEnumerable<T> items, Func<long, bool> predicate)
    {
        predicate.ThrowIfNull();

        long i = 0;
        return items.Where(item => predicate(i++));
    }

    public static IElseIf<T> If<T>(
        this IEnumerable<T> items,
        Func<T, bool> predicate,
        Action<T> action)
    {
        predicate.ThrowIfNull();
        action.ThrowIfNull();

        var @else = Enumerable.Empty<T>();

        foreach (var item in items)
        {
            if (predicate(item))
            {
                action(item);
                continue;
            }
            @else = @else.Append(item);
        }
        return new ElseIf<T>(@else);
    }

    public static IElse<T, TResult> If<T, TResult>(
        this IEnumerable<T> items,
        Func<T, bool> predicate, 
        Func<T, TResult> map)
    {
        predicate.ThrowIfNull();
        map.ThrowIfNull();

        return new ElseResult<T, TResult>(items, predicate, map);
    }

    /// <summary>
    /// Returns lhs. If lhs is empty rhs is returned;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static IEnumerable<T> IfEmpty<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs)
    {
        rhs.ThrowIfNull();

        var lIt = lhs.GetEnumerator();
        if (!lIt.MoveNext())
        {
            foreach (var r in rhs)
            {
                yield return r;
            }
            yield break;
        }

        yield return lIt.Current;

        while (lIt.MoveNext())
        {
            yield return lIt.Current;
        }
    }

    /// <summary>
    /// If lhs is empty it returns the items from factory otherwise lhs.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="factory"></param>
    /// <returns></returns>
    public static IEnumerable<T> IfEmpty<T>(this IEnumerable<T> lhs, Func<IEnumerable<T>> factory)
    {
        factory.ThrowIfNull();

        var lit = lhs.GetEnumerator();
        if (!lit.MoveNext())
        {
            foreach (var r in factory())
            {
                yield return r;
            }
            yield break;
        }

        yield return lit.Current;

        while (lit.MoveNext())
        {
            yield return lit.Current;
        }
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
    /// Intersects to lists using a compare function.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="compare"></param>
    /// <returns></returns>
    public static IEnumerable<T> Intersect<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs, Func<T?, T?, bool> compare)
    {
        return lhs.Intersect(rhs, new LambdaEqualityComparer<T>(compare));
    }

    /// <summary>
    /// Intersects all collections.
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

    public static IEnumerable<T> Intersect<T>(this IEnumerable<IEnumerable<T>> collections, Func<T?, T?, bool> compare)
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
    /// Returns true if all items are in an ascending order.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static bool IsInAscendingOrder<T>(this IEnumerable<T> items)
        where T : IComparable<T>
    {
        items.ThrowIfNull();

        var it = items.GetEnumerator();

        if (!it.MoveNext()) return true;

        var prev = it.Current;
        while (it.MoveNext())
        {
            if (1 == prev.CompareTo(it.Current))
                return false;

            prev = it.Current;
        }

        return true;
    }

    /// <summary>
    /// Returns true if all items are in an ascending order.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="compare"></param>
    /// <returns></returns>
    public static bool IsInAscendingOrder<T>(this IEnumerable<T> items, Func<T, T, CompareResult> compare)
    {
        items.ThrowIfNull();
        compare.ThrowIfNull();

        var it = items.GetEnumerator();

        if (!it.MoveNext()) return true;

        var prev = it.Current;
        while (it.MoveNext())
        {
            if (CompareResult.Greater == compare(prev, it.Current))
                return false;

            prev = it.Current;
        }

        return true;
    }

    /// <summary>
    /// Returns true, if all elements of lhs appear in rhs and the number of items and their occurrency are same.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <typeparam name="T"></typeparam>
    public static bool IsEqualToSet<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs)
    {
        if (null == lhs) return null == rhs;
        if (null == rhs) return false;

        if(typeof(T).ImplementsGenericInterface(typeof(IComparable<T>)))
            return !lhs.SymmetricDifference(rhs, retainDuplicates: true).Any();
        return !lhs.SymmetricDifference(rhs, retainDuplicates: true).Any();
    }

    /// <summary>
    /// Returns true if items is null or empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty<T>(this IEnumerable<T>? items)
    {
        if (items == null) return true;
        return !items.Any();
    }

    /// <summary>
    /// Checks if <paramref name="rhs"/> is a subset of <paramref name="lhs"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool IsSubsetOf<T>(this IEnumerable<T> lhs, IEnumerable<T> rhs)
    {
        rhs.ThrowIfNull();

        var search = new HashSet<T>(lhs);
        return search.IsSubsetOf(rhs);

    }

    /// <summary>
    /// Iterates to all items.
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
    /// Returns the last item of source if not empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static Option<T> LastAsOption<T>(this IEnumerable<T> source)
    {
        var it = source.ThrowIfNull().GetEnumerator();

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

        var lhsMap = new MultiValueMap<TKey, (T, int)>();
        var rhsMap = new MultiValueMap<TKey, (T, int)>();

        foreach (var (left, right) in tuples)
        {
            lhsMap.AddUnique(keySelector(left.item), left);
            rhsMap.AddUnique(keySelector(right.item), right);
        }

        return (lhsMap.Values.Select(t => t.Item1), rhsMap.Values.Select(t => t.Item1));
    }

    /// <summary>
    /// Returns all matching items between two lists. The items are returned as tuples with their occurrencies.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns>matching items with their occurrencies.</returns>
    public static IEnumerable<((T? item, int count) lhs, (T? item, int count) rhs)> MatchWithOccurrencies<T>(
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

        
        var lhsMap = new MultiValueMap<NullableKey<T>, (T?, int)>();
        var rhsMap = new MultiValueMap<NullableKey<T>, (T?, int)>();

        foreach (var (left, right) in tuples)
        {
            lhsMap.AddUnique(left.item, (left.item.Value, left.counter));
            rhsMap.AddUnique(right.item, (right.item.Value, right.counter));
        }

        foreach (var pair in lhsMap)
        {
            var lhsTuples = lhsMap.GetValues(pair.Key);
            var rhsTuples = rhsMap.GetValues(pair.Key);

            var leftTuple = (pair.Key.Value, lhsTuples.Count());
            var rightTuple = (pair.Key.Value, rhsTuples.Count());

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
    public static IEnumerable<((T item, int count) lhs, (T item, int count) rhs)> MatchWithOccurrencies<T, TKey>(
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

        var lhsMap = new MultiValueMap<TKey, (T, int)>();
        var rhsMap = new MultiValueMap<TKey, (T, int)>();

        foreach (var (left, right) in tuples)
        {
            var key = keySelector(left.item);

            lhsMap.AddUnique(key, left);
            rhsMap.AddUnique(key, right);
        }

        foreach(var pair in lhsMap)
        {
            var lhsTuples = lhsMap.GetValues(pair.Key);
            var rhsTuples = rhsMap.GetValues(pair.Key);

            var leftTuple = (pair.Value.Item1, lhsTuples.Count());
            var rightTuple = (pair.Value.Item1, rhsTuples.Count());

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

        var hasValue = false;

        foreach (var item in items
        .OnFirst(i =>
        {
            min = i;
            max = i;
            hasValue = true;
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

        return (hasValue && null != min && null != max)
            ? Option.Some((min, max)) 
            : Option.None<(T, T)>() ;
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
        KeyValuePair<TSelector, T> min = default;
        KeyValuePair<TSelector, T> max = default;

        var hasValue = false;

        foreach (var item in items
            .OnFirst(i =>
        {
            min = Pair.New(selector(i), i);
            max = Pair.New(selector(i), i);
            hasValue = true;
        })
        .Skip(1))
        {
            var selectorValue = selector(item);
            if (null == selectorValue) continue;

            if (-1 == selectorValue.CompareTo(min.Key))
            {
                min = Pair.New(selectorValue, item);
                continue;
            }

            if (1 == selectorValue.CompareTo(max.Key))
                max = Pair.New(selectorValue, item);
        }

        return hasValue && null != min.Value && null != max.Value
            ? Option.Some((min.Value, max.Value))
            : Option.None<(T, T)>();
    }

    /// <summary>
    /// Returns true if the list contains more than one element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static bool MoreThanOne<T>(this IEnumerable<T> items)
    {
        var it = items.GetEnumerator();
        if (!it.MoveNext()) return false;
        if (!it.MoveNext()) return false;
        return true;
    }

    /// <summary>
    /// returns the elements that occure most frequently.
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
    /// Filters null items. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> NotNull<T>(this IEnumerable<T?>? items)
    {
        if (null == items) yield break;

        foreach(var item in items)
        {
            if (null == item) continue;

            yield return item;
        }
    }

    /// <summary>
    /// Returns all items not of type <paramref name="types"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="selector"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static IEnumerable<T> NotOfType<T>(this IEnumerable<T> items, params Type[] types)
    {
        return items.NotOfType(x => x, types);
    }

    /// <summary>
    /// Returns all items not of type <paramref name="types"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="selector"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static IEnumerable<TResult> NotOfType<T, TResult>(
        this IEnumerable<T> items,
        Func<T, TResult> selector,
        params Type[] types)
    {
        foreach (var item in items.ThrowIfNull())
        {
            if (null == item) continue;

            var itemType = item.GetType();

            if (types.Any(t => t.Equals(itemType) || t.IsAssignableFrom(itemType))) continue;

            yield return selector(item);
        }
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
    public static IEnumerable<T> Nths<T>(this IEnumerable<T> items, params int[] indexes)
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
    /// Returns all items whose type matches with a list of types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="types"></param>
    /// <returns></returns>
    public static IEnumerable<T> OfType<T, TResult>(this IEnumerable<T> items, params Type[] types)
    {
        return items.OfType(x => x, types);
    }

    /// <summary>
    /// Returns all items with their occurrencies.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="items">list of items</param>
    /// <returns>Returns all items with their occurrencies.</returns>
    public static IEnumerable<(T value, int quantity)> Occurrencies<T>(this IEnumerable<T> items)
    {
        var set = new HashSet<Countable<T>>(new NullableEqualityComparer<Countable<T>>(
                        (x, y) => x.ValueEquals(y), 
                        x => x.GetValueHashCode()));

        foreach(var item in items)
        {
            var countable = Countable.New(item);
            if (set.TryGetValue(countable, out var existing))
            {
                existing.Inc();
                continue;
            }

            countable.Inc();
            set.Add(countable);
        }

        foreach(var countable in set)
        {
            yield return (value: countable.Value, quantity: countable.Count);
        }
    }

    /// <summary>
    /// Returns all items with their occurrencies.
    /// </summary>
    /// <typeparam name="T">Item type</typeparam>
    /// <param name="items">List of items</param>
    /// <param name="comparer">custom comparer.</param>
    /// <returns>Returns all items with their occurrencies.</returns>
    public static IEnumerable<(T value, int quantity)> Occurrencies<T>(
        this IEnumerable<T> items,
        IEqualityComparer<T> comparer)
    {
        var set = new HashSet<Countable<T>>(new NullableEqualityComparer<Countable<T>>(
                        (x, y) => comparer.Equals(x.Value, y.Value),
                        x => comparer.GetHashCode(x.Value)));

        foreach (var item in items)
        {
            var countable = Countable.New(item);
            if (set.TryGetValue(countable, out var existing))
            {
                existing.Inc();
                continue;
            }

            countable.Inc();
            set.Add(countable);
        }

        foreach (var countable in set)
        {
            yield return (value: countable.Value, quantity: countable.Count);
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
        foreach (var item in items.ThrowIfNull())
        {
            if (null == item) continue;

            var type = item.GetType();
            if (types.Any(t => t.Equals(type) || t.IsAssignableFrom(type)))
                yield return selector(item);
        }
    }

    /// <summary>
    /// Calls action on each element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        action.ThrowIfNull();

        foreach (var item in items.ThrowIfNull())
        {
            action(item);
            yield return item;
        }
    }

    /// <summary>
    /// Calls action if items is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnEmpty<T>(this IEnumerable<T> items, Action action)
    {
        action.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext())
        {
            action();
            yield break;
        }
        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Returns a value if list is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="onEmpty"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnEmpty<T>(this IEnumerable<T> items, Func<T> onEmpty)
    {
        onEmpty.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext())
        {
            yield return onEmpty();
            yield break;
        }
        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Calls action on first item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnFirst<T>(this IEnumerable<T> items, Action action)
    {
        action.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext()) yield break;
        action();
        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Is called on first item.
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    /// <param name="items">List of items</param>
    /// <param name="action">Is called on first item.</param>
    /// <returns></returns>
    public static IEnumerable<T> OnFirst<T>(this IEnumerable<T> items, Action<T> action)
    {
        action.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (!it.MoveNext()) yield break;

        action(it.Current);
        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Calls action on last item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnLast<T>(this IEnumerable<T> items, Action action)
    {
        action.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (it.MoveNext())
        {
            while (true)
            {
                yield return it.Current;
                if (!it.MoveNext())
                {
                    action();
                    yield break;
                }
            }
        }
    }

    /// <summary>
    /// Calls action on last item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnLast<T>(this IEnumerable<T> items, Action<T> action)
    {
        action.ThrowIfNull();

        var it = items.ThrowIfNull()
                      .GetEnumerator();

        if (it.MoveNext())
        {
            while (true)
            {
                var last = it.Current;
                yield return last;
                if (!it.MoveNext())
                {
                    action(last);
                    yield break;
                }
            }
        }
    }

    /// <summary>
    /// Calls action when reached an index during iteration.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="index"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnNth<T>(this IEnumerable<T> items, int index, Action<T> action)
    {
        action.ThrowIfNull();

        var counter = 0;
        foreach (var item in items.ThrowIfNull())
        {
            if (index == counter)
                action(item);

            yield return item;
            counter++;
        }
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
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <param name="match">projection to TResult.</param>
    /// <param name="noMatch">projection to TResult.</param>
    /// <returns></returns>
    public static (IEnumerable<TResult> matching, IEnumerable<TResult> notMatching) Partition<T, TResult>(
        this IEnumerable<T> items,
        Func<T, bool> predicate,
        Func<T, TResult> match,
        Func<T, TResult> noMatch)
    {
        predicate.ThrowIfNull();
        match.ThrowIfNull();
        noMatch.ThrowIfNull();

        (IEnumerable<TResult> matching, IEnumerable<TResult> notMatching) = (Enumerable.Empty<TResult>(), Enumerable.Empty<TResult>());
        foreach (var item in items.ThrowIfNull())
        {
            if (predicate(item))
            {
                matching = matching.Append(match(item));
                continue;
            }
            notMatching = notMatching.Append(noMatch(item));
        }

        return (matching, notMatching);
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
    /// <param name="item"></param>
    /// <param name="index"></param>
    /// <returns></returns>
    public static IEnumerable<T> Replace<T>(this IEnumerable<T> items, T item, int index)
    {
        items.ThrowIfNull();
        item.ThrowIfNull();

        return Replace(items, new[] {(item, index)}, item => item);
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
        IEnumerable<(T item, int index)> replaceTuples)
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
        Func<T, int, TResult> project)
    {
        items.ThrowIfNull();
        project.ThrowIfNull();

        foreach (var (item, counter) in items.Enumerate())
        {
            yield return project(item, counter);
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
        IEnumerable<(T item, int index)> replaceTuples,
        Func<T, TResult> project)
    {
        items.ThrowIfNull();
        replaceTuples.ThrowIfNull();
        project.ThrowIfNull();

        var orderedTuples = replaceTuples.Where(tuple => 0 <= tuple.index)
                                         .OrderBy(tuple => tuple.index);

        return items.Enumerate()
                    .ZipLeft(orderedTuples,
                            (lhs, rhs) => lhs.Item2 == rhs.Item2 ? BinarySelection.Right : BinarySelection.Left,
                            tuple => project(tuple.item));
    }

    /// <summary>
    /// Returns items only if all projections do have a value (are Option.Some).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="items"></param>
    /// <param name="project"></param>
    /// <returns>Returns an empty list if not all projections are Option.Some.</returns>
    public static IEnumerable<TResult> SelectAll<T, TResult>(
        this IEnumerable<T> items, 
        Func<T, Option<TResult>> project)
    {
        project.ThrowIfNull();

        var results = Enumerable.Empty<TResult>();
        var notAllItems = false;

        foreach (var item in items.ThrowIfNull())
        {
            var opt = project(item);
            if (opt.IsNone)
            {
                notAllItems = true;
                break;
            }
            results = results.Append(opt.OrThrow());
        }

        return notAllItems ? Enumerable.Empty<TResult>() : results;
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
                    .Select(result => result.Error);
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
                    .Select(result => result.Ok!);
    }

    /// <summary>
    /// Returns all Some values.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> SelectSome<T>(this IEnumerable<Option<T>> items)
    {
        return items.ThrowIfNull()
                    .Where(item => item.IsSome)
                    .Select(opt => opt.OrThrow());
    }

    /// <summary>
    /// Returns Some if exactly one item exists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static Option<T> SingleAsOpt<T>(this IEnumerable<T> items)
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
    /// Returns the symmetric difference of two lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="comparer"></param>
    /// <param name="retainDuplicates">If true then duplicates are taken into account.</param>
    /// <returns></returns>
    public static IEnumerable<T> SymmetricDifferenceSorted<T>(
        this IEnumerable<T> lhs,
        IEnumerable<T> rhs,
        IComparer<T> comparer,
        bool retainDuplicates = false)
    {
        lhs.ThrowIfNull();
        rhs.ThrowIfNull();
        comparer.ThrowIfNull();

        if (!retainDuplicates)
        {
            var set = new HashSet<T>(lhs);
            set.SymmetricExceptWith(rhs);
            return set;
        }

        return lhs.ExceptWithDuplicatesSorted(rhs, comparer).Concat(rhs.ExceptWithDuplicatesSorted(lhs, comparer));
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
    /// Throws an ArgumentNullException when an element is null.
    /// Use this method only for value objects with small collections because the check is done in an eager way.
    /// Don't use <see cref="ThrowIfElementNull"/> for general collections because of performance reasons and also to keep the collection lazy. 
    /// Attention: This method runs into an endless loop when using with a generator!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static IEnumerable<T> ThrowIfElementNull<T>(this IEnumerable<T> items)
    {
        return items.Any(x => null == x) ? throw new ArgumentNullException(nameof(items)) : items;
    }

    /// <summary>
    /// Throws an ArgumentNullException if items is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> ThrowIfEmpty<T>(this IEnumerable<T> items, [CallerArgumentExpression("items")] string name = "")
    {
        return ThrowIfEmpty(items, () => new ArgumentNullException(name));
    }

    /// <summary>
    /// Throws an Exception if items is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="exceptionFactory"></param>
    /// <returns></returns>
    public static IEnumerable<T> ThrowIfEmpty<T>(this IEnumerable<T> items, Func<Exception> exceptionFactory)
    {
        exceptionFactory.ThrowIfNull();

        if (!items.Any())
        {
            var exception = exceptionFactory() ?? throw new ArgumentException("returned null", nameof(exceptionFactory));
            throw exception;
        }
        return items;
    }

    /// <summary>
    /// Returns <paramref name="numberOfElements"/> if items contains exactly <paramref name="numberOfElements"/> or throws an excption.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="numberOfElements"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">is thrown when number of elements differs from  <paramref name="numberOfElements"/></exception>
    public static IEnumerable<T> ThrowIfNumberNotExact<T>(this IEnumerable<T> items, int numberOfElements)
    {
        if (!items.TakeExact(numberOfElements).Any()) 
            throw new ArgumentException($"items does not have exact {numberOfElements} elements");

        return items;
    }

    /// <summary>
    /// Makes the enumerable interruptible. This can be used for nested foreach loops.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="stop"></param>
    /// <returns></returns>
    public static IEnumerable<T> ToBreakable<T>(this IEnumerable<T> items, ref ObservableValue<bool> stop)
    {
        return new BreakableEnumerable<T>(items.ThrowIfNull(), ref stop);
    }

    /// <summary>
    /// Returns an empty enumerable if items is null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> ToEmptyIfNull<T>(this IEnumerable<T>? items)
    {
        return items ?? Enumerable.Empty<T>();
    }

    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> items)
    {
        return new HashSet<T>(items.ThrowIfNull());
    }

    public static IMultiValueMap<TKey, T> ToMultiValueMap<T, TKey>(this IEnumerable<T> items, Func<T, TKey> keySelector)
        where TKey : notnull
    {
        items.ThrowIfNull();
        keySelector.ThrowIfNull();

        return ToMultiValueMap(items, keySelector, x => x);
    }

    public static IMultiValueMap<TKey, TValue> ToMultiValueMap<T, TKey, TValue>(
        this IEnumerable<T> items, 
        Func<T, TKey> keySelector, 
        Func<T, TValue> valueSelector)
        where TKey : notnull
    {
        items.ThrowIfNull();
        keySelector.ThrowIfNull();

        var dictionary = new MultiValueMap<TKey, TValue>();
        foreach (var item in items)
            dictionary.Add(keySelector(item), valueSelector(item));

        return dictionary;
    }

    public static IEnumerable<KeyValuePair<TLhs, IEnumerable<TRhs>>> ToOneToMany<TSource, TLhs, TRhs>(
        this IEnumerable<TSource> source,
        Func<TSource, TLhs> lhsSelector,
        Func<TSource, TRhs> rhsSelector)
        where TLhs : notnull
    {
        source.ThrowIfNull();
        lhsSelector.ThrowIfNull();
        rhsSelector.ThrowIfNull();

        var one2Many = new MultiValueMap<TLhs, TRhs>();
        foreach (var sourceElem in source)
        {
            var lhsElem = lhsSelector(sourceElem);
            var rhsElem = rhsSelector(sourceElem);
            if (!one2Many.Contains(lhsElem, rhsElem))
                one2Many.Add(lhsElem, rhsElem);
        }

        return one2Many.GetKeyValues();
    }

    /// <summary>
    /// Normalizes a one to many collection to one to one.
    /// 
    /// A -> [1, 2, 3]
    /// B -> [4, 5]
    /// 
    /// creates tuples:
    /// 
    /// (A,1), (A,2), (A,3), (B,4), (B5)
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TLhs"></typeparam>
    /// <typeparam name="TRhs"></typeparam>
    /// <param name="source"></param>
    /// <param name="lhsSelector"></param>
    /// <param name="rhsSelector"></param>
    /// <returns></returns>
    public static IEnumerable<(TLhs, TRhs)> ToOneToOne<TSource, TLhs, TRhs>(
        this IEnumerable<TSource> source,
        Func<TSource, TLhs> lhsSelector,
        Func<TSource, IEnumerable<TRhs>> rhsSelector)
    {
        source.ThrowIfNull();
        lhsSelector.ThrowIfNull();
        rhsSelector.ThrowIfNull();

        foreach (var sourceElem in source)
        {
            var lhsElem = lhsSelector(sourceElem);
            foreach (var rhsElem in rhsSelector(sourceElem))
            {
                yield return (lhsElem, rhsElem);
            }
        }
    }

    /// <summary>
    /// Creates optional items from items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="project"></param>
    /// <returns></returns>
    public static IEnumerable<Option<TResult>> ToOptionals<T, TResult>(
        this IEnumerable<T> items,
        Func<T, Option<TResult>> project)
    {
        items.ThrowIfNull();
        project.ThrowIfNull();

        return items.Select(project);
    }

    public static IEnumerable<Ordinal<T>> ToOrdinals<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        items.ThrowIfNull();
        predicate.ThrowIfNull();

        return items.Enumerate()
                    .Where(tuple => predicate(tuple.item))
                    .Select(tuple => new Ordinal<T> { Position = tuple.counter, Value = tuple.item });
    }

    /// <summary>
    /// creates a string from the items separated by separator.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string ToReadableString<T>(this IEnumerable<T> items, string separator = ",")
    {
        items.ThrowIfNull();
        var sb = new StringBuilder();
        foreach (var item in items.AfterEach(() => sb.Append(separator)))
        {
            sb.Append(item);
        }
        return sb.ToString();
    }

    public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> items)
    {
        items.ThrowIfNull();
        return new ReadOnlyCollection<T>(items);
    }

    public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> items, int count)
    {
        items.ThrowIfNull();
        return new ReadOnlyCollection<T>(items, count);
    }

    public static string[] ToStringArray<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.ThrowIfNull()
                         .Select(item => item.ToStringOrEmpty())
                         .ToArray();
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

public interface IElseIf<T>
{
    IEnumerable<T> Else();
    IEnumerable<T> Else(Action<T> action);
    IElseIf<T> ElseIf(Func<T, bool> condition, Action<T> action);
    void EndIf();
}

public interface IElse<T, TResult>
{
    IEnumerable<TResult> Else(Func<T, TResult> map);
}

