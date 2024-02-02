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

public static class DualOrdinalStreamsExtensions
{
    /// <summary>
    /// Filters the left stream and adds the matching items to the right stream. If isExhaustive is true then matching items are not added to the left stream.
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    /// <param name="dualStreams"></param>
    /// <param name="predicate">Filter of left stream.</param>
    /// <param name="project">Projection from left to right stream.</param>
    /// <param name="isExhaustive">If isExhaustive is true then matching items are not added to the left stream.</param>
    /// <returns></returns>
    public static DualOrdinalStreams<TLeft, TRight> AddToRight<TLeft, TRight>(
        this DualOrdinalStreams<TLeft, TRight> dualStreams,
        Func<TLeft, bool> predicate,
        Func<TLeft, TRight> project,
        bool isExhaustive)
    {
        predicate.ThrowIfNull();
        project.ThrowIfNull();

        var streams = new DualOrdinalStreams<TLeft, TRight>
        {
            Right = dualStreams.Right
        };

        foreach (var left in dualStreams.Left)
        {
            if (predicate(left.Value))
            {
                var right = new Ordinal<TRight> { Position = left.Position, Value = project(left.Value) };

                streams.Right = streams.Right.Append(right);

                if (isExhaustive) continue;
            }

            streams.Left = streams.Left.Append(left);
        }

        return streams;
    }

    /// <summary>
    /// Merge Left and Right stream to one stream. If element with same position exists in boths streams, the element from Right is taken.
    /// </summary>
    /// <typeparam name="TLeft"></typeparam>
    /// <typeparam name="TRight"></typeparam>
    /// <param name="dualStreams"></param>
    /// <param name="project"></param>
    /// <returns></returns>
    public static IEnumerable<TRight> MergeStreams<TLeft, TRight>(
        this DualOrdinalStreams<TLeft, TRight> dualStreams, 
        Func<TLeft, TRight> project)
    {
        project.ThrowIfNull();

        Option<TRight> toRightValue(Option<TLeft> opt)
        {
            return opt.IsSome ? Option.Some(project(opt.OrThrow())) : Option.None<TRight>();
        }

        var left = dualStreams.Left.OrdinalFill().Select(toRightValue);
        var right = dualStreams.Right.OrdinalFill();

        var itLeft = left.GetEnumerator();
        var itRight = right.GetEnumerator();

        while (true)
        {
            var hasNextLeft = itLeft.MoveNext();
            var hasNextRight = itRight.MoveNext();

            if (!hasNextLeft && !hasNextRight) break;

            if (!hasNextLeft)
            {
                if (itRight.Current.IsSome) yield return itRight.Current.OrThrow();
                continue;
            }

            if (!hasNextRight)
            {
                if (itLeft.Current.IsSome) yield return itLeft.Current.OrThrow();
                continue;
            }

            if (itLeft.Current.IsSome)
            {
                if (itRight.Current.IsSome)
                {
                    yield return itRight.Current.OrThrow();
                    continue;
                }

                yield return itLeft.Current.OrThrow();
                continue;
            }

            if (itRight.Current.IsSome) yield return itRight.Current.OrThrow();
        }
    }

    public static DualOrdinalStreams<TLeft, TRight> OrderByPosition<TLeft, TRight>(this DualOrdinalStreams<TLeft, TRight> dualStreams)
    {
        static IEnumerable<Ordinal<T>> sort<T>(IEnumerable<Ordinal<T>> items)
        {
            foreach (var item in items.OrderBy(o => o.Position))
            {
                yield return item;
            }
        }

        return new DualOrdinalStreams<TLeft, TRight>
        {
            Left = sort(dualStreams.Left),
            Right = sort(dualStreams.Right)
        };
    }
}
