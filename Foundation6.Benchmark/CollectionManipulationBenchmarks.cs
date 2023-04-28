using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;
using System.Collections.ObjectModel;

namespace Foundation.Benchmark
{
    [MemoryDiagnoser]
    public class CollectionManipulationBenchmarks
    {
        private readonly Collection<int> _collection = new();
        //private readonly List<int> _list = new();

        [Params(1000, 100000)]
        public int NumberOfIterations;

        [IterationCleanup]
        public void Cleanup()
        {
            _collection?.Clear();
            //_list.Clear();
        }

        [Benchmark]
        public int Collection_Add()
        {
            foreach(var i in Enumerable.Range(1, NumberOfIterations))
                _collection.Add(i);

            return _collection.Count;
        }

        //[Benchmark]
        //public int List_Add()
        //{
        //    foreach (var i in Enumerable.Range(1, NumberOfIterations))
        //        _list.Add(i);

        //    return _list.Count;
        //}
    }
}
