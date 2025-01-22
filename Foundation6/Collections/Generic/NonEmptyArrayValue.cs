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

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public static class NonEmptyArrayValue
{
    public static NonEmptyArrayValue<T> New<T>(params T[] values) =>
        new (values);
}

/// <summary>
/// This is an immutable array that compares each element on <see cref="Equals(ArrayValue{T})"/>.
/// That enables the comparison of two arrays.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct NonEmptyArrayValue<T>
    : ICloneable
    , IEnumerable<T>
    , IEquatable<NonEmptyArrayValue<T>>
    , IEquatable<T[]>
{
    private readonly int _hashCode;
    private readonly ArrayValue<T> _arrayValue;

    public NonEmptyArrayValue(ArrayValue<T> arrayValue)
    {
        _arrayValue = arrayValue.ThrowIfNull();
        if(_arrayValue.Length == 0) throw new ArgumentOutOfRangeException(nameof(arrayValue), $"{nameof(arrayValue)} must have at least one element");
        
        _hashCode = HashCode.FromObjects(_arrayValue);
    }

    public static implicit operator NonEmptyArrayValue<T>(T[] array) => NonEmptyArrayValue.New(array);

    public static implicit operator T[](NonEmptyArrayValue<T> array) => array._arrayValue;

    public static bool operator ==(NonEmptyArrayValue<T> left, NonEmptyArrayValue<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NonEmptyArrayValue<T> left, NonEmptyArrayValue<T> right)
    {
        return !(left == right);
    }

    public T this[int index] => _arrayValue[index];

    public object Clone() => _arrayValue.Clone();

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is NonEmptyArrayValue<T> other && Equals(other);

    public bool Equals(T[]? other) => _arrayValue.Equals(other);

    public bool Equals(NonEmptyArrayValue<T> other) => _arrayValue.Equals(other._arrayValue);

    public IEnumerator<T> GetEnumerator() => _arrayValue.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _arrayValue.GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public bool IsEmpty => _arrayValue.IsEmpty;

    public int Length => _arrayValue.Length;
    
    public override string? ToString() => _arrayValue.ToString();
}
