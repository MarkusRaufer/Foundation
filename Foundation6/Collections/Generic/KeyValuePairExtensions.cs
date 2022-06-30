using System.Runtime.CompilerServices;

namespace Foundation.Collections.Generic
{
    public static class KeyValuePairExtensions
    {
        /// <summary>
        /// Returns Some(<typeparamref name="TValue"/>) if key found otherwise None.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="keyValues"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Opt<TValue> Nth<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValues, TKey key)
            where TKey : notnull
            => keyValues.FirstAsOpt(kv => kv.Key.Equals(key))
                        .Match(kv => Opt.Some(kv.Value),
                               () => Opt.None<TValue>());


        public static KeyValuePair<TKey, TValue> ThrowIfEmpty<TKey, TValue>(
            this KeyValuePair<TKey, TValue> pair,
            [CallerArgumentExpression("pair")] string name = "")
        {
            return null != pair.Key ? pair : throw new ArgumentNullException(name);
        }

        public static IEnumerable<KeyValue<TKey, TValue>> ToKeyValues<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
            where TKey : notnull
        {
            return keyValues.ThrowIfNull()
                            .Select(kvp => new KeyValue<TKey, TValue>(kvp.Key, kvp.Value));
        }
    }
}
