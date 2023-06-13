using BenchmarkDotNet.Running;
using Foundation.Benchmark;
using Foundation.Collections.Generic;
using System.Text;

namespace Foundation.Benchmarks
{

    public class Program
    {
        public static void Main(string[] args)
        {
            //BenchmarkRunner.Run<StringExtensionsBenchmark>();
            //BenchmarkRunner.Run<ReadOnlySpanAndMemoryExtensionsBenchmarks>();
            //BenchmarkRunner.Run<CollectionManipulationBenchmarks>();
            BenchmarkRunner.Run<CollectionIterationBenchmarks>();
            //BenchmarkRunner.Run<CollectionsBenchMarks>();
            //BenchmarkRunner.Run<HashCodeBenchMarks>();
            //BenchmarkRunner.Run<EnumerableBenchMarks>();
            //BenchmarkRunner.Run<OptionBenchmarks>();
            //BenchmarkRunner.Run<PermutationsBenchmark>();
        }

        
    }
}
