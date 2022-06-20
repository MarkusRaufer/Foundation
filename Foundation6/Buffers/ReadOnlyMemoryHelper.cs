namespace Foundation.Buffers;

using Foundation.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

public static class ReadOnlyMemoryHelper
{
    public static string ToString<T>(IEnumerable<ReadOnlyMemory<T>> items, T separator)
    {
        var sb = new StringBuilder();
        foreach (var item in items.AfterEach(() => sb.Append(separator)))
        {
            sb.Append(item);
        }

        return sb.ToString();
    }

    public static string ToString<T>(IEnumerable<ReadOnlyMemory<T>> items, ReadOnlyMemory<T> separator)
    {
        var sb = new StringBuilder();
        foreach (var item in items.AfterEach(() => sb.Append(separator)))
        {
            sb.Append(item);
        }

        return sb.ToString();
    }
}

