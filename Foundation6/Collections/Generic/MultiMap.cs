namespace Foundation.Collections.Generic;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Dictionary that supports multiple values for one key.
/// It is used for 1 to many relations.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class MultiMap<TKey, TValue> : IMultiMap<TKey, TValue>
    where TKey : notnull
{
    private readonly IDictionary<TKey, ICollection<TValue>> _dictionary;
    private readonly Func<ICollection<TValue>> _valueCollectionFactory;

    public MultiMap() : this(new Dictionary<TKey, ICollection<TValue>>(), () => new List<TValue>())
    {
    }

    public MultiMap(int capacity) : this(capacity, () => new List<TValue>())
    {
    }

    public MultiMap(int capacity, Func<ICollection<TValue>> valueCollectionFactory)
        : this(new Dictionary<TKey, ICollection<TValue>>(capacity), valueCollectionFactory)
    {
    }

    public MultiMap(IEqualityComparer<TKey> comparer) : this(comparer, () => new List<TValue>())
    {
    }

    public MultiMap(IEqualityComparer<TKey> comparer, Func<ICollection<TValue>> valueCollectionFactory)
        : this(new Dictionary<TKey, ICollection<TValue>>(comparer), valueCollectionFactory)
    {
    }

    public MultiMap(IDictionary<TKey, ICollection<TValue>> dictionary)
        : this(dictionary, () => new List<TValue>())
    {
    }

    public MultiMap(Func<ICollection<TValue>> valueCollectionFactory)
        : this(new Dictionary<TKey, ICollection<TValue>>(), valueCollectionFactory)
    {
    }


    public MultiMap(
        IDictionary<TKey, ICollection<TValue>> dictionary,
        Func<ICollection<TValue>> valueCollectionFactory)
    {
        _dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
        _valueCollectionFactory = valueCollectionFactory ?? throw new ArgumentNullException(nameof(valueCollectionFactory));
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    public void Add(TKey key, TValue value)
    {
        if (!_dictionary.TryGetValue(key, out ICollection<TValue>? values))
        {
            values = _valueCollectionFactory();
            _dictionary.Add(key, values);
        }

        values.Add(value);
    }

    public void AddSingle(KeyValuePair<TKey, TValue> item)
    {
        AddSingle(item.Key, item.Value);
    }

    /// <summary>
    /// Has only one value for a key.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void AddSingle(TKey key, TValue value)
    {
        if (!_dictionary.TryGetValue(key, out ICollection<TValue>? values))
        {
            values = _valueCollectionFactory();
            _dictionary.Add(key, values);
        }

        values.Clear();
        values.Add(value);
    }

    public bool AddUnique(TKey key, TValue value, bool replaceExisting = false)
    {
        if (!_dictionary.TryGetValue(key, out ICollection<TValue>? values))
        {
            values = _valueCollectionFactory();
            values.Add(value);

            _dictionary.Add(key, values);
            return true;
        }

        if (values.Contains(value))
        {
            if (!replaceExisting) return false;
            values.Remove(value);
        }

        values.Add(value);
        return true;
    }

    public virtual void Clear()
    {
        _dictionary.Clear();
    }

    /// <summary>
    /// Checks if key value pair exists.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return Contains(item.Key, item.Value);
    }

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

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool ContainsValue(TValue value)
    {
        foreach (var pair in _dictionary)
        {
            if (null != pair.Value && pair.Value.Contains(value))
                return true;
        }

        return false;
    }

    /// <summary>
    /// returns number of keys.
    /// </summary>
    public int Count => _dictionary.Count;

    /// <summary>
    /// Copies only first value of each key.
    /// </summary>
    /// <param name="array"></param>
    /// <param name="arrayIndex"></param>
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        var i = 0;
        foreach(var kv in GetKeyValues())
        {
            if (i < arrayIndex)
            {
                i++;
                continue;
            }

            array[i] = Pair.New(kv.Key, kv.Value.First());
            i++;
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return GetFlattenedKeyValues().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Returns key values as flat list. If keys is empty it returns all key values.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public IEnumerable<KeyValuePair<TKey, TValue>> GetFlattenedKeyValues(params TKey[] keys)
    {

        if (0 == keys.Length)
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
    public IEnumerable<TValue> GetFlattenedValues(params TKey[] keys)
    {
        return 0 == keys.Length
            ? _dictionary.Keys.SelectMany(key => GetValues(key))
            : keys.SelectMany(key => GetValues(key));
    }
    
    /// <summary>
    /// Returns the keys containing the value.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public IEnumerable<TKey> GetKeys(params TValue[] values)
    {
        if (0 == values.Length)
        {
            foreach (var key in _dictionary.Keys)
                yield return key;

            yield break;
        }

        foreach (var kvp in _dictionary)
        {
            if (Array.Exists(values, value => kvp.Value.EqualsNullable(value)))
                yield return kvp.Key;
        }
    }

    /// <summary>
    /// Returns all keys with their values.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> GetKeyValues(params TKey[] keys)
    {
        if (0 == keys.Length)
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

    /// <summary>
    /// Returns the values of the keys.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public IEnumerable<TValue> GetValues(params TKey[] keys)
    {
        IEnumerable<TValue> getValues(TKey key)
        {
            if (_dictionary.TryGetValue(key, out ICollection<TValue>? values))
            {
                foreach (var value in values)
                    yield return value;
            }
            yield break;
        }
        return keys.SelectMany(getValues);
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

    public bool IsReadOnly
    {
        get { return _dictionary.IsReadOnly; }
    }

    public ICollection<TKey> Keys
    {
        get { return _dictionary.Keys; }
    }

    public virtual bool Remove(TKey key) => _dictionary.Remove(key);

    public virtual bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key, item.Value);

    public virtual bool Remove(TKey key, TValue value)
    {
        if (_dictionary.TryGetValue(key, out ICollection<TValue>? values))
        {
            var removed = values.Remove(value);
            if (!values.Any())
                removed = _dictionary.Remove(key);

            return removed;
        }

        return false;
    }

    /// <summary>
    /// Removes value from all keys.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public virtual bool RemoveValue(TValue value)
    {
        var removed = Fused.Value(false).BlowIfChanged();
        foreach (var values in _dictionary.Values)
        {
            removed.Value = values.Remove(value);
        }

        return removed.Value;
    }

    /// <summary>
    /// Removes value from keys.
    /// </summary>
    /// <param name=""></param>
    /// <param name="keys"></param>
    /// <returns></returns>
    public virtual bool RemoveValue(TValue value, params TKey[] keys)
    {
        var removed = Fused.Value(false).BlowIfChanged();
        foreach (var key in keys)
        {
            if (!_dictionary.TryGetValue(key, out ICollection<TValue>? values)) continue;

            if (values.Remove(value))
            {
                removed.Value = true;

                if (0 == values.Count)
                    _dictionary.Remove(key);
            }
        }

        return removed.Value;
    }

#pragma warning disable CS8767
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue? value)
    {
        if (_dictionary.TryGetValue(key, out ICollection<TValue>? values))
        {
            if (0 < values.Count)
            {
                value = values.First();
                return true;
            }
        }

        value = default;
        return false;
    }
#pragma warning restore

    /// <summary>
    /// Returns values of key if true otherwise an empty list.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public bool TryGetValues(TKey key, out IEnumerable<TValue> values)
    {
        if (_dictionary.TryGetValue(key, out ICollection<TValue>? vals))
        {
            values = vals;
            return true;
        }

        values = Enumerable.Empty<TValue>();
        return false;
    }

    public virtual TValue this[TKey key]
    {
        get { return _dictionary[key].First(); }
        set { Add(key, value); }
    }

    /// <summary>
    /// Returns the number of all values.
    /// </summary>
    public int ValuesCount => _dictionary.Aggregate(0, (sum, elem) => sum + elem.Value.Count);

    /// <summary>
    /// Gets all values from all keys.
    /// </summary>
    public ICollection<TValue> Values
    {
        get { return GetFlattenedValues().ToList(); }
    }
}

