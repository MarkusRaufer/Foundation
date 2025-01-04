namespace Foundation.Collections;

public interface IHashMap<TKey, TValue>
    : IDictionary<TKey, TValue>
    , IReadOnlyHashMap<TKey, TValue>
    where TKey : notnull
{
}
