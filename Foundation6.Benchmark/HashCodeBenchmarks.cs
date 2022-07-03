using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;

namespace Foundation.Benchmark
{

    [MemoryDiagnoser]
    public class HashCodeBenchMarks
    {
        //[Params(10, 100, 10000)]
        //public int NumberOfObjects;

        private readonly int _hashCode = 123456;
        private readonly string _object = "alöskdfjöalskdfjölaskdfjlöasdjflj";

        public IEnumerable<object[]> Inputs()
        {
            yield return new object[] { Enumerable.Range(1, 10).ToArray(), Enumerable.Range(1, 10).Select(x => $"{x}").ToArray() };
            yield return new object[] { Enumerable.Range(1, 100).ToArray(), Enumerable.Range(1, 100).Select(x => $"{x}").ToArray() };
            yield return new object[] { Enumerable.Range(1, 10000).ToArray(), Enumerable.Range(1, 10000).Select(x => $"{x}").ToArray() };
        }

        [Benchmark]
        [ArgumentsSource(nameof(Inputs))]
        public int HashCodeFactory(int[] hashCodes, string[] objects)
        {
            var builder = HashCode.CreateFactory();

            builder.AddObjects(objects);
            builder.AddHashCode(_hashCode);
            builder.AddHashCodes(hashCodes);
            builder.AddObject(_object);

            return builder.GetHashCode();
        }

        [Benchmark]
        [ArgumentsSource(nameof(Inputs))]
        public int HashCodeBuilder(int[] hashCodes, string[] objects)
        {
            return HashCode.CreateBuilder()
                           .AddObjects(objects)
                           .AddHashCode(_hashCode)
                           .AddHashCodes(hashCodes)
                           .AddObject(_object)
                           .GetHashCode();
        }
    }
}
