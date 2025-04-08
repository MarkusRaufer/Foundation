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
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

/// <summary>
/// Dictionary that supports multiple values per key.
/// It is used for 1 to many relations.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class ImmutableKeysMultiMap<TKey, TValue> : IImmutableKeysMultiMap<TKey, TValue> where TKey : notnull
{
    private readonly IDictionary<TKey, ICollection<TValue>> _dictionary;
    private readonly Func<ICollection<TValue>> _valueCollectionFactory;

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    public ImmutableKeysMultiMap(IEnumerable<TKey> keys) : this(keys, () => [])
    {
    }

    public ImmutableKeysMultiMap(IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> keyValues) : this (keyValues, () => [])
    {
    }

    public ImmutableKeysMultiMap(IEnumerable<TKey> keys, Func<ICollection<TValue>> valueCollectionFactory)
    {
        _dictionary = new Dictionary<TKey, ICollection<TValue>>();
        _valueCollectionFactory = valueCollectionFactory.ThrowIfNull();

        foreach (var key in keys)
        {
            _dictionary[key] = valueCollectionFactory();
        }
    }

    public ImmutableKeysMultiMap(IEnumerable<TKey> keys, IEqualityComparer<TKey> comparer, Func<ICollection<TValue>> valueCollectionFactory)
    {
        _dictionary = new Dictionary<TKey, ICollection<TValue>>(comparer);
        _valueCollectionFactory = valueCollectionFactory.ThrowIfNull();

        foreach (var key in keys)
        {
            _dictionary[key] = valueCollectionFactory();
        }
    }

    public ImmutableKeysMultiMap(IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> keyValues, Func<ICollection<TValue>> valueCollectionFactory)
    {
        _dictionary = new Dictionary<TKey, ICollection<TValue>>();
        _valueCollectionFactory = valueCollectionFactory.ThrowIfNull();

        foreach (var pair in keyValues.ThrowIfNull())
        {
            _dictionary[pair.Key] = pair.Value;
        }
    }

    public ImmutableKeysMultiMap(
        IEnumerable<KeyValuePair<TKey, ICollection<TValue>>> keyValues,
        IEqualityComparer<TKey> comparer,
        Func<ICollection<TValue>> valueCollectionFactory)
    {
        _dictionary = new Dictionary<TKey, ICollection<TValue>>(comparer);
        _valueCollectionFactory = valueCollectionFactory.ThrowIfNull();

        foreach (var pair in keyValues.ThrowIfNull())
        {
            _dictionary[pair.Key] = pair.Value;
        }
    }


    public ImmutableKeysMultiMap(IDictionary<TKey, ICollection<TValue>> dictionary, Func<ICollection<TValue>> valueCollectionFactory)
    {
        _dictionary = dictionary.ThrowIfNull();
        _valueCollectionFactory = valueCollectionFactory.ThrowIfNull();
    }

    /// <summary>
    /// Checks if key value pair exists.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(KeyValuePair<TKey, TValue> item) => Contains(item.Key, item.Value);

    /// <summary>
    /// Checks if key value pair exists.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Contains(TKey key, TValue value)
    {
        if (_dictionary.TryGetValue(key, out ICollection<TValue>? values))
            return values.Contains(value);

        return false;
    }

    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    public bool ContainsValue(TValue value)
    {
        foreach (var pair in _dictionary)
        {
            if (pair.Value.Contains(value)) return true;
        }

        return false;
    }

    /// <summary>
    /// returns number of all key values.
    /// </summary>
    public int Count => _dictionary.Aggregate(0, (sum, elem) => sum + elem.Value.Count);

    /// <summary>
    /// Copies only first value of each key.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="arrayIndex"></param>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        var it = GetEnumerator();
        for (var i = arrayIndex; i < array.Length; i++)
        {
            if (!it.MoveNext()) return;

            array[i] = it.Current;
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return GetFlattenedKeyValues().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Returns key values as flat list. If keys is empty it returns all key values.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public IEnumerable<KeyValuePair<TKey, TValue>> GetFlattenedKeyValues(IEnumerable<TKey>? keys = default)
    {

        if (null == keys || !keys.Any())
        {
            foreach (var kvp in _dictionary)
            {
                foreach (var value in kvp.Value)
                {
                    yield return Pair.New(kvp.Key, value);
                }
            }
            yield break;
        }

        foreach (var kvp in GetKeyValues(keys))
        {
            foreach (var value in kvp.Value)
            {
                yield return Pair.New(kvp.Key, value);
            }
        }
    }

    /// <summary>
    /// Returns a flat list of values. If keys is empty all values are returned.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public IEnumerable<TValue> GetFlattenedValues(IEnumerable<TKey>? keys = default)
    {
        return null != keys && keys.Any()
            ? GetValues(keys)
            : _dictionary.Values.SelectMany(values => values);
    }

    /// <summary>
    /// Returns the keys containing the value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public IEnumerable<TKey> GetKeys(IEnumerable<TValue> values)
    {
        if (!values.Any())
        {
            foreach (var key in _dictionary.Keys)
                yield return key;

            yield break;
        }

        foreach (var kvp in _dictionary)
        {
            if (Array.Exists(values.ToArray(), value => kvp.Value.Contains(value)))
                yield return kvp.Key;
        }
    }

    /// <summary>
    /// Returns all keys with their values.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> GetKeyValues(IEnumerable<TKey>? keys = default)
    {
        if (null == keys || !keys.Any())
        {
            foreach (var kvp in _dictionary)
            {
                yield return new KeyValuePair<TKey, IEnumerable<TValue>>(kvp.Key, kvp.Value);
            }

            yield break;
        }

        foreach (var key in keys)
        {
            if (_dictionary.TryGetValue(key, out ICollection<TValue>? values))
                yield return new KeyValuePair<TKey, IEnumerable<TValue>>(key, values);
        }
    }

    /// <inheritdoc/>
    public IEnumerable<TValue> GetValues(TKey key)
    {
        if (!_dictionary.TryGetValue(key, out ICollection<TValue>? values)) yield break;
        foreach (var value in values)
        {
            yield return value;
        }
    }

    /// <inheritdoc/>
    public IEnumerable<TValue> GetValues(IEnumerable<TKey> keys)
    {
        foreach (var key in keys)
        {
            foreach (var value in GetValues(key))
            {
                yield return value;
            }
        }
    }

    /// <summary>
    /// Returns the number of values of the key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public int GetValuesCount(TKey key)
    {
        if (_dictionary.TryGetValue(key, out ICollection<TValue>? values))
            return values.Count;

        return 0;
    }


    /// <inheritdoc/>
    public bool IsDirty { get; set; }

    /// <summary>
    /// Gets a value indicating whether the map is readonly or not.
    /// </summary>
    public bool IsReadOnly => _dictionary.IsReadOnly;

    /// <summary>
    /// Returns the number of keys.
    /// </summary>
    public int KeyCount => _dictionary.Count;

    public IEnumerable<TKey> Keys => _dictionary.Keys;


#pragma warning disable CS8767
    public bool TryGetValue(TKey key, [NotNullWhen(true)] out TValue? value)
    {
        if (_dictionary.TryGetValue(key, out ICollection<TValue>? values))
        {
            if (0 < values.Count)
            {
                value = values.First();
#pragma warning disable CS8762 // Parameter must have a non-null value when exiting in some condition.
                return true;
#pragma warning restore CS8762 // Parameter must have a non-null value when exiting in some condition.
            }
        }

        value = default;
        return false;
    }
#pragma warning restore

    /// <inheritdoc/>
    public bool TryGetValues(TKey key, [NotNullWhen(true)] out ICollection<TValue>? values)
    {
        if (_dictionary.TryGetValue(key, out ICollection<TValue>? vals))
        {
            values = vals;
            return true;
        }

        values = default;
        return false;
    }

    /// <inheritdoc/>
    public bool TryGetKey(TValue value, out TKey? key)
    {
        foreach (var dictionary in _dictionary)
        {
            if (dictionary.Value.Contains(value))
            {
                key = dictionary.Key;
                return true;
            }
        }

        key = default;
        return false;
    }

    /// <inheritdoc/>
    public bool TryGetKeys(TValue value, out IEnumerable<TKey> keys)
    {
        var foundKeys = new List<TKey>();

        foreach (var dictionary in _dictionary)
        {
            if (dictionary.Value.Contains(value)) foundKeys.Add(dictionary.Key);
        }

        if (foundKeys.Count > 0)
        {
            keys = foundKeys;
            return true;
        }
        keys = Enumerable.Empty<TKey>();
        return false;
    }

    public virtual TValue this[TKey key]
    {
        get => _dictionary[key].First();
        set
        {
            if (!_dictionary.TryGetValue(key, out ICollection<TValue>? values)) return;

            if (null == values)
            {
                values = _valueCollectionFactory();
                _dictionary[key] = values;
            }

            values.Add(value);

            IsDirty = true;

            if (null == CollectionChanged) return;

            var args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, Pair.New(key, value), null);
            CollectionChanged?.Invoke(this, args);
        }
    }
}

