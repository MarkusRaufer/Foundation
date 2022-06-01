namespace Foundation.Collections.ComponentModel;

using Foundation.Collections.Generic;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

[Serializable]
public class EquatableCollection<T>
    : ICollection<T>
    , ISerializable
    , IEquatable<EquatableCollection<T>>
{
    private readonly ICollection<T> _collection;
    private int _hashCode;

    public EquatableCollection() : this(new List<T>())
    {
    }

    public EquatableCollection([DisallowNull] ICollection<T> collection)
    {
        _collection = collection.ThrowIfNull();
        _hashCode = 0 == collection.Count ? 0 : HashCode.FromObjects(_collection);

        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableCollection(int capacity) : this(new List<T>(capacity))
    {
    }

    public EquatableCollection(SerializationInfo info, StreamingContext context)
    {
        if (info.GetValue(nameof(_collection), typeof(List<T>)) is List<T> collection)
        {
            _collection = collection;
            _hashCode = HashCode.FromObjects(_collection);
        }
        else
        {
            _hashCode = 0;
            _collection = new List<T>();
        }
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public void Add(T item)
    {
        item.ThrowIfNull();
        
        _collection.Add(item);

        var hcb = HashCode.CreateBuilder();
        hcb.AddHashCode(_hashCode);
        hcb.AddObject(item);

        _hashCode = hcb.GetHashCode();

        CollectionChanged.Publish(new { State = CollectionChangedState.ElementAdded, Element = item });
    }

    public void Clear()
    {
        _collection.Clear();
        _hashCode = 0;
        CollectionChanged.Publish(new { State = CollectionChangedState.CollectionCleared });
    }

    public Event<Action<CollectionEvent<T>>> CollectionChanged { get; private set; }

    public bool Contains(T item) => _collection.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

    public int Count => _collection.Count;

    public bool IsReadOnly => _collection.IsReadOnly;

    public override bool Equals(object? obj) => obj is EquatableCollection<T> other && Equals(other);

    public bool Equals(EquatableCollection<T>? other)
    {
        if (other is null) return false;
        if (_hashCode != other._hashCode) return false;

        return _collection.IsEqualTo(other._collection);
    }

    IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();

    public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(_collection), _collection);
    }

    public bool Remove(T item)
    {
        item.ThrowIfNull();

        var removed = _collection.Remove(item);

        if (removed)
        {
            _hashCode = HashCode.FromObjects(_collection);

            CollectionChanged.Publish(new { State = CollectionChangedState.ElementRemoved, Element = item });
        }
        return removed;
    }
}
