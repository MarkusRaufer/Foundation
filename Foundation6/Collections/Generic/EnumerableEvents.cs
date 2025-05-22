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

namespace Foundation.Collections.Generic;

/// <summary>
/// Extends IEnumerable with LINQ like events.
/// </summary>
public static class EnumerableEvents
{

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

        var it = items.ThrowIfEnumerableIsNull()
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

        var it = items.ThrowIfEnumerableIsNull()
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
    /// Calls action on each element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnEach<T>(this IEnumerable<T> items, Action action)
    {
        action.ThrowIfNull();

        foreach (var item in items.ThrowIfEnumerableIsNull())
        {
            action();
            yield return item;
        }
    }

    /// <summary>
    /// Calls <paramref name="action"/> on each element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnEach<T>(this IEnumerable<T> items, Action<T> action)
    {
        action.ThrowIfNull();

        foreach (var item in items.ThrowIfEnumerableIsNull())
        {
            action(item);
            yield return item;
        }
    }

    /// <summary>
    /// Calls <paramref name="action"/> every time <paramref name="predicate"/> returns true.
    /// </summary>
    /// <typeparam name="T">Type of item.</typeparam>
    /// <param name="items">List of items.</param>
    /// <param name="predicate">Predicate which enables the call of <paramref name="action"/></param>
    /// <param name="action">The action which will be executed when <paramref name="predicate"/> is true.</param>
    /// <returns>All <paramref name="items"/>.</returns>
    public static IEnumerable<T> OnEach<T>(this IEnumerable<T> items, Func<T, bool> predicate, Action<T> action)
    {
        predicate.ThrowIfNull();
        action.ThrowIfNull();

        foreach (var item in items.ThrowIfEnumerableIsNull())
        {
            if (predicate(item)) action(item);
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

        var it = items.ThrowIfEnumerableIsNull()
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
    /// If list is empty onEmpty is called. After returning the single value the iteration stops.
    /// If the list is not empty it behaves as normal IEnumerable<typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    /// <param name="items">List of items.</param>
    /// <param name="onEmpty">Factory method which is called if list of items is empty.</param>
    /// <returns>List of items.</returns>
    public static IEnumerable<T> OnEmpty<T>(this IEnumerable<T> items, Func<T> onEmpty)
    {
        onEmpty.ThrowIfNull();

        var it = items.ThrowIfEnumerableIsNull()
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
    /// Returns a value if list is empty. After returning the single value the iteration stops.
    /// If the list is not empty it behaves as normal IEnumerable<typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    /// <typeparam name="TResult">Type of returned items.</typeparam>
    /// <param name="items">List of items.</param>
    /// <param name="onEmpty">Is called if list is empty.</param>
    /// <param name="onNotEmpty">Is called if list is not empty.</param>
    /// <returns>List of items.</returns>
    public static IEnumerable<TResult> OnEmpty<T, TResult>(this IEnumerable<T> items, Func<TResult> onEmpty, Func<T, TResult> onNotEmpty)
    {
        onEmpty.ThrowIfNull();
        onNotEmpty.ThrowIfNull();

        var it = items.ThrowIfEnumerableIsNull()
                      .GetEnumerator();

        if (!it.MoveNext())
        {
            yield return onEmpty();
            yield break;
        }
        yield return onNotEmpty(it.Current);

        while (it.MoveNext())
        {
            yield return onNotEmpty(it.Current);
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

        var it = items.ThrowIfEnumerableIsNull()
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

        var it = items.ThrowIfEnumerableIsNull()
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
    /// <paramref name="action"/> is called when <paramref name="predicate"/> returns true the first time.
    /// </summary>
    /// <typeparam name="T">Type of element.</typeparam>
    /// <param name="items">List of elements.</param>
    /// <param name="predicate">Predicate to trigger <paramref name="action"/>.</param>
    /// <param name="action">Will be triggered once when <paramref name="predicate"/> returns true.</param>
    /// <returns>All items.l</returns>
    public static IEnumerable<T> OnFirst<T>(this IEnumerable<T> items, Func<T, bool> predicate, Action<T> action)
    {
        items.ThrowIfNull();
        predicate.ThrowIfNull();
        action.ThrowIfNull();

        var called = false;
        foreach (var item in items)
        {
            if (!called && predicate(item))
            {
                called = true;
                action(item);
            }
            yield return item;
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

        var it = items.ThrowIfEnumerableIsNull()
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

        var it = items.ThrowIfEnumerableIsNull()
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

        var it = items.ThrowIfEnumerableIsNull().GetEnumerator();
        var counter = 0;
        bool hasNext;
        while (hasNext = it.MoveNext())
        {
            if (index == counter)
            {
                action(it.Current);
                break;
            }

            yield return it.Current;
            counter++;
        }

        if (!hasNext) yield break;

        while(it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Calls action when reached the indices during iteration.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="indices"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable<T> OnNths<T>(this IEnumerable<T> items, IEnumerable<int> indices, Action<int, T> action)
    {
        action.ThrowIfNull();

        var counter = 0;
        var idx = indices.ToArray();

        foreach (var item in items.ThrowIfEnumerableIsNull())
        {
            if (idx.Contains(counter))
                action(counter, item);

            yield return item;
            counter++;
        }
    }

    public static IEnumerable<TResult> OnTypeSelect<TSource, TTarget, TResult>(this IEnumerable<TSource> items, Func<TTarget, TResult> projection)
    {
        projection.ThrowIfNull();

        foreach (var item in items)
        {
            if(item is TTarget target)
            {
                yield return projection(target);
            }
        }
    }
}
