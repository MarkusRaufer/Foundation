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
﻿// The MIT License (MIT)
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
using System.Linq.Expressions;

namespace Foundation.ComponentModel;

public static class ExistingDictionaryBuilderExtensions
{
    /// <summary>Creates a builder that enables to change a dictionary and record the changes.
    /// New keys can be added. Existing keys can be updated and removed.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TValue">Type of the values of <paramref name="source"/>.</typeparam>
    /// <param name="source">An instance of a dictionary.</param>
    /// <param name="key">The key selector</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>Returns the same changed dictionary.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static ExistingDictionaryBuilder<TKey, TValue> ChangeWith<TKey, TValue>(
        this IDictionary<TKey, TValue> source,
        TKey key,
        TValue newValue)
        where TKey : notnull
    {
        source.ThrowIfNull();
        key.ThrowIfNull();

        return new ExistingDictionaryBuilder<TKey, TValue>(
                        ExistingDictionaryBuilder.BuildMode.ChangeWith,
                        source,
                        key,
                        newValue);
    }

    /// <summary>Creates a builder that enables to change a dictionary and record the changes.
    /// New keys can be added. Existing keys can be updated and removed.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys of source.</typeparam>
    /// <typeparam name="TValue">Type of the values of source.</typeparam>
    /// <param name="source">An instance of a dictionary.</param>
    /// <param name="keyValues">The KeyValuePair tuples to change <paramref name="source"/>.</param>
    /// <returns>Returns the same changed dictionary.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static ExistingDictionaryBuilder<TKey, TValue> ChangeWith<TKey, TValue>(
        this IDictionary<TKey, TValue> source,
        IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        where TKey : notnull
    {
        source.ThrowIfNull();
        keyValues.ThrowIfNull();

        return new ExistingDictionaryBuilder<TKey, TValue>(
                        ExistingDictionaryBuilder.BuildMode.ChangeWith,
                        source,
                        keyValues);
    }

    /// <summary>Creates a builder that enables to change a dictionary and record the changes.
    /// New keys can be added. Existing keys can be updated and removed.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="source"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static ExistingDictionaryBuilder<TKey, TValue> RemoveWith<TKey, TValue>(
        this IDictionary<TKey, TValue> source,
        TKey key)
        where TKey : notnull
    {
        source.ThrowIfNull();
        key.ThrowIfNull();

        return new ExistingDictionaryBuilder<TKey, TValue>(
                        ExistingDictionaryBuilder.BuildMode.ChangeWith,
                        source,
                        key);
    }

    /// <summary>Creates a builder that enables to change a dictionary and record the changes.
    /// New keys can be added. Existing keys can be updated and removed.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="source"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static ExistingDictionaryBuilder<TKey, TValue> RemoveWith<TKey, TValue>(
        this IDictionary<TKey, TValue> source,
        IEnumerable<TKey> keys)
        where TKey : notnull
    {
        source.ThrowIfNull();
        keys.ThrowIfNull();

        return new ExistingDictionaryBuilder<TKey, TValue>(
                        ExistingDictionaryBuilder.BuildMode.ChangeWith,
                        source,
                        keys);
    }
    /// <summary>
    /// Creates a builder that enables to update a dictionary and record the changes.
    /// New keys cannot be added. Existing keys can be updated but not removed.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TValue">The type of the values of <paramref name="source"/>.</typeparam>
    /// <param name="source">An instance of a dictionary.</param>
    /// <param name="key">The key that should be updated.</param>
    /// <param name="newValue">The new value of the existing key.</param>
    /// <returns></returns>
    public static ExistingDictionaryBuilder<TKey, TValue> UpdateWith<TKey, TValue>(
        this IDictionary<TKey, TValue> source,
        TKey key,
        TValue newValue)
        where TKey : notnull
    {
        source.ThrowIfNull();
        key.ThrowIfNull();

        return new ExistingDictionaryBuilder<TKey, TValue>(
                        ExistingDictionaryBuilder.BuildMode.UpdateWith,
                        source,
                        key,
                        newValue);
    }

    public static ExistingDictionaryBuilder<TKey, TValue> UpdateWith<TKey, TValue>(
        this IDictionary<TKey, TValue> source,
        IEnumerable<KeyValuePair<TKey, TValue>> properties)
        where TKey : notnull
    {
        source.ThrowIfNull();
        properties.ThrowIfNull();

        return new ExistingDictionaryBuilder<TKey, TValue>(
                        ExistingDictionaryBuilder.BuildMode.UpdateWith,
                        source,
                        properties);
    }
}
