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
﻿namespace Foundation.Collections.Generic
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
        /// <typeparam name="TKey">Type of keys.</typeparam>
        /// <typeparam name="TValue">Type of values.</typeparam>
        /// <param name="dictionary">Dictionary which key values should be replaced.</param>
        /// <param name="replacements"></param>
        /// <param name="addNonExistingReplacements">Add key value from replacements which do not exist in dictionary.</param>
        /// <returns></returns>
        public static IEnumerable<KeyValuePair<TKey, TValue>> Replace<TKey, TValue>(
            this IReadOnlyDictionary<TKey, TValue> dictionary,
            IEnumerable<KeyValuePair<TKey, TValue>> replacements,
            bool addNonExistingReplacements = false)
            where TKey : notnull
        {
            var rhs = replacements.ToDictionary(x => x.Key, x => x.Value);

            foreach (var lhs in dictionary)
            {
                if (rhs.TryGetValue(lhs.Key, out TValue? rhsValue))
                {
                    yield return new KeyValuePair<TKey, TValue>(lhs.Key, rhsValue);
                    rhs.Remove(lhs.Key);

                    continue;
                }

                yield return lhs;
            }

            if (!addNonExistingReplacements) yield break;

            foreach(var r in rhs)
            {
                if (!dictionary.ContainsKey(r.Key)) yield return r;
            }
        }
    }
}
