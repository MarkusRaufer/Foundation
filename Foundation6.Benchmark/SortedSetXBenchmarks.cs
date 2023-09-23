using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;
using System.Collections;
using System.Collections.Generic;

namespace Foundation.Benchmark;

[HtmlExporter]
[MemoryDiagnoser(false)]
public class SortedSetXBenchmarks
{
    private readonly Random _random = new(1);

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private int[] _intValues;
    private SortedSet<int> _sortedSet;
    private SortedSetX<int> _sortedSetX;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


    //[Params(5000, 9900)]
    //public int Index;

    [GlobalSetup]
    public void Setup()
    {
        _intValues = _random.IntegersWithoutDuplicates(1, 10000).ToArray();
        _sortedSet = new SortedSet<int>(_intValues);
        _sortedSetX = new SortedSetX<int>(_intValues);
    }

    [Benchmark]
    public List<int> SortedSet_LinqWhere()
    {
        return _sortedSet.Where(x => x % 1000 == 0).ToList();
    }
    [Benchmark]
    public List<int> SortedSetX_FindAll()
    {
        return _sortedSetX.FindAll((int x) => x % 1000 == 0);
    }

    [Benchmark]
    public List<int> SortedSet_Contains()
    {
        return new[] { 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000 }.Where(_sortedSet.Contains).ToList();
    }

    [Benchmark]
    public List<int> SortedSetX_Contains()
    {
        return new[] { 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000 }.Where(_sortedSetX.Contains).ToList();
    }
}
