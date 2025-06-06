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

/// <summary>
/// Dictionary that supports multiple values per key.
/// It is used for 1 to many relations.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class SortedMultiMap<TKey, TValue> : MultiMap<TKey, TValue>
    where TKey : notnull
{
    public SortedMultiMap() : base(new SortedDictionary<TKey, ICollection<TValue>>(), () => new SortedList<TValue>())
    {
    }

    public SortedMultiMap(IComparer<TKey>? comparer)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(comparer), () => new SortedList<TValue>())
    {
    }

    public SortedMultiMap(IDictionary<TKey, ICollection<TValue>> dictionary)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(dictionary), () => new SortedList<TValue>())
    {
    }

    public SortedMultiMap(SortedDictionary<TKey, ICollection<TValue>> dictionary)
        : base(dictionary, () => new SortedList<TValue>())
    {
    }

    public SortedMultiMap(Func<ICollection<TValue>> valueCollectionFactory)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(), valueCollectionFactory)
    {
    }

    public SortedMultiMap(IComparer<TKey>? comparer, Func<ICollection<TValue>> valueCollectionFactory)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(comparer), valueCollectionFactory)
    {
    }

    public SortedMultiMap(
        IDictionary<TKey, ICollection<TValue>> dictionary,
        Func<ICollection<TValue>> valueCollectionFactory)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(dictionary), valueCollectionFactory)
    {
    }

    public SortedMultiMap(
        IDictionary<TKey, ICollection<TValue>> dictionary,
        IComparer<TKey>? comparer,
        Func<ICollection<TValue>> valueCollectionFactory)
        : base(new SortedDictionary<TKey, ICollection<TValue>>(dictionary, comparer), valueCollectionFactory)
    {
    }

    public SortedMultiMap(
        SortedDictionary<TKey, ICollection<TValue>> dictionary,
        Func<ICollection<TValue>> valueCollectionFactory)
        : base(dictionary, valueCollectionFactory)
    {
    }
}

