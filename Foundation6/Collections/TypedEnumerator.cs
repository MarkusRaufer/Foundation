namespace Foundation.Collections;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public class TypedEnumerator<T> : IEnumerator<T>
{
    private readonly IEnumerator _enumerator;

    public TypedEnumerator([DisallowNull] IEnumerator enumerator)
    {
        _enumerator = enumerator.ThrowIfNull(nameof(enumerator));
    }

    public T Current => (T)_enumerator.Current;

    object IEnumerator.Current => _enumerator.Current;

    public void Dispose()
    {
        if (_enumerator is IDisposable disposable)
            disposable.Dispose();
    }

    public bool MoveNext() => _enumerator.MoveNext();

    public void Reset() => _enumerator.Reset();
}

