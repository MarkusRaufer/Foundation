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

namespace Foundation.Collections
{
    public class LambdaComparer : IComparer
    {
        private readonly Func<object?, object?, int> _compare;

        public LambdaComparer(Func<object?, object?, int> compare)
        {
            _compare = compare.ThrowIfNull();
        }

        public int Compare(object? x, object? y)
        {
            return (x, y) switch
            {
                (null, null) => 0,
                (null, _) => -1,
                (_, null) => 1,
                (_, _) => _compare(x, y)
            };
        }
    }

    public class LambdaComparer<T> : IComparer
    {
        private readonly Func<T, T, int> _compare;

        public LambdaComparer(Func<T, T, int> compare)
        {
            _compare = compare.ThrowIfNull();
        }

        public int Compare(object? x, object? y)
        {
            if(x is T tx && y is T ty)
                return _compare(tx, ty);

            if(x is IComparable comparableX) return comparableX.CompareTo(y);
            
            if(y is IComparable comparableY) return comparableY.CompareTo(x);

            return (x, y) switch
            {
                (null, null) => 0,
                (null, _) => -1,
                (_, null) => 1,
                (_, _) => throw new InvalidOperationException("values are not comparable")
            };
        }
    }
}
