using BenchmarkDotNet.Attributes;
using Foundation.Collections;

namespace Foundation.Benchmark;

[MemoryDiagnoser]
public class PermutationsBenchmark
{
    private object[] _booleans = new object[] { false, true };
    private object[] _charsSmall;
    private object[] _charsLarge;
    private object[] _numbersSmall;
    private object[] _numbersLarge;
    private object[][] _small;
    private List<List<object>> _smallList;

    private object[][] _large;
    private List<List<object>> _largeList;


    public PermutationsBenchmark()
    {
        _charsSmall = Collections.Generic.EnumerableEx.Range('a', 'c').Select(x => (object)x).ToArray();
        _charsLarge = Collections.Generic.EnumerableEx.Range('a', 'z').Select(x => (object)x).ToArray();
        _numbersSmall = Enumerable.Range(1, 2).Select(x => (object)x).ToArray();
        _numbersLarge = Enumerable.Range(1, 1000).Select(x => (object)x).ToArray();

        _small = new object[][] { _charsSmall, _numbersSmall, _booleans };
        _smallList = new List<List<object>> { _charsSmall.ToList(), _numbersSmall.ToList(), _booleans.ToList() };
        _large = new object[][] { _charsLarge, _numbersLarge, _booleans };
        _largeList = new List<List<object>> { _charsLarge.ToList(), _numbersLarge.ToList(), _booleans.ToList() };
    }

    [Benchmark]
    public void Permutations_Small()
    {
        foreach (var permutation in _small.Permutations())
        {
        }
    }

    [Benchmark]
    public void PermutationsArrays_Small()
    {
        foreach(var permutation in _small.PermutationsArrays())
        {
        }
    }

    [Benchmark]
    public void PermutationsLists_Small()
    {
        foreach(var permutation in _smallList.PermutationsLists())
        {
        }
    }


    [Benchmark]
    public void Permutations_Large()
    {
        foreach (var permutation in _large.Permutations())
        {
        }
    }

    [Benchmark]
    public void PermutationsArrays_Large()
    {
        foreach (var permutation in _large.PermutationsArrays())
        {
        }
    }


    [Benchmark]
    public void PermutationsLists_Large()
    {
        foreach (var permutation in _largeList.PermutationsLists())
        {
        }
    }


    [Benchmark]
    public void Permutations_Search_Small()
    {
        foreach (var items in _small.Permutations())
        {
            if ((char)items.First() == 'm') break;
        }
    }

    [Benchmark]
    public void PermutationsArrays_Search_Small()
    {
        foreach (var items in _small.PermutationsArrays())
        {
            if ((char)items.First() == 'm') break;
        }
    }

    [Benchmark]
    public void PermutationsLists_Search_Small()
    {
        foreach (var items in _smallList.PermutationsLists())
        {
            if ((char)items.First() == 'm') break;
        }
    }

    [Benchmark]
    public void Permutations_Search_Large()
    {
        foreach (var items in _large.Permutations())
        {
            if ((char)items.First() == 'm') break;
        }
    }

    [Benchmark]
    public void PermutationsArrays_Search_Large()
    {
        foreach (var items in _large.PermutationsArrays())
        {
            if ((char)items.First() == 'm') break;
        }
    }

    [Benchmark]
    public void PermutationsLists_Search_Large()
    {
        foreach (var items in _largeList.PermutationsLists())
        {
            if ((char)items.First() == 'm') break;
        }
    }

}
