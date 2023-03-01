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
        public static Option<TValue> Nth<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValues, TKey key)
            where TKey : notnull
            => keyValues.FirstAsOption(kv => kv.Key.Equals(key))
                        .Either(kv => Option.Some(kv.Value),
                               () => Option.None<TValue>());

        public static KeyValuePair<TKey, TValue> ThrowIfKeyOrValueIsNull<TKey, TValue>(
            this KeyValuePair<TKey, TValue> pair,
            [CallerArgumentExpression("pair")] string name = "")
        {
            return pair.ThrowIfKeyIsNull(name).ThrowIfValueIsNull(name);
        }

        public static KeyValuePair<TKey, TValue> ThrowIfKeyIsNull<TKey, TValue>(
            this KeyValuePair<TKey, TValue> pair,
            [CallerArgumentExpression("pair")] string name = "")
        {
            return null != pair.Key ? pair : throw new ArgumentNullException(name);
        }

        public static KeyValuePair<string, TValue> ThrowIfKeyIsNullOrWhiteSpace<TValue>(
            this KeyValuePair<string, TValue> pair,
            [CallerArgumentExpression("pair")] string name = "")
        {
            if (string.IsNullOrWhiteSpace(pair.Key)) throw new ArgumentNullException(name);

            return pair;
        }

        public static KeyValuePair<string, TValue> ThrowIfKeyIsNullOrWhiteSpaceOrValueIsNull<TValue>(
            this KeyValuePair<string, TValue> pair,
            [CallerArgumentExpression("pair")] string name = "")
        {
            return pair.ThrowIfKeyIsNullOrWhiteSpace(name).ThrowIfValueIsNull(name);
        }

        public static KeyValuePair<string, string> ThrowIfKeyOrValueIsNullOrWhiteSpace(
            this KeyValuePair<string, string> pair,
            [CallerArgumentExpression("pair")] string name = "")
        {
            return pair.ThrowIfKeyIsNullOrWhiteSpace(name).ThrowIfValueIsNullOrWhiteSpace(name);
        }

        public static KeyValuePair<TKey, TValue> ThrowIfValueIsNull<TKey, TValue>(
            this KeyValuePair<TKey, TValue> pair,
            [CallerArgumentExpression("pair")] string name = "")
        {
            return null != pair.Value ? pair : throw new ArgumentNullException(name);
        }

        public static KeyValuePair<TKey, string> ThrowIfValueIsNullOrWhiteSpace<TKey>(
            this KeyValuePair<TKey, string> pair,
            [CallerArgumentExpression("pair")] string name = "")
        {
            return !string.IsNullOrWhiteSpace(pair.Value) ? pair : throw new ArgumentNullException(name);
        }

        public static IEnumerable<KeyValue<TKey, TValue>> ToKeyValues<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
            where TKey : notnull
        {
            return keyValues.ThrowIfNull()
                            .Select(kvp => new KeyValue<TKey, TValue>(kvp.Key, kvp.Value));
        }
    }
}
