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
/// This is a circular array that has an internal fixed sized buffer.
/// If the maximum index of the array is reached the oldest entry will be overwritten.
/// This array allows the use of internal indices, which makes it possible to synchronize external systems.
/// </summary>
/// <typeparam name="T">The type of the elements.</typeparam>
public class CircularArray<T> : IEnumerable<T>
{
    private readonly T[] _array;
    private int _head;
    private readonly bool _isInitialized;
    private readonly int _maxIndex;
    private int _length;
    private int _tail;

    /// <summary>
    /// the constructor of the <see cref="CircularArray{T}"/>. The <paramref name="capacity"/>a is the size of the internal array.
    /// </summary>
    /// <param name="capacity"></param>
    /// <exception cref="ArgumentOutOfRangeException">The size must be at list 1.</exception>
    public CircularArray(int capacity)
    {
        if (capacity < 1) throw new ArgumentOutOfRangeException(nameof(capacity));

        _array = new T[capacity];
        _maxIndex = capacity - 1;
        _length = 0;

        _head = 0;
        _tail = -1;
    }

    /// <summary>
    /// Initializes the internal array with the items. The number of the items is the buffer size.
    /// </summary>
    /// <param name="items">A list of items that determines the size of the array.</param>
    public CircularArray(IEnumerable<T> items)
    {
        _array = [..items];
        _maxIndex = _array.Length - 1;
        _length = _array.Length;
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
            return _array[internalIndex];
        }
        set
        {
            var internalIndex = GetInternalIndex(index);
            _array[internalIndex] = value;
        }
    }

    /// <summary>
    /// Adds an element to the array. If the capacity of the array is reached the oldest item is removed before the new one will be added.
    /// The item will not be physically removed.
    /// </summary>
    /// <param name="item"></param>
    public void Add(T item)
    {
        _tail++;
        if (_tail > _maxIndex)
        {
            _tail = 0;
            _head++;
            _array[_tail] = item;
            return;
        }

        if (_tail == _head && _length > 0)
        {
            _head++;
            if (_head > _maxIndex) _head = 0;
        }

        _array[_tail] = item;
        if (_length < _array.Length) _length++;
    }

    /// <summary>
    /// Removes all elements of the array. The elements are not physically removed.
    /// </summary>
    public void Clear()
    {
        if (_isInitialized)
        {
            _tail = _maxIndex;
        }
        else
        {
            _tail = -1;
            _length = 0;
        }
        _head = 0;
    }

    /// <summary>
    /// An enumerator of the <see cref="CircularArray{T}"/>.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<T> GetEnumerator()
    {
        foreach (var index in Indices)
        {
            yield return _array[index];
        }
    }

    /// <summary>
    /// An enumerator of the <see cref="CircularArray{T}"/>.
    /// </summary>
    /// <returns></returns>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>
    /// Returns the internal index of the <see cref="CircularArray{T}"/> from the ordinal index.
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
    /// The first element of the <see cref="CircularArray{T}"/>.
    /// </summary>
    public T Head => _array[_head];

    /// <summary>
    /// Thr internal index of the first element of the <see cref="CircularArray{T}"/>.
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
            var element = _array[i];
            if (element.EqualsNullable(item)) return j;

            j++;
        }

        return -1;
    }

    /// <summary>
    /// Returns the internal index of the item in the <see cref="CircularArray{T}"/>.
    /// </summary>
    /// <param name="item">The item which index should be returned.</param>
    /// <returns>Returns the index if found otherwise -1.</returns>
    public int InternalIndexOf(T item) => Array.IndexOf(_array, item);

    private IEnumerable<int> Indices
    {
        get
        {
            var i = _head;
            var n = 0;
            for (; i < _length; i++, n++)
            {
                yield return i;
            }

            if (n == _length) yield break;

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

        return index < _length;
    }

    /// <summary>
    /// The length of the array.
    /// </summary>
    public int Length => _length;

    /// <summary>
    /// Replaces the object with <paramref name="item"/> at a specific internal <paramref name="index"/> of the <see cref="CircularArray{T}"/>.
    /// </summary>
    /// <param name="item">the <paramref name="item"/> that should replace the object at the specific index <paramref name="index"/>.</param>
    /// <param name="index">The internal index of the array.</param>
    public void ReplaceAtInternalIndex(T item, int index)
    {
        _array[index] = item;
    }

    /// <summary>
    /// The last object of the array.
    /// </summary>
    public T Tail => _array[_tail];

    /// <summary>
    /// The internal index of the last object.
    /// </summary>
    public int TailIndex => _tail;
}
