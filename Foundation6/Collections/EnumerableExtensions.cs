// The MIT License (MIT)
//
// Copyright (c) 2020 Markus Raufer
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
﻿namespace Foundation.Collections;

using Foundation.Collections.Generic;
using System.Collections;

public static class EnumerableExtensions
{
    public static IEnumerable AfterEveryObject(this IEnumerable items, Action action)
    {
        items.ThrowIfNull();
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
    /// Like <see cref="Any"/>
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static bool AnyObject(this IEnumerable items)
    {
        items.ThrowIfNull();

        return items.GetEnumerator().MoveNext();
    }

    /// <summary>
    /// Like <see cref="=Any"/>
    /// </summary>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static bool AnyObject(this IEnumerable items, Func<object, bool> predicate)
    {
        items.ThrowIfNull();
        predicate.ThrowIfNull();

        foreach (var item in items)
        {
            if (predicate(item))
                return true;
        }
        return false;
    }

    /// <summary>
    /// List <see cref="=CastTo<typeparamref name="T"/>"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> CastTo<T>(this IEnumerable items)
    {
        if(null == items) return Enumerable.Empty<T>();

        return items.SelectObject(obj => (T)obj);
    }

    /// <summary>
    /// Returns the number of elements in items.
    /// </summary>
    /// <param name="items">List of items.</param>
    /// <returns>Number of elements in list.</returns>
    public static int Count(this IEnumerable items)
    {
        if (null == items) return 0;

        var count = 0;
        foreach(var item in items)
            count++;

        return count;
    }

    /// <summary>
    /// Like <see cref="=First"/>
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static object FirstObject(this IEnumerable items)
    {
        items.ThrowIfNull();
        var item = FirstOrDefaultObject(items);

        if (!item.TryGet(out object? value)) throw new InvalidOperationException("sequence is emtpy");

        return value!;
    }

    /// <summary>
    /// Like <see cref="=FirstOrDefault"/>
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    private static Option<object> FirstOrDefaultObject(this IEnumerable items)
    {
        items.ThrowIfNull();

        var enumerator = items.GetEnumerator();
        if (enumerator.MoveNext())
        {
            return Option.Some(enumerator.Current);
        }

        return Option.None<object>();
    }

    /// <summary>
    /// Like <see cref="ForEach"/>
    /// </summary>
    /// <param name="items"></param>
    /// <param name="action"></param>
    public static void ForEachObject(this IEnumerable items, Action<object> action)
    {
        items.ThrowIfNull();
        action.ThrowIfNull();

        foreach (var item in items)
            action(item);
    }

    /// <summary>
    /// Ignores item if predicate returns true.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static IEnumerable<T> Ignore<T>(this IEnumerable<T> items, Func<T, bool> predicate)
    {
        items.ThrowIfNull();
        predicate.ThrowIfNull();

        foreach (var item in items)
        {
            if (predicate(item))
                continue;

            yield return item;
        }
    }

    /// <summary>
    /// Ignores items at indicies.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="indices"></param>
    /// <returns></returns>
    public static IEnumerable<T> Ignore<T>(this IEnumerable<T> items, int[] indices)
    {
        items.ThrowIfNull();
        indices.ThrowIfNull();

        var i = 0;
        foreach (var item in items)
        {
            if (indices.Contains(i++)) continue;
            
            yield return item;
        }
    }

    /// <summary>
    /// Like <see cref="=OfType<typeparamref name="T"/>"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable<T> ObjectOfType<T>(this IEnumerable items)
    {
        foreach (var item in items.ThrowIfNull())
        {
            if (item is T t) yield return t;
        }
    }

    public static IEnumerable<object> OfTypes(this IEnumerable items, params Type[] types)
    {
        items = items.ThrowIfNull();
        types.ThrowIfOutOfRange(() => types.Length == 0);

        foreach (var item in items)
        {
            if(null == item) continue;

            var type = item.GetType();
            if (types.Any(t => t.Equals(type) || t.IsAssignableFrom(type)))
                yield return item;
            
        }
    }

    public static IEnumerable<T> OfTypes<T>(this IEnumerable items, params Type[] types)
    {
        items.ThrowIfNull();
        foreach (var item in items.OfTypes(types))
        {
            if (item is T t) yield return t;
        }
    }

    public static IEnumerable<object[]> Permutations(this IEnumerable<IEnumerable<object>> lists)
    {
        if (!lists.Any())
        {
            yield return Array.Empty<object>();
            yield break;
        }

        var firstList = lists.First();
        var remainingLists = lists.Skip(1);

        var withoutFirstList = Permutations(remainingLists);

        foreach (var lhs in firstList)
        {
            foreach (var rhs in withoutFirstList)
            {
                var permutation = new object[1 + rhs.Length];
                permutation[0] = lhs;

                if (rhs.Length > 0) Array.Copy(rhs, 0, permutation, 1, rhs.Length);

                yield return permutation;
            }
        }
    }

    public static object[][] PermutationsArrays(this object[][] lists)
    {
        if (lists.Length == 0)
        {
            return new object[][] { Array.Empty<object>() };
        }

        var firstList = lists.First();
        var remainingLists = lists.Skip(1).ToArray();
        var permutationsWithoutFirstList = PermutationsArrays(remainingLists);

        var permutations = new List<object[]>();

        foreach (var item in firstList)
        {
            foreach (var permutationWithoutFirstList in permutationsWithoutFirstList)
            {
                var permutation = new object[1 + permutationWithoutFirstList.Length];
                permutation[0] = item;
                if (permutationWithoutFirstList.Length > 0)
                    Array.Copy(permutationWithoutFirstList, 0, permutation, 1, permutationWithoutFirstList.Length);

                permutations.Add(permutation);
            }
        }

        return permutations.ToArray();
    }

    public static List<List<object>> PermutationsLists(this List<List<object>> lists)
    {
        if (lists.Count == 0)
        {
            return new List<List<object>> { new List<object>() };
        }

        var firstList = lists.First();
        var remainingLists = lists.Skip(1).ToList();
        var permutationsWithoutFirstList = PermutationsLists(remainingLists);

        var permutations = new List<List<object>>();

        foreach (var item in firstList)
        {
            foreach (var permutationWithoutFirstList in permutationsWithoutFirstList)
            {
                var permutation = new List<object> { item };
                permutation.AddRange(permutationWithoutFirstList);
                permutations.Add(permutation);
            }
        }

        return permutations;
    }

    /// <summary>
    /// Returns all items that are not null.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static IEnumerable SelectNotNull(this IEnumerable items)
    {
        if(items is null) yield break;

        foreach (object item in items)
        {
            if (null != item) yield return item;
        }
    }

    public static IEnumerable SelectObjectByIndex(this IEnumerable items, Func<long, bool> selector)
    {
        items.ThrowIfNull();
        selector.ThrowIfNull();

        long i = 0;
        return items.WhereObject(item => selector(i++));
    }

    /// <summary>
    /// Like <see cref="=Single<typeparamref name="T"/>"/>
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static object SingleObject(this IEnumerable items)
    {
        items.ThrowIfNull();

        var enumerator = items.GetEnumerator();
        if (!enumerator.MoveNext()) throw new InvalidOperationException("no element");

        var current = enumerator.Current;
        if (enumerator.MoveNext()) throw new InvalidOperationException("more than one element");

        return current;
    }
}

