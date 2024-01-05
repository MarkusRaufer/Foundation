using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Foundation;

/// <summary>
/// Typed identifier.
/// </summary>
[Serializable]
public readonly struct Id
    : IComparable
    , IComparable<Id>
    , IEquatable<Id>
{
    private readonly IComparable _comparable;
    private readonly object _value;

    public static bool operator ==(Id lhs, Id rhs) => lhs.Equals(rhs);

    public static bool operator !=(Id lhs, Id rhs) => !(lhs == rhs);

    public static bool operator <(Id lhs, Id rhs) => -1 == lhs.CompareTo(rhs);

    public static bool operator <=(Id lhs, Id rhs) => 0 >= lhs.CompareTo(rhs);

    public static bool operator >(Id lhs, Id rhs) => 1 == lhs.CompareTo(rhs);

    public static bool operator >=(Id lhs, Id rhs) => 0 <= lhs.CompareTo(rhs);

    public int CompareTo(Id other) => _comparable.CompareNullableTo(other._value);

    public int CompareTo(object? obj) => obj is Id other ? CompareTo(other) : 1;

    public static readonly Id Empty;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Id other && Equals(other);

    public bool Equals(Id other)
    {
        if(IsEmpty) return other.IsEmpty;

        return _comparable.Equals(other._value);
    }

    public override int GetHashCode() => _value.GetNullableHashCode();

    [JsonIgnore]
    public readonly bool IsEmpty => _comparable is null;

    public static Id New() => New(Guid.NewGuid());

    public static Id New(object value) => new() { Value = value };

    public static Id New<T>(T value) where T : IComparable, IComparable<T> => new() { Value = value };

    public readonly override string ToString() => $"{_value}";

    public readonly Type Type { get; private init; }

    public readonly object Value
    {
        get => _value;
        init
        {
            if (value is not IComparable cmp)
            {
                throw new ArgumentException($"{nameof(Value)} must implement {nameof(IComparable)}", nameof(Value));
            }
            _comparable = cmp;
            Type = value.GetType();
            _value = value;
        }
    }
}
