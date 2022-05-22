namespace Foundation.ComponentModel;

using Foundation;
using Foundation.Collections.Generic;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

public class EquatableMap<TKey, TValue>
    : IDictionary<TKey, TValue>
    , IEquatable<IDictionary<TKey, TValue>>
    , IEquatable<IReadOnlyDictionary<TKey, TValue>>
    where TKey : notnull
{
    private readonly IDictionary<TKey, TValue> _keyValues;

    public EquatableMap() : this(new Dictionary<TKey, TValue>())
    {
    }

    public EquatableMap([DisallowNull] IEnumerable<KeyValuePair<TKey, TValue>> keyValues) 
        : this(keyValues.ToDictionary(kv => kv.Key, kv => kv.Value))
    {
    }

    public EquatableMap([DisallowNull] IDictionary<TKey, TValue> keyValues)
    {
        _keyValues = keyValues.ThrowIfNull();
    }

    public TValue this[TKey key]
    {
        get => _keyValues[key];
        set => _keyValues[key] = value;
    }

    public void Add(TKey key, TValue value)
    {
        key.ThrowIfNull();

        Add(Pair.New(key, value));
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        item.ThrowIfEmpty();

        _keyValues.Add(item);

        var hcb = Foundation.HashCode.CreateBuilder();
        hcb.AddHashCode(HashCode);
        hcb.AddObject(item);

        HashCode = hcb.GetHashCode();
    }

    public void Add(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
    {
        keyValues.ThrowIfNull();

        var hcb = Foundation.HashCode.CreateBuilder();
        
        foreach (var keyValue in keyValues.OnFirst(() => hcb.AddHashCode(HashCode)))
        {
            _keyValues.Add(keyValue);
            hcb.AddObject(keyValue);
        }

        HashCode = hcb.GetHashCode();
    }

    public void Clear()
    {
        _keyValues.Clear();
        HashCode = DefaultHashCode;
    }

    public int Count => _keyValues.Count;

    public bool Contains(KeyValuePair<TKey, TValue> item) => _keyValues.Contains(item);

    public bool ContainsKey(TKey key) => _keyValues.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _keyValues.CopyTo(array, arrayIndex);

    protected int CreateHashCode()
    {
        var hcb = Foundation.HashCode.CreateBuilder();
        hcb.AddHashCode(DefaultHashCode);
        hcb.AddObjects(_keyValues);

        return hcb.GetHashCode();
    }

    protected static int DefaultHashCode { get; } = typeof(EquatableMap<TKey, TValue>).GetHashCode();

    public override bool Equals(object? obj) => obj is IDictionary<TKey, TValue> other && Equals(other);

    public bool Equals(IDictionary<TKey, TValue>? other)
    {
        return null != other && _keyValues.IsEqualTo(other);
    }

    public bool Equals(IReadOnlyDictionary<TKey, TValue>? other)
    {
        return null != other && _keyValues.IsEqualTo(other);
    }

    IEnumerator IEnumerable.GetEnumerator() => _keyValues.GetEnumerator();

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _keyValues.GetEnumerator();

    public override int GetHashCode() => HashCode;

    protected int HashCode { get; set; } = DefaultHashCode;

    public bool IsReadOnly => _keyValues.IsReadOnly;

    public ICollection<TKey> Keys => _keyValues.Keys;

    public bool Remove(TKey key)
    {
        var removed = _keyValues.Remove(key);
        if (removed) HashCode = CreateHashCode();

        return removed;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        var removed = _keyValues.Remove(item);
        if (removed) HashCode = CreateHashCode();

        return removed;
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _keyValues.TryGetValue(key, out value);

    public ICollection<TValue> Values => _keyValues.Values;
}
