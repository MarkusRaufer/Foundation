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
﻿namespace Foundation.Buffers;

using Foundation.Collections.Generic;

public static class ReadOnlyMemoryExtensions
{
    /// <summary>
    /// Returns all indices of the found value starting from the end.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="memory"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static IEnumerable<int> IndicesFromEnd<T>(this ReadOnlyMemory<T> memory, ReadOnlyMemory<T> value)
    {
        if (0 == memory.Length) yield break;
        if (0 == value.Length) yield break;
        if (memory.Length < value.Length) yield break;

        var index = memory.Length;

        while (0 <= index)
        {
            var startIndex = index - value.Length;
            if (0 > startIndex) break;

            var sub = memory[startIndex..index];

            if (value.IsSameAs(sub)) yield return startIndex;

            index--;
        }
    }

    /// <summary>
    /// Returns the indices of a search buffer.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="memories"></param>
    /// <param name="selectors"></param>
    /// <returns></returns>
    public static IEnumerable<int> IndicesOf<T>(this ReadOnlyMemory<T> memory, ReadOnlyMemory<T> search)
        where T : IEquatable<T>
    {
        int index;
        var pos = -1;

        var span = memory.Span;

        while (-1 != (index = span.IndexOf(search.Span)))
        {
            if (-1 == pos) pos = index;
            else pos += index + 1;

            yield return pos;
            span = memory.Span[(pos + 1)..];
        }
    }

    /// <summary>
    /// Returns tuples of selectors with the found indices.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="memory"></param>
    /// <param name="selectors"></param>
    /// <returns>selector: is the ordinal index of selectors. index: the index of the found selector.
    /// </returns>
    public static IEnumerable<(int selector, int index)> IndicesOfAny<T>(this ReadOnlyMemory<T> memory, ReadOnlyMemory<T>[] selectors)
        where T : IEquatable<T>
    {
        if (0 == selectors.Length) yield break;

        var selectorIndices = Enumerable.Range(0, selectors.Length).ToList();
        
        foreach (var index in Enumerable.Range(0, memory.Length))
        {
            var illegalSelectors = selectorIndices.Where(i => memory.Length < (selectors[i].Length + index)).ToArray();
            illegalSelectors.ForEach(i => selectorIndices.Remove(i));

            if (0 == selectorIndices.Count) yield break;

            foreach (var selectorIndex in selectorIndices)
            {
                var selector = selectors[selectorIndex].Span;

                var sub = memory.Span[index..(selector.Length + index)];

                if (sub.IsSameAs(selector))
                    yield return (selector: selectorIndex, index);
            }
        }
    }

    /// <summary>
    /// returns the indexes of the found selectors in array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="memory"></param>
    /// <param name="selector"></param>
    /// <returns></returns>
    public static IEnumerable<int> IndicesOfAny<T>(
        this ReadOnlyMemory<T> memory, 
        IEnumerable<T> selectors)
    {
        for (var i = 0; i < memory.Length; i++)
        {
            if (selectors.Any(selector => selector.EqualsNullable(memory.Span[i])))
            {
                yield return i;
            }
        }
    }

    /// <summary>
    /// checks if size, values and position are same.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool IsSameAs<T>(this ReadOnlyMemory<T> lhs, ReadOnlyMemory<T> rhs)
    {
        if (lhs.Length != rhs.Length) return false;

        var lhsSpan = lhs.Span;
        var rhsSpan = rhs.Span;

        for (var i = 0; i < lhs.Length; i++)
        {
            var left = lhsSpan[i];
            var right = rhsSpan[i];
            if (null == left)
            {
                if (null != right) return false;
                continue;
            }

            if (!left.Equals(right)) return false;
        }

        return true;
    }
}
