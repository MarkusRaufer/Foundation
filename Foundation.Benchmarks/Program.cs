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
﻿using BenchmarkDotNet.Running;
using Foundation.Benchmark;

namespace Foundation.Benchmarks;

public class Program
{
    public static void Main(string[] args)
    {
        //Call_PermutationsBenchmark();

        //BenchmarkRunner.Run<ArrayValueBenchmarks>();
        //BenchmarkRunner.Run<ByteStringBenchMarks>();

        //BenchmarkRunner.Run<CircularArrayBenchmarks>();
        //BenchmarkRunner.Run<CollectionsCreationBenchmarks>();
        //BenchmarkRunner.Run<CollectionsSearchBenchmarks>();
        //BenchmarkRunner.Run<EnumerableBenchMarks>();
        //BenchmarkRunner.Run<HashCodeBenchMarks>();
        //BenchmarkRunner.Run<HashCollectionBenchmarks>();
        //BenchmarkRunner.Run<OptionBenchmarks>();
        //BenchmarkRunner.Run<PermutationsBenchmark>();
        //BenchmarkRunner.Run<ReadOnlySpanAndMemoryExtensionsBenchmarks>();
        //BenchmarkRunner.Run<SortedListBenchmarks_AddItems>();
        //BenchmarkRunner.Run<SortedListBenchmarks_SeachItems>();
        //BenchmarkRunner.Run<SortedSetXBenchmarks>();
        //BenchmarkRunner.Run<StringExtensionsBenchmark>();
    }

    public static void Call_PermutationsBenchmark()
    {
        var bm = new PermutationsBenchmark();
        bm.Permutations_Search_Large();
        //bm.Permutations_Search_LargeZLinq();
    }

}
