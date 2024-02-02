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

public static class CollectionValue
{
    public static CollectionValue<T> New<T>(params T[] values)
    {
        return new CollectionValue<T>(values);
    }
}

/// <summary>
/// This is an immutable collecion that compares each element on <see cref="Equals(CollectionValue{T})"/>.
/// That enables the comparison of two collecions.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct CollectionValue<T>
    : ICloneable
    , IReadOnlyCollection<T>
    , IEquatable<CollectionValue<T>>
    , IEquatable<T[]>
{
    private readonly int _hashCode;
    private readonly T[] _values;

    public CollectionValue(IEnumerable<T> values)
        : this(values.ToArray())
    {

    }
    public CollectionValue(T[] values)
    {
        _values = values.ThrowIfNull();
        _hashCode = HashCode.FromOrderedObjects(_values);
    }

    /// <inheritdoc/>
    public static implicit operator CollectionValue<T>(T[] array) => CollectionValue.New(array);

    /// <inheritdoc/>
    public static implicit operator T[](CollectionValue<T> array) => array._values;

    /// <inheritdoc/>
    public static bool operator ==(CollectionValue<T> left, CollectionValue<T> right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(CollectionValue<T> left, CollectionValue<T> right) => !(left == right);

    /// <inheritdoc/>
    public int Count => IsEmpty ? 0 : _values.Length;

    /// <inheritdoc/>
    public object Clone()
    {
        return IsEmpty
            ? new ArrayValue<T>()
            : new ArrayValue<T>((T[])_values.Clone());
    }

    /// <summary>
    /// Considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is CollectionValue<T> other && Equals(other);

    /// <summary>
    /// Considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(T[]? other)
    {
        if (IsEmpty) return null == other || 0 == other.Length;

        return null != other && _values.EqualsCollection(other);
    }

    /// <summary>
    /// Considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(CollectionValue<T> other)
    {
        if (IsEmpty) return other.IsEmpty;
        if (other.IsEmpty) return false;

        if (GetHashCode() != other.GetHashCode()) return false;

        return _values.EqualsCollection(other._values);
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => IsEmpty ? Enumerable.Empty<T>().GetEnumerator() : _values.GetEnumerator<T>();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Considers all elements.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _hashCode;

    /// <summary>
    /// Returns true if not initialzed.
    /// </summary>
    public bool IsEmpty => _values is null;
}
