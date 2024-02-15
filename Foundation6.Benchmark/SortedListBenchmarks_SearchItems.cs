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

namespace Foundation.Benchmark
{
    [MemoryDiagnoser]
    public class SortedListBenchmarks_SeachItems
    {
        private readonly SortedList<int> _sortedList = new();
        private readonly SortedList<int, int> _msSortedList = new();
        private readonly Random _random = new(1);

        [Params(100000)]
        public int NumberOfIterations;

        [Params(0, 50000, 99000)]
        public int Index;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private int[] _values;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [GlobalSetup]
        public void GlobalSetup()
        {
            _values = _random.IntegersWithoutDuplicates(1, NumberOfIterations).ToArray();

            foreach (var value in _values)
                _sortedList.Add(value);

            foreach (var value in _values)
                _msSortedList.Add(value, value);
        }

        [Benchmark]
        public bool Contains_SortedList()
        {
            var value = _values[Index];
            return _sortedList.Contains(value);
        }


        [Benchmark]
        public bool Contains_MS_SortedList()
        {
            var value = _values[Index];
            return _msSortedList.ContainsKey(value);
        }
    }
}
