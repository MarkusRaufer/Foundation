namespace Foundation.Buffers;

/// <summary>
/// Splits strings as spans.
/// Must be a ref struct as it contains a ReadOnlySpan<char>
/// </summary>
public ref struct CharSplitEnumerator
{
    private readonly bool _notFoundReturnsNothing;
    private bool _passed;
    private ReadOnlySpan<char> _span;

    public CharSplitEnumerator(ReadOnlySpan<char> span, char separator, bool notFoundReturnsNothing = true)
    {
        _span = span;
        Separator = separator;
        _notFoundReturnsNothing = notFoundReturnsNothing;

        Current = default;
        _passed = false;
    }


    public ReadOnlySpan<char> Current { get; private set; }

    // Needed to be compatible with the foreach operator
    public CharSplitEnumerator GetEnumerator() => this;

    public bool MoveNext()
    {
        var span = _span;
        if (span.Length == 0) return false; // span is empty

        var index = span.IndexOf(Separator);
        if (index == -1) // separator not found
        {
            _span = ReadOnlySpan<char>.Empty; // The remaining span is an empty span

            if (!_passed && _notFoundReturnsNothing) return false;

            Current = span;
            return true;
        }
        _passed = true;

        Current = span.Slice(0, index);
        _span = span.Slice(index + 1);
        return true;
    }

    public char Separator { get; }
}

