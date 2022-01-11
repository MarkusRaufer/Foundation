namespace Foundation.Collections.Generic;

using System.Collections;
using System.Diagnostics.CodeAnalysis;

public static class ReadOnlyCollection
{
    public static IReadOnlyCollection<T> New<T>([DisallowNull] ICollection<T> items)
    {
        return new ReadOnlyCollection<T>(items, items.Count);
    }

    public static IReadOnlyCollection<T> New<T>([DisallowNull] IEnumerable<T> items)
    {
        return new ReadOnlyCollection<T>(items);
    }

    public static IReadOnlyCollection<T> New<T>([DisallowNull] IEnumerable<T> items, int count)
    {
        return new ReadOnlyCollection<T>(items, count);
    }
}

public class ReadOnlyCollection<T> : IReadOnlyCollection<T>
{
    private readonly IEnumerable<T> _items;

    public ReadOnlyCollection([DisallowNull] IEnumerable<T> items)
        : this(items, items.Count())
    {
    }

    public ReadOnlyCollection(IEnumerable<T> items, int count)
    {
        _items = items ?? throw new ArgumentNullException(nameof(items));
        Count = count;
    }

    public int Count { get; private set; }

    public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

