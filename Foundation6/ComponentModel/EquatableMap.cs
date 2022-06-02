namespace Foundation.ComponentModel;

using Foundation;
using Foundation.Collections.Generic;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

/// <summary>
/// Equals of this map checks the equality of all elements.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
[Serializable]
public class EquatableMap<TKey, TValue>
    : IDictionary<TKey, TValue>
    , ICollectionChanged<KeyValuePair<TKey, TValue>>
    , IEquatable<EquatableMap<TKey, TValue>>
    , ISerializable
    where TKey : notnull
{
    private int _hashCode;
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
        CreateHashCodeFromKeyValues();

        CollectionChanged = new Event<Action<CollectionEvent<KeyValuePair<TKey, TValue>>>>();
    }

    public EquatableMap(SerializationInfo info, StreamingContext context)
    {
        if (info.GetValue(nameof(_keyValues), typeof(Dictionary<TKey, TValue>)) is Dictionary<TKey, TValue> keyValues)
        {
            _keyValues = keyValues;
        }
        else
        {
            _keyValues = new Dictionary<TKey, TValue>();
        }

        CreateHashCodeFromKeyValues();

        CollectionChanged = new Event<Action<CollectionEvent<KeyValuePair<TKey, TValue>>>>();
    }

    public TValue this[TKey key]
    {
        get => _keyValues[key];
        set
        {
            var keyExists = _keyValues.ContainsKey(key);
            if (keyExists)
            {
                var existingValue = _keyValues[key];
                if (EqualityComparer<TValue>.Default.Equals(existingValue, value)) return;
            }

            _keyValues[key] = value;

            CreateHashCodeFromKeyValues();

            var changeEvent = keyExists
                ? new { State = CollectionChangedState.ElementReplaced, Element = Pair.New(key, value) }
                : new { State = CollectionChangedState.ElementAdded, Element = Pair.New(key, value) };

            CollectionChanged.Publish(changeEvent);
        }
    }

    public void Add(TKey key, TValue value)
    {
        key.ThrowIfNull();

        Add(Pair.New(key, value));
    }

    public void Add(KeyValuePair<TKey, TValue> keyValue)
    {
        keyValue.ThrowIfEmpty();

        _keyValues.Add(keyValue);

        AddHashCodeFromKeyValues(new[] { keyValue });

        CollectionChanged.Publish(new { State = CollectionChangedState.ElementAdded, Element = keyValue });
    }

    public void Add(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
    {
        keyValues.ThrowIfNull();
        
        foreach (var keyValue in keyValues)
        {
            _keyValues.Add(keyValue);
            CollectionChanged.Publish(new { State = CollectionChangedState.ElementAdded, Element = keyValue });
        }

        AddHashCodeFromKeyValues(keyValues);

    }

    protected void AddHashCodeFromKeyValues(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
    {
        var builder = HashCode.CreateBuilder();
        builder.AddHashCode(_hashCode);
        builder.AddObjects(keyValues);
        _hashCode = builder.GetHashCode();
    }

    public void Clear()
    {
        _keyValues.Clear();
        CreateHashCodeFromKeyValues();

        CollectionChanged.Publish(new { State = CollectionChangedState.CollectionCleared });
    }

    public Event<Action<CollectionEvent<KeyValuePair<TKey, TValue>>>> CollectionChanged { get; private set; }

    public bool Contains(KeyValuePair<TKey, TValue> item) => _keyValues.Contains(item);

    public bool ContainsKey(TKey key) => _keyValues.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _keyValues.CopyTo(array, arrayIndex);

    public int Count => _keyValues.Count;

    protected void CreateHashCodeFromKeyValues()
    {
        var hcb = HashCode.CreateBuilder();

        hcb.AddHashCode(DefaultHashCode);
        hcb.AddObjects(_keyValues);

        _hashCode = hcb.GetHashCode();
    }

    protected static int DefaultHashCode { get; } = typeof(EquatableMap<TKey, TValue>).GetHashCode();

    public override bool Equals(object? obj) => obj is IDictionary<TKey, TValue> other && Equals(other);

    public bool Equals(EquatableMap<TKey, TValue>? other)
    {
        if (other is null) return false;
        if (_hashCode != other._hashCode) return false;

        return _keyValues.IsEqualTo(other);
    }

    IEnumerator IEnumerable.GetEnumerator() => _keyValues.GetEnumerator();

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _keyValues.GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(_keyValues), _keyValues);
    }

    public bool IsReadOnly => _keyValues.IsReadOnly;

    public ICollection<TKey> Keys => _keyValues.Keys;

    public bool Remove(TKey key)
    {
        if (_keyValues.Remove(key))
        {
            CreateHashCodeFromKeyValues();
            var element = Pair.New<TKey, TValue?>(key, default);

            CollectionChanged.Publish(new { State = CollectionChangedState.ElementRemoved, Element = element });
            return true;
        }

        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        if (_keyValues.Remove(item))
        {
            CreateHashCodeFromKeyValues();
            CollectionChanged.Publish(new { State = CollectionChangedState.ElementRemoved, Element = item });
            return true;
        }
        return false;
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _keyValues.TryGetValue(key, out value);

    public ICollection<TValue> Values => _keyValues.Values;
}
