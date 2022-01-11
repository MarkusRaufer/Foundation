namespace Foundation.Collections.ComponentModel;

using Foundation.Collections.Generic;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

public class EquatableCollection<T>
    : ICollection<T>
    , IEquatable<EquatableCollection<T>>
{
    private readonly ICollection<T> _collection;
    private int _hashCode;

    public EquatableCollection(bool allowDuplicates = true) : this(new List<T>())
    {
    }

    public EquatableCollection([DisallowNull] ICollection<T> collection, bool allowDuplicates = true)
    {
        _collection = collection.ThrowIfNull(nameof(collection));
        AllowDuplicates = allowDuplicates;

        _hashCode = HashCode.FromObjects(_collection);

        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public void Add(T item)
    {
        item.ThrowIfNull(nameof(item));
        if(!AllowDuplicates && _collection.Contains(item)) throw new ArgumentException($"{item} exists");

        var hcb = HashCode.CreateBuilder();
        hcb.AddHashCode(_hashCode);
        hcb.AddObject(item);

        _hashCode = hcb.GetHashCode();

        CollectionChanged.Publish(new { State = CollectionChangedState.ElementAdded, Element = item });
    }

    public bool AllowDuplicates { get; }

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
        return null != other && _collection.IsEqualTo(other._collection);
    }

    IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();

    public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public bool Remove(T item)
    {
        item.ThrowIfNull(nameof(item));

        var removed = _collection.Remove(item);

        if (removed) _hashCode = HashCode.FromObjects(_collection);

        CollectionChanged.Publish(new { State = CollectionChangedState.ElementRemoved, Element = item });
        return removed;
    }
}
