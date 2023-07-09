namespace Foundation.Collections.Generic;

/// <summary>
/// Dictionary that supports multiple values per key.
/// It is used for 1 to many relations.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class SortedMultiValueMap<TKey, TValue> : MultiValueMap<TKey, TValue>
    where TKey : notnull
{
    public SortedMultiValueMap() : base(new SortedDictionary<TKey, ICollection<TValue>>(), () => new SortedList<TValue>())
    {
    }

    public SortedMultiValueMap(IComparer<TKey>? comparer)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(comparer), () => new SortedList<TValue>())
    {
    }

    public SortedMultiValueMap(IDictionary<TKey, ICollection<TValue>> dictionary)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(dictionary), () => new SortedList<TValue>())
    {
    }

    public SortedMultiValueMap(SortedDictionary<TKey, ICollection<TValue>> dictionary)
        : base(dictionary, () => new SortedList<TValue>())
    {
    }

    public SortedMultiValueMap(Func<ICollection<TValue>> valueCollectionFactory)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(), valueCollectionFactory)
    {
    }

    public SortedMultiValueMap(IComparer<TKey>? comparer, Func<ICollection<TValue>> valueCollectionFactory)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(comparer), valueCollectionFactory)
    {
    }

    public SortedMultiValueMap(
        IDictionary<TKey, ICollection<TValue>> dictionary,
        Func<ICollection<TValue>> valueCollectionFactory)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(dictionary), valueCollectionFactory)
    {
    }

    public SortedMultiValueMap(
        IDictionary<TKey, ICollection<TValue>> dictionary,
        IComparer<TKey>? comparer,
        Func<ICollection<TValue>> valueCollectionFactory)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(dictionary, comparer), valueCollectionFactory)
    {
    }

    public SortedMultiValueMap(
        SortedDictionary<TKey, ICollection<TValue>> dictionary,
        Func<ICollection<TValue>> valueCollectionFactory)
        : base(dictionary, valueCollectionFactory)
    {
    }
}

