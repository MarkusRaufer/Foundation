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
public static class CircularArray
{
    /// <summary>
    /// Creates a new <see cref="CircularArray{T}"/> with a size of <paramref name="capacity"/>.
    /// </summary>
    /// <param name="capacity"></param>
    /// <exception cref="ArgumentOutOfRangeException">The size must be at list 1.</exception>
    public static CircularArray<T> New<T>(int capacity)
    {
        if (capacity < 1) throw new ArgumentOutOfRangeException(nameof(capacity));
        return new(capacity);
    }

    /// <summary>
    /// Creates a new <see cref="CircularArray{T}"/> with <paramref name="items"/>. The number of the items determine the array size.
    /// </summary>
    /// <param name="items">A list of items that initialize the array and determine the size.</param>
    public static CircularArray<T> New<T>(IEnumerable<T> items) => new([.. items]);
}

/// <summary>
/// This is a circular array with a fixed size.
/// If the capacity of the array is reached the oldest entry will be overwritten.
/// This array allows the use of internal indices, which makes it possible to synchronize external systems.
/// </summary>
/// <typeparam name="T">The type of the elements.</typeparam>
public class CircularArray<T> : IReadOnlyCollection<T>
{
    public Event<Action<T>> OnReplaced = new();

    private readonly T[] _array;
    private int _head;
    private readonly bool _isInitialized;
    private readonly int _maxIndex;
    private int _numberOfElements;
    private int _tail;

    /// <summary>
    /// The constructor of <see cref="CircularArray{T}"/>. The <paramref name="capacity"/> determines the size of the array.
    /// </summary>
    /// <param name="capacity"></param>
    /// <exception cref="ArgumentOutOfRangeException">The size must be at list 1.</exception>
    internal CircularArray(int capacity)
    {
        if (capacity < 1) throw new ArgumentOutOfRangeException(nameof(capacity));

        _array = new T[capacity];
        _maxIndex = capacity - 1;
        _numberOfElements = 0;

        _head = 0;
        _tail = -1;
    }

    /// <summary>
    /// The constructor of <see cref="CircularArray{T}"/> which will be initialized with <paramref name="items"/>.
    /// </summary>
    internal CircularArray(T[] items)
    {
        _array = items;
        _maxIndex = _array.Length - 1;
        _numberOfElements = _array.Length;
        _head = 0;
        _tail = _maxIndex;
        _isInitialized = true;
    }

    /// <summary>
    /// The indexer where you can get and set an item at a specific index.
    /// If there is no element at the index an IndexOutOfRangeException is thrown.
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

            var oldValue = _array[internalIndex];
            _array[internalIndex] = value;

            OnReplaced.Publish(oldValue);
        }
    }

    /// <summary>
    /// Adds an element to the array. If the capacity of the array is reached the oldest item will be overwritten with the new item.
    /// The item will not be physically removed.
    /// </summary>
    /// <param name="item">The item that will be added.</param>
    public void Add(T item)
    {
        _tail++;
        if (_tail > _maxIndex)
        {
            _tail = 0;
            _head++;

            var tail = _array[_tail];

            _array[_tail] = item;
            OnReplaced.Publish(tail);
            return;
        }

        var replace = false;
        if (_tail == _head && _numberOfElements > 0)
        {
            replace = true;
            _head++;
            if (_head > _maxIndex) _head = 0;
        }

        
        if (replace)
        {
            var tail = _array[_tail];
            _array[_tail] = item;
            OnReplaced.Publish(tail);
        }
        else
        {
            _array[_tail] = item;
        }

        if (_numberOfElements < _array.Length) _numberOfElements++;
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
            _numberOfElements = 0;
        }
        _head = 0;
    }

    /// <summary>
    /// The number of the elements.
    /// </summary>
    public int Count => _numberOfElements;

    private void DecrementNumberOfElements()
    {
        _numberOfElements--;

        if (_numberOfElements < 0) _numberOfElements = 0;
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
    /// Returns the internal index of the <see cref="CircularArray{T}"/> from the ordinal <paramref name="index"/>.
    /// If the array at that index is not set a -1 is returned. If <paramref name="index"/> is out of range a -2 is returned.  
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public int GetInternalIndex(int index)
    {
        if (!IsIndexInRange(index)) return -2;

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
    /// Returns the ordinal index of the first item found with a specific predicate.
    /// </summary>
    /// <param name="predicate">The predicate to select an item.</param>
    /// <returns>The index of an item with a specific predicate.</returns>
    public int IndexOf(Func<T, bool> predicate)
    {
        predicate.ThrowIfNull();

        var j = 0;
        foreach (var i in Indices)
        {
            var element = _array[i];
            if (predicate(element)) return j;
            j++;
        }

        return -1;
    }

    /// <summary>
    /// Returns all indices of items where the predicate returns true.
    /// </summary>
    /// <param name="predicate">The predicate to select items.</param>
    /// <returns>List of indices of items which match the predicate.</returns>
    public IEnumerable<int> IndicesOf(Func<T, bool> predicate)
    {
        predicate.ThrowIfNull();

        var j = 0;
        foreach (var i in Indices)
        {
            var element = _array[i];
            if (predicate(element)) yield return j;
            j++;
        }
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
            for (; i < _numberOfElements; i++, n++)
            {
                yield return i;
            }

            if (n == _numberOfElements) yield break;

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

        return index < _numberOfElements;
    }

    /// <summary>
    /// Checks if the internal index <paramref name="index"/> is in range.
    /// </summary>
    /// <param name="index">Internal index.</param>
    /// <returns></returns>
    public bool IsInternalIndexInRange(int index)
    {
        var (min, max) = _tail < _head ? (0, _maxIndex) : (_head, _tail);

        return index >= min && index <= max;
    }
    
    /// <summary>
    /// Removes an item at a specific index.
    /// </summary>
    /// <param name="index">Index at which an item will be removed.</param>
    public void RemoveAt(int index)
    {
        if (!IsIndexInRange(index)) return;

        var internalIndex = GetInternalIndex(index);
        if (-1 == internalIndex) return;

        if (internalIndex == _tail)
        {
            _tail = internalIndex - 1;
            if (_tail < 0) _tail = 0;
            DecrementNumberOfElements();
            return;
        }

        var sourceIndex = internalIndex + 1;
        var length = _numberOfElements - sourceIndex;
        Array.Copy(_array, sourceIndex, _array, internalIndex,  length);

        DecrementNumberOfElements();
    }

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

    /// <summary>
    /// Tries to get value from index.
    /// </summary>
    /// <param name="index">The index of the array.</param>
    /// <param name="value">The found value at the index.</param>
    /// <returns>True if value found otherwise false.</returns>
    public bool TryGetValue(int index, out T? value)
    {
        var internalIndex = GetInternalIndex(index);

        if (internalIndex < 0)
        {
            value = default;
            return false;
        }
        value = _array[internalIndex];
        return true;
    }

    /// <summary>
    /// Tries to get a value from the internal index.
    /// </summary>
    /// <param name="index">The index of the array.</param>
    /// <param name="value">The found value.</param>
    /// <returns>True if value found otherwise false.</returns>
    public bool TryGetValueFromInternalIndex(int index, out T? value)
    {
        if (!IsIndexInRange(index))
        {
            value = default;
            return false;
        }
        value = _array[index];
        return true;
    }
}
