using Foundation.Collections.Generic;
using Foundation.ComponentModel;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.ObjectModel;

public class ObservableMultiMapDecorator<TKey, TValue>
    : IMultiMap<TKey, TValue>
    , IMutable
    , INotifyCollectionChanged

    where TKey : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly IMultiMap<TKey, TValue> _map;

    public ObservableMultiMapDecorator(IMultiMap<TKey, TValue> map)
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

    public IEnumerable<KeyValuePair<TKey, TValue>> GetFlattenedKeyValues(params TKey[] keys)
    {
        return _map.GetFlattenedKeyValues(keys);
    }

    public IEnumerable<TValue> GetFlattenedValues(params TKey[] keys)
    {
        return _map.GetFlattenedValues(keys);
    }

    public IEnumerable<TKey> GetKeys(params TValue[] values) => _map.GetKeys(values);

    public IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> GetKeyValues(params TKey[] keys)
    {
        return _map.GetKeyValues(keys);
    }

    public IEnumerable<TValue> GetValues(params TKey[] keys) => _map.GetValues(keys);

    public int GetValuesCount(TKey key) => _map.GetValuesCount(key);

    public bool Remove(TKey key, TValue value) => _map.Remove(key, value);

    public bool Remove(TKey key) => _map.Remove(key);

    public bool Remove(KeyValuePair<TKey, TValue> item) => _map.Remove(item);

    public bool RemoveValue(TValue value) => _map.RemoveValue(value);

    public bool RemoveValue(TValue value, params TKey[] keys) => _map.RemoveValue(value, keys);

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        => ((IReadOnlyMultiMap<TKey, TValue>)_map).TryGetValue(key, out value);

    public bool TryGetValues(TKey key, out IEnumerable<TValue> values) => _map.TryGetValues(key, out values);

    public ICollection<TValue> Values => _map.Values;
}
