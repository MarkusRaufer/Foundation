using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Collections.Generic
{
    public static  class OrdinalEnumerableExtensions
    {
        /// <summary>
        /// Returns a list of elements at their dedicated ordinal position. Positions which do not exist in the ordinals are replaced with None.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ordinals"></param>
        /// <returns></returns>
        public static IEnumerable<Opt<T>> OrdinalFill<T>(this IEnumerable<Ordinal<T>> ordinals)
        {
            var i = 0;
            foreach (var ordinal in ordinals.OrderBy(o => o.Position))
            {
                if (i < ordinal.Position)
                {
                    foreach (var pos in EnumerableEx.Range(i, ordinal.Position - 1))
                    {
                        yield return Opt.None<T>();
                        ++i;
                    }
                }

                if (i == ordinal.Position) yield return Opt.Some(ordinal.Value);

                ++i;
            }
        }
    }
}
