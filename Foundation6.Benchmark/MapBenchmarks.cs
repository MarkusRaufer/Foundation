namespace Foundation.Benchmarks;

using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;

public class MapBenchmarks
{
    //private Dictionary<string, int> _dict;
    //private Map<string, int> _map;
    //private KeyValue<string, int>[] _keyValues;
    //public MapBenchmarks()
    //{
    //    _keyValues = Enumerable.Range(0, 1000).Select(n => KeyValue.New(n.ToString(), n)).ToArray();

    //    _dict = _keyValues.ToDictionary(kv => kv.Key, kv => kv.Value);
    //    _map = new Map<string, int>(_keyValues);
    //}

    //[Benchmark]
    //public bool TryGetValue_Dictionary()
    //{
    //    var key = (_keyValues.Length / 2);
    //    return _dict.TryGetValue(key.ToString(), out _);
    //}

    //[Benchmark]
    //public bool TryGetValue_Map()
    //{
    //    var key = (_keyValues.Length / 2);
    //    return _map.TryGetValue(key.ToString(), out _);
    //}
}
