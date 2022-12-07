﻿namespace Foundation.Collections.Generic;

using Foundation;
using Foundation.ComponentModel;
using System.Collections;
using System.Runtime.Serialization;

/// <summary>
/// This collection considers the equality of all elements <see cref="Equals"/>. The position of the elements are ignored.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class CollectionValue<T>
    : ICollection<T>
    , ICollectionChanged<T>
    , ISerializable
    , IEquatable<CollectionValue<T>>
{
    private readonly ICollection<T> _collection;
    private int _hashCode;

    public CollectionValue() : this(new List<T>())
    {
    }

    public CollectionValue(ICollection<T> collection)
    {
        _collection = collection.ThrowIfNull();

        _hashCode = CreateHashCode();

        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public CollectionValue(int capacity) : this(new List<T>(capacity))
    {
    }

    public CollectionValue(SerializationInfo info, StreamingContext context)
    {
        if (info.GetValue(nameof(_collection), typeof(List<T>)) is List<T> collection)
        {
            _collection = collection;
        }
        else
        {
            _collection = new List<T>();
        }

        _hashCode = CreateHashCode();

        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public void Add(T item)
    {
        item.ThrowIfNull();

        _collection.Add(item);

        _hashCode = CreateHashCode();

        CollectionChanged.Publish(new { State = CollectionChangedState.ElementAdded, Element = item });
    }

    public void Clear()
    {
        _collection.Clear();
        _hashCode = CreateHashCode();

        CollectionChanged.Publish(new { State = CollectionChangedState.CollectionCleared });
    }

    public Event<Action<CollectionEvent<T>>> CollectionChanged { get; private set; }

    public bool Contains(T item) => _collection.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

    public int Count => _collection.Count;

    protected int CreateHashCode()
    {
        return HashCode.CreateBuilder()
                       .AddHashCode(DefaultHashCode)
                       .AddOrderedObjects(_collection)
                       .GetHashCode();
    }

    protected static int DefaultHashCode { get; } = typeof(CollectionValue<T>).GetHashCode();

    /// <summary>
    /// Checks the equality of all elements. Positions of the elements are ignored.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object? obj) => obj is CollectionValue<T> other && Equals(other);

    /// <summary>
    /// Checks the equality of all elements. Positions of the elements are ignored.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(CollectionValue<T>? other)
    {
        if (other is null) return false;
        if (_hashCode != other._hashCode) return false;

        return _collection.IsEqualToSet(other._collection);
    }

    IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();

    public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

    /// <summary>
    /// Considers values only. Positions of the elements are ignored.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _hashCode;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(_collection), _collection);
    }

    public bool IsReadOnly => _collection.IsReadOnly;

    public bool Remove(T item)
    {
        item.ThrowIfNull();

        var removed = _collection.Remove(item);

        if (removed)
        {
            _hashCode = CreateHashCode();

            CollectionChanged.Publish(new { State = CollectionChangedState.ElementRemoved, Element = item });
        }
        return removed;
    }
}