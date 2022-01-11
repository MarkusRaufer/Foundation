using BenchmarkDotNet.Running;

namespace Foundation.Benchmarks
{

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<MapBenchmarks>();
        }
    }
}
