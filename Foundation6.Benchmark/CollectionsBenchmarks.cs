using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;
using System.Collections.Immutable;

namespace Foundation.Benchmark
{
    [MemoryDiagnoser(false)]
    public class CollectionsBenchMarks
    {
        private int[]? _values;

        [Params(100)]
        public int NumberOfItems;

        [GlobalSetup]
        public void Setup()
        {
            _values = Enumerable.Range(1, NumberOfItems).ToArray();
            //foreach (var i in Enumerable.Range(1, NumberOfItems))
            //    _list.Add(i);
        }

        //[GlobalCleanup]
        //public void Cleanup()
        //{
        //}

        [Benchmark]
        public HashSetValue<int> EquatableHashSet()
        {
            return new HashSetValue<int>(_values!);
        }

        [Benchmark]
        public ImmutableHashSet<int> ImmutableHashSet()
        {
            return _values!.ToImmutableHashSet();
        }

        [Benchmark]
        public UniqueArrayValue<int> UniqueOnlyArray()
        {
            return new UniqueArrayValue<int>(_values!);
        }

        [Benchmark]
        public UniqueHashSetValue<int> UniqueOnlyHashSet()
        {
            return new UniqueHashSetValue<int>(_values!);
        }
    }
}
