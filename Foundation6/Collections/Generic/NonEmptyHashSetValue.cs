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

/// <summary>
/// This immutable collection of values includes only unique values and considers the equality and number of all elements <see cref="Equals"/>. 
/// The position of the elements are ignored. The set must not be empty.
/// </summary>
public static class NonEmptyHashSetValue
{
    /// <summary>
    /// Creates a new <see cref="NonEmptySetValue{T}"/> from values.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <param name="values"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">Is thrown if values is empty.</exception>
    public static NonEmptySetValue<T> New<T>(IEnumerable<T> values)
       => new (HashSetValue.New(values));
}

/// <summary>
/// Immutable set that considers the equality and number of all elements <see cref="Equals"/>. The set must not be empty.
/// By definition a set only includes unique values. The position of the elements are ignored.
/// </summary>
/// <typeparam name="T">The type of the values</typeparam>
[Serializable]
public readonly struct NonEmptySetValue<T>
    : IReadOnlyCollection<T>
    , IEquatable<NonEmptySetValue<T>>
{
    private readonly HashSetValue<T> _values;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="values">The unique values. Duplicates are ignored.</param>
    /// <exception cref="ArgumentOutOfRangeException">Is thrown if values is empty.</exception>
    public NonEmptySetValue(HashSetValue<T> values)
    {
        _values = values.ThrowIfNull();
        if(_values.Count == 0) throw new ArgumentOutOfRangeException(nameof(values), $"{nameof(values)} must have at least one element");
    }

    /// <summary>
    /// Considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator ==(NonEmptySetValue<T> left, NonEmptySetValue<T> right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static bool operator !=(NonEmptySetValue<T> left, NonEmptySetValue<T> right)
    {
        return !(left == right);
    }

    public static implicit operator NonEmptySetValue<T>(T[] values) => new(values);

    /// <inheritdoc/>
    public int Count => _values.Count;

    /// <summary>
    /// Considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is NonEmptySetValue<T> other && Equals(other);

    /// <summary>
    /// Considers the equality and number of all elements <see cref="Equals"/>. The position of the elements are ignored.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(NonEmptySetValue<T> other) => _values.Equals(other._values);

    /// Hash code considers all elements.
    public override int GetHashCode() => _values.GetHashCode();

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

    /// <summary>
    /// Returns true if not initialized.
    /// </summary>
    public bool IsEmpty => _values.IsEmpty;

    /// <inheritdoc/>
    public override string ToString() => $"{_values}";
}