// The MIT License (MIT)
//
// Copyright (c) 2020 Markus Raufer
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
﻿using Foundation.Collections.Generic;
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
