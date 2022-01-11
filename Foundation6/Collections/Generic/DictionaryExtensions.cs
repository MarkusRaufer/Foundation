namespace Foundation.Collections.Generic
{
    public static  class DictionaryExtensions
    {
        public static bool IsEqualTo<TKey, TValue>(this IDictionary<TKey, TValue> lhs, IEnumerable<KeyValuePair<TKey, TValue>> rhs)
                        where TKey : notnull

        {
            var rhsCount = 0;
            foreach (var r in rhs)
            {
                if (!lhs.TryGetValue(r.Key, out TValue? lhsValue) || !lhsValue.EqualsNullable(r.Value)) return false;
                rhsCount++;
            }

            return lhs.Count == rhsCount;
        }

        public static bool IsEqualTo<TKey, TValue>(this IDictionary<TKey, TValue> lhs, IDictionary<TKey, TValue> rhs)
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
