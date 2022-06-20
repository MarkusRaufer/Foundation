namespace Foundation.Collections.Generic;

using System.Collections.Generic;
using System.Runtime.Serialization;

/// <summary>
/// This hashset considers the equality of all elements <see cref="Equals"/>. The position of the elements are ignored.
/// Throws exception on duplicates.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public class UniqueEquatableHashSet<T> : EquatableHashSet<T>
{
    public UniqueEquatableHashSet(bool exceptionOnDuplicates = false) : base(exceptionOnDuplicates)
    {
    }

    public UniqueEquatableHashSet(EnumerableCounter<T> collection) : base(collection, true)
    {
    }

    public UniqueEquatableHashSet(IEqualityComparer<T>? comparer) : base(comparer, true)
    {
    }

    public UniqueEquatableHashSet(int capacity) : base(capacity, true)
    {
    }

    public UniqueEquatableHashSet(EnumerableCounter<T> collection, IEqualityComparer<T>? comparer)
        : base(collection, comparer, true)
    {
    }

    public UniqueEquatableHashSet(int capacity, IEqualityComparer<T>? comparer)
        : base(capacity, comparer, true)
    {
    }

    protected UniqueEquatableHashSet(SerializationInfo serializationInfo, StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }
}
