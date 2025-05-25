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
using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;
using Microsoft.VisualStudio.Utilities;

namespace Foundation.Benchmark;

[HtmlExporter]
[MemoryDiagnoser(false)]
public class CircularArrayBenchmarks
{
    private readonly Random _random = new(123);

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private CircularArray<string> _circularArray;
    private CircularBuffer<string> _circularBuffer;
    
    private string[] _values;

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


    [Params(1000, 10000, 100000)]
    public int NumberOfElements;

    [Params(1, 100, 1000)]
    public int NumbersToAdd;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _circularArray = new CircularArray<string>(NumberOfElements);
        _circularBuffer = new CircularBuffer<string>(NumberOfElements);

        var numbers = _random.IntegersWithoutDuplicates(0, NumberOfElements);

        foreach (var number in numbers)
        {
            _circularArray.Add(number.ToString());
            _circularBuffer.Add(number.ToString());
        }
    }

    [GlobalCleanup]
    public void GlobalCleanup()
    {
        _circularArray.Clear();
        _circularBuffer.Clear();
    }

    [IterationSetup]
    public void IterationSetup()
    {
        _values = Enumerable.Range(NumberOfElements + 1, NumbersToAdd).Select(x => x.ToString()).ToArray();
    }

    [Benchmark]
    public void CircularArray_Add()
    {
        foreach (var value in _values)
            _circularArray.Add(value);
    }

    [Benchmark]
    public void CircularBuffer_Add()
    {
        foreach (var value in _values)
            _circularBuffer.Add(value);
    }

    [Benchmark]
    public string CircularArray_Head()
    {
        return _circularArray.Head;
    }

    [Benchmark]
    public string CircularBuffer_Head()
    {
        return _circularBuffer[0];
    }

    [Benchmark]
    public string CircularArray_Tail()
    {
        return _circularArray.Tail;
    }

    [Benchmark]
    public string CircularBuffer_Tail()
    {
        return _circularBuffer[^1];
    }
}
