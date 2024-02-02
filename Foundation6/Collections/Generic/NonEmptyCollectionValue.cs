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

public static class NonEmptyCollectionValue
{
    public static NonEmptyCollectionValue<T> New<T>(params T[] values)
    {
        return new NonEmptyCollectionValue<T>(values);
    }
}

/// <summary>
/// Immutable unordered collection that considers the equality of each element on <see cref="Equals(NonEmptyCollectionValue{T}). The collection must not be empty."/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct NonEmptyCollectionValue<T>
    : ICloneable
    , IReadOnlyCollection<T>
    , IEquatable<NonEmptyCollectionValue<T>>
    , IEquatable<T[]>
{
    private readonly int _hashCode;
    private readonly T[] _values;

    public NonEmptyCollectionValue(IEnumerable<T> values)
        : this(values.ToArray())
    {

    }
    public NonEmptyCollectionValue(T[] values)
    {
        _values = values.ThrowIfNullOrEmpty();
        if(_values.Length == 0) throw new ArgumentOutOfRangeException(nameof(values), $"{nameof(values)} must have at least one element");

        _hashCode = HashCode.FromOrderedObjects(_values);
    }

    public static implicit operator NonEmptyCollectionValue<T>(T[] array) => NonEmptyCollectionValue.New(array);

    public static implicit operator T[](NonEmptyCollectionValue<T> array) => array._values;

    public static bool operator ==(NonEmptyCollectionValue<T> left, NonEmptyCollectionValue<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(NonEmptyCollectionValue<T> left, NonEmptyCollectionValue<T> right)
    {
        return !(left == right);
    }

    public T this[int index] => _values[index];

    public int Count => _values.Length;

    public object Clone()
    {
        return IsEmpty
            ? new ArrayValue<T>(Array.Empty<T>())
            : new ArrayValue<T>((T[])_values.Clone());
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is NonEmptyCollectionValue<T> other && Equals(other);

    public bool Equals(T[]? other)
    {
        if (IsEmpty) return null == other || 0 == other.Length;

        return null != other && _values.EqualsCollection(other);
    }

    public bool Equals(NonEmptyCollectionValue<T> other)
    {
        if (IsEmpty) return other.IsEmpty;
        if (other.IsEmpty) return false;

        if (GetHashCode() != other.GetHashCode()) return false;

        return _values.EqualsCollection(other._values);
    }

    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator<T>();

    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public bool IsEmpty => null == _values || 0 == _values.Length;

    public int Length => IsEmpty ? 0 : _values.Length;

    public override string ToString() => string.Join(", ", _values);
}
