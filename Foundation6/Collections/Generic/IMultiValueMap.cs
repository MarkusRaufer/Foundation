namespace Foundation.Collections.Generic;

public interface IMultiValueMap<TKey, TValue>
    : IDictionary<TKey, TValue>
    , IReadOnlyMultiValueMap<TKey, TValue>
    where TKey : notnull
{
    /// <summary>
    /// Adds a single value to the key. If a value exists for this key, it will be removed before.
    /// </summary>
    /// <param name="item"></param>
    void AddSingle(KeyValuePair<TKey, TValue> item);

    /// <summary>
    /// Adds a single value to the key. If a value exists for this key, it will be removed before.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void AddSingle(TKey key, TValue value);

    /// <summary>
    /// Adds a unique value for the key.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value">If the value exists for this key it will not be added.</param>
    /// <param name="replaceExisting">If true an existing value is replaced by value, if false no value is added.</param>
    bool AddUnique(TKey key, TValue value, bool replaceExisting = false);

    bool Remove(TKey key, TValue value);

    /// <summary>
    /// Removes a value from all keys.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    bool RemoveValue(TValue value);

    /// <summary>
    /// Remove value from keys.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="keys"></param>
    /// <returns></returns>
    bool RemoveValue(TValue value, params TKey[] keys);
}
