using System.Runtime.CompilerServices;

namespace Foundation;

public static class NullableExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> ToOption<T>(this T? obj) => Option.Maybe(obj);
}

