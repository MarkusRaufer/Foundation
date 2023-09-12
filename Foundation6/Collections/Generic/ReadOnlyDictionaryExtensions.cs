namespace Foundation.Collections.Generic
{
    public static class ReadOnlyDictionaryExtensions
    {
        /// <summary>
        /// Intersects the keys of lhs with the keys of rhs.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <param name="selector">Expects the common key followed by the lhs value and rhs value.</param>
        /// <returns></returns>
        public static IEnumerable<TResult> IntersectBy<TKey, TValue, TResult>(
            this IReadOnlyDictionary<TKey, TValue> lhs,
            IEnumerable<KeyValuePair<TKey, TValue>> rhs,
            Func<TKey, TValue, TValue, TResult> selector)
        {
            foreach (var right in rhs)
            {
                if (!lhs.TryGetValue(right.Key, out TValue? lhsValue)) continue;

                yield return selector(right.Key, lhsValue, right.Value);
            }
        }

        public static bool IsEqualTo<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> lhs, IReadOnlyDictionary<TKey, TValue> rhs)
            where TKey : notnull
        {
            if (null == lhs) return null == rhs;
            if (null == rhs) return false;
            if (lhs.Count != rhs.Count) return false;

            foreach (var kvp in lhs)
            {
                if (!rhs.TryGetValue(kvp.Key, out TValue? rhsValue)) return false;
                if (!kvp.Value.EqualsNullable(rhsValue)) return false;
            }
            return true;
        }

        /// <summary>
        /// Returns all key values from dictionary. Existing key values of dictionary are replaced by the values of replacements.
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="replacements"></param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, TValue>> Replace<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> dictionary,
            IEnumerable<KeyValuePair<TKey, TValue>> replacements)
            where TKey : notnull
        {
            var rhs = replacements.ToDictionary(x => x.Key, x => x.Value);

            foreach (var lhs in dictionary)
            {
                if (rhs.TryGetValue(lhs.Key, out TValue? rhsValue))
                {
                    yield return new KeyValuePair<TKey, TValue>(lhs.Key, rhsValue);
                    continue;
                }

                yield return lhs;
            }
        }
    }
}
