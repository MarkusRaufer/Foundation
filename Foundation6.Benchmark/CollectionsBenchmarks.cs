using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;
using System;
using System.Collections.Immutable;

namespace Foundation.Benchmark
{
    [MemoryDiagnoser(false)]
    public class CollectionsBenchMarks
    {
        private Random _random = new Random(100);
        private int[] _values;

        public CollectionsBenchMarks()
        {
            _values = SetupRandomValues();
        }

        [Params(1000)]
        public int NumberOfItems;


        //[GlobalSetup]
        //public void Setup()
        //{
        //}

        //[GlobalCleanup]
        //public void Cleanup()
        //{
        //}

        private int[] SetupRandomValues(int min = 1)
        {
            var max = NumberOfItems + 1;
            return Enumerable.Range(1, NumberOfItems).Select(_ => _random.Next(min, max)).ToArray();
        }
        
        private int[] SetupSortedValues(int min = 1)
        {
            return Enumerable.Range(min, NumberOfItems).ToArray();
        }

        //[Benchmark]
        //public EquatableHashSet<int> EquatableHashSet()
        //{
        //    return new EquatableHashSet<int>(_values!);
        //}

        //[Benchmark]
        //public ImmutableHashSet<int> ImmutableHashSet()
        //{
        //    return _values!.ToImmutableHashSet();
        //}

        //[Benchmark]
        //public UniqueValues<int> UniqueOnlyHashSet()
        //{
        //    return new UniqueValues<int>(_values!);
        //}
    }
}
