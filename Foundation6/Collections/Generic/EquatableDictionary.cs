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
using Foundation.ComponentModel;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

/// <summary>
/// This dictionary considers the equality of all keys and values <see cref="Equals"/>.
/// The position of the elements are ignored.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
[Serializable]
public class EquatableDictionary<TKey, TValue>
    : IDictionary<TKey, TValue>
    , ICollectionChanged<KeyValuePair<TKey, TValue>, DictionaryEvent<TKey, TValue>>
    , IEquatable<EquatableDictionary<TKey, TValue>>
    , ISerializable
    where TKey : notnull
{
    private int _hashCode;
    private readonly IDictionary<TKey, TValue> _keyValues;

    public EquatableDictionary() : this(new Dictionary<TKey, TValue>())
    {
    }

    public EquatableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        : this(keyValues.ToDictionary(kv => kv.Key, kv => kv.Value))
    {
    }

    public EquatableDictionary(IDictionary<TKey, TValue> keyValues)
    {
        _keyValues = keyValues.ThrowIfNull();
        _hashCode = CreateHashCode();

        CollectionChanged = new Event<Action<DictionaryEvent<TKey, TValue>>>();
    }

    public EquatableDictionary(SerializationInfo info, StreamingContext context)
    {
        if (info.GetValue(nameof(_keyValues), typeof(Dictionary<TKey, TValue>)) is Dictionary<TKey, TValue> keyValues)
        {
            _keyValues = keyValues;
        }
        else
        {
            _keyValues = new Dictionary<TKey, TValue>();
        }

        _hashCode = CreateHashCode();

        CollectionChanged = new Event<Action<DictionaryEvent<TKey, TValue>>>();
    }

    /// <inheritdoc/>
    public TValue this[TKey key]
    {
        get => _keyValues[key];
        set
        {
            var keyExists = _keyValues.ContainsKey(key);
            if (keyExists)
            {
                var existingValue = _keyValues[key];
                if (EqualityComparer<TValue>.Default.Equals(existingValue, value)) return;
            }

            _keyValues[key] = value;

            _hashCode = CreateHashCode();

            var changeEvent = keyExists
                ? new { Action = DictionaryAction.Replace, Element = Pair.New(key, value) }
                : new { Action = DictionaryAction.Add, Element = Pair.New(key, value) };

            CollectionChanged.Publish(changeEvent);
        }
    }

    /// <inheritdoc/>
    public void Add(TKey key, TValue value)
    {
        key.ThrowIfNull();

        Add(Pair.New(key, value));
    }

    /// <inheritdoc/>
    public void Add(KeyValuePair<TKey, TValue> keyValue)
    {
        keyValue.ThrowIfKeyIsNull();

        _keyValues.Add(keyValue);

        _hashCode = CreateHashCode();

        CollectionChanged.Publish(new { Action = DictionaryAction.Add, Element = keyValue });
    }

    /// <summary>
    /// Adds a list of <see cref="=KeyValuePair<typeparamref name="TKey"/>, <typeparamref name="TValue"/>" to the dictionary./>
    /// </summary>
    /// <param name="keyValues"></param>
    public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
    {
        keyValues.ThrowIfEnumerableIsNull();

        foreach (var keyValue in keyValues)
        {
            _keyValues.Add(keyValue);
            CollectionChanged.Publish(new { Action = DictionaryAction.Add, Element = keyValue });
        }

        _hashCode = CreateHashCode();

    }

    /// <inheritdoc/>
    public void Clear()
    {
        _keyValues.Clear();
        _hashCode = CreateHashCode();

        CollectionChanged.Publish(new { Action = DictionaryAction.Clear });
    }

    public Event<Action<DictionaryEvent<TKey, TValue>>> CollectionChanged { get; private set; }

    /// <inheritdoc/>
    public bool Contains(KeyValuePair<TKey, TValue> item) => _keyValues.Contains(item);

    /// <inheritdoc/>
    public bool ContainsKey(TKey key) => _keyValues.ContainsKey(key);

    /// <inheritdoc/>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _keyValues.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public int Count => _keyValues.Count;

    protected int CreateHashCode()
    {
        return HashCode.CreateBuilder()
                       .AddHashCode(DefaultHashCode)
                       .AddObjects(_keyValues)
                       .GetHashCode();
    }

    protected static int DefaultHashCode { get; } = typeof(EquatableDictionary<TKey, TValue>).GetHashCode();

    /// <summary>
    /// Considers keys and values only. Positions of the elements are ignored.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => obj is IDictionary<TKey, TValue> other && Equals(other);

    /// <summary>
    /// Considers keys and values only. Positions of the elements are ignored.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(EquatableDictionary<TKey, TValue>? other)
    {
        if (other is null) return false;
        if (_hashCode != other._hashCode) return false;

        return _keyValues.IsEqualToSet(other);
    }

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _keyValues.GetEnumerator();

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _keyValues.GetEnumerator();

    /// <summary>
    /// Considers all keys and values. Positions of the elements are ignored.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _hashCode;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(_keyValues), _keyValues);
    }

    /// <inheritdoc/>
    public bool IsReadOnly => _keyValues.IsReadOnly;

    /// <inheritdoc/>
    public ICollection<TKey> Keys => _keyValues.Keys;

    /// <inheritdoc/>
    public bool Remove(TKey key)
    {
        if (_keyValues.Remove(key))
        {
            _hashCode = CreateHashCode();
            var element = Pair.New<TKey, TValue?>(key, default);

            CollectionChanged.Publish(new { Action = DictionaryAction.Remove, Element = element });
            return true;
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (_keyValues.Remove(item))
        {
            _hashCode = CreateHashCode();
            CollectionChanged.Publish(new { Action = DictionaryAction.Remove, Element = item });
            return true;
        }
        return false;
    }

    /// <inheritdoc/>
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _keyValues.TryGetValue(key, out value);

    /// <inheritdoc/>
    public ICollection<TValue> Values => _keyValues.Values;
}
