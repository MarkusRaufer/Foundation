using Foundation.Collections.Generic;
using System.Collections;

namespace Foundation.Collections
{
    public static class EnumerableTransformations
    {
        /// <summary>
        /// Convert to typed enumerable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable<T>(this IEnumerable items) => items.CastTo<T>();

        /// <summary>
        /// Like <see cref="=Select<typeparamref name="T"/>"/>
        /// </summary>
        /// <param name="items"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable SelectObject(this IEnumerable items, Func<object, object> selector)
        {
            items.ThrowIfNull();
            selector.ThrowIfNull();

            foreach (var item in items)
                yield return selector(item);
        }

        /// <summary>
        /// Like <see cref="=Select<typeparamref name="T"/>"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static IEnumerable<T> SelectObject<T>(this IEnumerable items, Func<object, T> selector)
        {
            items.ThrowIfNull();
            selector.ThrowIfNull();

            foreach (var item in items)
                yield return selector(item);
        }

        public static IList<T> ToList<T>(this IEnumerable items)
        {
            var list = new List<T>();
            foreach (var item in items)
                list.Add((T)item);

            return list;
        }

        public static object?[] ToObjectArray(this IEnumerable items)
        {
            var list = new ArrayList();
            items.ForEachObject(i => list.Add(i));
            return list.ToArray();
        }

        public static IList ToObjectList(this IEnumerable items)
        {
            var list = new ArrayList();
            items.ForEachObject(i => list.Add(i));
            return list;
        }

        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> items)
        {
            return ReadOnlyCollection.New(items);
        }
    }
}
