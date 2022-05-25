namespace Foundation;

public static class NullableExtensions
{
    public static Opt<T> ToOpt<T>(this T? obj)
    {
        return Opt.Maybe(obj);
    }
}

