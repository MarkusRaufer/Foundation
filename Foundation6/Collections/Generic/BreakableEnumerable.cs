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

using Foundation.ComponentModel;
using System.Collections;

public class BreakableEnumerable<T> : IEnumerable<T>
{
    private readonly IEnumerable<T> _items;
    private Action? _stop;

    public BreakableEnumerable(IEnumerable<T> items, ref ObservableValue<bool> stop)
    {
        _items = items;
        stop.ValueChanged += Break;
    }

    private void Break(bool stop)
    {
        _stop?.Invoke();
    }

    public IEnumerator<T> GetEnumerator() => new BreakableEnumerator(_items.GetEnumerator(), out _stop);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private class BreakableEnumerator : IEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;
        private bool _stop;

        public BreakableEnumerator(IEnumerator<T> enumerator, out Action stop)
        {
            _enumerator = enumerator;
            stop = Break;
        }

        public void Break()
        {
            _stop = true;
        }

        public T Current => _enumerator.Current;

        public void Dispose()
        {
            _stop = false;
            _enumerator.Dispose();
        }

        object? IEnumerator.Current => _enumerator.Current;

        public bool MoveNext()
        {
            if (_stop) return false;
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _stop = false;
            _enumerator.Reset();
        }
    }
}

