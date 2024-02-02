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

public class RingEnumerator
{
    public static RingEnumerator<T> Create<T>(IEnumerable<T> enumerable, bool infinite = false, int index = 0)
    {
        return new RingEnumerator<T>(enumerable, infinite, index);
    }
}

public class RingEnumerator<T> : IEnumerator<T?>
{
    private readonly IEnumerator<T> _enumerator;
    private bool _passed;
    private readonly bool _infinite;
    private int _pos = 0;
    private readonly int _startIndex;

    public RingEnumerator(IEnumerable<T> enumerable, bool infinite = false, int index = 0)
    {
        if (null == enumerable) throw new ArgumentNullException(nameof(enumerable));
        _enumerator = enumerable.GetEnumerator();
        _infinite = infinite;
        _startIndex = index;
        _pos = index;
    }

    public T? Current => _enumerator.Current;

    public void Dispose()
    {
        _enumerator.Dispose();
    }

    object? System.Collections.IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (!_passed)
        {
            _passed = true;
            for (var i = 0; i <= _startIndex; i++)
            {
                if (!_enumerator.MoveNext())
                {
                    if (0 == i) return false;

                    _enumerator.Reset();
                    _pos = 0;
                }
                else
                    _pos = i;
            }
            return true;
        }

        if (!_enumerator.MoveNext())
        {
            _enumerator.Reset();
            _enumerator.MoveNext();
            _pos = 0;
        }
        else
            _pos++;

        if (!_infinite && _pos == _startIndex) return false;

        return true;
    }

    public void Reset()
    {
        _enumerator.Reset();
        _passed = false;
        _pos = _startIndex;
    }
}

