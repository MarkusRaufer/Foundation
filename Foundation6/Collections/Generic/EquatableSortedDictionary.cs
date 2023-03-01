namespace Foundation.Collections.Generic;

using Foundation;
using Foundation.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;

/// <summary>
/// This dictionary considers the equality of all keys and values <see cref="Equals"/>.
/// The position of the elements are ignored.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
[Serializable]
public class EquatableSortedDictionary<TKey, TValue>
    : IDictionary<TKey, TValue>
    , ICollectionChanged<KeyValuePair<TKey, TValue>>
    , IEquatable<EquatableSortedDictionary<TKey, TValue>>
    , ISerializable
    where TKey : notnull
{
    private int _hashCode;
    private readonly SortedDictionary<TKey, TValue> _keyValues;

    public EquatableSortedDictionary() : this(new SortedDictionary<TKey, TValue>())
    {
    }

    public EquatableSortedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        : this(keyValues.ToSortedDictionary())
    {
    }

    public EquatableSortedDictionary(SortedDictionary<TKey, TValue> keyValues)
    {
        _keyValues = keyValues.ThrowIfNull();
        _hashCode = CreateHashCode();

        CollectionChanged = new Event<Action<CollectionEvent<KeyValuePair<TKey, TValue>>>>();
    }

    public EquatableSortedDictionary(SerializationInfo info, StreamingContext context)
    {
        if (info.GetValue(nameof(_keyValues), typeof(SortedDictionary<TKey, TValue>)) is SortedDictionary<TKey, TValue> keyValues)
        {
            _keyValues = keyValues;
        }
        else
        {
            _keyValues = new SortedDictionary<TKey, TValue>();
        }

        _hashCode = CreateHashCode();

        CollectionChanged = new Event<Action<CollectionEvent<KeyValuePair<TKey, TValue>>>>();
    }

    public TValue this[TKey key]
    {
        get => _keyValues[key];
        set
        {
            var keyExists = _keyValues.ContainsKey(key);
            TValue? existingValue = default;

            if (keyExists)
            {
                existingValue = _keyValues[key];
                if (EqualityComparer<TValue>.Default.Equals(existingValue, value)) return;
            }

            _keyValues[key] = value;

            _hashCode = CreateHashCode();

            var changeEvent = keyExists
                ? new { State = CollectionChangedState.ElementReplaced, Element = Pair.New(key, existingValue) }
                : new { State = CollectionChangedState.ElementAdded, Element = Pair.New<TKey, TValue?>(key, value) };

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
        keyValue.ThrowIfKeyIsNull();

        _keyValues.Add(keyValue.Key, keyValue.Value);

        _hashCode = CreateHashCode();

        CollectionChanged.Publish(new { State = CollectionChangedState.ElementAdded, Element = keyValue });

        Add(keyValue.Key, keyValue.Value);
    }

    public void Add(IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
    {
        keyValues.ThrowIfNull();

        foreach (var keyValue in keyValues)
        {
            Add(keyValue);
        }

        _hashCode = CreateHashCode();

    }

    public void Clear()
    {
        _keyValues.Clear();
        _hashCode = CreateHashCode();

        CollectionChanged.Publish(new { State = CollectionChangedState.CollectionCleared });
    }

    public Event<Action<CollectionEvent<KeyValuePair<TKey, TValue>>>> CollectionChanged { get; private set; }

    public bool Contains(KeyValuePair<TKey, TValue> item) => _keyValues.Contains(item);

    public bool ContainsKey(TKey key) => _keyValues.ContainsKey(key);

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _keyValues.CopyTo(array, arrayIndex);

    public int Count => _keyValues.Count;

    protected int CreateHashCode()
    {
        return HashCode.CreateBuilder()
                       .AddHashCode(DefaultHashCode)
                       .AddObjects(_keyValues)
                       .GetHashCode();
    }

    protected static int DefaultHashCode { get; } = typeof(EquatableDictionary<TKey, TValue>).GetHashCode();

    /// <summary>
    /// Considers keys and values only. Positions of the elements are ignored.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => Equals(obj as EquatableSortedDictionary<TKey, TValue>);

    /// <summary>
    /// Considers keys and values only. Positions of the elements are ignored.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(EquatableSortedDictionary<TKey, TValue>? other)
    {
        if (other is null) return false;
        if (_hashCode != other._hashCode) return false;

        return _keyValues.SequenceEqual(other);
    }

    IEnumerator IEnumerable.GetEnumerator() => _keyValues.GetEnumerator();

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _keyValues.GetEnumerator();

    /// <summary>
    /// Considers all keys and values. Positions of the elements are ignored.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _hashCode;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(_keyValues), _keyValues);
    }

    public bool IsReadOnly => false;

    public ICollection<TKey> Keys => _keyValues.Keys;

    public bool Remove(TKey key)
    {
        if (_keyValues.Remove(key))
        {
            _hashCode = CreateHashCode();
            var element = Pair.New<TKey, TValue?>(key, default);

            CollectionChanged.Publish(new { State = CollectionChangedState.ElementRemoved, Element = element });
            return true;
        }

        return false;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return Remove(item.Key);
    }

    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value) => _keyValues.TryGetValue(key, out value);

    public ICollection<TValue> Values => _keyValues.Values;
}
