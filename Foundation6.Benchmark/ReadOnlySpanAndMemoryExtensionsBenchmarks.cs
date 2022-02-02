namespace Foundation.Benchmarks;

using BenchmarkDotNet.Attributes;
using Foundation.Buffers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[MemoryDiagnoser]
public class ReadOnlySpanAndMemoryExtensionsBenchmarks
{
    [Benchmark]
    public IReadOnlyCollection<int> ReadOnlySpan_IndexesFromEnd()
    {
        var span = "the \"very\" \"important\" case".AsSpan();
        return span.IndexesFromEnd("\"".AsSpan());
    }

    [Benchmark]
    public int[] ReadOnlyMemory_IndexesFromEnd()
    {
        var memory = "the \"very\" \"important\" case".AsMemory();
        return memory.IndexesFromEnd("\"".AsMemory()).ToArray();
    }

    [Benchmark]
    public int[] ReadOnlyMemory_IndexesFromEnd_2Elements()
    {
        var memory = "the \"very\" \"important\" case".AsMemory();
        return memory.IndexesFromEnd("\"".AsMemory()).Take(2).ToArray();
    }
}

