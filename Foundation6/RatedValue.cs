using System.Diagnostics.CodeAnalysis;

namespace Foundation;

public record struct RatedValue<TValue, TRating>(TValue Value, TRating Rating)
{
    public bool IsEmpty() => 0 == GetHashCode();
}
