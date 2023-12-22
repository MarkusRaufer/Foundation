using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;

namespace Foundation.Benchmark
{
    [MemoryDiagnoser]
    public class ByteStringBenchMarks
    {
        private ByteString _lhs;
        private ByteString _rhs;

        private readonly Random _random = new Random();
        private IComparer<ByteString> _nullComparer = ByteStringComparer.NullValuesAreGreater;

        [Params(100, 1000, 100000)]
        public int NumberOfItems;

        [GlobalSetup]
        public void Setup()
        {
            var bytes = new byte[NumberOfItems];
            _lhs = new ByteString(bytes);
            _rhs = new ByteString(bytes);
        }

        [Benchmark]
        public int ByteStringComparer_Default()
        {
            return _lhs.CompareTo(_rhs);
        }

        [Benchmark]
        public int ByteStringComparer_Null()
        {
            return _nullComparer.Compare(_lhs, _rhs);
        }
    }
}
