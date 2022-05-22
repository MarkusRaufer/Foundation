using System.Runtime.CompilerServices;

namespace Foundation.Collections.Generic
{
    public static class KeyValuePairExtensions
    {
        public static KeyValuePair<TKey, TValue> ThrowIfEmpty<TKey, TValue>(
            this KeyValuePair<TKey, TValue> pair,
            [CallerArgumentExpression("pair")] string name = "")
        {
            return null != pair.Key ? pair : throw new ArgumentNullException(name);
        }

        public static IEnumerable<KeyValue<TKey, TValue>> ToKeyValues<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
            where TKey : notnull
        {
            return keyValues.Select(kvp => new KeyValue<TKey, TValue>(kvp.Key, kvp.Value));
        }
    }
}
