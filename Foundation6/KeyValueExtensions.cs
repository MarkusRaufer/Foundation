namespace Foundation;

public static class KeyValueExtensions
{
    public static bool IsEmpty<TKey, TValue>(this KeyValue<TKey, TValue> keyValue) where TKey : notnull
    {
        return null == keyValue.Key;
    }

    public static KeyValue<TKey, TValue> ThrowIfEmpty<TKey, TValue>(this KeyValue<TKey, TValue> keyValue, string argumentName)
        where TKey : notnull
    {
        if (keyValue.IsEmpty()) throw new ArgumentNullException(argumentName);

        return keyValue;
    }

    public static KeyValuePair<TKey, TValue> ToKeyValuePair<TKey, TValue>(this KeyValue<TKey, TValue> keyValue)
        where TKey : notnull  => new(keyValue.Key, keyValue.Value);
}
