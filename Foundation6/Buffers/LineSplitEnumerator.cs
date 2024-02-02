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
/// Splits a text into lines. The lines are represented by ReadOnlySpans.
/// Must be a ref struct as it contains a ReadOnlySpan<char>
/// </summary>
public ref struct LineSplitEnumerator
{
    private ReadOnlySpan<char> _str;

    public LineSplitEnumerator(ReadOnlySpan<char> str)
    {
        _str = str;
        Current = default;
    }

    // Needed to be compatible with the foreach operator
    public LineSplitEnumerator GetEnumerator() => this;

    public bool MoveNext()
    {
        var span = _str;
        if (span.Length == 0) // Reach the end of the string
            return false;

        var index = span.IndexOfAny('\r', '\n');
        if (index == -1) // The string is composed of only one line
        {
            _str = ReadOnlySpan<char>.Empty; // The remaining string is an empty string
            Current = span;
            return true;
        }

        if (index < span.Length - 1 && span[index] == '\r')
        {
            // Try to consume the the '\n' associated to the '\r'
            var next = span[index + 1];
            if (next == '\n')
            {
                Current = span[..index];
                _str = span[(index + 2)..];
                return true;
            }
        }

        Current = span[..index];
        _str = span[(index + 1)..];
        return true;
    }

    public ReadOnlySpan<char> Current { get; private set; }
}

