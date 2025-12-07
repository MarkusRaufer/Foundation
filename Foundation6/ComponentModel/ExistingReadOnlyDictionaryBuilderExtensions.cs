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
using System.Collections.ObjectModel;

namespace Foundation.ComponentModel;

public static class ExistingReadOnlyDictionaryBuilderExtensions
{
    private static IReadOnlyDictionary<TKey, TValue> ReadOnlyDictionaryFactory<TKey, TValue>(
        IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        where TKey : notnull
        => new ReadOnlyDictionary<TKey, TValue>(keyValues.ToDictionary(x => x.Key, x => x.Value));

    /// <summary>
    /// Creates a builder that enables the creation of a new dictionary by copiing <paramref name="source"/>
    /// and replacing the new values. Keys can also be removed. The changes are recorded.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TValue">Type of the values of <paramref name="source"/>.</typeparam>
    /// <param name="source">An instance of a dictionary.</param>
    /// <param name="key">The selector of the property.</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>An IExistingObjectBuilder.</returns>    
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static ExistingReadOnlyDictionaryBuilder<TKey, TValue, IReadOnlyDictionary<TKey, TValue>> NewWith<TKey, TValue>(
        this IReadOnlyDictionary<TKey, TValue> source,
        TKey key,
        TValue newValue)
        where TKey : notnull
    {
        source.ThrowIfNull();
        key.ThrowIfNull();

        return NewWith(source, key, newValue, ReadOnlyDictionaryFactory);
    }

    /// <summary>Creates a builder that enables the creation of a new dictionary by copiing <paramref name="source"/>
    /// and replacing the new values. Keys can also be removed. The changes are recorded.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TValue">Type of the values of <paramref name="source"/>.</typeparam>
    /// <param name="source">An instance of dictionary.</param>
    /// <param name="keyValues">The KeyValuePair tuples to create the dictionary.</param>
    /// <param name="newValue">The new value.</param>
    /// <returns>An IExistingObjectBuilder.</returns>    
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static ExistingReadOnlyDictionaryBuilder<TKey, TValue, IReadOnlyDictionary<TKey, TValue>> NewWith<TKey, TValue>(
        this IReadOnlyDictionary<TKey, TValue> source,
        IEnumerable<KeyValuePair<TKey, TValue>> keyValues)
        where TKey : notnull
    {
        source.ThrowIfNull();
        keyValues.ThrowIfNull();

        return NewWith(source, keyValues, ReadOnlyDictionaryFactory);
    }

    /// <summary>Creates a builder that enables the creation of a new dictionary by copiing <paramref name="source"/>
    /// and replacing the new values. Keys can also be removed. The changes are recorded.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="source"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public static ExistingReadOnlyDictionaryBuilder<TKey, TValue, IReadOnlyDictionary<TKey, TValue>> RemoveNewWith<TKey, TValue>(
        this IReadOnlyDictionary<TKey, TValue> source,
        TKey key)
        where TKey : notnull
    {
        source.ThrowIfNull();
        key.ThrowIfNull();

        return new ExistingReadOnlyDictionaryBuilder<TKey, TValue, IReadOnlyDictionary<TKey, TValue>>(
                        source,
                        key,
                        ReadOnlyDictionaryFactory);
    }

    /// <summary>Creates a builder that enables the creation of a new dictionary by copiing <paramref name="source"/>
    /// and replacing the new values. Keys can also be removed. The changes are recorded.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="source"></param>
    /// <param name="keys"></param>
    /// <returns></returns>
    public static ExistingReadOnlyDictionaryBuilder<TKey, TValue, IReadOnlyDictionary<TKey, TValue>> RemoveNewWith<TKey, TValue>(
        this IReadOnlyDictionary<TKey, TValue> source,
        IEnumerable<TKey> keys)
        where TKey : notnull
    {
        source.ThrowIfNull();
        keys.ThrowIfNull();

        return new ExistingReadOnlyDictionaryBuilder<TKey, TValue, IReadOnlyDictionary<TKey, TValue>>(
                        source,
                        keys,
                        ReadOnlyDictionaryFactory);
    }

    /// <summary>
    /// Creates a builder that enables the creation of a new dictionary by copiing <paramref name="source"/>
    /// and replacing the new values. Keys can also be removed. The changes are recorded.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TValue">Type of the values of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TDictionary">The type of the dictionary.</typeparam>
    /// <param name="source">An instance of a dictionary.</param>
    /// <param name="key">The selector of the property.</param>
    /// <param name="newValue">The new value.</param>
    /// <param name="factory">The factory to create a new instance of <typeparamref name="TDictionary"/>.</param>
    /// <returns>An ExistingReadOnlyDictionaryBuilder.</returns>    
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static ExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary> NewWith<TKey, TValue, TDictionary>(
        this TDictionary source,
        TKey key,
        TValue newValue,
        Func<IEnumerable<KeyValuePair<TKey, TValue>>, TDictionary> factory)
        where TKey : notnull
        where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        source.ThrowIfNull();
        key.ThrowIfNull();

        return new ExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary>(
                        source,
                        key,
                        newValue,
                        factory);
    }

    /// <summary>Creates a builder that enables the creation of a new dictionary by copiing <paramref name="source"/>
    /// and replacing the new values. Keys can also be removed. The changes are recorded.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys of <paramref name="source"/>.</typeparam>
    /// <typeparam name="TValue">Type of the values of <paramref name="source"/>.</typeparam>
    /// <param name="source">An instance of dictionary.</param>
    /// <param name="keyValues">The KeyValuePair tuples to create the dictionary.</param>
    /// <param name="factory">The factory to create a new instance of <typeparamref name="TDictionary"/>.</param>
    /// <returns>An IExistingObjectBuilder.</returns>    
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    public static ExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary> NewWith<TKey, TValue, TDictionary>(
        this TDictionary source,
        IEnumerable<KeyValuePair<TKey, TValue>> keyValues,
        Func<IEnumerable<KeyValuePair<TKey, TValue>>, TDictionary> factory)
        where TKey : notnull
        where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        source.ThrowIfNull();
        keyValues.ThrowIfNull();

        return new ExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary>(
                        source,
                        keyValues,
                        factory);
    }

    public static ExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary> NewWith<TKey, TValue, TDictionary>(
        this TDictionary source,
        IEnumerable<KeyValuePair<TKey, EventActionValue<TValue>>> updates,
        Func<IEnumerable<KeyValuePair<TKey, TValue>>, TDictionary> factory)
        where TKey : notnull
        where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        source.ThrowIfNull();
        updates.ThrowIfNull();

        return new ExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary>(
                        source,
                        updates,
                        factory);
    }

    /// <summary>Creates a builder that enables the creation of a new dictionary by copiing <paramref name="source"/>
    /// and replacing the new values. Keys can also be removed. The changes are recorded.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <typeparam name="TDictionary">The type of the dictionary.</typeparam>
    /// <param name="source"></param>
    /// <param name="key"></param>
    /// <param name="factory">The factory to create an instance of <typeparamref name="TDictionary"/>.</param>
    /// <returns></returns>
    public static ExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary> RemoveNewWith<TKey, TValue, TDictionary>(
        this TDictionary source,
        TKey key,
        Func<IEnumerable<KeyValuePair<TKey, TValue>>, TDictionary> factory)
        where TKey : notnull
        where TDictionary : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        source.ThrowIfNull();
        key.ThrowIfNull();

        return new ExistingReadOnlyDictionaryBuilder<TKey, TValue, TDictionary>(
                        source,
                        key,
                        factory);
    }
}
