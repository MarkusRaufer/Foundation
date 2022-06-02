namespace Foundation.ComponentModel;

using Foundation.Collections.Generic;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

/// <summary>
/// This Equals of this collection checks the equality of all elements.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class EquatableCollection<T>
    : ICollection<T>
    , ICollectionChanged<T>
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
        CreateHashCode();

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
        }
        else
        {
            _hashCode = 0;
            _collection = new List<T>();
        }

        CreateHashCode();

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
        CreateHashCode();

        CollectionChanged.Publish(new { State = CollectionChangedState.CollectionCleared });
    }

    public Event<Action<CollectionEvent<T>>> CollectionChanged { get; private set; }

    public bool Contains(T item) => _collection.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

    public int Count => _collection.Count;

    protected void CreateHashCode()
    {
        var builder = HashCode.CreateBuilder();
        builder.AddObject(typeof(EquatableCollection<T>));
        builder.AddObjects(_collection);
    }
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
            CreateHashCode();

            CollectionChanged.Publish(new { State = CollectionChangedState.ElementRemoved, Element = item });
        }
        return removed;
    }
}
