namespace Foundation.Collections.Generic;

using Foundation;
using Foundation.ComponentModel;
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
    , ISerializable
{
    private int _hashCode;

    public EquatableHashSet(bool exceptionOnDuplicates = false) : base()
    {
        ExceptionOnDuplicates = exceptionOnDuplicates;

        _hashCode = 0;
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection">EnumerableCounter is needed to detect wether the collection contains duplicates.
    /// Use T[], Collection<typeparamref name="T"/> or List<typeparamref name="T"/></param>
    /// <param name="exceptionOnDuplicates"></param>
    /// <exception cref="ArgumentException"></exception>
    public EquatableHashSet(EnumerableCounter<T> collection, bool exceptionOnDuplicates = false) : base(collection)
    {
        ExceptionOnDuplicates = exceptionOnDuplicates;

        if (ExceptionOnDuplicates && collection.Count != Count)
            throw new ArgumentException($"contains duplicates", nameof(collection));

        CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(IEqualityComparer<T>? comparer, bool exceptionOnDuplicates = false) : base(comparer)
    {
        ExceptionOnDuplicates = exceptionOnDuplicates;

        CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(int capacity, bool exceptionOnDuplicates = false) : base(capacity)
    {
        ExceptionOnDuplicates = exceptionOnDuplicates;

        CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection">EnumerableCounter is needed to detect wether the collection contains duplicates.
    /// Use T[], Collection<typeparamref name="T"/> or List<typeparamref name="T"/></param>
    /// <param name="comparer"></param>
    /// <param name="exceptionOnDuplicates"></param>
    /// <exception cref="ArgumentException"></exception>
    public EquatableHashSet(
        EnumerableCounter<T> collection, 
        IEqualityComparer<T>? comparer,
        bool exceptionOnDuplicates = false) : base(collection, comparer)
    {
        ExceptionOnDuplicates = exceptionOnDuplicates;

        if (ExceptionOnDuplicates && collection.Count != Count)
            throw new ArgumentException($"contains duplicates", nameof(collection));

        CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(int capacity, IEqualityComparer<T>? comparer, bool exceptionOnDuplicates = false)
        : base(capacity, comparer)
    {
        ExceptionOnDuplicates = exceptionOnDuplicates;

        CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    public EquatableHashSet(SerializationInfo info, StreamingContext context) : base(info, context)
    {
        if (info.GetValue(nameof(ExceptionOnDuplicates), typeof(bool)) is bool exceptionOnDuplicates)
        {
            ExceptionOnDuplicates = exceptionOnDuplicates;
        }
        
        CreateHashCode();
        CollectionChanged = new Event<Action<CollectionEvent<T>>>();
    }

    /// <summary>
    /// Adds an item to the hashset.
    /// </summary>
    /// <param name="item"></param>
    /// <exception cref="ArgumentException">If ExceptionOnDuplicates is true an exception is thrown if the item exists.</exception>
    public new void Add(T item)
    {
        var added = base.Add(item.ThrowIfNull());
        if (added)
        {
            CreateHashCode();

            CollectionChanged.Publish(new { State = CollectionChangedState.ElementAdded, Element = item });
            return;
        }
        if (ExceptionOnDuplicates) throw new ArgumentException($"{nameof(item)} exists", nameof(item));
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
        if (_hashCode != other._hashCode) return false;

        return this.IsEqualTo(other, ignoreDuplicates: true);
    }

    /// <summary>
    /// If true an exception is thrown, if same value is added twice.
    /// </summary>
    protected bool ExceptionOnDuplicates { get; }

    /// <summary>
    /// Considers values only. Position of the values are ignored.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => _hashCode;

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(ExceptionOnDuplicates), ExceptionOnDuplicates);
        base.GetObjectData(info, context);
    }

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
