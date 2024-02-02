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

public class NullableEqualityComparer<T> : EqualityComparer<T>
{
    private readonly Func<T, int> _hashCodeFunc;
    private readonly Func<T, T, bool> _predicate;

    public override bool Equals(T? x, T? y)
    {
        if (null == x) return null == y;
        if(null == y) return false;

        return _predicate(x, y);
    }

    public override int GetHashCode([DisallowNull] T obj)
    {
        if(null == obj) return 0;

        return _hashCodeFunc(obj);
    }

    /// <summary>
    /// Default hashCodeFunc is DefaultHashCodeFunc.
    /// </summary>
    /// <param name="predicate"></param>
    public NullableEqualityComparer(Func<T, T, bool> predicate) : this(predicate, DefaultHashCodeFunc)
    {
    }

    /// <summary>
    /// Default predicate uses Equals method.
    /// </summary>
    /// <param name="hashCodeFunc"></param>
    public NullableEqualityComparer(Func<T, int> hashCodeFunc) : this(DefaultPredicate, hashCodeFunc)
    {
    }

    public NullableEqualityComparer(Func<T, T, bool> predicate, Func<T, int>? hashCodeFunc)
    {
        _predicate = predicate.ThrowIfNull();
        _hashCodeFunc = hashCodeFunc.ThrowIfNull();
    }

    public static Func<T, int> DefaultHashCodeFunc { get; } = (t => null == t ? 0 : t.GetHashCode());

    public static Func<T, T, bool> DefaultPredicate { get; } =
        (x, y) =>
        {
            if (null == x) return null == y;
            if (null == y) return false;

            return x.Equals(y);
        };
}


