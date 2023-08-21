namespace Foundation.Collections.Generic;

using System.Collections.Generic;
using System.Collections.ObjectModel;

public static class ReadOnlyCollection
{
    public static IReadOnlyCollection<T> New<T>(IEnumerable<T> items)
    {
        return new ReadOnlyCollection<T>(new List<T>(items));
    }

    public static IReadOnlyCollection<T> New<T>(IList<T> items)
    {
        return new ReadOnlyCollection<T>(items);
    }
}

