using Foundation.Collections.Generic;
using System.Collections;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Immutable;

public static class ImmutableMultiValuesMap
{
    public static ImmutableMultiValuesMap<TKey, TValue> New<TKey, TValue>()
        where TKey : notnull
    {
        return new(new MultiValueMap<TKey, TValue>(), () => new List<TValue>());
    }

    public static ImmutableMultiValuesMap<TKey, TValue> New<TKey, TValue>(
        Func<ICollection<TValue>> valueCollectionFactory)
        where TKey : notnull
    {
        return new(new MultiValueMap<TKey, TValue>(), valueCollectionFactory);
    }

    public static ImmutableMultiValuesMap<TKey, TValue> New<TKey, TValue>(
        IMultiValueMap<TKey, TValue> map,
        Func<ICollection<TValue>> valueCollectionFactory)
        where TKey : notnull
    {
        return new (map, valueCollectionFactory);
    }
}

public class ImmutableMultiValuesMap<TKey, TValue> : ImmutableMultiValuesMap<TKey, TValue, IImmutableDictionary<TKey, TValue>>
    where TKey : notnull
{
    public ImmutableMultiValuesMap()
    {
    }

    public ImmutableMultiValuesMap(Func<ICollection<TValue>> valueCollectionFactory) : base(valueCollectionFactory)
    {
    }

    public ImmutableMultiValuesMap(IMultiValueMap<TKey, TValue> map, Func<ICollection<TValue>> valueCollectionFactory) : base(map, valueCollectionFactory)
    {
    }
}

public class ImmutableMultiValuesMap<TKey, TValue, TMap>
    : IImmutableDictionary<TKey, TValue>
    , IReadOnlyMultiValueMap<TKey, TValue>
    where TKey : notnull
    where TMap : IImmutableDictionary<TKey, TValue>
{
    private readonly IMultiValueMap<TKey, TValue> _map;
    private readonly Func<ICollection<TValue>> _valueCollectionFactory;

    public ImmutableMultiValuesMap() : this(new MultiValueMap<TKey, TValue>(), () => new List<TValue>())
    {
    }

    public ImmutableMultiValuesMap(Func<ICollection<TValue>> valueCollectionFactory)
        : this(new MultiValueMap<TKey, TValue>(), valueCollectionFactory)
    {
    }

    public ImmutableMultiValuesMap(
        IMultiValueMap<TKey, TValue> map,
        Func<ICollection<TValue>> valueCollectionFactory)
    {
        _map = new MultiValueMap<TKey, TValue>(map, valueCollectionFactory);
        _valueCollectionFactory = valueCollectionFactory;
    }

    public TValue this[TKey key] => _map[key];

    public IEnumerable<TKey> Keys => _map.Keys;

    public IEnumerable<TValue> Values => _map.Values;

    public int Count => _map.Count;

    public int KeyCount => throw new NotImplementedException();

    public IImmutableDictionary<TKey, TValue> Add(TKey key, TValue value)
    {
        var newMap = new MultiValueMap<TKey, TValue>(_map, _valueCollectionFactory)
        {
            { key, value }
        };
        return ImmutableMultiValuesMap.New(newMap, _valueCollectionFactory);
    }

    public IImmutableDictionary<TKey, TValue> AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs)
    {
        var newMap = new MultiValueMap<TKey, TValue>(_map, _valueCollectionFactory);
        foreach (var pair in pairs)
            newMap.Add(pair.Key, pair.Value);

        return ImmutableMultiValuesMap.New(newMap, _valueCollectionFactory);
    }

    public IImmutableDictionary<TKey, TValue> Clear()
    {
        return ImmutableMultiValuesMap.New<TKey, TValue>(_valueCollectionFactory);
    }

    public bool Contains(KeyValuePair<TKey, TValue> pair) => _map.Contains(pair);

    public bool Contains(TKey key, TValue value) => _map.Contains(key, value);

    public bool ContainsKey(TKey key) => _map.ContainsKey(key);

    public bool ContainsValue(TValue value) => _map.ContainsValue(value);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _map.GetEnumerator();

    public IEnumerable<KeyValuePair<TKey, TValue>> GetFlattenedKeyValues(IEnumerable<TKey>? keys = null)
    {
        return _map.GetFlattenedKeyValues(keys);
    }

    public IEnumerable<TValue> GetFlattenedValues(IEnumerable<TKey>? keys = null)
    {
        return _map.GetFlattenedValues(keys);
    }

    public IEnumerable<TKey> GetKeys(IEnumerable<TValue> values)
    {
        return _map.GetKeys(values);
    }

    public IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> GetKeyValues(IEnumerable<TKey>? keys = null)
    {
        return _map.GetKeyValues(keys);
    }

    public IEnumerable<TValue> GetValues(TKey key) => _map.GetValues(key);

    public IEnumerable<TValue> GetValues(IEnumerable<TKey> keys) => _map.GetValues(keys);

    public int GetValuesCount(TKey key) => _map.GetValuesCount(key);

    public IImmutableDictionary<TKey, TValue> Remove(TKey key)
    {
        var newMap = new MultiValueMap<TKey, TValue>(_valueCollectionFactory);
        foreach(var pair in _map)
        {
            if (pair.Key.Equals(key)) continue;
            newMap.Add(pair);
        }
        return ImmutableMultiValuesMap.New(newMap, _valueCollectionFactory);
    }

    public IImmutableDictionary<TKey, TValue> RemoveRange(IEnumerable<TKey> keys)
    {
        var removeKeys = keys.ToArray();
        var newMap = new MultiValueMap<TKey, TValue>(_valueCollectionFactory);
        foreach (var pair in _map)
        {
            if (removeKeys.Any(x => x.Equals(pair.Key))) continue;
            newMap.Add(pair);
        }
        return ImmutableMultiValuesMap.New(newMap, _valueCollectionFactory);
    }

    public IImmutableDictionary<TKey, TValue> SetItem(TKey key, TValue value)
    {
        var newMap = new MultiValueMap<TKey, TValue>(_map, _valueCollectionFactory);
        newMap.Add(key, value);

        return ImmutableMultiValuesMap.New(newMap, _valueCollectionFactory);
    }

    public IImmutableDictionary<TKey, TValue> SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items)
    {
        var newMap = new MultiValueMap<TKey, TValue>(_map, _valueCollectionFactory);

        foreach(var grp in items.GroupBy(x => x.Key))
        {
            if(!newMap.TryGetValues(grp.Key, out var values))
            {
                values = _valueCollectionFactory();
            }

            foreach (var kvp in grp)
                values!.Add(kvp.Value);
        }

        return ImmutableMultiValuesMap.New(newMap, _valueCollectionFactory);
    }

    public bool TryGetKey(TKey equalKey, out TKey actualKey)
    {
        if(_map.ContainsKey(equalKey))
        {
            actualKey = equalKey;
            return true;
        }
        actualKey = default;

        return false;
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _map.TryGetValue(key, out value);

    public bool TryGetValues(TKey key, out ICollection<TValue>? values)
    {
        return _map.TryGetValues(key, out values);
    }

    IEnumerator IEnumerable.GetEnumerator() => _map.GetEnumerator();
}
