namespace Foundation.Collections.ComponentModel;

using Foundation.Collections.Generic;
using System.Runtime.Serialization;

[Serializable]
public class EquatableHashSet<T>
    : HashSet<T>
    , ISerializable
    , IEquatable<EquatableHashSet<T>>
{
    private int _hashCode;

    public EquatableHashSet() : base()
    {
        _hashCode = 0;
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(IEnumerable<T> collection) : base(collection)
    {
        _hashCode = HashCode.FromObjects(collection);
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(IEqualityComparer<T>? comparer) :base(comparer)
    {
        _hashCode = 0;
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(int capacity) : base(capacity)
    {
        _hashCode = 0;
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(IEnumerable<T> collection, IEqualityComparer<T>? comparer) : base(collection, comparer)
    {
        _hashCode = HashCode.FromObjects(collection);
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(int capacity, IEqualityComparer<T>? comparer) : base(capacity, comparer)
    {
        _hashCode = 0;
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        _hashCode = HashCode.FromObjects(this);
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public new void Add(T item)
    {
        if (base.Add(item.ThrowIfNull()))
        {
            var hcb = HashCode.CreateBuilder();
            hcb.AddHashCode(_hashCode);
            hcb.AddObject(item);

            _hashCode = hcb.GetHashCode();

            CollectionChanged.Publish(new { State = CollectionChangedState.ElementAdded, Element = item });
        }
    }

    public new void Clear()
    {
        base.Clear();
        _hashCode = 0;
        CollectionChanged.Publish(new { State = CollectionChangedState.CollectionCleared });
    }

    public Event<Action<CollectionEvent<T>>> CollectionChanged { get; private set; }

    public override bool Equals(object? obj) => obj is EquatableHashSet<T> other && Equals(other);

    public bool Equals(EquatableHashSet<T>? other)
    {
        if (null == other) return false;
        if(_hashCode != other._hashCode) return false;

        return this.IsEqualTo(other);
    }

    public override int GetHashCode() => _hashCode;

    public new bool Remove(T item)
    {
        item.ThrowIfNull();

        var removed = base.Remove(item);
        if (removed)
        {
            _hashCode = HashCode.FromObjects(this);
            CollectionChanged.Publish(new { State = CollectionChangedState.ElementRemoved, Element = item });
        }
        return removed;
    }
}
