using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;

namespace Foundation.Benchmark
{
    [MemoryDiagnoser]
    public class SortedListBenchmarks_AddItems
    {
        private readonly SortedList<int> _sortedList = new();
        private readonly SortedList<int, int> _msSortedList = new();
        private readonly Random _random = new(1);

        [Params(100000)]
        public int NumberOfIterations;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private int[] _values;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        [GlobalSetup]
        public void GlobalSetup()
        {
            _values = _random.IntegersWithoutDuplicates(1, NumberOfIterations).ToArray();
        }

        [IterationSetup]
        public void Setup()
        {
            _msSortedList.Clear();
            _sortedList.Clear();
        }

        [Benchmark]
        public void Add_SortedList()
        {
            foreach (var value in _values)
            {
                _sortedList.Add(value);
            }
        }


        [Benchmark]
        public void Add_MS_SortedList()
        {
            foreach (var value in _values)
            {
                _msSortedList.Add(value, value);
            }
        }
    }
}

