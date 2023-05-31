using System;
using System.Reflection;

namespace Foundation.Buffers;

public ref struct IndexSplitEnumerator<T>
{
    private readonly ReadOnlySpan<T> _span;
    private readonly (int index, int length)[] _split;

    private int _splitIndex = 0;

    public IndexSplitEnumerator(ReadOnlySpan<T> span, (int index, int length)[] split)
    {
        _span = span;
        _split = split;
        Current = default;
    }

    public ReadOnlySpan<T> Current { get; private set; }

    public bool MoveNext()
    {
        if (_split.Length == 0) return false;
        if (_splitIndex > _split.Length - 1) return false;

        var (index, length) = _split[_splitIndex++];

        Current = _span.Slice(index, length);

        return true;
    }
}
