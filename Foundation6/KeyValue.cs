namespace Foundation;

public static class KeyValue
{
    public static KeyValue<TKey, TValue> New<TKey, TValue>(TKey key, TValue value)
        where TKey : notnull => new (key, value);
}

/// <summary>
/// This key value exists because KeyValuePair is not serializable.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
/// <typeparam name="TValue">The type of the value.</typeparam>
[Serializable]
public record struct KeyValue<TKey, TValue>(TKey Key, TValue Value) where TKey : notnull;
