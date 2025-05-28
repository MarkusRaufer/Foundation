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
using System.Collections;

namespace Foundation.Collections.Generic;

/// <summary>
/// This is a circular array with a fixed size.
/// </summary>
public static class CircularList
{
    /// <summary>
    /// Creates a new <see cref="CircularList{T}"/> with a size of <paramref name="capacity"/>.
    /// </summary>
    /// <param name="capacity"></param>
    /// <exception cref="ArgumentOutOfRangeException">The size must be at list 1.</exception>
    public static CircularList<T> New<T>(int capacity)
    {
        if (capacity < 1) throw new ArgumentOutOfRangeException(nameof(capacity));
        return new(capacity);
    }

    /// <summary>
    /// Creates a new <see cref="CircularList{T}"/> with <paramref name="items"/>. The number of the items determine the array size.
    /// </summary>
    /// <param name="items">A list of items that initialize the array and determine the size.</param>
    public static CircularList<T> New<T>(IEnumerable<T> items) => new([.. items]);
}

/// <summary>
/// This is a circular array with a fixed size.
/// If the capacity of the array is reached the oldest entry will be overwritten.
/// This array allows the use of internal indices, which makes it possible to synchronize external systems.
/// </summary>
/// <typeparam name="T">The type of the elements.</typeparam>
public class CircularList<T> : IReadOnlyCollection<T>
{
    public Event<Action<T>> OnAddReplaced = new();

    private readonly int _capacity;
    private int _head;
    private readonly bool _isInitialized;
    private readonly List<T> _list;
    private readonly int _maxIndex;
    private int _tail;

    /// <summary>
    /// The constructor of <see cref="CircularList{T}"/>. The <paramref name="capacity"/> determines the size of the array.
    /// </summary>
    /// <param name="capacity"></param>
    /// <exception cref="ArgumentOutOfRangeException">The size must be at list 1.</exception>
    internal CircularList(int capacity)
    {
        if (capacity < 1) throw new ArgumentOutOfRangeException(nameof(capacity));

        _capacity = capacity;
        _list = new List<T>(capacity);
        _maxIndex = capacity - 1;

        _head = 0;
        _tail = -1;
    }

    /// <summary>
    /// The constructor of <see cref="CircularList{T}"/> which will be initialized with <paramref name="items"/>.
    /// </summary>
    internal CircularList(List<T> items)
    {
        _list = items;
        _maxIndex = _list.Count - 1;
        _head = 0;
        _tail = _maxIndex;
        _isInitialized = true;
    }

    /// <summary>
    /// The indexer where you can get and set an item at a specific index.
    /// </summary>
    /// <param name="index">The index at which you want to retrieve or set an element.</param>
    /// <returns></returns>
    public T this[int index]
    {
        get
        {
            var internalIndex = GetInternalIndex(index);
            return _list[internalIndex];
        }
        set
        {
            var internalIndex = GetInternalIndex(index);
            _list[internalIndex] = value;
        }
    }

    /// <summary>
    /// Adds an element to the list. If the capacity of the list is reached the oldest item will be overwritten with the new item.
    /// The item will not be physically removed.
    /// </summary>
    /// <param name="item">The item that will be added.</param>
    public void Add(T item)
    {
        var tail = _tail + 1;
        if (tail > _maxIndex)
        {
            _tail = 0;
            _head++;

            var tailElement = _list[_tail];

            _list[_tail] = item;
            OnAddReplaced.Publish(tailElement);
            return;
        }

        if (tail == _head && _list.Count > 0)
        {
            _head++;
            if (_head > _maxIndex) _head = 0;
        }

        if (_list.Count < _capacity)
        {
            _list.Add(item);
            _tail = tail;
        }
        else _list[tail] = item;
    }

    /// <summary>
    /// Removes all elements of the array. The elements are not physically removed.
    /// </summary>
    public void Clear()
    {
        _list.Clear();
        _head = 0;
        _tail = _isInitialized ? _maxIndex : -1;
        
    }

    /// <summary>
    /// The number of the elements.
    /// </summary>
    public int Count => _list.Count;

    /// <summary>
    /// An enumerator of the <see cref="CircularList{T}"/>.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<T> GetEnumerator()
    {
        foreach (var index in Indices)
        {
            yield return _list[index];
        }
    }

    /// <summary>
    /// An enumerator of the <see cref="CircularList{T}"/>.
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Returns the internal index of the <see cref="CircularList{T}"/> from the ordinal index.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetInternalIndex(int index)
    {
        if (!IsIndexInRange(index)) return -1;

        var j = 0;
        foreach (var i in Indices)
        {
            if (j == index) return i;
            
            j++;
        }

        return -1;
    }

    /// <summary>
    /// The first element of the <see cref="CircularList{T}"/>.
    /// </summary>
    public T Head => _list[_head];

    /// <summary>
    /// Thr internal index of the first element of the <see cref="CircularList{T}"/>.
    /// </summary>
    public int HeadIndex => _head;

    /// <summary>
    /// Returns the ordinal index of the item.
    /// </summary>
    /// <param name="item">The item which index should be returned.</param>
    /// <returns>Returns the index if found otherwise -1.</returns>
    public int IndexOf(T item)
    {
        var j = 0;
        foreach (var i in Indices)
        {
            var element = _list[i];
            if (element.EqualsNullable(item)) return j;

            j++;
        }

        return -1;
    }

    /// <summary>
    /// Returns the internal index of the item in the <see cref="CircularList{T}"/>.
    /// </summary>
    /// <param name="item">The item which index should be returned.</param>
    /// <returns>Returns the index if found otherwise -1.</returns>
    public int InternalIndexOf(T item) => _list.IndexOf(item);

    private IEnumerable<int> Indices
    {
        get
        {
            var i = _head;
            var n = 0;
            for (; i < _list.Count; i++, n++)
            {
                yield return i;
            }

            if (n == _list.Count) yield break;

            i = 0;
            for (; i < _head; i++)
            {
                yield return i;
            }
        }
    }

    /// <summary>
    /// Checks if the index is a valid index otherwise false.
    /// </summary>
    /// <param name="index">The index that should be verified.</param>
    /// <returns>Returns true if it is a valid index otherwise false.</returns>
    public bool IsIndexInRange(int index)
    {
        if (index < 0) return false;

        return index < _list.Count;
    }

    /// <summary>
    /// Replaces the object with <paramref name="item"/> at a specific internal <paramref name="index"/> of the <see cref="CircularList{T}"/>.
    /// </summary>
    /// <param name="item">the <paramref name="item"/> that should replace the object at the specific index <paramref name="index"/>.</param>
    /// <param name="index">The internal index of the array.</param>
    public void ReplaceAtInternalIndex(T item, int index)
    {
        _list[index] = item;
    }

    /// <summary>
    /// The last object of the array.
    /// </summary>
    public T Tail => _list[_tail];

    /// <summary>
    /// The internal index of the last object.
    /// </summary>
    public int TailIndex => _tail;
}
