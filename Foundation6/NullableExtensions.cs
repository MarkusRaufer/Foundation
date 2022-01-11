namespace Foundation;

public static class NullableExtensions
{
    public static Opt<T> ToOpt<T>(this T? obj)
    {
        return (obj is null) ? Opt.None<T>() : Opt.Some(obj);
    }
}

