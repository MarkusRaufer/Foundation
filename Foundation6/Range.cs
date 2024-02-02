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
﻿namespace Foundation;

using System.Diagnostics;

public class Range
{
    public static Range<T, T> New<T>(params IRangeExpression<T>[] rangeExpressions)
    {
        return New<T, T>(rangeExpressions);
    }

    public static Range<TIn, TOut> New<TIn, TOut>(params IRangeExpression<TIn>[] rangeExpressions)
    {
        return new Range<TIn, TOut>(rangeExpressions);
    }
}

/// <summary>
/// Defines a range of values.
/// </summary>
[DebuggerDisplay("Min={Min}, Max={Max}")]
public readonly struct Range<TIn, TOut>
{
    private readonly bool _containsOnlyValueExpressions;
    private readonly IRangeExpression<TIn>[] _rangeExpressions;

    public Range(params IRangeExpression<TIn>[] rangeExpressions)
    {
        _rangeExpressions = rangeExpressions.ThrowIfNull();
        _containsOnlyValueExpressions = rangeExpressions.All(re => re is IValueRangeExpression);
    }

    /// <summary>
    /// Returns true if all range expressions are of type IValueRangeExpression.
    /// </summary>
    public bool ContainsOnlyValueExpressions => _containsOnlyValueExpressions;

    /// <summary>
    /// Returns true if the value is whithing the range.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool IsInRange(TIn value) => _rangeExpressions.Any(re => re.IsInRange(value));

    public IEnumerable<IRangeExpression<TIn>> RangeExpressions => _rangeExpressions;

    /// <summary>
    /// The values of the range expressions. If ContainsOnlyValueExpressions if false, no value is returned.
    /// </summary>
    public IEnumerable<TOut> Values
    {
        get
        {
            if (!_containsOnlyValueExpressions)
                yield break;

            foreach (var rangeExpression in _rangeExpressions)
            {
                if (rangeExpression is ISingleValueRangeExpression<TIn, TOut> singleValue)
                {
                    yield return singleValue.Value;
                    continue;
                }

                if (rangeExpression is not IMultiValueRangeExpression<TOut> multiValue) continue;

                foreach (var value in multiValue.Values)
                {
                    yield return value;
                }
            }
        }
    }
}

