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
    public class HashCodeBenchMarks
    {
        //[Params(10, 100, 10000)]
        //public int NumberOfObjects;

        private readonly int _hashCode = 123456;
        private readonly string _object = "alöskdfjöalskdfjölaskdfjlöasdjflj";

        public IEnumerable<object[]> Inputs()
        {
            yield return new object[] { Enumerable.Range(1, 10).ToArray(), Enumerable.Range(1, 10).Select(x => $"{x}").ToArray() };
            yield return new object[] { Enumerable.Range(1, 100).ToArray(), Enumerable.Range(1, 100).Select(x => $"{x}").ToArray() };
            yield return new object[] { Enumerable.Range(1, 10000).ToArray(), Enumerable.Range(1, 10000).Select(x => $"{x}").ToArray() };
        }

        //[Benchmark]
        //[ArgumentsSource(nameof(Inputs))]
        //public int HashCodeFactory(int[] hashCodes, string[] objects)
        //{
        //    var builder = HashCode.CreateFactory();

        //    builder.AddObjects(objects);
        //    builder.AddHashCode(_hashCode);
        //    builder.AddHashCodes(hashCodes);
        //    builder.AddObject(_object);

        //    return builder.GetHashCode();
        //}

        [Benchmark]
        [ArgumentsSource(nameof(Inputs))]
        public int HashCodeBuilder(int[] hashCodes, string[] objects)
        {
            return HashCode.CreateBuilder()
                           .AddObjects(objects)
                           .AddHashCode(_hashCode)
                           .AddHashCodes(hashCodes)
                           .AddObject(_object)
                           .GetHashCode();
        }
    }
}
