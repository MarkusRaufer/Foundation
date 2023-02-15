namespace Foundation.Collections.Generic;

using System.Diagnostics.CodeAnalysis;

public interface IReadOnlyMultiValueMap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    where TKey : notnull
{
    /// <summary>
    /// Checks if key value pair exists.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    bool Contains(TKey key, TValue value);

    /// <summary>
    /// Checks if value exists in a value list.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    bool ContainsValue(TValue value);

    /// <summary>
    /// Returns key values as flat list of the specified keys. If keys is empty it returns all key values.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    IEnumerable<KeyValuePair<TKey, TValue>> GetFlattenedKeyValues(IEnumerable<TKey>? keys = default);

    /// <summary>
    /// Returns values as flat list of the specified keys.
    /// </summary>
    /// <returns></returns>
    IEnumerable<TValue> GetFlattenedValues(IEnumerable<TKey>? keys = default);

    /// <summary>
    /// Returns the keys containing the value. If values is empty all keys are returned.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    IEnumerable<TKey> GetKeys(IEnumerable<TValue> values);

    /// <summary>
    /// Gets all keys with their values.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    IEnumerable<KeyValuePair<TKey, IEnumerable<TValue>>> GetKeyValues(IEnumerable<TKey>? keys = default);

    /// <summary>
    /// Returns the values of a specified key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IEnumerable<TValue> GetValues(TKey key);

    /// <summary>
    /// Returns the values of the specified keys.
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    IEnumerable<TValue> GetValues(IEnumerable<TKey> keys);

    /// <summary>
    /// Returns the number of values of the specified key.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    int GetValuesCount(TKey key);

    /// <summary>
    /// Returns the number of keys.
    /// </summary>
    public int KeyCount { get; }

    bool TryGetValues(TKey key, out IEnumerable<TValue> values);
}
