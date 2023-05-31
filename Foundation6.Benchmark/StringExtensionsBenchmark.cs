namespace Foundation.Benchmarks;

using BenchmarkDotNet.Attributes;
using Foundation.Buffers;
using Foundation.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[MemoryDiagnoser]
public class StringExtensionsBenchmark
{
    [Params(10, 100, 1000)]
    public int N;

    private string? _str;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _str = NumberWithSpaces(1, N);
    }

    [Benchmark]
    public void ReduceSpaces()
    {
        var str = _str!.ReduceSpaces();
    }

    //[Benchmark]
    //public IReadOnlyCollection<int> ReadOnlySpan_IndexesFromEnd()
    //{
    //    var span = "the \"very\" \"important\" case".AsSpan();
    //    return span.IndicesFromEnd("\"".AsSpan());
    //}

    //[Benchmark]
    //public int[] ReadOnlyMemory_IndexesFromEnd()
    //{
    //    var memory = "the \"very\" \"important\" case".AsMemory();
    //    return memory.IndicesFromEnd("\"".AsMemory()).ToArray();
    //}

    //[Benchmark]
    //public int[] ReadOnlyMemory_IndexesFromEnd_2Elements()
    //{
    //    var memory = "the \"very\" \"important\" case".AsMemory();
    //    return memory.IndicesFromEnd("\"".AsMemory()).Take(2).ToArray();
    //}

    private static string NumberWithSpaces(int seed, int quantity)
    {
        var sb = new StringBuilder();
        foreach (var i in Enumerable.Range(seed, quantity)
                                    .AfterEach(x => sb.Append(" ".Repeat((uint)x))))
        {
            sb.Append(i);
        }
        return sb.ToString();
    }
}

