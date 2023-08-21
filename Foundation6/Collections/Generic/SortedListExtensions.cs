using System.Runtime.CompilerServices;

namespace Foundation.Collections.Generic;

public static class SortedListExtensions
{
    public static SortedList<T> ThrowIfEmpty<T>(this SortedList<T> list, [CallerArgumentExpression("list")] string paramName = "")
        => 0 < list.Count
           ? list
           : throw new ArgumentOutOfRangeException($"{paramName} must not be empty");

    public static SortedList<T> ThrowIfNull<T>(this SortedList<T> list, [CallerArgumentExpression("list")] string paramName = "")
        => list ?? throw new ArgumentException($"{paramName} must not be empty");

    public static SortedList<T> ThrowIfNullOrEmpty<T>(this SortedList<T> list, [CallerArgumentExpression("list")] string paramName = "")
        => ThrowIfNull(list, paramName).ThrowIfEmpty(paramName);
}
