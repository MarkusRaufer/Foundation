using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

public class FixedKeysDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    where TKey : notnull
{
    private readonly HashSet<TKey> _keys;
    private readonly IDictionary<TKey, TValue> _dictionary;

    public FixedKeysDictionary(IEnumerable<TKey> keys)
    {
        _keys = new HashSet<TKey>(keys);
        _dictionary = new Dictionary<TKey, TValue>();
    }

    public FixedKeysDictionary(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
    {
        _dictionary = new Dictionary<TKey, TValue>(keyValues);
        _keys = new HashSet<TKey>(_dictionary.Keys);
    }

    public TValue this[TKey key] 
    { 
        get => _dictionary[key]; 
        set
        {
            if (!_keys.Contains(key)) return;

            _dictionary[key] = value;
        }
    }

    public bool IsComplete => _dictionary.Count == _keys.Count;

    public ICollection<TKey> Keys => _keys.ToArray();

    public ICollection<TValue> Values => _dictionary.Values;

    public int Count => _dictionary.Count;

    public bool IsReadOnly => _dictionary.IsReadOnly;

    public void Add(TKey key, TValue value)
    {
        if (!_keys.Contains(key)) throw new ArgumentOutOfRangeException(nameof(key));

        _dictionary.Add(key, value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        if (!_keys.Contains(item.Key)) throw new ArgumentOutOfRangeException(nameof(item), "key not allowed");
        _dictionary.Add(item);
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public bool Contains(KeyValuePair<TKey, TValue> item) => _dictionary.Contains(item);

    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        _dictionary.CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

    public bool Remove(TKey key) => _dictionary.Remove(key);

    public bool Remove(KeyValuePair<TKey, TValue> item) => _dictionary.Remove(item);

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) =>
        _dictionary.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();
}
