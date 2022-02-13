namespace Foundation.Collections;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public class ArrayEnumerator<T> : IEnumerator<T>
{
    private readonly T[] _array;
    private readonly IEnumerator _enumerator;

    public ArrayEnumerator([DisallowNull] T[] array)
    {
        _array = array.ThrowIfNull();
        _enumerator = _array.GetEnumerator();
    }

    public T Current => (T)_enumerator.Current;

    object IEnumerator.Current => _enumerator.Current;

    public void Dispose()
    {
    }

    public bool MoveNext()
    {
        return _enumerator.MoveNext();
    }

    public void Reset()
    {
        _enumerator.Reset();
    }
}

