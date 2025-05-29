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

using System.Collections;

public class CyclicEnumerable<T> : IEnumerable<T>
{
    private readonly IEnumerable<T> _items;

    public CyclicEnumerable(IEnumerable<T> items)
    {
        _items = items.ThrowIfNull();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new CyclicEnumerator(_items);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new CyclicEnumerator(_items);
    }

    private class CyclicEnumerator : IEnumerator<T>
    {
        private readonly IEnumerable<T> _enumerable;
        private IEnumerator<T> _enumerator;

        public CyclicEnumerator(IEnumerable<T> enumerable)
        {
            _enumerable = enumerable.ThrowIfNull();

            _enumerator = _enumerable.GetEnumerator();
        }

        public T Current => _enumerator.Current;

        object? IEnumerator.Current => _enumerator.Current;

        public void Dispose()
        {
            _enumerator.Dispose();
        }

        public bool MoveNext()
        {
            if (_enumerator.MoveNext()) return true;

            Reset();
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator = _enumerable.GetEnumerator();
        }
    }
}

public class CyclicEnumerable<T, TCount> : IEnumerable<(T, TCount)>
    where TCount : IComparable<TCount>
{
    private readonly IEnumerable<T> _enumerable;
    private readonly Func<TCount, TCount> _increment;
    public CyclicEnumerable(IEnumerable<T> items, TCount min, TCount max, Func<TCount, TCount> increment)
    {
        _enumerable = items.ThrowIfNull();
        _increment = increment.ThrowIfNull();

        Min = min;
        Max = max;
    }

    public TCount Max { get; private set; }
    public TCount Min { get; private set; }

    public IEnumerator GetEnumerator()
    {
        return GetEnumerator();
    }

    IEnumerator<(T, TCount)> IEnumerable<(T, TCount)>.GetEnumerator()
    {
        return new CyclicEnumerator(_enumerable, Min, Max, _increment);
    }


    private class CyclicEnumerator : IEnumerator<(T, TCount)>
    {
        private readonly IEnumerable<T> _items;
        private IEnumerator<T> _enumerator;
        private readonly Func<TCount, TCount> _increment;
        private bool _passed;

        public CyclicEnumerator(IEnumerable<T> items, TCount min, TCount max, Func<TCount, TCount> increment)
        {
            _items = items.ThrowIfNull().Cycle();
            _enumerator = items.GetEnumerator();
            _increment = increment;
            Counter = min;
            Min = min;
            Max = max;

            _passed = false;
        }

        public TCount Counter { get; set; }

        public void Dispose()
        {
        }

        public TCount Max { get; private set; }
        public TCount Min { get; private set; }

        (T, TCount) IEnumerator<(T, TCount)>.Current => (_enumerator.Current, Counter);

        public object? Current => _enumerator.Current;

        public bool MoveNext()
        {
            _enumerator.MoveNext();

            var count = _passed ? _increment(Counter) : Min;

            Counter = (1 == count.CompareTo(Max)) ? Min : count;

            _passed = true;
            return true;
        }

        public void Reset()
        {
            _enumerator = _items.GetEnumerator();
        }
    }
}

