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
﻿namespace Foundation.Benchmarks;

using BenchmarkDotNet.Attributes;
using Foundation.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;

[MemoryDiagnoser]
public class ReadOnlySpanAndMemoryExtensionsBenchmarks
{
    public static char[] _separators = ['_', '.', '-'];

    public static string _strWithSeparators = "123_45.678-9";

    private static string _str = "the \"very\" \"important\" case";

    [Benchmark]
    public IReadOnlyCollection<int> ReadOnlySpan_IndexesFromEnd()
    {
        var span = _str.AsSpan();
        return span.IndicesFromEnd("\"".AsSpan());
    }

    [Benchmark]
    public int[] ReadOnlyMemory_IndexesFromEnd()
    {
        var memory = _str.AsMemory();
        return memory.IndicesFromEnd("\"".AsMemory()).ToArray();
    }

    [Benchmark]
    public int[] ReadOnlyMemory_IndexesFromEnd_2Elements()
    {
        var memory = _str.AsMemory();
        return memory.IndicesFromEnd("\"".AsMemory()).Take(2).ToArray();
    }

    [Benchmark]
    public IReadOnlyCollection<int> ReadOnlySpan_IndicesOfSingleCharacters()
    {
        var span = _strWithSeparators.AsSpan();
        return span.IndicesOfSingleCharacters(_separators);
    }
}

