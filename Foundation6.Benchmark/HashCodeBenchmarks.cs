using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;

namespace Foundation.Benchmark
{

    [MemoryDiagnoser]
    public class HashCodeBenchMarks
    {
        private static int _numberOfObjects = 10;
        private int _hashCode = 123456;
        private string _object = "alöskdfjöalskdfjölaskdfjlöasdjflj";
        private int[] _hashCodes = Enumerable.Range(1, _numberOfObjects).ToArray();
        private string[] _objects = Enumerable.Range(1, _numberOfObjects).Select(x => $"{x}").ToArray();

        [Benchmark]
        public int HashCodeFactory()
        {
            var builder = HashCode.CreateFactory();

            builder.AddObjects(_objects);
            builder.AddHashCode(_hashCode);
            builder.AddHashCodes(_hashCodes);
            builder.AddObject(_object);

            return builder.GetHashCode();
        }

        [Benchmark]
        public int HashCodeBuilder()
        {
            return HashCode.CreateBuilder()
                           .AddObjects(_objects)
                           .AddHashCode(_hashCode)
                           .AddHashCodes(_hashCodes)
                           .AddObject(_object)
                           .GetHashCode();
        }
    }
}
