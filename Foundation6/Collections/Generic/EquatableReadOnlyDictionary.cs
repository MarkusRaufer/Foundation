namespace Foundation.Collections.Generic;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public class EquatableReadOnlyDictionary<TKey, TValue>
    : IReadOnlyDictionary<TKey, TValue>
    , IEquatable<EquatableReadOnlyDictionary<TKey, TValue>>
    where TKey : notnull
{
    private readonly IDictionary<TKey, TValue> _dictionary;
    private readonly int _hashCode;

    public EquatableReadOnlyDictionary(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        : this(keyValues.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
    {
    }

    public EquatableReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
    {
        _dictionary = dictionary.ThrowIfNull();
        _hashCode = HashCode.FromObjects(_dictionary.ToKeyValues());
    }

    public TValue this[TKey key] => _dictionary[key];

    public int Count => _dictionary.Count;

    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    public override bool Equals(object? obj) => Equals(obj as EquatableReadOnlyDictionary<TKey, TValue>);

    public bool Equals(EquatableReadOnlyDictionary<TKey, TValue>? other)
    {
        return null != other && _dictionary.IsEqualTo(other._dictionary);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public IEnumerable<TKey> Keys => _dictionary.Keys;

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    public IEnumerable<TValue> Values => _dictionary.Values;
}
