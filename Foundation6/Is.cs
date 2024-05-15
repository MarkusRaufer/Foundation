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

public static class Is
{
    /// <summary>
    /// Creates a Between range expression. 
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="from">Min.</param>
    /// <param name="to">Max.</param>
    /// <returns></returns>
    public static IRangeExpression<T> Between<T>(T from, T to)
        where T : struct, IComparable<T>, IEquatable<T>
    {
        return new Between<T>(from, to);
    }

#if NET6_0_OR_GREATER
    /// <summary>
    /// Creates a Between range expression. 
    /// </summary>
    /// <param name="range">includes the min and max value.</param>
    /// <returns></returns>
    public static IRangeExpression<int> Between(System.Range range)
    {
        var min = range.Start.IsFromEnd ? 0 : range.Start.Value;
        var max = range.End.IsFromEnd ? int.MaxValue : range.End.Value;
        return Between<int>(min, max);
    }
#endif

    /// <summary>
    /// Creates an Exactly range expression.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="value">A value.</param>
    /// <returns></returns>
    public static IRangeExpression<T> Exactly<T>(T value)
        where T : IComparable<T>, IEquatable<T>
    {
        return new Exactly<T>(value);
    }

    /// <summary>
    /// Creates a Matching range expression.
    /// </summary>
    /// <typeparam name="T">Type of the compare value.</typeparam>
    /// <param name="predicate">The match predicate.</param>
    /// <returns></returns>
    public static IRangeExpression<T> Matching<T>(Func<T, bool> predicate)
        where T : IComparable<T>, IEquatable<T>
    {
        return new Matching<T>(predicate);
    }

    /// <summary>
    /// Creates a NumericBetween range expression.
    /// </summary>
    /// <typeparam name="T">Type of the values.</typeparam>
    /// <param name="from">Min.</param>
    /// <param name="to">Max.</param>
    /// <returns></returns>
    public static IRangeExpression<T> NumericBetween<T>(T from, T to)
        where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        return new NumericBetween<T>(from, to);
    }

    /// <summary>
    /// Creates an OneOf range expression.
    /// </summary>
    /// <typeparam name="T">Type of the values.</typeparam>
    /// <param name="values">Values to compare.</param>
    /// <returns></returns>
    public static IRangeExpression<T> OneOf<T>(params T[] values)
        where T : IComparable<T>, IEquatable<T>
    {
        return new OneOf<T>(values);
    }

    /// <summary>
    /// Creates an TypeOf range expression.
    /// </summary>
    /// <typeparam name="T">Type of the value.</typeparam>
    /// <param name="value">value to compare.</param>
    /// <returns></returns>
    public static IRangeExpression<object> OfType<T>()
    {
        return new OfType<T>();
    }
}

