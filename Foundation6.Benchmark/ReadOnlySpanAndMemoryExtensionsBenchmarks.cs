namespace Foundation.Benchmarks;

using BenchmarkDotNet.Attributes;
using Foundation.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;

[MemoryDiagnoser]
public class ReadOnlySpanAndMemoryExtensionsBenchmarks
{
    [Benchmark]
    public IReadOnlyCollection<int> ReadOnlySpan_IndexesFromEnd()
    {
        var span = "the \"very\" \"important\" case".AsSpan();
        return span.IndicesFromEnd("\"".AsSpan());
    }

    [Benchmark]
    public int[] ReadOnlyMemory_IndexesFromEnd()
    {
        var memory = "the \"very\" \"important\" case".AsMemory();
        return memory.IndicesFromEnd("\"".AsMemory()).ToArray();
    }

    [Benchmark]
    public int[] ReadOnlyMemory_IndexesFromEnd_2Elements()
    {
        var memory = "the \"very\" \"important\" case".AsMemory();
        return memory.IndicesFromEnd("\"".AsMemory()).Take(2).ToArray();
    }
}

