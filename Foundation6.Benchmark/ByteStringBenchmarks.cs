using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;

namespace Foundation.Benchmark
{
    [MemoryDiagnoser]
    public class ByteStringBenchMarks
    {
        private ByteString? _lhs;
        private ByteString? _rhs;

        private readonly IComparer<ByteString> _nullComparer = ByteStringComparer.NullIsGreater;

        private decimal _value = 12345M;
        [Params(100, 1000, 100000)]
        public int NumberOfItems;

        [GlobalSetup]
        public void Setup()
        {
            var lhsBytes = new byte[NumberOfItems];
            var rhsBytes = new byte[NumberOfItems];
            byte value = 1;
            for (var i = 0; i < NumberOfItems; i++)
            {
                lhsBytes[i] = value;
                rhsBytes[i] = value;
                value++;
            }

            _lhs = new ByteString(lhsBytes);
            _rhs = new ByteString(rhsBytes);
        }

        [Benchmark]
        public int ByteStringComparer_Default()
        {
            return _lhs!.CompareTo(_rhs);
        }

        [Benchmark]
        public int ByteStringComparer_Null()
        {
            return _nullComparer.Compare(_lhs, _rhs);
        }
    }
}
