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

namespace Foundation.Collections.Generic;

public static class SortedSetValue
{
    /// <summary>
    /// Creates a new <see cref="=SortedSetValue<typeparamref name="T"/>"/> from values.
    /// </summary>
    /// <typeparam name="T">The type of the values.</typeparam>
    /// <param name="values">The values in the <see cref="=SortedSetValue<typeparamref name="T"/>.</param>
    /// <returns>a <see cref="=SortedSetValue<typeparamref name="T"/> object.</returns>
    public static SortedSetValue<T> New<T>(params T[] values) => new(values);
}

/// <summary>
/// Immutable sorted set that considers the equality and number of all elements <see cref="Equals"/>.
/// By definition a set only includes unique values.
/// </summary>
/// <typeparam name="T">The type of the items.</typeparam>
public readonly struct SortedSetValue<T> 
    : IReadOnlyCollection<T>
    , IEquatable<SortedSetValue<T>>
{
    private readonly int _hashCode;
    private readonly SortedSet<T> _values;

    public SortedSetValue(IEnumerable<T> values) : this(new SortedSet<T>(values))
    {
    }

    public SortedSetValue(IEnumerable<T> values, IComparer<T>? comparer)
        : this(new SortedSet<T>(values, comparer))
    {
    }

    private SortedSetValue(SortedSet<T> values)
    {
        _values = values.ThrowIfNull();
        _hashCode = HashCode.FromObjects(_values);
    }

    /// <summary>
    /// All elements of <paramref name="lhs"/> are compared with <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns>True if all elements of <paramref name="left"/> are same than <paramref name="right"/></returns>
    public static bool operator ==(SortedSetValue<T> lhs, SortedSetValue<T> rhs)
    {
        return lhs.EqualsNullable(rhs);
    }

    /// <summary>
    /// All elements of <paramref name="lhs"/> are compared with <paramref name="rhs"/>.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns>True if not all elements of <paramref name="left"/> are same than <paramref name="right"/></returns>
    public static bool operator !=(SortedSetValue<T> lhs, SortedSetValue<T> rhs)
    {
        return !(lhs == rhs);
    }

    /// <inheritdoc/>
    public int Count => IsEmpty ? 0 : _values.Count;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is SortedSetValue<T> other && Equals(other);


    /// <summary>
    /// All elements are compared with the elements of <paramref name="other"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(SortedSetValue<T> other)
    {
        if(IsEmpty) return other.IsEmpty;

        return _hashCode == other._hashCode && _values.SetEquals(other._values);
    }

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

    /// <summary>
    /// Hash code considers all elements.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _hashCode;

    /// <summary>
    /// Returns true if not initialized.
    /// </summary>
    public bool IsEmpty => _values is null;
}
