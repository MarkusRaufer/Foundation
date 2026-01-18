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
﻿using Foundation.Linq.Expressions;
using System.Collections;
using System.Linq.Expressions;

namespace Foundation.Collections.Generic;

/// <summary>
/// This is a sorted set that can be filtered by a lambda expression.
/// This set supports only unique values like the <see cref="SortedSet{TKey, TValue}" of Microsoft./>
/// </summary>
/// <typeparam name="T">TOk should implement IComparable<typeparamref name="T"/> or use your own IComparer<typeparamref name="T"/></typeparam>
public class SortedSetX<T>
    : ICollection<T>
    , IReadOnlyList<T>
    , ILambdaSearch<T, List<T>>
{
    private readonly IComparer<T>? _comparer;
    private readonly List<T> _list;
     
    public SortedSetX()
    {
        _list = new List<T>();
    }

    public SortedSetX(int capacity)
    {
        _list = new List<T>(capacity);
    }

    public SortedSetX(IComparer<T> comparer) : this()
    {
        _comparer = comparer.ThrowIfNull();
    }

    /// <summary>
    /// Constructor expects a collection of items.
    /// </summary>
    /// <param name="collection">The collection can be unsorted.</param>
    /// <param name="isSorted">If false the items will be sorted otherwise just added.</param>
    public SortedSetX(IEnumerable<T> collection, bool isSorted = false)
    {
        _list = new List<T>();
        var sortedItems = isSorted ? collection : collection.OrderBy(x => x);

        foreach (T item in sortedItems)
        {
            _list.Add(item);
        }
    }

    public SortedSetX(IComparer<T> comparer, int capacity) : this(capacity)
    {
        _comparer = comparer.ThrowIfNull();
    }

    public SortedSetX(IComparer<T> comparer, IEnumerable<T> collection) : this(collection)
    {
        _comparer = comparer.ThrowIfNull();
    }

    /// <inheritdoc/>
    public T this[int index] => _list[index];

    /// <inheritdoc/>
    public void Add(T item)
    {
        var index = null == _comparer
            ? _list.BinarySearch(item)
            : _list.BinarySearch(item, _comparer);

        if (0 <= index) return;
        
        if (0 > index) index = ~index;

        _list.Insert(index, item);
    }

    /// <summary>
    /// Adds a list of elements to the list.
    /// </summary>
    /// <param name="items"></param>
    public void AddRange(IEnumerable<T> items)
    {
        foreach (T item in items.OrderBy(x => x))
        {
            Add(item);
        }
    }

    /// <summary>
    /// <see cref="List{T}.BinarySearch(T)"/>
    /// </summary>
    public int BinarySearch(T item) => null == _comparer
            ? _list.BinarySearch(item)
            : _list.BinarySearch(item, _comparer);

    /// <summary>
    /// <see cref="List{T}.BinarySearch(T, IComparer{T}?)"/>
    /// </summary>
    public int BinarySearch(T item, IComparer<T>? comparer) => _list.BinarySearch(item, comparer);

    /// <summary>
    /// <see cref="List{T}.BinarySearch(int, int, T, IComparer{T}?)"/>
    /// </summary>
    public int BinarySearch(int index, int count, T item, IComparer<T>? comparer)
        => _list.BinarySearch(index, count, item, comparer);

    /// <inheritdoc/>
    public void Clear() => _list.Clear();

    /// <inheritdoc/>
    public bool Contains(T item) => null == _comparer
            ? _list.BinarySearch(item) > -1
            : _list.BinarySearch(item, _comparer) > -1;

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
        => _list.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public int Count => _list.Count;

    /// <summary>
    /// <see cref="List{T}.Find(Predicate{T})"/>
    /// </summary>
    public T? Find(Predicate<T> match) => _list.Find(match);

    /// <summary>
    /// Retrieves all the elements that match the conditions defined by the specified lambda.
    /// </summary>
    /// <param name="list">The list to execute the lambda.</param>
    /// <param name="lambda">Filter for FindAll method.</param>
    /// <returns></returns>
    public List<T> FindAll(LambdaExpression lambda)
    {
        if (!lambda.ThrowIfNull().IsPredicate())
            throw new ArgumentOutOfRangeException(nameof(lambda), $"is not a predicate");

        if (1 != lambda.Parameters.Count)
            throw new ArgumentOutOfRangeException(nameof(lambda), $"one parameter expected");

        if (lambda.Parameters.First().Type != typeof(T))
            throw new ArgumentOutOfRangeException(nameof(lambda), $"wrong parameter type");

        var func = (Func<T, bool>)lambda.Compile();
        return FindAll(new Predicate<T>(func));
    }

    /// <summary>
    /// <see cref="List{T}.FindAll(Predicate{T})"/>
    /// </summary>
    public List<T> FindAll(Predicate<T> match) => _list.FindAll(match);

    /// <summary>
    /// <see cref="List{T}.FindIndex(Predicate{T})"/>
    /// </summary>
    public int FindIndex(Predicate<T> match) => _list.FindIndex(match);

    /// <summary>
    /// <see cref="List{T}.FindIndex(int, Predicate{T})"/>
    /// </summary>
    public int FindIndex(int startIndex, Predicate<T> match)
        => _list.FindIndex(startIndex, match);

    /// <summary>
    /// <see cref="List{T}.FindIndex(int, int, Predicate{T})"/>
    /// </summary>
    public int FindIndex(int startIndex, int count, Predicate<T> match)
        => _list.FindIndex(startIndex, count, match);

    /// <summary>
    /// <see cref="List{T}.FindLast(Predicate{T})"/>
    /// </summary>
    public T? FindLast(Predicate<T> match) => _list.FindLast(match);

    /// <summary>
    /// <see cref="List{T}.FindLastIndex(Predicate{T})"/>
    /// </summary>
    public int FindLastIndex(Predicate<T> match) => _list.FindLastIndex(match);

    /// <summary>
    /// <see cref="List{T}.FindLastIndex(int, Predicate{T})"/>
    /// </summary>
     public int FindLastIndex(int startIndex, Predicate<T> match)
        => _list.FindLastIndex(startIndex, match);

    /// <summary>
    /// <see cref="List{T}.FindLastIndex(int, int, Predicate{T})"/>
    /// </summary>
    public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        => _list.FindLastIndex(startIndex, count, match);

    /// <summary>
    /// Returs the first element if list is not empty.
    /// </summary>
    public T? First => 0 == _list.Count ? default : _list[0];

    /// <summary>
    /// Returns the index of the first element. If <see cref="SortedList{T}"/> is empty -1 is returned.
    /// </summary>
    /// <returns>Index greater or equal 0 or -1 if emtpy.</returns>
    public int FirstIndex() => 0 == _list.Count ? -1 : 0;

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

    /// <summary>
    /// <see cref="List{T}.GetRange(int, int)"/>
    /// </summary>
    public SortedList<T> GetRange(int index, int count) => new (_list.GetRange(index, count));

    /// <summary>
    /// Returns a view of a subset in a <see cref="SortedList{T}"/>.
    /// </summary>
    /// <param name="lowerValue">The lowest desired value in the view.
    /// If lowerValue is smaller than the minimum value, the first element is taken.
    /// If lowerValue does not exist, the next higher value is taken.
    /// </param>
    /// <param name="upperValue">The highest desired value in the view.
    /// If upperValue is greater than the maximum value, the last element is taken.
    /// If upperValue does not exist, the next lower value is taken.
    /// </param>
    /// <returns>A subset view that contains only the values in the specified range.</returns>
    public SortedList<T> GetViewBetween(T? lowerValue, T? upperValue)
    {
        var lowerIndex = 0;
        var upperIndex = _list.Count - 1;

        if (lowerValue is T lower)
        {
            var index = _list.BinarySearch(lower);
            if (0 > index) index = ~index;

            lowerIndex = index;
        }

        if (upperValue is T upper)
        {
            var index = _list.BinarySearch(upper);
            if (0 > index) index = ~index;

            if (index <= upperIndex)
            {
                upperIndex = index;

                var valueAtIndex = _list[upperIndex];
                if (!upper.Equals(valueAtIndex)) --upperIndex;
            }
        }

        return GetRange(lowerIndex, (upperIndex - lowerIndex) + 1);
    }

    /// <summary>
    /// <see cref="List{T}.IndexOf(T)"/>
    /// </summary>
    public int IndexOf(T item) => _list.IndexOf(item);

    /// <summary>
    /// <see cref="List{T}.IndexOf(T, int)"/>
    /// </summary>
    public int IndexOf(T item, int index) => _list.IndexOf(item, index);

    /// <summary>
    /// <see cref="List{T}.IndexOf(T, int, int)"/>
    /// </summary>
    public int IndexOf(T item, int index, int count) => _list.IndexOf(item, index, count);

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <summary>
    /// Returns the last element of the list if not empty.
    /// </summary>
    public T? Last => 0 == _list.Count ? default : _list[LastIndex()];

    /// <summary>
    /// Returns the index of the last element. If <see cref="SortedList{T}"/> is empty -1 is returned.
    /// </summary>
    /// <returns>Index greater or equal 0 or -1 if emtpy.</returns>
    public int LastIndex() => _list.Count - 1;

    /// <summary>
    /// <see cref="List{T}.LastIndexOf(T)"/>
    /// </summary>
    public int LastIndexOf(T item) => _list.LastIndexOf(item);

    /// <summary>
    /// <see cref="List{T}.LastIndexOf(T, int)"/>
    /// </summary>
    public int LastIndexOf(T item, int index) => _list.LastIndexOf(item, index);

    /// <summary>
    /// <see cref="List{T}.LastIndexOf(T, int, int)"/>
    /// </summary>
    public int LastIndexOf(T item, int index, int count) => _list.LastIndexOf(item, index, count);

    /// <inheritdoc/>
    public bool Remove(T item) => _list.Remove(item);

    /// <inheritdoc/>
    public void RemoveAt(int index) => _list.RemoveAt(index);

    /// <summary>
    /// <see cref="List{T}.Reverse()"/>
    /// </summary>
    public void Reverse() => _list.Reverse();

    /// <summary>
    /// <see cref="List{T}.Reverse(int, int)"/>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public void Reverse(int index, int count) => _list.Reverse(index, count);

    /// <summary>
    /// <see cref="List{T}.TrimExcess"/>
    /// </summary>
    public void TrimExcess() => _list.TrimExcess();

    /// <summary>
    /// <see cref="List{T}.TrueForAll(Predicate{T})"/>
    /// </summary>
    /// <param name="match"></param>
    /// <returns></returns>
    public bool TrueForAll(Predicate<T> match) => _list.TrueForAll(match);
}
