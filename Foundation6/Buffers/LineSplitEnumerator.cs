namespace Foundation.Buffers;

/// <summary>
/// Splits a text into lines. The lines are represented by ReadOnlySpans.
/// Must be a ref struct as it contains a ReadOnlySpan<char>
/// </summary>
public ref struct LineSplitEnumerator
{
    private ReadOnlySpan<char> _str;

    public LineSplitEnumerator(ReadOnlySpan<char> str)
    {
        _str = str;
        Current = default;
    }

    // Needed to be compatible with the foreach operator
    public LineSplitEnumerator GetEnumerator() => this;

    public bool MoveNext()
    {
        var span = _str;
        if (span.Length == 0) // Reach the end of the string
            return false;

        var index = span.IndexOfAny('\r', '\n');
        if (index == -1) // The string is composed of only one line
        {
            _str = ReadOnlySpan<char>.Empty; // The remaining string is an empty string
            Current = span;
            return true;
        }

        if (index < span.Length - 1 && span[index] == '\r')
        {
            // Try to consume the the '\n' associated to the '\r'
            var next = span[index + 1];
            if (next == '\n')
            {
                Current = span[..index];
                _str = span[(index + 2)..];
                return true;
            }
        }

        Current = span[..index];
        _str = span[(index + 1)..];
        return true;
    }

    public ReadOnlySpan<char> Current { get; private set; }
}

