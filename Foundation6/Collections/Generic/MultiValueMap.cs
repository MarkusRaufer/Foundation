namespace Foundation.Collections.Generic;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Dictionary that supports multiple values per key.
/// It is used for 1 to many relations. 
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class MultiValueMap<TKey, TValue>
    : MultiValueMap<TKey, TValue, ICollection<TValue>>
    , IMultiValueMap<TKey, TValue>
    where TKey : notnull
{
    public MultiValueMap() : this(new Dictionary<TKey, ICollection<TValue>>(), () => new List<TValue>())
    {
    }

    public MultiValueMap(int capacity) : this(capacity, () => new List<TValue>())
    {
    }

    public MultiValueMap(int capacity, Func<ICollection<TValue>> valueCollectionFactory)
        : this(new Dictionary<TKey, ICollection<TValue>>(capacity), valueCollectionFactory)
    {
    }

    public MultiValueMap(IEqualityComparer<TKey> comparer) : this(comparer, () => new List<TValue>())
    {
    }

    public MultiValueMap(IEqualityComparer<TKey> comparer, Func<ICollection<TValue>> valueCollectionFactory)
        : this(new Dictionary<TKey, ICollection<TValue>>(comparer), valueCollectionFactory)
    {
    }

    public MultiValueMap(int capasity, IEqualityComparer<TKey> comparer, Func<ICollection<TValue>> valueCollectionFactory)
        : this(new Dictionary<TKey, ICollection<TValue>>(capasity, comparer), valueCollectionFactory)
    {
    }

    public MultiValueMap(IDictionary<TKey, ICollection<TValue>> dictionary)
        : this(dictionary, () => new List<TValue>())
    {
    }

    public MultiValueMap(Func<ICollection<TValue>> valueCollectionFactory)
        : this(new Dictionary<TKey, ICollection<TValue>>(), valueCollectionFactory)
    {
    }

    public MultiValueMap(
            IEnumerable<KeyValuePair<TKey, TValue>> keyValues,
            Func<ICollection<TValue>> valueCollectionFactory)
        : base(keyValues, valueCollectionFactory)
    { 
    }

    public MultiValueMap(
        IDictionary<TKey, ICollection<TValue>> dictionary,
        Func<ICollection<TValue>> valueCollectionFactory)
        : base(dictionary, valueCollectionFactory)
    {
    }
}

public class MultiValueMap<TKey, TValue, TValueCollection>
    : IMultiValueMap<TKey, TValue, TValueCollection>
    where TKey : notnull
    where TValueCollection : ICollection<TValue>
{
    private readonly IDictionary<TKey, TValueCollection> _dictionary;
    private readonly Func<TValueCollection> _valueCollectionFactory;


    public MultiValueMap(Func<TValueCollection> valueCollectionFactory)
        : this(new Dictionary<TKey, TValueCollection>(), valueCollectionFactory)
    {
    }

    public MultiValueMap(int capacity, Func<TValueCollection> valueCollectionFactory)
        : this(new Dictionary<TKey, TValueCollection>(capacity), valueCollectionFactory)
    {
    }

    public MultiValueMap(IEqualityComparer<TKey> comparer, Func<TValueCollection> valueCollectionFactory)
        : this(new Dictionary<TKey, TValueCollection>(comparer), valueCollectionFactory)
    {
    }

    public MultiValueMap(int capasity, IEqualityComparer<TKey> comparer, Func<TValueCollection> valueCollectionFactory)
        : this(new Dictionary<TKey, TValueCollection>(capasity, comparer), valueCollectionFactory)
    {
    }

    public MultiValueMap(
        IEnumerable<KeyValuePair<TKey, TValue>> keyValues,
        Func<TValueCollection> valueCollectionFactory)
    {
        _dictionary = new Dictionary<TKey, TValueCollection>();
        _valueCollectionFactory = valueCollectionFactory.ThrowIfNull();

        foreach (var (key, value) in keyValues)
        {
            Add(key, value);
        }
    }

    public MultiValueMap(
        IDictionary<TKey, TValueCollection> dictionary,
        Func<TValueCollection> valueCollectionFactory)
    {
        _dictionary = dictionary.ThrowIfNull();
        _valueCollectionFactory = valueCollectionFactory.ThrowIfNull();
    }

    /// <summary>
    /// Adds key value to the map.
    /// </summary>
    /// <param name="item"></param>
    public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

    /// <summary>
    /// Adds key value to the map.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void Add(TKey key, TValue value)
    {
        if (!_dictionary.TryGetValue(key, out TValueCollection? values))
        {
            values = _valueCollectionFactory();
            _dictionary.Add(key, values);
        }

        values.Add(value);
    }

    /// <inheritdoc/>
    public void AddRange(TKey key, IEnumerable<TValue> values)
    {
        if (!_dictionary.TryGetValue(key, out TValueCollection? valueCollection))
        {
            valueCollection = _valueCollectionFactory();
            _dictionary.Add(key, valueCollection);
        }

        foreach (var value in values)
            valueCollection.Add(value);
    }

    /// <inheritdoc/>
    public void AddSingle(KeyValuePair<TKey, TValue> item) => AddSingle(item.Key, item.Value);

    /// <inheritdoc/>
    public void AddSingle(TKey key, TValue value)
    {
        if (!_dictionary.TryGetValue(key, out TValueCollection? values))
        {
            values = _valueCollectionFactory();
            values.Add(value);

            _dictionary.Add(key, values);
            return;
        }

        values.Clear();
        values.Add(value);
    }

    /// <inheritdoc/>
    public bool AddUnique(TKey key, TValue value, bool replaceExisting = false)
    {
        if (!_dictionary.TryGetValue(key, out TValueCollection? values))
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

    /// <inheritdoc/>
    public virtual void Clear() => _dictionary.Clear();

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
        if (_dictionary.TryGetValue(key, out TValueCollection? values))
            return values.Contains(value);

        return false;
    }
    /// <inheritdoc/>
    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    /// <inheritdoc/>
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
    /// <param name="arrayIndex">The index of the array where to start copiing.</param>
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
        return GetFlattenedKeyValues(Array.Empty<TKey>()).GetEnumerator();
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
            if (_dictionary.TryGetValue(key, out TValueCollection? values))
                yield return new KeyValuePair<TKey, IEnumerable<TValue>>(key, values);
        }
    }


    public IEnumerable<TValue> GetValues(TKey key)
    {
        if (!_dictionary.TryGetValue(key, out TValueCollection? values)) yield break;
        foreach (var value in values)
        {
            yield return value;
        }
    }

    /// <summary>
    /// Returns the values of the keys.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
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
        if (_dictionary.TryGetValue(key, out TValueCollection? values))
            return values.Count;

        return 0;
    }

    public bool IsReadOnly => _dictionary.IsReadOnly;

    /// <summary>
    /// Returns the number of keys.
    /// </summary>
    public int KeyCount => _dictionary.Count;

    public ICollection<TKey> Keys
    {
        get { return _dictionary.Keys; }
    }

    public virtual bool Remove(TKey key) => _dictionary.Remove(key);

    public virtual bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key, item.Value);

    public virtual bool Remove(TKey key, TValue value)
    {
        if (_dictionary.TryGetValue(key, out TValueCollection? values))
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
    public virtual bool RemoveValue(TValue value, IEnumerable<TKey> keys)
    {
        var removed = Fused.Value(false).BlowIfChanged();
        foreach (var key in keys)
        {
            if (!_dictionary.TryGetValue(key, out TValueCollection? values)) continue;

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
    public bool TryGetValue(TKey key, out TValue value)
    {
        if (_dictionary.TryGetValue(key, out TValueCollection? values))
        {
            if (0 < values.Count)
            {
                value = values.First();
                return true;
            }
        }

#pragma warning disable CS8601 // Possible null reference assignment.
        value = default;
#pragma warning restore CS8601 // Possible null reference assignment.
        return false;
    }
#pragma warning restore

    /// <summary>
    /// Returns values of key if true otherwise an empty list.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public bool TryGetValues(TKey key, [NotNullWhen(true)] out TValueCollection? values)
    {
        if (_dictionary.TryGetValue(key, out TValueCollection? vals))
        {
            values = vals;
            return true;
        }

        values = default;
        return false;
    }

    public virtual TValue this[TKey key]
    {
        get { return _dictionary[key].First(); }
        set { Add(key, value); }
    }

    /// <summary>
    /// Gets all values from all keys.
    /// </summary>
    public ICollection<TValue> Values => GetFlattenedValues(Array.Empty<TKey>()).ToList();
}
