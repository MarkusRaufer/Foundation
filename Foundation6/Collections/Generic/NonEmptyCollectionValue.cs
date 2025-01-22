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
    public static NonEmptyCollectionValue<T> New<T>(params T[] values) => new(values);
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
    private readonly CollectionValue<T> _values;

    public NonEmptyCollectionValue(CollectionValue<T> values)
    {
        _values = values.ThrowIfNull();
        if(_values.Count == 0) throw new ArgumentOutOfRangeException(nameof(values), $"{nameof(values)} must have at least one element");
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

    public int Count => _values.Count;

    public object Clone() => _values.Clone();

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is NonEmptyCollectionValue<T> other && Equals(other);

    public bool Equals(T[]? other) => _values.Equals(other);

    public bool Equals(NonEmptyCollectionValue<T> other) => _values.Equals(other._values);

    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

    public override int GetHashCode() => _values.GetHashCode();

    public bool IsEmpty => _values.IsEmpty;

    public override string ToString() => string.Join(", ", _values);
}
