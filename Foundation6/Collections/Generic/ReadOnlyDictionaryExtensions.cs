namespace Foundation.Collections.Generic
{
    public static class ReadOnlyDictionaryExtensions
    {
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
    }
}
