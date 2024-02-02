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
﻿using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

public static class Params
{
    public static Params<object> New(params object[] values) => new(values);
    public static Params<T> New<T>(params T[] values) => new(values);
}

public struct Params<T> 
    : IEnumerable<T>
    , IEquatable<Params<T>>
{
    private readonly int _hashCode;
    private readonly bool _isInitialized;
    private readonly ICollection<T> _values;

    public Params(params T[] values)
    {
        values.ThrowIfNull();

        _values = values;
        _hashCode = HashCode.FromObject(_values);
        _isInitialized = true;
    }


    public static bool operator ==(Params<T> left, Params<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Params<T> left, Params<T> right)
    {
        return !(left == right);
    }

    public int Count => _values.Count;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Params<T> other && Equals(other);

    public bool Equals(Params<T> other)
    {
        if (IsEmpty) return other.IsEmpty;

        return !other.IsEmpty && _values.EqualsCollection(other._values);
    }

    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public bool IsEmpty => !_isInitialized;

    public override string ToString() => string.Join(", ", _values);
}
