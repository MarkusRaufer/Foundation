namespace Foundation.Collections.Generic;

public static class RangeExtensions
{
	public static RangeEnumerator GetEnumerator(this System.Range range) => new(range);
}

public ref struct RangeEnumerator
{
	private readonly System.Range _range;
	private int _current;

	public RangeEnumerator(System.Range range)
	{
		_range = range.ThrowIfOutOfRange(() => range.End.IsFromEnd, "range end must be defined");
		_current = range.Start.IsFromEnd ? -1 : range.Start.Value - 1;
    }

	public int Current => _current;

	public bool MoveNext()
	{
        _current++;

        if (_range.End.IsFromEnd) return true;

		if (_current > _range.End.Value) return false;
		
		return true;
	}
}
