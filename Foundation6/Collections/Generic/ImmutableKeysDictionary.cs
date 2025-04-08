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
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

/// <summary>
/// Dictionary with immutable keys.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class ImmutableKeysDictionary<TKey, TValue>
    : IImmutableKeysDictionary<TKey, TValue>
    where TKey : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly IDictionary<TKey, TValue> _keyValues;

    public ImmutableKeysDictionary(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
    {
        _keyValues = keyValues.ToDictionary(x => x.Key, x => x.Value);
    }

    public ImmutableKeysDictionary(IDictionary<TKey, TValue> dictionary)
    {
        _keyValues = dictionary.ThrowIfNull();
    }

    /// <inheritdoc/>
    public TValue this[TKey key]
    {
        get => _keyValues[key];
        set
        {
            if (!_keyValues.TryGetValue(key, out TValue? oldValue) || oldValue.EqualsNullable(value)) return;

            _keyValues[key] = value;

            IsDirty = true;

            if (null == CollectionChanged) return;

            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, Pair.New(key, value), Pair.New(key, oldValue));
            CollectionChanged?.Invoke(this, args);
        }
    }

    /// <inheritdoc/>
    public int Count => _keyValues.Count;

    /// <inheritdoc/>
    public IEnumerable<TKey> Keys => _keyValues.Keys;

    /// <inheritdoc/>
    public IEnumerable<TValue> Values => _keyValues.Values;

    /// <inheritdoc/>
    public bool ContainsKey(TKey key) => _keyValues.ContainsKey(key);

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _keyValues.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _keyValues.GetEnumerator();

    /// <inheritdoc/>
    public bool IsDirty { get; set; }

#if NETSTANDARD2_0
    public bool TryGetValue(TKey key, out TValue value) => _keyValues.TryGetValue(key, out value);
#else
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _keyValues.TryGetValue(key, out value);
#endif
}
