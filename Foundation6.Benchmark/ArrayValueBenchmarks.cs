using BenchmarkDotNet.Attributes;
using Foundation.Collections.Generic;
using System.Collections;

namespace Foundation.Benchmark;

[HtmlExporter]
[MemoryDiagnoser(false)]
public class ArrayValueBenchmarks
{
    private readonly Random _random = new(1);

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ArrayValue<int> _intArrayValue;
    private ArrayValue<int> _intArrayValue2;
    private ArrayValue<string> _stringArrayValue;
    private ArrayValue<string> _stringArrayValue2;

    private int[] _intValues;
    private int[] _intValues2;
    private string[] _stringValues;
    private string[] _stringValues2;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.


    //[Params(5000, 9900)]
    //public int Index;

    [GlobalSetup]
    public void Setup()
    {
        _intValues = _random.IntegersWithoutDuplicates(1, 10000).ToArray();
        _intValues2 = _intValues.ToArray();
        _stringValues = _intValues.Select(x => x.ToString()).ToArray();
        _stringValues2 = _intValues2.Select(x => x.ToString()).ToArray();

        _intArrayValue = new ArrayValue<int>(_intValues);
        _intArrayValue2 = new ArrayValue<int>(_intValues2);

        _stringArrayValue = new ArrayValue<string>(_stringValues);
        _stringArrayValue2 = new ArrayValue<string>(_stringValues2);
    }

    [Benchmark]
    public bool Array_int_SequenceEqual_Array()
    {
        return _intValues.SequenceEqual(_intValues2);
    }

    [Benchmark]
    public bool ArrayValue_int_Equals_Array()
    {
        return _intArrayValue.Equals(_intValues2);
    }

    [Benchmark]
    public bool ArrayValue_int_Equals_ArrayValue()
    {
        return _intArrayValue.Equals(_intArrayValue2);
    }

    [Benchmark]
    public bool Array_int_IStructuralEquatable()
    {
        IStructuralEquatable arr1 = _intValues;
        IStructuralEquatable arr2 = _intValues2;
        return arr1.Equals(arr2, EqualityComparer<int>.Default);
    }

    [Benchmark]
    public bool Array_string_SequenceEqual_Array()
    {
        return _stringValues.SequenceEqual(_stringValues2);
    }

    [Benchmark]
    public bool ArrayValue_string_Equals_Array()
    {
        return _stringArrayValue.Equals(_stringValues2);
    }

    [Benchmark]
    public bool ArrayValue_string_Equals_ArrayValue()
    {
        return _stringArrayValue.Equals(_stringArrayValue2);
    }
}
