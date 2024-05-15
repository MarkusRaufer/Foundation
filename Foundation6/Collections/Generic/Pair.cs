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
﻿namespace Foundation.Collections.Generic;

public static class Pair
{
    public static IEnumerable<KeyValuePair<TKey, TValue>> CreateMany<TKey, TValue>(params (TKey, TValue)[] keyValues)
    {
        foreach(var (key, value) in keyValues)
            yield return new KeyValuePair<TKey, TValue>(key, value);
    }

    public static KeyValuePair<TKey, TValue> Empty<TKey, TValue>()
    {
        return new KeyValuePair<TKey, TValue>();
    }

    public static bool IsEmtpy<TKey, TValue>(this KeyValuePair<TKey, TValue> pair)
    {
        return pair.Equals(Empty<TKey, TValue>());
    }

    public static bool IsInitialized<TKey, TValue>(this KeyValuePair<TKey, TValue> pair)
    {
        var empty = Empty<TKey, TValue>();
        return !(pair.Key.EqualsNullable(empty.Key) && pair.Value.EqualsNullable(empty.Value));
    }

    public static KeyValuePair<TKey, TValue> New<TKey, TValue>(TKey key, TValue value)
    {
        return new KeyValuePair<TKey, TValue>(key, value);
    }

    public static KeyValuePair<TKey, TValue> ThrowIfNotInitialized<TKey, TValue>(
        this KeyValuePair<TKey, TValue> pair,
        string name)
    {
        name.ThrowIfEnumerableIsNullOrEmpty();

        if (!pair.IsInitialized()) throw new ArgumentException("not initialized", name);
        return pair;
    }
}

