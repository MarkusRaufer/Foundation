namespace Foundation.Collections.Generic;

using System.Collections.Generic;
using System.Runtime.Serialization;

public static class UniqueOnlyHashSet
{
    public static UniqueOnlyHashSet<T> New<T>(params T[] values)
    {
        return new UniqueOnlyHashSet<T>(values);
    }
}

/// <summary>
/// This hashset considers the equality of all elements <see cref="Equals"/>. The position of the elements are ignored.
/// Throws exception on duplicates.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class UniqueOnlyHashSet<T> : EquatableHashSet<T>
{
#pragma warning disable CS0414
    private readonly Func<EnumerableCounter<T>>? _counter;
#pragma warning restore CS0414 

    public UniqueOnlyHashSet() : base()
    {
        _counter = null;
    }

    public UniqueOnlyHashSet(IEnumerable<T> collection) 
        : base(new EnumerableCounter<T>(collection, out Func<EnumerableCounter<T>> _counter))
    {
        _counter.ThrowIfNull();

        var counter = _counter();
        if (counter.Count != Count) throw new ArgumentException(" values are not unique", nameof(collection));
    }

    public UniqueOnlyHashSet(IEqualityComparer<T>? comparer) : base(comparer)
    {
    }

    public UniqueOnlyHashSet(int capacity) : base(capacity)
    {
    }

    public UniqueOnlyHashSet(IEnumerable<T> collection, IEqualityComparer<T>? comparer)
        : base(new EnumerableCounter<T>(collection, out Func<EnumerableCounter<T>> _counter), comparer)
    {
        _counter.ThrowIfNull();

        var counter = _counter();
        if (counter.Count != Count) throw new ArgumentException(" values are not unique", nameof(collection));
     }

    public UniqueOnlyHashSet(int capacity, IEqualityComparer<T>? comparer)
        : base(capacity, comparer)
    {
    }

    protected UniqueOnlyHashSet(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }

    public new bool Add(T item)
    {
        if (!base.Add(item)) throw new ArgumentException($"{item} exists");

        return true;
    }
}
