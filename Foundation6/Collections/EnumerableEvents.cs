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
﻿using System.Collections;

namespace Foundation.Collections;

public static class EnumerableEvents
{
    /// <summary>
    /// Returns true if items is empty.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static bool IsEmpty(this IEnumerable items)
    {
        return !items.GetEnumerator().MoveNext();
    }

    /// <summary>
    /// Returns true if items is null.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static bool IsNull(this IEnumerable items)
    {
        return null == items;

    }
    /// <summary>
    /// Returns true if items is null or empty
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    public static bool IsNullOrEmpty(this IEnumerable items)
    {
        if (IsNull(items)) return true;
        return IsEmpty(items);
    }

    /// <summary>
    /// Calls action on first item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    public static IEnumerable OnFirstObject(this IEnumerable items, Action action)
    {
        items.ThrowIfNull();
        action.ThrowIfNull();

        var it = items.GetEnumerator();
        if (!it.MoveNext()) yield break;

        action();
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
    public static IEnumerable OnFirstObject(this IEnumerable items, Action<object> action)
    {
        items.ThrowIfNull();
        action.ThrowIfNull();

        var it = items.GetEnumerator();
        if (!it.MoveNext()) yield break;

        action(it.Current);
        yield return it.Current;

        while (it.MoveNext())
        {
            yield return it.Current;
        }
    }

    /// <summary>
    /// Like <see cref="=Where"/>
    /// </summary>
    /// <param name="items"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IEnumerable WhereObject(this IEnumerable items, Func<object, bool> selector)
    {
        selector.ThrowIfNull();

        foreach (var item in items)
        {
            if (selector(item))
                yield return item;
        }
    }
}
