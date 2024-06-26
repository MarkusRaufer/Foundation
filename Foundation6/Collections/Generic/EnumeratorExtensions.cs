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

public static class EnumeratorExtensions
{
    /// <summary>
    /// Creates an enumerable from an enumerator.
    /// </summary>
    /// <typeparam name="T">Type of items.</typeparam>
    /// <param name="enumerator"></param>
    /// <returns>IEnumerable<typeparamref name="T"/></returns>
    public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> enumerator)
    {
        enumerator.ThrowIfNull();

        while(enumerator.MoveNext())
        {
            yield return enumerator.Current;
        }
    }

    /// <summary>
    /// Creates an enumerable from an enumerator. Only elements which match the predicate are included.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="enumerator"></param>
    /// <param name="predicate">Only if predicate returns true the element is returned.</param>
    /// <returns></returns>
    public static IEnumerable<T> ToEnumerable<T>(this IEnumerator<T> enumerator, Func<T, bool> predicate)
    {
        enumerator.ThrowIfNull();

        while (enumerator.MoveNext())
        {
            if (predicate(enumerator.Current)) yield return enumerator.Current;
        }
    }
}
