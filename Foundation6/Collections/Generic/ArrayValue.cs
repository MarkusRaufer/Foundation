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

public static class ArrayValue
{
    public static ArrayValue<T> From<T>(IEnumerable<T> values) =>
       new (values);

    public static ArrayValue<T> New<T>(params T[] values) =>
        new (values);

    public static ArrayValue<T> NewWith<T>(
        this ArrayValue<T> arrayValue,
        IEnumerable<T> replacements)
    {
        return arrayValue.Replace(replacements).ToArrayValue();
    }
}

/// <summary>
/// This is an immutable array that acts like a value. ArrayValue compares each element and its position on <see cref="Equals(ArrayValue{T})"/>.
/// <see cref="GetHashCode"/> considers all elements and their position.
/// That enables the comparison of two arrays.
/// </summary>
/// <typeparam name="T"></typeparam>
public readonly struct ArrayValue<T>
    : ICloneable
    , IEnumerable<T>
    , IEquatable<ArrayValue<T>>
    , IEquatable<T[]>
{
    private readonly int _hashCode;
    private readonly T[] _values;

    public ArrayValue(T[] values)
    {
        _values = values.ThrowIfNull();
        _hashCode = HashCode.FromObjects(_values);
    }

    public ArrayValue(IEnumerable<T> values)
    {
        _values = values.ThrowIfNull().ToArray();
        _hashCode = HashCode.FromObjects(_values);
    }

    public static implicit operator ArrayValue<T>(T[] array) => ArrayValue.New(array);

    public static implicit operator T[](ArrayValue<T> array) => array._values;

    public static bool operator ==(ArrayValue<T> left, ArrayValue<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ArrayValue<T> left, ArrayValue<T> right)
    {
        return !(left == right);
    }

    public T this[int index] => _values[index];

    /// <summary>
    /// Returns a ReadOnlySpan of the internal array.
    /// </summary>
    /// <returns></returns>
    public ReadOnlySpan<T> AsReadOnlySpan() => _values;

    public object Clone()
    {
        return IsEmpty ? new ArrayValue<T>([]) : new ArrayValue<T>((T[])_values.Clone());
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is ArrayValue<T> other && Equals(other);

    /// <summary>
    /// All elements are compared with the elements of <paramref name="other"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(T[]? other)
    {
        if (IsEmpty) return null == other || 0 == other.Length;

        return null != other && _values.SequenceEqual(other);
    }

    /// <summary>
    /// All elements are compared with the elements of <paramref name="other"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ArrayValue<T> other)
    {
        if (IsEmpty) return other.IsEmpty;

        return !other.IsEmpty 
            && _hashCode == other._hashCode
            && _values.SequenceEqual(other._values);
    }

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, and returns the first occurrence within the entire array.
    /// </summary>
    /// <param name="predicate">The predicate that defines the conditions of the element to search for.</param>
    /// <returns>The first element that matches the conditions defined by the specified predicate, if found; otherwise, the default value for type T.</returns>
    public T? Find(Predicate<T> predicate) => Array.Find(_values, predicate);

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, and returns the zero-based index of the first occurrence within the entire Array.
    /// </summary>
    /// <param name="predicate">The Predicate<T> that defines the conditions of the element to search for.</param>
    /// <returns></returns>
    public int FindIndex(Predicate<T> predicate) => Array.FindIndex(_values, predicate);

    public int FindIndex(int startIndex, Predicate<T> predicate) => Array.FindIndex(_values, startIndex, predicate);

    public int FindIndex(int startIndex, int count, Predicate<T> predicate) => Array.FindIndex(_values, startIndex, count, predicate);

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator<T>();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

    /// <summary>
    /// Hash code considers all elements.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _hashCode;

    /// <summary>
    /// True if not initialized.
    /// </summary>
    public bool IsEmpty => _values is null;

    /// <summary>
    /// Number of elements.
    /// </summary>
    public int Length => IsEmpty ? 0 : _values.Length;

    /// <inheritdoc/>
    public override string? ToString() => IsEmpty ? "" : string.Join(",", _values);
}
