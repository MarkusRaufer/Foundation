namespace Foundation.Collections.Generic
{
    public interface IMultiKeyMap<TKey, TValue> : IDictionary<TKey, TValue>
    {
        IEnumerable<TKey> GetKeys(IEnumerable<TValue> values);

        IEnumerable<TValue> GetValues(IEnumerable<TKey> keys);
    }
}
