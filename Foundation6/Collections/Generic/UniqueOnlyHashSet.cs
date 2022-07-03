namespace Foundation.Collections.Generic;

using System.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
/// This hashset considers the equality of all elements <see cref="Equals"/>. The position of the elements are ignored.
/// Throws exception on duplicates.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class UniqueOnlyHashSet<T> : EquatableHashSet<T>
{
    private readonly Func<EnumerableCounter<T>>? _counter;

    public UniqueOnlyHashSet() : base()
    {
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
