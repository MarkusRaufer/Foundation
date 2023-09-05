using BenchmarkDotNet.Attributes;
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
