using System.Collections;

namespace Foundation.Collections.Generic;

public struct Params : IEnumerable<object>
{
    public bool _isInitialized;

    private readonly ICollection<object> _values;

    public Params(params object[] values)
    {
        _values = values.ThrowIfNull();
        _isInitialized = true;
    }

    public IEnumerator<object> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();

    public bool IsEmpty => !_isInitialized;

    public static Params<T> New<T>(params T[] values)
    {
        return new Params<T>(values);
    }
}

public struct Params<T> : IEnumerable<T>
{
    public bool _isInitialized;

    private readonly ICollection<T> _values;

    public Params(params T[] values)
    {
        values.ThrowIfNull();

        _values = values;
        _isInitialized = true;
    }

    public bool IsEmpty => !_isInitialized;

    public IEnumerator<T> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _values.GetEnumerator();
}
