using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Collections.Generic
{
    public static class MultiMapExtensions
    {
        public static KeyValuePair<TKey, TValue>[] ToArray<TKey, TValue>(this IReadOnlyMultiMap<TKey, TValue> map)
            where TKey : notnull
        {
            var array = new KeyValuePair<TKey, TValue>[map.ValuesCount];

            var i = 0;
            foreach(var kv in map)
            {
                array[i] = kv;
                i++;
            }

            return array;
        }
    }
}
