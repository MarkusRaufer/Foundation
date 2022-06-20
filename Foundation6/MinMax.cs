namespace Foundation;

public static class MinMax
{
    public static MinMax<T> New<T>(T min, T max) where T : notnull => new(min, max);

    public static MinMax<int> New(System.Range range)
    {
        if (range.Start.IsFromEnd) throw new ArgumentException($"{range.Start}.IsFromEnd is not allowed");
        if (range.End.IsFromEnd) throw new ArgumentException($"{range.End}.IsFromEnd is not allowed");

        return new MinMax<int>(range.Start.Value, range.End.Value);
    }
}

public record struct MinMax<T>(T Min, T Max) where T : notnull;

