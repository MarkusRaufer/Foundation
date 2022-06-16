namespace Foundation.ComponentModel;

using Foundation.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
/// This hashset considers the equality of all elements <see cref="Equals"/>. The position of the elements are ignored.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class EquatableHashSet<T>
    : HashSet<T>
    , ICollectionChanged<T>
    , IEquatable<EquatableHashSet<T>>
{
    private const string SerializationKey = "items";

    private int _hashCode;

    public EquatableHashSet() : base()
    {
        _hashCode = 0;
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(IEnumerable<T> collection) : base(collection)
    {
        CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(IEqualityComparer<T>? comparer) :base(comparer)
    {
        CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(int capacity) : base(capacity)
    {
        CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(IEnumerable<T> collection, IEqualityComparer<T>? comparer) : base(collection, comparer)
    {
        CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(int capacity, IEqualityComparer<T>? comparer) : base(capacity, comparer)
    {
        CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public new void Add(T item)
    {
        if (base.Add(item.ThrowIfNull()))
        {
            var hcb = HashCode.CreateBuilder();
            hcb.AddHashCode(_hashCode);
            hcb.AddObjects(this);

            _hashCode = hcb.GetHashCode();

            CollectionChanged.Publish(new { State = CollectionChangedState.ElementAdded, Element = item });
        }
    }

    public new void Clear()
    {
        base.Clear();
        CreateHashCode();

        CollectionChanged.Publish(new { State = CollectionChangedState.CollectionCleared });
    }

    public Event<Action<CollectionEvent<T>>> CollectionChanged { get; private set; }

    protected void CreateHashCode()
    {
        var builder = HashCode.CreateBuilder();

        builder.AddHashCode(DefaultHashCode);
        builder.AddHashCodes(this.Select(x => x.GetNullableHashCode())
                                 .OrderBy(x => x));

        _hashCode = builder.GetHashCode();
    }

    protected static int DefaultHashCode { get; } = typeof(EquatableHashSet<T>).GetHashCode();

    public override bool Equals(object? obj) => obj is EquatableHashSet<T> other && Equals(other);

    /// <summary>
    /// Checks the equality of all elements. The position of the elements are ignored.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(EquatableHashSet<T>? other)
    {
        if (null == other) return false;
        if(_hashCode != other._hashCode) return false;

        return this.IsEqualTo(other);
    }

    /// <summary>
    /// Considers values only. Position of the values are ignored.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _hashCode;

    public new bool Remove(T item)
    {
        item.ThrowIfNull();

        if (base.Remove(item))
        {
            CreateHashCode();
            CollectionChanged.Publish(new { State = CollectionChangedState.ElementRemoved, Element = item });
            return true;
        }
        return false;
    }
}
