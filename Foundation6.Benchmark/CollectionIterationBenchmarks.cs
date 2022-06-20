using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;
using System.Collections.ObjectModel;

namespace Foundation.Benchmark
{
    [MemoryDiagnoser]
    public class CollectionIterationBenchmarks
    {
        private readonly Bag<int> _bag = new();
        private readonly Collection<int> _collection = new();
        private readonly List<int> _list = new();

        [Params(10000)]
        public int NumberOfIterations;

        [Params(5000)]
        public int Search;

        [GlobalSetup]
        public void Setup()
        {
            foreach (var i in Enumerable.Range(1, NumberOfIterations))
                _bag.Add(i);

            foreach (var i in Enumerable.Range(1, NumberOfIterations))
                _collection.Add(i);

            foreach (var i in Enumerable.Range(1, NumberOfIterations))
                _list.Add(i);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _bag.Clear();
            _collection.Clear();
            _list.Clear();
        }

        [Benchmark]
        public bool BagContains()
        {
            return _bag.Contains(Search);
        }

        [Benchmark]
        public bool CollectionContains()
        {
            return _collection.Contains(Search);
        }

        [Benchmark]
        public bool ListContains()
        {
            return _list.Contains(Search);
        }
    }
}
