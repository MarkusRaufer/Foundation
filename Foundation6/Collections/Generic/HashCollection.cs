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
﻿#if NET6_0_OR_GREATER
using System.Collections;
using System.Collections.Generic;

namespace Foundation.Collections.Generic;

/// <summary>
/// This collection works like a <see cref="HashSet{T} /> but allows duplicates.
/// </summary>
/// <typeparam name="T"></typeparam>
public class HashCollection<T>
    : ICollection<T>
    , IReadOnlyCollection<T>
    where T : notnull
{
    #region internal structures
    private class ValueContainer 
        : IEquatable<ValueContainer>
        , IEnumerable<T>
    {
        public ValueContainer(T value, ICollection<T>? values = null)
        {
            Value = value;
            Values = values;
        }

        public override bool Equals(object? obj) => obj is ValueContainer other && Equals(other);

        public bool Equals(ValueContainer? other)
        {
            return null != other && Value.EqualsNullable(other.Value);
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator()
        {
            yield return Value;

            if(null == Values) yield break;

            foreach (var value in Values)
            {
                yield return value;
            }
        }

        public override int GetHashCode() => Value.GetNullableHashCode();

        public int Count => null == Values ? 1 : Values.Count + 1;

        public T Value { get; }

        public ICollection<T>? Values { get; set; }

        public override string ToString() => $"Value: {Value}, Count: {Count}";
    }
    #endregion internal structures

    private int _count;
    private readonly HashSet<ValueContainer> _values = [];

    /// <inheritdoc/>
    public int Count => _count;

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <inheritdoc/>
    public void Add(T item)
    {
        _count++;

        var newValue = new ValueContainer(item);
        if (!_values.TryGetValue(newValue, out var container))
        {
            _values.Add(newValue);
            return;
        }

        container.Values ??= [];

        container.Values.Add(item);
    }

    /// <inheritdoc/>
    public void Clear()
    {
        _values.Clear();
        _count = 0;
    }

    /// <summary>
    /// This method uses hashing and is therefore quicker than a List<typeparamref name="T"/>.Contains.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(T item) => _values.Contains(new ValueContainer(item));

    public void CopyTo(T[] array, int arrayIndex)
    {
        var it = GetEnumerator();
        for (var i = arrayIndex; i < array.Length; i++)
        {
            if (!it.MoveNext()) break;

            array[i] = it.Current;
        }
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator()
    {
        foreach(var container in  _values)
        {
            foreach (var value in container)
                yield return value;
        }
    }

    /// <summary>
    /// Searches for a specific value and returns all found values.
    /// </summary>
    /// <param name="value">Value to search.</param>
    /// <returns>Returns found values.</returns>
    public IEnumerable<T> GetValues(T value)
    {
        if (_values.TryGetValue(new ValueContainer(value), out var container))
        {
            return container;
        }
        return [];
    }

    /// <inheritdoc/>
    public bool Remove(T item)
    {
        var remove = new ValueContainer(item);
        if (!_values.TryGetValue(remove, out var container)) return false;

        bool removed;
        if (null != container.Values)
        {
            removed = container.Values.Remove(item);
            if (removed)
            {
                _count--;
                return true;
            }
        }

        removed = _values.Remove(remove);
        if (removed) _count--;

        return removed;
    }

    /// <summary>
    /// Searches for a specific value and returns the found value.
    /// </summary>
    /// <param name="value">Value to be found.</param>
    /// <param name="foundValue">The found value.</param>
    /// <returns></returns>
    public bool TryGetValue(T value, out T? foundValue)
    {
        if (_values.TryGetValue(new ValueContainer(value), out var container))
        {
            foundValue = container.Value;
            return true;
        }
        foundValue = default;
        return false;
    }

    /// <summary>
    /// Returns all found <paramref name="values"/> like <paramref name="value"/>.
    /// </summary>
    /// <param name="value">Value to find.</param>
    /// <param name="values">Found values.</param>
    /// <returns></returns>
    public bool TryGetValues(T value, out IEnumerable<T> values)
    {
        if (_values.TryGetValue(new ValueContainer(value), out var container))
        {
            values = container;
            return true;
        }
        values = [];
        return false;
    }
}
#endif
