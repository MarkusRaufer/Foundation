using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;

namespace Foundation.Benchmark
{
    [MemoryDiagnoser]
    public class EnumerableBenchMarks
    {
        private readonly List<int> _lhs = new();
        private readonly List<int> _rhs = new();
        private IComparer<int> _comparer = Comparer<int>.Default;

        [Params(100000)]
        public int NumberOfItems;

        //[Params(5000)]
        //public int Search;

        [GlobalSetup]
        public void Setup()
        {
            //foreach (var i in Enumerable.Range(1, NumberOfItems))
            //    _list.Add(i);

            foreach(var i in Enumerable.Range(1, NumberOfItems))
                _lhs.Add(i);

            foreach (var i in Enumerable.Range(NumberOfItems / 2, NumberOfItems))
                _rhs.Add(i);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _lhs.Clear();
            _rhs.Clear();
        }

        [Benchmark]
        public int[] ExceptWithDuplicates()
        {
            return _lhs.ExceptWithDuplicates(_rhs).ToArray();
        }

        [Benchmark]
        public int[] ExceptWithDuplicatesSorted()
        {
            return _lhs.ExceptWithDuplicatesSorted(_rhs).ToArray();
        }


        //[Benchmark]
        //public int[] ExceptWithDuplicatesSorted_WithComparer()
        //{
        //    return _lhs.ExceptWithDuplicatesSorted(_rhs, _comparer).ToArray();
        //}

        //[Benchmark]
        //public int[] SymmetricDifference()
        //{
        //    return _lhs.SymmetricDifference(_rhs).ToArray();
        //}
    }
}
