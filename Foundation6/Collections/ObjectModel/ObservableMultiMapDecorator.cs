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
﻿using Foundation.Collections.Generic;
using Foundation.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.ObjectModel;

public class ObservableMultiMapDecorator<TKey, TValue>
    : IMultiValueMap<TKey, TValue>
    , IMutable
    , INotifyCollectionChanged

    where TKey : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly IMultiValueMap<TKey, TValue> _map;

    public ObservableMultiMapDecorator(IMultiValueMap<TKey, TValue> map)
    {
        _map = map.ThrowIfNull();
    }

    public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

    public void Add(TKey key, TValue value)
    {
        _map.Add(key, value);

        IsDirty = true;
        if (CollectionChangedEnabled)
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value)));
    }

    public void AddRange(TKey key, IEnumerable<TValue> values)
    {
        _map.AddRange(key, values);
        if (!CollectionChangedEnabled || null == CollectionChanged) return;

        foreach(var value in values)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value)));
        }
    }

    public void AddSingle(KeyValuePair<TKey, TValue> item) => AddSingle(item.Key, item.Value);

    public void AddSingle(TKey key, TValue value)
    {
        _map.AddSingle(key, value);

        IsDirty = true;
        if (CollectionChangedEnabled)
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value)));
    }

    public bool AddUnique(TKey key, TValue value, bool replaceExisting = false)
    {
        if (!_map.AddUnique(key, value)) return false;

        IsDirty = true;
        if (CollectionChangedEnabled)
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value)));
        
        return true;
    }

    public TValue this[TKey key]
    {
        get { return _map[key]; }
        set
        {
            _map[key] = value;
            IsDirty = true;

            if (CollectionChangedEnabled)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
            }
        }
    }

    public ICollection<TKey> Keys => _map.Keys;

    public void Clear()
    {
        if (0 == _map.KeyCount) return;

        _map.Clear();
        IsDirty = true;
        if (CollectionChangedEnabled)
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    public bool CollectionChangedEnabled { get; set; }

    public bool Contains(KeyValuePair<TKey, TValue> item) => _map.Contains(item);

    public bool Contains(TKey key, TValue value) => _map.Contains(key, value);

    public bool ContainsKey(TKey key) => _map.ContainsKey(key);

    public bool ContainsValue(TValue value) => _map.ContainsValue(value);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        _map.CopyTo(array, arrayIndex);
    }

    public int Count => _map.Count;

    public bool IsDirty { get; set; }

    public bool IsReadOnly => _map.IsReadOnly;

    public int KeyCount => _map.KeyCount;

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _map.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _map.GetEnumerator();

    public IEnumerable<KeyValuePair<TKey, TValue>> GetFlattenedKeyValues(IEnumerable<TKey>? keys = default)
    {
        return _map.GetFlattenedKeyValues(keys);
    }

    public IEnumerable<TValue> GetFlattenedValues(IEnumerable<TKey>? keys = default)
    {
        return _map.GetFlattenedValues(keys);
    }

    public IEnumerable<TKey> GetKeys(IEnumerable<TValue> values) => _map.GetKeys(values);

    public IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> GetKeyValues(IEnumerable<TKey>? keys = default)
    {
        return _map.GetKeyValues(keys);
    }

    public IEnumerable<TValue> GetValues(TKey key) => _map.GetValues(key);

    public IEnumerable<TValue> GetValues(IEnumerable<TKey> keys) => _map.GetValues(keys);

    public int GetValuesCount(TKey key) => _map.GetValuesCount(key);

    public bool Remove(TKey key, TValue value) => _map.Remove(key, value);

    public bool Remove(TKey key) => _map.Remove(key);

    public bool Remove(KeyValuePair<TKey, TValue> item) => _map.Remove(item);

    public bool RemoveValue(TValue value) => _map.RemoveValue(value);

    public bool RemoveValue(TValue value, IEnumerable<TKey> keys) => _map.RemoveValue(value, keys);

#if NETSTANDARD2_0
    public bool TryGetValue(TKey key, out TValue value)
        => _map.TryGetValue(key, out value);
#else
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        => _map.TryGetValue(key, out value);
#endif

    public bool TryGetValues(TKey key, [NotNullWhen(true)] out ICollection<TValue>? values) => _map.TryGetValues(key, out values);

    public ICollection<TValue> Values => _map.Values;
}
