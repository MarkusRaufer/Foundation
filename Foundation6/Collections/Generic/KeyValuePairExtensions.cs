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
using System.Runtime.CompilerServices;

namespace Foundation.Collections.Generic
{
    public static class KeyValuePairExtensions
    {
#if NETSTANDARD2_0
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> keyValue, out TKey key, out TValue value)
        {
            key = keyValue.Key;
            value = keyValue.Value;
        }
#endif

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
            [CallerArgumentExpression(nameof(pair))] string name = "")
        {
            return pair.ThrowIfKeyIsNull(name).ThrowIfValueIsNull(name);
        }

        public static KeyValuePair<TKey, TValue> ThrowIfKeyIsNull<TKey, TValue>(
            this KeyValuePair<TKey, TValue> pair,
            [CallerArgumentExpression(nameof(pair))] string name = "")
        {
            return null != pair.Key ? pair : throw new ArgumentNullException(name);
        }

        public static KeyValuePair<string, TValue> ThrowIfKeyIsNullOrWhiteSpace<TValue>(
            this KeyValuePair<string, TValue> pair,
            [CallerArgumentExpression(nameof(pair))] string name = "")
        {
            if (string.IsNullOrWhiteSpace(pair.Key)) throw new ArgumentNullException(name);

            return pair;
        }

        public static KeyValuePair<string, TValue> ThrowIfKeyIsNullOrWhiteSpaceOrValueIsNull<TValue>(
            this KeyValuePair<string, TValue> pair,
            [CallerArgumentExpression(nameof(pair))] string name = "")
        {
            return pair.ThrowIfKeyIsNullOrWhiteSpace(name).ThrowIfValueIsNull(name);
        }

        public static KeyValuePair<string, string> ThrowIfKeyOrValueIsNullOrWhiteSpace(
            this KeyValuePair<string, string> pair,
            [CallerArgumentExpression(nameof(pair))] string name = "")
        {
            return pair.ThrowIfKeyIsNullOrWhiteSpace(name).ThrowIfValueIsNullOrWhiteSpace(name);
        }

        public static KeyValuePair<TKey, TValue> ThrowIfValueIsNull<TKey, TValue>(
            this KeyValuePair<TKey, TValue> pair,
            [CallerArgumentExpression(nameof(pair))] string name = "")
        {
            return null != pair.Value ? pair : throw new ArgumentNullException(name);
        }

        public static KeyValuePair<TKey, string> ThrowIfValueIsNullOrWhiteSpace<TKey>(
            this KeyValuePair<TKey, string> pair,
            [CallerArgumentExpression(nameof(pair))] string name = "")
        {
            return !string.IsNullOrWhiteSpace(pair.Value) ? pair : throw new ArgumentNullException(name);
        }
    }
}
