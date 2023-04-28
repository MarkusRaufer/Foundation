namespace Foundation.Collections.Generic;

using Foundation;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public static class NonEmptyDictionaryValue
{
    public static NonEmptyDictionaryValue<TKey, TValue> New<TKey, TValue>(IDictionary<TKey, TValue> dictionary)
        where TKey : notnull
        => new(dictionary);
}

/// <summary>
/// Immutable dictionary that considers the equality of all keys and values <see cref="Equals"/>.The dictionary must not be empty.
/// The position of the elements are ignored.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class NonEmptyDictionaryValue<TKey, TValue>
    : IReadOnlyDictionary<TKey, TValue>
    , IEquatable<NonEmptyDictionaryValue<TKey, TValue>>
    where TKey : notnull
{
    private readonly IDictionary<TKey, TValue> _dictionary;
    private readonly int _hashCode;

    public NonEmptyDictionaryValue(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        : this(keyValues.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
    {
    }

    public NonEmptyDictionaryValue(IDictionary<TKey, TValue> dictionary)
    {
        _dictionary = dictionary.ThrowIfNullOrEmpty();
        _hashCode = HashCode.FromObjects(_dictionary);
    }
    
    public TValue this[TKey key] => _dictionary[key];

    public int Count => _dictionary.Count;

    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    protected static int DefaultHashCode { get; } = typeof(NonEmptyDictionaryValue<TKey, TValue>).GetHashCode();

    public override bool Equals(object? obj) => Equals(obj as NonEmptyDictionaryValue<TKey, TValue>);

    public bool Equals(NonEmptyDictionaryValue<TKey, TValue>? other)
    {
        if (other is null) return false;
        if (_hashCode != other._hashCode) return false;

        return _dictionary.IsEqualToSet(other._dictionary);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public IEnumerable<TKey> Keys => _dictionary.Keys;

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    public override string ToString() => string.Join(", ", _dictionary);

    public IEnumerable<TValue> Values => _dictionary.Values;
}
