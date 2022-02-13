namespace Foundation.Collections.Generic;

public struct Params
{
    public bool _isInitialized;

    private readonly ICollection<object> _values;

    public Params(params object[] values)
    {
        _values = values.ThrowIfNull();
        _isInitialized = true;
    }

    public ValueEnumerator<object> GetEnumerator()
    {
        var values = _values ?? Array.Empty<object>();

        return new ValueEnumerator<object>(values.GetEnumerator());
    }

    public bool IsEmpty => !_isInitialized;

    public static Params<T> New<T>(params T[] values)
    {
        return new Params<T>(values);
    }

    public IEnumerable<object> ToEnumerable() => new Enumerable<object>(GetEnumerator());
}

public struct Params<T>
{
    public bool _isInitialized;

    private readonly ICollection<T> _values;

    public Params(params T[] values)
    {
        _values = values.ThrowIfNull();
        _isInitialized = true;
    }

    public ValueEnumerator<T> GetEnumerator()
    {
        var values = _values ?? Array.Empty<T>();

        return new ValueEnumerator<T>(values.GetEnumerator());
    }

    public bool IsEmpty => !_isInitialized;

    public IEnumerable<T> ToEnumerable() => new Enumerable<T>(GetEnumerator());
}

