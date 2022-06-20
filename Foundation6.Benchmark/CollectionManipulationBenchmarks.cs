using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;
using System.Collections.ObjectModel;

namespace Foundation.Benchmark
{
    [MemoryDiagnoser]
    public class CollectionManipulationBenchmarks
    {
        private readonly Bag<int> _bag = new();
        private readonly Collection<int> _collection = new();
        private readonly List<int> _list = new();

        [Params(1000, 10000)]
        public int NumberOfIterations;

        [IterationCleanup]
        public void Cleanup()
        {
            _bag.Clear();
            _collection?.Clear();
            _list.Clear();
        }

        [Benchmark]
        public int BagAdd()
        {
            foreach (var i in Enumerable.Range(1, NumberOfIterations))
                _bag.Add(i);

            return _bag.Count;
        }

        [Benchmark]
        public int CollectionAdd()
        {
            foreach(var i in Enumerable.Range(1, NumberOfIterations))
                _collection.Add(i);

            return _collection.Count;
        }

        [Benchmark]
        public int ListAdd()
        {
            foreach (var i in Enumerable.Range(1, NumberOfIterations))
                _list.Add(i);

            return _list.Count;
        }
    }
}
