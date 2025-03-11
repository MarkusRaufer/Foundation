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
﻿namespace Foundation.Collections.Generic;

public static class ListExtensions
{
    /// <summary>
    /// Returns the last element in the list without iterating the list.
    /// </summary>
    /// <typeparam name="T">Type of the elements.</typeparam>
    /// <param name="items">List of items.</param>
    /// <returns><see cref="Option.Some{T}"/> if not empty or <see cref="Option.None{T}"/>.</returns>
    public static Option<T> LastAsOption<T>(this IList<T> items)
    {
        if (0 == items.Count) return Option.None<T>();

        return Option.Maybe(items[^1]);
    }

    /// <summary>
    /// Swap values at indices.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <param name="lhsIndex"></param>
    /// <param name="rhsIndex"></param>
    public static void Swap<T>(this IList<T> items, int lhsIndex, int rhsIndex)
    {
        (items[rhsIndex], items[lhsIndex]) = (items[lhsIndex], items[rhsIndex]);
    }

    /// <summary>
    /// Picks an element of a list which maches a predicate.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="predicate"></param>
    /// <param name="value"></param>
    /// <returns>true if element found, otherwise false.</returns>
    public static bool TryGet<T>(this IList<T> list, Func<T, bool> predicate, out T? value)
    {
        var index = list.IndexOf(predicate);
        if(-1 == index)
        {
            value = default;
            return false;
        }
        value = list[index];
        return true;
    }
}
