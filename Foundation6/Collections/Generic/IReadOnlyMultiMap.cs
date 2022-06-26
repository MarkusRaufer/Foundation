namespace Foundation.Collections.Generic;

using System.Diagnostics.CodeAnalysis;

public interface IReadOnlyMultiMap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    where TKey : notnull
{
    bool ContainsValue(TValue value);

    /// <summary>
    /// Returns key values as flat list. If keys is empty it returns all key values.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    //IEnumerable<KeyValuePair<TKey, TValue>> GetFlattenedKeyValues(params TKey[] keys);

    /// <summary>
    /// Flattens all values of all keys.
    /// </summary>
    /// <returns></returns>
    IEnumerable<TValue> GetFlattenedValues(params TKey[] keys);

    /// <summary>
    /// Returns the keys containing the value. If values is empty all keys are returned.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    IEnumerable<TKey> GetKeys(params TValue[] values);

    /// <summary>
    /// Gets all keys with their values.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> GetKeyValues(params TKey[] keys);
    IEnumerable<TValue> GetValues(params TKey[] keys);
    int GetValuesCount(TKey key);

    /// <summary>
    /// Counts all key values.
    /// </summary>
    int KeyValueCount { get; }
    bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue? value);
    bool TryGetValues(TKey key, out IEnumerable<TValue> values);

    /// <summary>
    /// Returns the number of all values.
    /// </summary>
    int ValuesCount { get; }
}
