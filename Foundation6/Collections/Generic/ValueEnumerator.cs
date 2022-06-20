namespace Foundation.Collections.Generic;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public struct ValueEnumerator<T> : IEnumerator<T?>
{
    private readonly IEnumerator<T> _enumerator;

    public ValueEnumerator(IEnumerator<T> enumerator)
    {
        _enumerator = enumerator.ThrowIfNull();
    }
    public T Current => _enumerator.Current;

    object? IEnumerator.Current => _enumerator.Current;

    public void Dispose() => _enumerator.Dispose();

    public bool MoveNext() => _enumerator.MoveNext();

    public void Reset() => _enumerator.Reset();
}

