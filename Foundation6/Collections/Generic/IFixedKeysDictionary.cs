using System.Collections.Specialized;

namespace Foundation.Collections.Generic
{
    public interface IFixedKeysDictionary<TKey, TValue> 
        : IReadOnlyDictionary<TKey, TValue>
        , INotifyCollectionChanged
        where TKey : notnull
    {
        new TValue this[TKey key] { get; set; }
    }
}