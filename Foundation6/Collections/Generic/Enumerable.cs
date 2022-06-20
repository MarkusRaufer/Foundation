namespace Foundation.Collections.Generic;

using System.Collections;

public class Enumerable<T> : IEnumerable<T>
{
    private readonly IEnumerator<T> _enumerator;

    public Enumerable(IEnumerator<T> enumerator)
    {
        _enumerator = enumerator.ThrowIfNull();
    }

    public IEnumerator<T> GetEnumerator() => _enumerator;

    IEnumerator IEnumerable.GetEnumerator() => _enumerator;
}

