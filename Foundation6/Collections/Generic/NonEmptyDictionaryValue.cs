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

using Foundation;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public static class NonEmptyDictionaryValue
{
    public static NonEmptyDictionaryValue<TKey, TValue> New<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        where TKey : notnull
        => new (DictionaryValue.New(keyValues));
}

/// <summary>
/// Immutable keyValues that considers the equality of all keys and values <see cref="Equals"/>.The keyValues must not be empty.
/// The position of the elements are ignored.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class NonEmptyDictionaryValue<TKey, TValue>
    : IReadOnlyDictionary<TKey, TValue>
    , IEquatable<NonEmptyDictionaryValue<TKey, TValue>>
    where TKey : notnull
{
    private readonly DictionaryValue<TKey, TValue> _dictionary;

    public NonEmptyDictionaryValue(DictionaryValue<TKey, TValue> dictionary)
    {
        _dictionary = dictionary.ThrowIfNull();
        if (_dictionary.Count == 0) throw new ArgumentOutOfRangeException(nameof(dictionary), $"{nameof(dictionary)} must have at least one element");
    }

    /// <inheritdoc/>
    public TValue this[TKey key] => _dictionary[key];

    /// <inheritdoc/>
    public int Count => _dictionary.Count;

    /// <inheritdoc/>
    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    protected static int DefaultHashCode { get; } = typeof(NonEmptyDictionaryValue<TKey, TValue>).GetHashCode();

    
    public override bool Equals(object? obj) => Equals(obj as NonEmptyDictionaryValue<TKey, TValue>);

    public bool Equals(NonEmptyDictionaryValue<TKey, TValue>? other) => null != other && _dictionary.Equals(other._dictionary);

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

    public override int GetHashCode() => _dictionary.GetHashCode();

    /// <inheritdoc/>
    public IEnumerable<TKey> Keys => _dictionary.Keys;

    /// <inheritdoc/>
#if NETSTANDARD2_0
    public bool TryGetValue(TKey key, out TValue value)
#else
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
#endif
    {
        return _dictionary.TryGetValue(key, out value);
    }

    /// <inheritdoc/>
    public override string ToString() => _dictionary.ToString();

    /// <inheritdoc/>
    public IEnumerable<TValue> Values => _dictionary.Values;
}
