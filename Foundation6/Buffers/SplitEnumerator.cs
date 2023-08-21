namespace Foundation.Buffers;

// Must be a ref struct as it contains a ReadOnlySpan<char>
public ref struct SplitEnumerator<T>
    where T : IEquatable<T>
{
    private readonly bool _notFoundReturnsNothing;
    private bool _passed;
    private ReadOnlySpan<T> _span;

    public SplitEnumerator(ReadOnlySpan<T> span, ReadOnlySpan<T> separators, bool notFoundReturnsNothing = true)
    {
        _span = span;
        Separators = separators;
        _notFoundReturnsNothing = notFoundReturnsNothing;

        Current = default;
        _passed = false;
    }

    public SplitEnumerator(ReadOnlySpan<T> span, params T[] separators)
        : this(span, separators.AsSpan(), false)
    {
        separators.ThrowIf(() => 0 == separators.Length);
    }

    public SplitEnumerator(ReadOnlySpan<T> span, bool notFoundReturnsNothing, params T[] separators)
        : this(span, separators.AsSpan(), notFoundReturnsNothing)
    {
    }

    public ReadOnlySpan<T> Current { get; private set; }

    // Needed to be compatible with the foreach operator
    public SplitEnumerator<T> GetEnumerator() => this;

    public bool MoveNext()
    {
        var span = _span;
        if (span.Length == 0) return false; // span is empty

        var index = span.IndexOfAny(Separators);
        if (index == -1) // The string is composed of only one line
        {
            _span = ReadOnlySpan<T>.Empty; // The remaining span is an empty span

            if (!_passed && _notFoundReturnsNothing) return false;

            Current = span;
            return true;
        }
        _passed = true;

        Current = span[..index];
        _span = span[(index + 1)..];
        return true;
    }

    ReadOnlySpan<T> Separators { get; }
}

