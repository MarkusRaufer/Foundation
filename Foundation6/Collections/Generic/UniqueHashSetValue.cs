namespace Foundation.Collections.Generic;

using System.Collections.Generic;
using System.Runtime.Serialization;

public static class UniqueHashSetValue
{
    public static UniqueHashSetValue<T> New<T>(params T[] values)
    {
        return new UniqueHashSetValue<T>(values);
    }
}

/// <summary>
/// This hashset considers the equality of all elements <see cref="Equals"/>. The position of the elements are ignored.
/// Throws exception on duplicates.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class UniqueHashSetValue<T> : HashSetValue<T>
{
#pragma warning disable CS0414
    private readonly Func<EnumerableCounter<T>>? _counter;
#pragma warning restore CS0414 

    public UniqueHashSetValue() : base()
    {
        _counter = null;
    }

    public UniqueHashSetValue(IEnumerable<T> collection) 
        : base(new EnumerableCounter<T>(collection, out Func<EnumerableCounter<T>> _counter))
    {
        _counter.ThrowIfNull();

        var counter = _counter();
        if (counter.Count != Count) throw new ArgumentException(" values are not unique", nameof(collection));
    }

    public UniqueHashSetValue(IEqualityComparer<T>? comparer) : base(comparer)
    {
    }

    public UniqueHashSetValue(int capacity) : base(capacity)
    {
    }

    public UniqueHashSetValue(IEnumerable<T> collection, IEqualityComparer<T>? comparer)
        : base(new EnumerableCounter<T>(collection, out Func<EnumerableCounter<T>> _counter), comparer)
    {
        _counter.ThrowIfNull();

        var counter = _counter();
        if (counter.Count != Count) throw new ArgumentException(" values are not unique", nameof(collection));
     }

    public UniqueHashSetValue(int capacity, IEqualityComparer<T>? comparer)
        : base(capacity, comparer)
    {
    }

    protected UniqueHashSetValue(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }

    public new bool Add(T item)
    {
        if (!base.Add(item)) throw new ArgumentException($"{item} exists");

        return true;
    }
}
