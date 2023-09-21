namespace Foundation.Collections.Generic;

using Foundation;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public static class DictionaryValue
{
    public static DictionaryValue<TKey, TValue> New<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        where TKey : notnull
        => new(keyValues);

    public static DictionaryValue<TKey, TValue> New<TKey, TValue>(params KeyValuePair<TKey, TValue>[] keyValues)
        where TKey: notnull
        => new(keyValues);

    public static DictionaryValue<TKey, TValue> NewWith<TKey, TValue>(
        this DictionaryValue<TKey, TValue> dictionaryValue,
        IEnumerable<KeyValuePair<TKey, TValue>> replacements)
        where TKey : notnull
    {
        return dictionaryValue.Replace(replacements).ToDictionaryValue(x => x.Key, x => x.Value);
    }
}

/// <summary>
/// This immutable dictionary considers the equality of all keys and values <see cref="Equals"/>.
/// The position of the elements are ignored.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class DictionaryValue<TKey, TValue>
    : IReadOnlyDictionary<TKey, TValue>
    , IEquatable<DictionaryValue<TKey, TValue>>
    where TKey : notnull
{
    private readonly IDictionary<TKey, TValue> _dictionary;
    private readonly int _hashCode;

    public DictionaryValue(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        : this(keyValues.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
    {
    }

    public DictionaryValue(IEnumerable<KeyValuePair<TKey, TValue>> keyValues, IEqualityComparer<TKey> comparer)
        : this(new Dictionary<TKey, TValue>(keyValues, comparer))
    {
    }

    private DictionaryValue(IDictionary<TKey, TValue> dictionary)
    {
        _dictionary = dictionary.ThrowIfNull();
        _hashCode = HashCode.FromObjects(_dictionary);
    }

    public static implicit operator DictionaryValue<TKey, TValue>(KeyValuePair<TKey, TValue>[] keyValues)
        => new(keyValues);

    public static implicit operator DictionaryValue<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        => new(dictionary);

    /// <inheritdoc/>
    public TValue this[TKey key] => _dictionary[key];

    /// <inheritdoc/>
    public int Count => _dictionary.Count;

    /// <inheritdoc/>
    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    protected static int DefaultHashCode { get; } = typeof(DictionaryValue<TKey, TValue>).GetHashCode();

    /// <summary>
    /// Considers the equality and number of all elements <see cref="Equals"/>.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as DictionaryValue<TKey, TValue>);

    /// <summary>
    /// Considers the equality and number of all elements <see cref="Equals"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(DictionaryValue<TKey, TValue>? other)
    {
        if (other is null) return false;
        if (_hashCode != other._hashCode) return false;

        return _dictionary.IsEqualToSet(other._dictionary);
    }

    /// <inheritdoc/>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _dictionary.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _dictionary.GetEnumerator();

    /// <summary>
    /// Hash code considers all elements.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _hashCode;

    /// <inheritdoc/>
    public IEnumerable<TKey> Keys => _dictionary.Keys;

    /// <inheritdoc/>
    public override string ToString() => string.Join(", ", _dictionary);

    /// <inheritdoc/>
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    /// <inheritdoc/>
    public IEnumerable<TValue> Values => _dictionary.Values;
}
