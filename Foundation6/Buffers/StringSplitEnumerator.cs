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
#if NET6_0_OR_GREATER
﻿namespace Foundation.Buffers;

// Must be a ref struct as it contains a ReadOnlySpan<char>
public ref struct StringSplitEnumerator
{
    private readonly StringComparison _comparison;
    private readonly ReadOnlySpan<char> _part;
    private bool _passed;
    private ReadOnlySpan<char> _span;

    public StringSplitEnumerator(ReadOnlySpan<char> span, ReadOnlySpan<char> part, StringComparison comparison)
    {
        _span = span;
        _part = part;
        _comparison = comparison;

        Current = default;
        _passed = false;
    }

    public ReadOnlySpan<char> Current { get; private set; }

    // Needed to be compatible with the foreach operator
    public StringSplitEnumerator GetEnumerator() => this;

    public bool MoveNext()
    {
        var span = _span;
        if (span.Length == 0) return false; // span is empty

        var index = span.IndexOf(_part, _comparison);
        if (index == -1) // The string is composed of only one line
        {
            _span = ReadOnlySpan<char>.Empty; // The remaining span is an empty span

            if (!_passed) return false;

            Current = span;
            return true;
        }
        _passed = true;

        Current = span[..index];
        _span = span[(index + 1)..];
        return true;
    }
}

#endif