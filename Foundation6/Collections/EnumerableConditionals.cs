using Foundation.Collections.Generic;
using System.Collections;

namespace Foundation.Collections;

public static class EnumerableConditionals
{
    public static bool ExistsType(this IEnumerable items, bool includeAssignableTypes, params Type[] types)
    {
        items = items.ThrowIfNull();
        types.ThrowIfOutOfRange(() => types.Length == 0);

        var search = types.ToList();

        Func<Type, Type, Type?> checkType = includeAssignableTypes ? ofType : ofExactType;

        foreach (var item in items)
        {
            if (null == item) continue;

            var type = item.GetType();
            var checkedtype = search.Select(x => checkType(x, type)).FirstOrDefault(x => null != x);

            if (null != checkedtype)
            {
                search.Remove(checkedtype);

                if (0 == search.Count) return true;
            }
        }

        static Type? ofExactType(Type lhs, Type rhs)
        {
            return lhs.Equals(rhs) ? lhs : null;
        }

        static Type? ofType(Type lhs, Type rhs)
        {
            var exactType = ofExactType(lhs, rhs);
            if(null != exactType) return exactType;

            return lhs.IsAssignableFrom(rhs) ? lhs : null;
        }

        return false;
    }
}
