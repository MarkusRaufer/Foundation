namespace Foundation;

using System.Diagnostics.CodeAnalysis;

public static class MinMax
{
    public static MinMax<T> New<T>([DisallowNull] T min, [DisallowNull] T max) where T : notnull => new(min, max);
}

public record struct MinMax<T>([DisallowNull] T Min, [DisallowNull] T Max) where T : notnull;

