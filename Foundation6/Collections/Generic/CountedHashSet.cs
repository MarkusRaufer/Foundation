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
#if NET6_0_OR_GREATER
﻿using System.Collections;

namespace Foundation.Collections.Generic;

/// <summary>
/// This HashSet counts all equal items added. If same item is added again the counter is incremented.
/// If an existing item is removed the counter is decremented. If the counter is 0 after removing an item, the item is removed from the HashSet.
/// </summary>
/// <typeparam name="T"></typeparam>
public class CountedHashSet<T> : ICollection<T>
{
    private readonly HashSet<Countable<T>> _countables;

    public CountedHashSet() : this(new HashSet<Countable<T>>()) 
    {
    }

    public CountedHashSet(HashSet<Countable<T>> countables)
    {
        _countables = countables.ThrowIfNull();
    }

    /// <summary>
    /// if item exists the counter of this item is incremented.
    /// </summary>
    /// <param name="item"></param>
    public void Add(T item)
    {
        var newCountable = Countable.New(item);
        if (_countables.TryGetValue(newCountable, out Countable<T>? countable))
        {
            countable.Inc();
            return;
        }

        _countables.Add(newCountable);
    }

    /// <inheritdoc/>
    public int Count => _countables.Count;

    public IEnumerable<Countable<T>> Countables => _countables;

    /// <inheritdoc/>
    public void Clear() => _countables.Clear();

    /// <inheritdoc/>
    public bool Contains(T item) => _countables.Contains(Countable.New(item));

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        var it = GetEnumerator();
        for(int i = 0; i < array.Length; i++)
        {
            if (!it.MoveNext()) break;
            array[i] = it.Current;
        }
    }

    /// <summary>
    /// Returns all items with their counters as tuples.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<(T? item, int count)> GetCountedElements() => _countables.Select(x => (x.Value, x.Count));

    /// <summary>
    /// Returns the number of additions of this item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The number of additions of the items.</returns>
    public int GetCount(T item) => _countables.TryGetValue(Countable.New(item), out Countable<T>? countable) ? countable.Count : 0;

#pragma warning disable CS8619
    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => _countables.Select(x => x.Value).GetEnumerator();
#pragma warning restore CS8619

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _countables.Select(x => x.Value).GetEnumerator();

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <summary>
    /// If item exists the counter of this item is decremented.
    /// If the counter is 0 after removing an item, the item if removed from the HashSet.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Remove(T item)
    {
        var countable = Countable.New(item);
        if (_countables.TryGetValue(countable, out Countable<T>? found))
        {
            found.Dec();
            if (0 == found.Count) _countables.Remove(countable);
            return true;
        }
        return false;
    }
}
#endif
