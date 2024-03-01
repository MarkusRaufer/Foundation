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
﻿using System.Collections;
using System.Collections.Generic;

namespace Foundation.Collections.Generic;

public class ResilientLinkedList<T> : IList<T>
{
    private class Node(T value, Node? next)
    {
        public T Value { get; set; } = value;
        public Node? Next { get; set; } = next;

        public override string ToString() => $"{Value}";
    }

    private Node? _first;
    private Node? _last;

    public T? First => _first is null ? default : _first.Value;

    public T this[int index]
    {
        get => this.Nth(index).TryGet(out var value) ? value : throw new ArgumentOutOfRangeException($"{nameof(index)} {index}");
        set => Insert(index, value);
    }

    public int Count => GetNodeEnumerable().Count();

    public bool IsReadOnly => false;

    public T? Last => _last is null ? default : _last.Value;

    public void Add(T item)
    {
        if (_first is null)
        {
            _first = new Node(item, null);
            _last = _first;
            return;
        }

        if (_last is Node last)
        {
            _last = new Node(item, null);
            last.Next = _last;
        }
    }

    public void Clear()
    {
        _first = null;
        _last = null;
    }

    public bool Contains(T item)
    {
        foreach (var value in this)
        {
            if(value.EqualsNullable(item)) return true;
        }

        return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        var end = array.Length - arrayIndex;
        var i = arrayIndex;
        foreach (var value in this)
        {
            array[i] = value;
            if (i >= end) break;
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        var it = NodeEnumerator();
        while(it.MoveNext())
        {
            yield return it.Current.Value;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }

    public int IndexOf(T item)
    {
        var it = NodeEnumerator();
        var i = 0;
        while (it.MoveNext())
        {
            if(it.Current.Value.EqualsNullable(item)) return i;
            i++;
        }
        return -1;
    }
    
    private IEnumerator<Node> NodeEnumerator()
    {
        if (_first is null) yield break;

        var current = _first;
        while (true)
        {
            yield return current;

            if (current == _last) break;
            if (current.Next is null) break;

            current = current.Next;
        }
    }

    private IEnumerable<Node> GetNodeEnumerable() => EnumerableDecorator.New(() => NodeEnumerator());

    public void Insert(int index, T item)
    {
        if(0 == index && _first is null)
        {
            _first = new Node(item, null);
            _last = _first;
            return;
        }

        Node? prev = null;
        foreach (var (i, node) in GetNodeEnumerable().Enumerate())
        {
            if(i == index)
            {
                var newNode = new Node(item, node);
                if (prev is null)
                {
                    _first = newNode;
                    _last = newNode;
                    return;
                }
                prev.Next = newNode;
                return;
            }
            prev = node;
        }
    }

    public bool Remove(T item)
    {
        Node? prev = null;
        foreach (var node in GetNodeEnumerable())
        {
            if(node.Value.EqualsNullable(item))
            {
                if(prev is null)
                {
                    if (node.Next is null)
                    {
                        _first = null;
                        _last = null;
                    }
                    else
                    {
                        _first = node.Next;
                    }
                    return true;
                }

                if(node.Next is null) _last = prev;
                else prev.Next = node.Next;
                
                return true;
            }
            prev = node;
        }

        return false;
    }

    public void RemoveAt(int index)
    {
        if (0 == index)
        {
            if (_first is null) return;
            if (_first.Equals(_last))
            {
                _first = null;
                _last = null;
                return;
            }
        }

        Node? prev = null;
        var i = 0;

        foreach (var node in GetNodeEnumerable())
        {
            if(i == index)
            {
                if (prev is null)
                {
                    _first = node.Next;
                    return;
                }
                prev.Next = node.Next;
                return;
            }
            prev = node;
        }
    }
}
