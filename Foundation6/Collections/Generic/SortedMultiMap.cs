namespace Foundation.Collections.Generic;

/// <summary>
/// Dictionary that supports multiple values per key.
/// It is used for 1 to many relations.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class SortedMultiMap<TKey, TValue> : MultiValueMap<TKey, TValue>
    where TKey : notnull
{
    public SortedMultiMap() : base(new SortedDictionary<TKey, ICollection<TValue>>(), () => new List<TValue>())
    {
    }

    public SortedMultiMap(IComparer<TKey>? comparer)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(comparer), () => new List<TValue>())
    {
    }

    public SortedMultiMap(IDictionary<TKey, ICollection<TValue>> dictionary)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(dictionary), () => new List<TValue>())
    {
    }

    public SortedMultiMap(SortedDictionary<TKey, ICollection<TValue>> dictionary)
        : base(dictionary, () => new List<TValue>())
    {
    }

    public SortedMultiMap(Func<ICollection<TValue>> valueCollectionFactory)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(), valueCollectionFactory)
    {
    }

    public SortedMultiMap(IComparer<TKey>? comparer, Func<ICollection<TValue>> valueCollectionFactory)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(comparer), valueCollectionFactory)
    {
    }

    public SortedMultiMap(
        IDictionary<TKey, ICollection<TValue>> dictionary,
        Func<ICollection<TValue>> valueCollectionFactory)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(dictionary), valueCollectionFactory)
    {
    }

    public SortedMultiMap(
        IDictionary<TKey, ICollection<TValue>> dictionary,
        IComparer<TKey>? comparer,
        Func<ICollection<TValue>> valueCollectionFactory)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(dictionary, comparer), valueCollectionFactory)
    {
    }

    public SortedMultiMap(
        SortedDictionary<TKey, ICollection<TValue>> dictionary,
        Func<ICollection<TValue>> valueCollectionFactory)
        : base(dictionary, valueCollectionFactory)
    {
    }
}

