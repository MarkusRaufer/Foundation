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
using Foundation.Collections.Generic;

namespace Foundation.Buffers;

public static class ReadOnlySpanExtensions
{
    public static int IndexFromEnd(this ReadOnlySpan<char> span, char value)
    {
        return IndexFromEnd(span, span.Length - 1, value);
    }

    /// <summary>
    /// returns the index of value from the end.
    /// </summary>
    /// <param name="span"></param>
    /// <param name="index">This is the start index.</param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int IndexFromEnd(this ReadOnlySpan<char> span, int index, char value)
    {
        var lastIndex = span.Length - 1;
        if (lastIndex < index) return -1;

        while (0 <= index)
        {
            if (span[index] == value) return index;

            index--;
        }
        return -1;
    }

    public static int IndexFromEnd(this ReadOnlySpan<char> span, ReadOnlySpan<char> value)
    {
        return IndexFromEnd(span, span.Length, value);
    }

    /// <summary>
    /// Searches value in a span from back to front.
    /// </summary>
    /// <param name="span"></param>
    /// <param name="index">Index is exclusive.</param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static int IndexFromEnd(this ReadOnlySpan<char> span, int index, ReadOnlySpan<char> value)
    {
        if (span.Length < index) return -1;

        while (0 <= index)
        {
            var startIndex = index - value.Length;
            if (0 > startIndex) return -1;

            if (span[startIndex..index].IsSameAs(value)) return startIndex;

            index--;
        }
        return -1;
    }

    public static int IndexFromEnd<T>(this ReadOnlySpan<T> span, int index, ReadOnlySpan<T> value)
    {
        if (span.Length < index) return -1;

        while (0 <= index)
        {
            var startIndex = index - value.Length;
            if (0 > startIndex) return -1;

            if (span[startIndex..index].IsSameAs(value)) return startIndex;

            index--;
        }
        return -1;
    }

    public static IReadOnlyCollection<int> IndicesFromEnd<T>(
        this ReadOnlySpan<T> span, 
        ReadOnlySpan<T> value, 
        int stopAfterNumberOfHits = -1)
    {
        var indices = new List<int>();
        if (0 == span.Length) return indices;
        if(0 == value.Length) return indices;
        if (span.Length < value.Length) return indices;

        var index = span.Length;
        var numberOfHits = 0;

        while (0 <= index)
        {
            if (-1 < stopAfterNumberOfHits && numberOfHits >= stopAfterNumberOfHits) break;

            var startIndex = index - value.Length;
            if (0 > startIndex) break;

            var sub = span[startIndex..index];

            if (value.IsSameAs(sub))
            {
                indices.Add(startIndex);
                numberOfHits++;
            }

            index--;
        }

        return indices;
    }

    public static IReadOnlyCollection<int> IndicesOf(
        this ReadOnlySpan<char> span,
        ReadOnlySpan<char> search, 
        StringComparison comparisonType = StringComparison.InvariantCulture,
        int stopAfterNumberOfHits = -1)
    {
        var indices = new List<int>();
        var numberOfHits = 0;
        var pos = -1;
        int index;
        
        while (-1 != (index = span.IndexOf(search, comparisonType)))
        {
            if (-1 == pos) pos = index;
            else pos += index + search.Length;

            if (-1 < stopAfterNumberOfHits && numberOfHits >= stopAfterNumberOfHits) break;

            indices.Add(pos);
            numberOfHits++;

            span = span.Slice(index + search.Length);
        }

        return indices;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="span"></param>
    /// <param name="search"></param>
    /// <param name="stopAfterNumberOfHits"></param>
    /// <returns></returns>
    public static int[] IndicesOf(
        this ReadOnlySpan<char> span,
        char search,
        int stopAfterNumberOfHits = 0)
    {
        int index = 0;

        var indices = new List<int>();

        while (-1 != (index = span.IndexOf(index, search)))
        {
            indices.Add(index);

            if (stopAfterNumberOfHits > 0 && indices.Count <= stopAfterNumberOfHits) break;
            index++;
        }

        return [.. indices];
    }

    /// <summary>
    /// Returns all indices of all characters.
    /// </summary>
    /// <param name="span"></param>
    /// <param name="characters"></param>
    /// <param name="stopAfterNumberOfHits"></param>
    /// <returns></returns>
    public static IReadOnlyCollection<int> IndicesOfSingleCharacters(this ReadOnlySpan<char> span, char[] characters, int stopAfterNumberOfHits = 0)
    {
        var indices = new SortedSet<int>();

        foreach (var ch in characters)
        {
            foreach (var index in span.IndicesOf(ch))
            {
                indices.Add(index);

                if (stopAfterNumberOfHits > 0 && indices.Count <= stopAfterNumberOfHits) break;
            }
        }

        return indices;
    }

    public static IReadOnlyCollection<int> IndicesOf<T>(this ReadOnlySpan<T> span, T selector, int stopAfterNumberOfHits = -1)
        where T : IEquatable<T>
    {
        var indices = new List<int>();
        var numberOfHits = 0;
        var pos = -1;
        int index;
        while (-1 != (index = span.IndexOf(selector)))
        {
            if (-1 < stopAfterNumberOfHits && numberOfHits >= stopAfterNumberOfHits) break;

            if (-1 == pos) pos = index;
            else pos += index + 1;

            indices.Add(pos);
            numberOfHits++;
            span = span[(index + 1)..];
        }

        return indices;
    }

    public static IReadOnlyCollection<int> IndicesOfAny<T>(
        this ReadOnlySpan<T> span, 
        ReadOnlySpan<T> selectors, 
        int stopAfterNumberOfHits = -1)
        where T : IEquatable<T>
    {
        var indices = new List<int>();

        var numberOfHits = 0;
        var pos = -1;
        int index;
        while (-1 != (index = span.IndexOfAny(selectors)))
        {
            if (-1 < stopAfterNumberOfHits && numberOfHits >= stopAfterNumberOfHits) break;

            if (-1 == pos) pos = index;
            else pos += index + 1;

            indices.Add(pos);
            numberOfHits++;
            span = span[(index + 1)..];
        }

        return indices;
    }

    /// <summary>
    /// returns tuples (index, tokenLength).
    /// IReadOnlyCollection is used because IEnumerable is not allowed on ref structures (ReadOnlySpan)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static IReadOnlyCollection<(int, int)> IndexLengthTuples<T>(this ReadOnlySpan<T> span, T separator)
        where T : IEquatable<T>
    {
        var tuples = new List<(int, int)>();
        var sep = new [] { separator }.AsSpan();
        var index = 0;
        foreach (var chunk in span.Split(sep))
        {
            tuples.Add((index, chunk.Length));
            index += chunk.Length + 1;
        }

        return tuples;
    }

    public static IReadOnlyCollection<(int, int)> IndexLengthTuplesAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> separators)
        where T : IEquatable<T>
    {
        var tuples = new List<(int, int)>();

        var index = 0;
        foreach (var chunk in span.Split(separators))
        {
            tuples.Add((index, chunk.Length));
            index += chunk.Length + 1;
        }

        return tuples;
    }

    public static int IndexOf(this ReadOnlySpan<char> span, int index, char value)
    {
        if (0 > index) return -1;

        while (span.Length > index)
        {
            if (span[index] == value) return index;

            index++;
        }
        return -1;
    }

    public static int IndexOf(this ReadOnlySpan<char> span, int index, ReadOnlySpan<char> value)
    {
        if (0 > index) return -1;
        if (span.Length < value.Length) return -1;

        while (span.Length > index)
        {
            var endIndex = index + value.Length;
            if(endIndex > span.Length) return -1;

            if (span[index..endIndex].IsSameAs(value)) return index;

            index++;
        }
        return -1;
    }

    /// <summary>
    /// checks if size, values and position are same.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool IsSameAs<T>(this ReadOnlySpan<T> lhs, ReadOnlySpan<T> rhs)
    {
        if (lhs.Length != rhs.Length) return false;

        for (var i = 0; i < lhs.Length; i++)
        {
            var left = lhs[i];
            var right = rhs[i];
            if(null == left)
            {
                if(null != rhs) return false;
                continue;
            }

            if (!left.Equals(right)) return false;
        }

        return true;
    }

    /// <summary>
    /// returns a SplitEnumerator which can be iterated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="span"></param>
    /// <param name="separators">Any of the characters will split the span.</param>
    /// <param name="notFoundReturnsNothing"></param>
    /// <returns></returns>
    public static SplitEnumerator<T> Split<T>(
       this ReadOnlySpan<T> span,
       ReadOnlySpan<T> separators,
       bool notFoundReturnsNothing = true)
       where T : IEquatable<T>
    {
        return new SplitEnumerator<T>(span, separators, notFoundReturnsNothing);
    }

    public static StringSplitEnumerator SplitAtPart(this ReadOnlySpan<char> span, ReadOnlySpan<char> part, StringComparison comparison)
    {
        return new StringSplitEnumerator(span, part, comparison);
    }

    public static ReadOnlySpan<char> TrimAll(this ReadOnlySpan<char> span, char value)
    {
        var startIndex = span.IndexOf(value);
        if (-1 == startIndex) return span;

        var endIndex = span.IndexFromEnd(value);
        if (-1 == startIndex) return span;

        if (startIndex == endIndex) return span;

        return span[(startIndex + 1)..endIndex];
    }
}

