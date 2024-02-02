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

public static class DualOrdinalStreamsFactory
{
    /// <summary>
    /// Splits a stream into two streams and returns it as a new DualOrdinalStreams. 
    /// Matching items are added to the right stream.
    /// If isExhaustive is true, matching items are not added to the left stream.
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    /// <param name="items"></param>
    /// <param name="predicate">This is the split criteria.</param>
    /// <param name="project">A projection from TLeft to TRight.</param>
    /// <param name="isExhaustive">If true then matching items are not added to the left stream.</param>
    /// <returns></returns>
    public static DualOrdinalStreams<TLeft, TRight> ToDualOrdinalStreams<TLeft, TRight>(
        this IEnumerable<TLeft> items,
        Func<TLeft, bool> predicate,
        Func<TLeft, TRight> project,
        bool isExhaustive)
    {
        items.ThrowIfEnumerableIsNull();
        predicate.ThrowIfNull();
        project.ThrowIfNull();

        var streams = new DualOrdinalStreams<TLeft, TRight>();

        foreach (var (counter, item) in items.Enumerate())
        {
            if (predicate(item))
            {
                var ordinalRight = new Ordinal<TRight> { Position = counter, Value = project(item) };

                streams.Right = streams.Right.Append(ordinalRight);

                if (isExhaustive) continue;
            }

            var ordinalLeft = new Ordinal<TLeft> { Position = counter, Value = item };
            streams.Left = streams.Left.Append(ordinalLeft);
        }

        return streams;
    }
}
