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
﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Validators;
using Foundation.Collections.Generic;
using System.Collections;
using System.Collections.Generic;

namespace Foundation.Benchmark;

[HtmlExporter]
[MemoryDiagnoser(false)]
public class HashCollectionBenchmarks
{
    private readonly Random _random = new(1);

    //#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private readonly List<int> _intList = [];
    private readonly HashCollection<int> _intHashCollection = [];
    private readonly List<string> _stringList = [];
    private readonly HashCollection<string> _stringHashCollection = [];

//#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


    [Params(1000, 10000)]
    public int NumberOfItems;

    public int IntValue;
    public string StringValue = "";

    [GlobalSetup]
    public void Setup()
    {
        _intList.Clear();
        _intHashCollection.Clear();
        _stringList.Clear();
        _stringHashCollection.Clear();

        IntValue = NumberOfItems / 2;
        StringValue = IntValue.ToString();

        int maxValue = NumberOfItems / 100;

        for (var i = 0; i < NumberOfItems; i++)
        {
            var number = _random.Next(1, maxValue);

            _intList.Add(number);
            _intHashCollection.Add(number);

            _stringList.Add(number.ToString());
            _stringHashCollection.Add(number.ToString());
        }
    }

    [Benchmark]
    public bool Contains_IntDictionary()
    {
        return _intList.Contains(IntValue);
    }

    [Benchmark]
    public bool Contains_HashedIntValueDictionary()
    {
        return _intHashCollection.Contains(IntValue);
    }


    [Benchmark]
    public bool TryGetValues_IntDictionary()
    {
        List<int> values = [];
        foreach (var value in _intList)
        {
            if (value == IntValue) values.Add(value);
        }

        return values.Count > 0;
    }

    [Benchmark]
    public bool TryGetValues_HashedIntValueDictionary()
    {
        return _intHashCollection.TryGetValues(IntValue, out var values);
    }

    [Benchmark]
    public bool Contains_StringDictionary()
    {
        return _stringList.Contains(StringValue);
    }

    [Benchmark]
    public bool Contains_HashedStringValueDictionary()
    {
        return _stringHashCollection.Contains(StringValue);
    }

    [Benchmark]
    public bool TryGetValue_StringDictionary()
    {
        List<string> values = [];
        foreach (var value in _stringList)
        {
            if (value == StringValue) values.Add(value);
        }

        return values.Count > 0;
    }

    [Benchmark]
    public bool TryGetValue_HashedStringValueDictionary()
    {
        return _stringHashCollection.TryGetValues(StringValue, out var values);
    }
}
