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
﻿namespace Foundation.Buffers;

/// <summary>
/// Splits a string into parts by using indices and lengths. An index means where to split the string and the lenght means how many characters starting from index.
/// </summary>
/// <typeparam name="T"></typeparam>
public ref struct IndexSplitEnumerator<T>
{
    private readonly ReadOnlySpan<T> _span;
    private readonly (int index, int length)[] _split;

    private int _splitIndex = 0;

    public IndexSplitEnumerator(ReadOnlySpan<T> span, (int index, int length)[] split)
    {
        _span = span;
        _split = split;
        Current = default;
    }

    public ReadOnlySpan<T> Current { get; private set; }

    public bool MoveNext()
    {
        if (_split.Length == 0) return false;
        if (_splitIndex > _split.Length - 1) return false;

        var (index, length) = _split[_splitIndex++];

        Current = _span.Slice(index, length);

        return true;
    }
}
