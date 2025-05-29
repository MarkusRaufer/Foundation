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
using System.Collections.ObjectModel;

namespace Foundation.Collections.Generic
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EnumerableCounter<T> : IEnumerable<T>
    {
        private readonly IEnumerable<T> _items;

        public EnumerableCounter(IEnumerable<T> items)
        {
            _items = items.ThrowIfNull();
        }

        public EnumerableCounter(IEnumerable<T> items, out Func<EnumerableCounter<T>> self)
        {
            _items = items.ThrowIfNull();
            self = () => this;
        }
        
        public static implicit operator EnumerableCounter<T>(T[] items)
        {
            return new EnumerableCounter<T>(items);
        }

        public static implicit operator EnumerableCounter<T>(Collection<T> items)
        {
            return new EnumerableCounter<T>(items);
        }

        public static implicit operator EnumerableCounter<T>(List<T> items)
        {
            return new EnumerableCounter<T>(items);
        }

        public IEnumerator<T> GetEnumerator()
        {
            Count = 0;
            return new CounterEnumerator(_items.GetEnumerator(), IncrementCount);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            Count = 0;
            return new CounterEnumerator(_items.GetEnumerator(), IncrementCount);
        }

        public int Count { get; protected set; }

        protected void IncrementCount()
        {
            Count++;
        }

        private class CounterEnumerator : IEnumerator<T>
        {
            private readonly IEnumerator<T> _enumerator;
            private readonly Action _callOnMoveNext;

            public CounterEnumerator(IEnumerator<T> enumerator, Action callOnMoveNext)
            {
                _callOnMoveNext = callOnMoveNext;
                _enumerator = enumerator;
            }
            public T Current => _enumerator.Current;

            object? IEnumerator.Current => _enumerator.Current;

            public void Dispose() => _enumerator.Dispose();

            public bool MoveNext()
            {
                var next = _enumerator.MoveNext();
                if (next) _callOnMoveNext();

                return next;
            }

            public void Reset() => _enumerator.Reset();
        }
    }
}
