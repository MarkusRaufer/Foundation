using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using Foundation.Collections.Generic;
using Microsoft.Diagnostics.Runtime;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace Foundation.Benchmark
{
    [MemoryDiagnoser]
    public class CollectionIterationBenchmarks
    {
        //private readonly Collection<int> _collection = new();
        //private readonly ImmutableSortedList<int> _immutableSortedList = new();
        //private readonly List<int> _list = new();
        private readonly SortedList<int> _sortedList = new();
        private readonly SortedList<int, int> _msSortedList = new();
        private readonly Random _random = new(1);

        [Params(100000)]
        public int NumberOfIterations;

        //[Params(10, 5000, 9000)]
        //public int Search { get; set; }

        //private IList<int> _values;
        private int[] _values;

        public CollectionIterationBenchmarks()
        {
            //_values = _random.IntegersWithoutDuplicates(1, NumberOfIterations).ToArray();
        }
        //public static DictionaryValue<int, string> KeyValues
        //    => DictionaryValue.New(Enumerable.Range(1, NumberOfIterations)
        //                                     .Select(x => new KeyValuePair<int, string>(x, x.ToString())));

        //public static IEnumerable<KeyValuePair<int, string>> Replacements
        //    => Enumerable.Range(1, NumberOfIterations)
        //                 .Where(x => x % 2 == 0)
        //                 .Select(x => new KeyValuePair<int, string>(x, x.ToString()));

        [GlobalSetup]
        public void GlobalSetup()
        {
            _values = _random.IntegersWithoutDuplicates(1, NumberOfIterations).ToArray();
            //    _msSortedList.Clear();
            //    _values = _random.IntegersWithoutDuplicates(1, NumberOfIterations).ToArray();
            //    //foreach (var i in Values)
            //    //    _collection.Add(i);

            //    //foreach (var i in Enumerable.Range(1, NumberOfIterations))
            //    //    _list.Add(i);

            //    //foreach (var value in Values)
            //    //{
            //    //    _sortedList.Add(value);
            //    //}
        }

        //[GlobalCleanup]
        //public void Cleanup()
        //{
        //    _values.Clear();
        //    //_collection.Clear();
        //    //_immutableSortedList.Clear();
        //    //_list.Clear();
        //    _sortedList.Clear();
        //    //_msSortedList.Clear();
        //}

        [IterationSetup]
        public void Setup()
        {
            _msSortedList.Clear();
            _sortedList.Clear();
        }

        [Benchmark]
        public void Add_SortedList()
        {
            foreach (var value in _values)
            {
                _sortedList.Add(value);
            }
        }


        [Benchmark]
        public void Add_MS_SortedList()
        {
            foreach (var value in _values)
            {
                _msSortedList.Add(value, value);
            }
        }

        //[Benchmark]
        //public void Add_ImmutableSortedList()
        //{
        //    foreach (var value in Values)
        //    {
        //        _immutableSortedList.Add(value);
        //    }
        //}

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

        //[Benchmark]
        //public bool SortedList_Contains()
        //{
        //    return _sortedList.Contains(Search);
        //}

        //[Benchmark]
        //public SortedList<int> SortedList_GetViewBetween()
        //{
        //    return _sortedList.GetViewBetween(4000, 5000);
        //}
    }
    }
