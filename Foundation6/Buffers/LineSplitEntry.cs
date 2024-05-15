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

#if NET6_0_OR_GREATER
public readonly ref struct LineSplitEntry
{
    public LineSplitEntry(ReadOnlySpan<char> line, ReadOnlySpan<char> separator)
    {
        Line = line;
        Separator = separator;
    }

    public ReadOnlySpan<char> Line { get; }
    public ReadOnlySpan<char> Separator { get; }

    public void Deconstruct(out ReadOnlySpan<char> line, out ReadOnlySpan<char> separator)
    {
        line = Line;
        separator = Separator;
    }

    // This method allow to implicitly cast the type into a ReadOnlySpan<char>, so you can write the following code
    // foreach (ReadOnlySpan<char> entry in str.SplitLines())
    public static implicit operator ReadOnlySpan<char>(LineSplitEntry entry) => entry.Line;
}

#endif