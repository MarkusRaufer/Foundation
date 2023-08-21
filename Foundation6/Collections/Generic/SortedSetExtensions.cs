using System.Runtime.CompilerServices;

namespace Foundation.Collections.Generic;

public static class SortedSetExtensions
{
    public static SortedSet<T> ThrowIfEmpty<T>(this SortedSet<T> set, [CallerArgumentExpression("set")] string paramName = "")
        => 0 < set.Count
           ? set
           : throw new ArgumentOutOfRangeException($"{paramName} must not be empty");

    public static SortedSet<T> ThrowIfNull<T>(this SortedSet<T> set, [CallerArgumentExpression("set")] string paramName = "")
        => set ?? throw new ArgumentException($"{paramName} must not be empty");

    public static SortedSet<T> ThrowIfNullOrEmpty<T>(this SortedSet<T> set, [CallerArgumentExpression("set")] string paramName = "")
        => ThrowIfNull(set, paramName).ThrowIfEmpty(paramName);
}
