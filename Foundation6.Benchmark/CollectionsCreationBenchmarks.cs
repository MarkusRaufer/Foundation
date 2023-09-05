using BenchmarkDotNet.Attributes;
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
