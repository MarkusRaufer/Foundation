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
