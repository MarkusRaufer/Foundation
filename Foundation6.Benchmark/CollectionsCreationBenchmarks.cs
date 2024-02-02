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
using Foundation.Collections.Generic;
using System;
using System.Collections.Immutable;

namespace Foundation.Benchmark
{
    [MemoryDiagnoser(false)]
    public class CollectionsCreationBenchmarks
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private int[] _values;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


        [Params(1000, 10000)]
        public int NumberOfItems;


        [GlobalSetup]
        public void Setup()
        {
            _values = Enumerable.Range(1, NumberOfItems).ToArray();
        }

        [Benchmark]
        public ArrayValue<int> ArrayValue()
        {
            return new ArrayValue<int>(_values);
        }

        [Benchmark]
        public ImmutableArray<int> ImmutableArray()
        {
            return _values.ToImmutableArray();
        }

        [Benchmark]
        public SortedSetValue<int> SortedSetValue()
        {
            return new SortedSetValue<int>(_values);

        }
        [Benchmark]
        public ImmutableHashSet<int> ImmutableHashSet()
        {
            return _values!.ToImmutableHashSet();
        }

        [Benchmark]
        public DictionaryValue<int, int> DictionaryValue()
        {
            return new DictionaryValue<int, int>(_values.Select(x => new KeyValuePair<int, int>(x, x)));
        }

        [Benchmark]
        public ImmutableDictionary<int, int> ImmutableDictionary()
        { 
            return _values!.ToImmutableDictionary(x => x, x => x);
        }
    }
}
