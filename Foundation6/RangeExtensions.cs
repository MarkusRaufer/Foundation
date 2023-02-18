namespace Foundation;

public static class RangeExtensions
{
    public static RangeIntEnumerator GetEnumerator(this System.Range range)
    {
        return new RangeIntEnumerator(range);
    }

    public static bool Includes(this System.Range range, int value)
    {
        if(!range.Start.IsFromEnd)
        {
            if (range.Start.Value > value) return false;
        }
        if(!range.End.IsFromEnd)
        {
            return range.End.Value >= value;
        }

        return true;
    }

    public static int[] ToArray(this System.Range range)
    {
        var start = range.Start.IsFromEnd ? 0 : range.Start.Value;
        
        range.End.ThrowIfOutOfRange(() => range.End.IsFromEnd);
        var end = range.End.Value;

        var numberOfElements = end - start;

        var arr = new int[numberOfElements];
        var index = 0;
        var maxIndex = numberOfElements - 1;

        for (var i = start; index <= maxIndex; i++, index++)
        {
            arr[index] = i;
        }

        return arr;
    }

    /// <summary>
    /// Returns a list of integers.
    /// </summary>
    /// <param name="range"></param>
    /// <returns></returns>
    public static IEnumerable<int> ToEnumerable(this System.Range range)
    {
        var start = range.Start.IsFromEnd ? 0 : range.Start.Value;
        var end = range.End.IsFromEnd ? int.MaxValue : range.End.Value;
        for(var i = start; i <= end; i++)
            yield return i;
    }
}

public class RangeIntEnumerator
{
    private int _current;
    private readonly int _end;

    public RangeIntEnumerator(System.Range range)
    {
        _current = range.Start.IsFromEnd ? -1 : range.Start.Value - 1;
        _end = range.End.IsFromEnd ? int.MaxValue : range.End.Value;
    }

    public int Current => _current;

    public bool MoveNext()
    {
        _current++;
        return _current <= _end;
    }
}
