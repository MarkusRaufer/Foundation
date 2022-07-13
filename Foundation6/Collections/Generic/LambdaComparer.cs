namespace Foundation.Collections.Generic;

public class LambdaComparer<T> : IComparer<T>
{
    private readonly Func<T, T, int> _compare;

    public LambdaComparer(Func<T, T, int> compare)
    {
        _compare = compare.ThrowIfNull();
    }

    public int Compare(T? x, T? y)
    {
        return (x, y) switch
        {
            (null, null) => 0,
            (null, _) => -1,
            (_, null) => 1,
            (_, _) => _compare(x, y)
        };
    }
}
