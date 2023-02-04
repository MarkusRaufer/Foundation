using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

/// <summary>
/// Dictionary with immutable keys.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class FixedKeysDictionary<TKey, TValue>
    : IFixedKeysDictionary<TKey, TValue>
    where TKey : notnull
{
    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    private readonly HashSet<TKey> _keys;
    private readonly IDictionary<TKey, TValue> _keyValues;

    public FixedKeysDictionary(IEnumerable<TKey> keys)
    {
        _keys = new HashSet<TKey>(keys);
        _keyValues = new Dictionary<TKey, TValue>();
    }

    public FixedKeysDictionary(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
    {
        _keyValues = keyValues.ToDictionary(x => x.Key, x => x.Value);
        _keys = new HashSet<TKey>(_keyValues.Keys);
    }

    public FixedKeysDictionary(IDictionary<TKey, TValue> dictionary)
    {
        _keyValues = dictionary.ThrowIfNull();
        _keys = new HashSet<TKey>(_keyValues.Keys);
    }

    public TValue this[TKey key]
    {
        get => _keyValues[key];
        set
        {
            if (!_keys.Contains(key)) return;

            var valueExists = _keyValues.TryGetValue(key, out TValue? oldValue);
            if (valueExists && oldValue.EqualsNullable(value)) return;

            _keyValues[key] = value;

            if (null == CollectionChanged) return;

            var args = valueExists
                ? new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, Pair.New(key, value), Pair.New(key, oldValue))
                : new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, Pair.New(key, value), null);

            CollectionChanged?.Invoke(this, args);
        }
    }

    public int Count => _keyValues.Count;

    public IEnumerable<TKey> Keys => _keyValues.Keys;

    public IEnumerable<TValue> Values => _keyValues.Values;

    public bool ContainsKey(TKey key) => _keys.Contains(key);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _keyValues.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _keyValues.GetEnumerator();

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _keyValues.TryGetValue(key, out value);

}
