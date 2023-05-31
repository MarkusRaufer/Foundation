using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;
using System.Collections.ObjectModel;

namespace Foundation.Benchmark
{
    [MemoryDiagnoser]
    public class CollectionIterationBenchmarks
    {
        //private readonly Collection<int> _collection = new();
        //private readonly List<int> _list = new();

        public const int NumberOfIterations = 10000;

        //[Params(10, 5000, 9000)]
        //public int Search { get; set; }

        //public static IEnumerable<int> Values => Enumerable.Range(1, NumberOfIterations);

        public static DictionaryValue<int, string> KeyValues
            => DictionaryValue.New(Enumerable.Range(1, NumberOfIterations)
                                             .Select(x => new KeyValuePair<int, string>(x, x.ToString())));

        public static IEnumerable<KeyValuePair<int, string>> Replacements
            => Enumerable.Range(1, NumberOfIterations)
                         .Where(x => x % 2 == 0)
                         .Select(x => new KeyValuePair<int, string>(x, x.ToString()));

        [GlobalSetup]
        public void Setup()
        {
            //foreach (var i in Values)
            //    _collection.Add(i);

            //foreach (var i in Enumerable.Range(1, NumberOfIterations))
            //    _list.Add(i);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            //_collection.Clear();
            //_list.Clear();
        }

        //[Benchmark]
        //public bool CollectionContains()
        //{
        //    return _collection.Contains(Search);
        //}

        //[Benchmark]
        //public bool ListContains()
        //{
        //    return _list.Contains(Search);
        //}
    }
}
