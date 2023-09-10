namespace Foundation.Buffers;

// Must be a ref struct as it contains a ReadOnlySpan<char>
public ref struct StringSplitEnumerator
{
    private readonly StringComparison _comparison;
    private readonly ReadOnlySpan<char> _part;
    private bool _passed;
    private ReadOnlySpan<char> _span;

    public StringSplitEnumerator(ReadOnlySpan<char> span, ReadOnlySpan<char> part, StringComparison comparison)
    {
        _span = span;
        _part = part;
        _comparison = comparison;

        Current = default;
        _passed = false;
    }

    public ReadOnlySpan<char> Current { get; private set; }

    // Needed to be compatible with the foreach operator
    public StringSplitEnumerator GetEnumerator() => this;

    public bool MoveNext()
    {
        var span = _span;
        if (span.Length == 0) return false; // span is empty

        var index = span.IndexOf(_part, _comparison);
        if (index == -1) // The string is composed of only one line
        {
            _span = ReadOnlySpan<char>.Empty; // The remaining span is an empty span

            if (!_passed) return false;

            Current = span;
            return true;
        }
        _passed = true;

        Current = span[..index];
        _span = span[(index + 1)..];
        return true;
    }
}

