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

public static class DictionaryValue
{
    public static DictionaryValue<TKey, TValue> New<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        where TKey : notnull
        => new(keyValues);

    public static DictionaryValue<TKey, TValue> New<TKey, TValue>(params KeyValuePair<TKey, TValue>[] keyValues)
        where TKey: notnull
        => new(keyValues);

    public static DictionaryValue<TKey, TValue> NewWith<TKey, TValue>(
        this DictionaryValue<TKey, TValue> dictionaryValue,
        IEnumerable<KeyValuePair<TKey, TValue>> replacements)
        where TKey : notnull
    {
        return dictionaryValue.Replace(replacements).ToDictionaryValue(x => x.Key, x => x.Value);
    }
}

/// <summary>
/// This immutable dictionary considers the equality of all keys and values <see cref="Equals"/>.
/// The position of the elements are ignored.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class DictionaryValue<TKey, TValue>
    : IReadOnlyDictionary<TKey, TValue>
    , IEquatable<DictionaryValue<TKey, TValue>>
    where TKey : notnull
{
    private readonly IDictionary<TKey, TValue> _dictionary;
    private readonly int _hashCode;

    public DictionaryValue(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        : this(keyValues.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
    {
    }

#if NET6_0_OR_GREATER
    public DictionaryValue(IEnumerable<KeyValuePair<TKey, TValue>> keyValues, IEqualityComparer<TKey> comparer)
        : this(new Dictionary<TKey, TValue>(keyValues, comparer))
    {
    }
#endif

    private DictionaryValue(IDictionary<TKey, TValue> dictionary)
    {
        _dictionary = dictionary.ThrowIfNull();
        _hashCode = HashCode.FromObjects(_dictionary);
    }

    public static implicit operator DictionaryValue<TKey, TValue>(KeyValuePair<TKey, TValue>[] keyValues)
        => new(keyValues);

    public static implicit operator DictionaryValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        => new(dictionary);

    /// <inheritdoc/>
    public TValue this[TKey key] => _dictionary[key];

    /// <inheritdoc/>
    public int Count => _dictionary.Count;

    /// <inheritdoc/>
    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    protected static int DefaultHashCode { get; } = typeof(DictionaryValue<TKey, TValue>).GetHashCode();

    /// <summary>
    /// Considers the equality and number of all elements <see cref="Equals"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as DictionaryValue<TKey, TValue>);

    /// <summary>
    /// Considers the equality and number of all elements <see cref="Equals"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(DictionaryValue<TKey, TValue>? other)
    {
        return other is not null
            && GetHashCode() == other.GetHashCode()
            && _dictionary.IsEqualToSet(other._dictionary);
    }

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

    /// <summary>
    /// Hash code considers all elements.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _hashCode;

    /// <inheritdoc/>
    public IEnumerable<TKey> Keys => _dictionary.Keys;

    /// <inheritdoc/>
    public override string ToString() => string.Join(", ", _dictionary);

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
    public IEnumerable<TValue> Values => _dictionary.Values;
}
