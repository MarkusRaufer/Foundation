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
﻿using System.Runtime.CompilerServices;

namespace Foundation.Collections.Generic;

public static  class DictionaryExtensions
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
    this IDictionary<TKey, TValue> lhs,
    IEnumerable<KeyValuePair<TKey, TValue>> rhs,
    Func<TKey, TValue, TValue, TResult> selector)
    {
        foreach (var right in rhs)
        {
            if (!lhs.TryGetValue(right.Key, out TValue? lhsValue)) continue;

            yield return selector(right.Key, lhsValue, right.Value);
        }
    }

    /// <summary>
    /// Returns true if all keys with their values of lhs and rhs are equal.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool IsEqualToSet<TKey, TValue>(
        this IDictionary<TKey, TValue> lhs,
        IEnumerable<KeyValuePair<TKey, TValue>> rhs)
        where TKey : notnull
    {
        if (null == lhs) return null == rhs;
        if (null == rhs) return false;

        var rhsCount = 0;
        foreach (var r in rhs)
        {
            if (!lhs.TryGetValue(r.Key, out TValue? lhsValue)) return false;
            if (!lhsValue.EqualsNullable(r.Value)) return false;

            rhsCount++;

            if (lhs.Count < rhsCount) return false;
        }

        return lhs.Count == rhsCount;
    }

    /// <summary>
    /// Returns true if all keys with their values of lhs and rhs are equal.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool IsEqualToSet<TKey, TValue>(this IDictionary<TKey, TValue> lhs, IDictionary<TKey, TValue> rhs)
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

    public static Option<KeyValuePair<TKey, TValue>> RemovKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        where TKey : notnull
    {
        dictionary.ThrowIfNull();

        if (!dictionary.TryGetValue(key, out TValue? value)) return Option.None<KeyValuePair<TKey, TValue>>();
        
        dictionary.Remove(key);
        return Option.Some(Pair.New(key, value));
    }

    /// <summary>
    /// Replaces the values of lhs with the values of rhs if the key exists in lhs. 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static IDictionary<TKey, TValue> Replace<TKey, TValue>(
        this IDictionary<TKey, TValue> lhs,
        IEnumerable<KeyValuePair<TKey, TValue>> rhs)
    {
        foreach(var right in rhs)
        {
            if (!lhs.ContainsKey(right.Key)) continue;

            lhs[right.Key] = right.Value;
        }

        return lhs;
    }


    public static IDictionary<TKey, TValue> ThrowIfEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, [CallerArgumentExpression("dictionary")] string paramName = "")
        => 0 < dictionary.Count
        ? dictionary
        : throw new ArgumentOutOfRangeException($"{paramName} must not be empty");

    public static IDictionary<TKey, TValue> ThrowIfNull<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, [CallerArgumentExpression("dictionary")] string paramName = "")
        => dictionary ?? throw new ArgumentException($"{paramName} must not be empty");
    public static IDictionary<TKey, TValue> ThrowIfNullOrEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, [CallerArgumentExpression("dictionary")] string paramName = "")
        => ThrowIfNull(dictionary, paramName).ThrowIfEmpty(paramName);
}
