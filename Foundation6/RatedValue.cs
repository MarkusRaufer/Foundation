using System.Diagnostics.CodeAnalysis;

namespace Foundation;

public record struct RatedValue<TValue, TRating>([DisallowNull] TValue Value, [DisallowNull] TRating Rating)
{
    public bool IsEmpty() => 0 == GetHashCode();
}
