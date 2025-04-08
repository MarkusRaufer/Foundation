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
namespace Foundation.Collections.Generic;

public interface IMultiMap<TKey, TValue>
    : IMultiMap<TKey, TValue, ICollection<TValue>>
    , IReadOnlyMultiMap<TKey, TValue>
{
}

public interface IMultiMap<TKey, TValue, TValueCollection>
    : IDictionary<TKey, TValue>
    , IReadOnlyMultiMap<TKey, TValue, TValueCollection>
    where TValueCollection : IEnumerable<TValue>
{
    /// <summary>
    /// Indexer gets or sets a value with a specific key.
    /// </summary>
    /// <param name="key">The key of the dictionary.</param>
    /// <returns></returns>
    new TValue this[TKey key] { get; set; }

    /// <summary>
    /// Adds a list of values to a key.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="values"></param>
    void AddRange(TKey key, IEnumerable<TValue> values);

    /// <summary>
    /// Adds a single value to the key. If a value exists for this key, it will be removed before.
    /// </summary>
    /// <param name="item"></param>
    void AddSingle(KeyValuePair<TKey, TValue> item);

    /// <summary>
    /// Adds a single value to the key. If a value exists for this key, it will be removed before.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    void AddSingle(TKey key, TValue value);

    /// <summary>
    /// Adds a unique value for the key.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value">If the value exists for this key it will not be added.</param>
    /// <param name="replaceExisting">If true an existing value is replaced by value, if false no value is added.</param>
    bool AddUnique(TKey key, TValue value, bool replaceExisting = false);

    /// <summary>
    /// Removes a value from a key.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    bool Remove(TKey key, TValue value);

    /// <summary>
    /// Removes a value from all keys.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    bool RemoveValue(TValue value);

    /// <summary>
    /// Remove value from keys.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="keys"></param>
    /// <returns></returns>
    bool RemoveValue(TValue value, IEnumerable<TKey> keys);
}