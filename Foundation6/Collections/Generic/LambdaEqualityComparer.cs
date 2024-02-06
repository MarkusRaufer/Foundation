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
﻿using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

public static class LambdaEqualityComparer
{
    public static LambdaEqualityComparer<T> New<T>(Func<T?, T?, bool> equals) => new(equals);
    public static LambdaEqualityComparer<T> New<T>(Func<T?, int>? hashCodeFunc) => new(hashCodeFunc);
    public static LambdaEqualityComparer<T> New<T>(Func<T?, T?, bool> equals, Func<T?, int>? hashCodeFunc) => new(equals, hashCodeFunc);
}

public class LambdaEqualityComparer<T> : EqualityComparer<T>
{
    private readonly Func<T?, int> _hashCodeFunc;
    private readonly Func<T?, T?, bool> _equals;

    /// <summary>
    /// Default hashCodeFunc is DefaultHashCodeFunc.
    /// </summary>
    /// <param name="equals"></param>
    public LambdaEqualityComparer(Func<T?, T?, bool> equals) : this(equals, DefaultHashCodeFunc)
    {
    }

    /// <summary>
    /// Default predicate uses Equals method.
    /// </summary>
    /// <param name="hashCodeFunc"></param>
    public LambdaEqualityComparer(Func<T?, int>? hashCodeFunc) : this(DefaultEquals, hashCodeFunc)
    {
    }

    public LambdaEqualityComparer(Func<T?, T?, bool> equals, Func<T?, int>? hashCodeFunc)
    {
        _equals = equals.ThrowIfNull();
        _hashCodeFunc = hashCodeFunc.ThrowIfNull();
    }

    public static Func<T?, int> DefaultHashCodeFunc { get; } = x => x.GetNullableHashCode();

    public static Func<T?, T?, bool> DefaultEquals { get; } =
        (x, y) =>
        {
            if (null == x) return null == y;
            if (null == y) return false;

            return x.Equals(y);
        };

    public override bool Equals(T? x, T? y) => _equals(x, y);

    public override int GetHashCode([DisallowNull] T obj) => _hashCodeFunc(obj);
}

public class LambdaEqualityComparer<T, TKey> : IEqualityComparer<T>
{
    private readonly Func<TKey?, int> _hashCodeFunc;
    private readonly Func<T, TKey?> _selector;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="selector">value selector for equation and hashcode.</param>
    /// <param name="hashCodeFunc">a hashcode function</param>
    public LambdaEqualityComparer(Func<T, TKey?> selector, Func<TKey?, int>? hashCodeFunc = null)
    {
        _selector = selector.ThrowIfNull();
        _hashCodeFunc = hashCodeFunc ?? DefaultHashCodeFunc;
    }

    public static Func<TKey?, int> DefaultHashCodeFunc { get; } = x => x.GetNullableHashCode();

    public bool Equals(T? x, T? y)
    {
        if (null == x) return null == y;
        if (null == y) return false;
        var eq = _selector(x).EqualsNullable(_selector(y));
        return eq;
    }

    public int GetHashCode([DisallowNull] T obj) => _hashCodeFunc(_selector(obj));
}
