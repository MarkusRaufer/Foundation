using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Generic;

public static class RingEnumerable
{
    public static RingEnumerable<T> Create<T>([DisallowNull] IEnumerable<T> enumerable, bool infinite = false, int index = 0)
    {
        return new RingEnumerable<T>(new RingEnumerator<T>(enumerable, infinite, index));
    }

    public static RingEnumerable<T> Create<T>([DisallowNull] RingEnumerator<T> enumerator)
    {
        return new RingEnumerable<T>(enumerator);
    }
}

public class RingEnumerable<T> : IEnumerable<T?>
{
    private readonly RingEnumerator<T> _enumerator;

    public RingEnumerable([DisallowNull] RingEnumerator<T> enumerator)
    {
        _enumerator = enumerator.ThrowIfNull();
    }

    public IEnumerator<T?> GetEnumerator() => _enumerator;

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

