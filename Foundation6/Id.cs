namespace Foundation;

/// <summary>
/// Typed identifier.
/// </summary>
public readonly struct Id
    : IComparable
    , IComparable<Id>
    , IEquatable<Id>
{
    private readonly int _hashCode;
    private readonly IComparable _value;

    internal Id(Type entityType, IComparable value)
    {
        _value = value.ThrowIfNull();
        EntityType = entityType.ThrowIfNull();

        _hashCode = System.HashCode.Combine(EntityType, _value);
    }

    public static bool operator ==(Id lhs, Id rhs) => lhs.Equals(rhs);

    public static bool operator !=(Id lhs, Id rhs) => !lhs.Equals(rhs);

    public static bool operator <(Id lhs, Id rhs) => -1 == lhs.CompareTo(rhs);

    public static bool operator <=(Id lhs, Id rhs) => 0 >= lhs.CompareTo(rhs);

    public static bool operator >(Id lhs, Id rhs) => 1 == lhs.CompareTo(rhs);

    public static bool operator >=(Id lhs, Id rhs) => 0 <= lhs.CompareTo(rhs);

    public int CompareTo(Id other)
    {
        if (IsEmpty) return other.IsEmpty ? 0 : -1;
        if (other.IsEmpty) return 1;

        if (!EntityType.Equals(other.EntityType)) return 1;

        return _value.CompareTo(other._value);
    }

    public int CompareTo(object? obj)
    {
        if (obj is Id other) return CompareTo(other);

        return 1;
    }

    public Type EntityType { get; }

    public override bool Equals(object? obj) => obj is Id other && Equals(other);

    public bool Equals(Id other) => _hashCode == other._hashCode && 0 == CompareTo(other);

    public override int GetHashCode() => _hashCode;

    public bool IsEmpty => EntityType is null;

    public static Id New() => New(Guid.NewGuid());

    public static Id New(byte[] value) => New(typeof(object), value);

    public static Id New(Type type, byte[] value) => new (type, ByteString.CopyFrom(value));

    public static Id New<TValue>(TValue value)
        where TValue : IComparable
        => New(typeof(object), value);

    public static Id New<TValue>(Type entityType, TValue value)
        where TValue : IComparable
        => new (entityType, value);

    public override string ToString() => IsEmpty ? "" : $"{_value}";
}

/// <summary>
/// Typed identifier. Boxing happens with value.
/// </summary>
/// <typeparam name="TEntityType"></typeparam>
public readonly struct Id<TEntityType>
    : IComparable
    , IComparable<Id<TEntityType>>
    , IEquatable<Id<TEntityType>>
    where TEntityType : notnull
{
    private readonly int _hashCode;
    private readonly IComparable _value;

    internal Id(TEntityType entityType, IComparable value)
    {
        _value = value.ThrowIfNull();

        EntityType = entityType;

        _hashCode = System.HashCode.Combine(EntityType, _value);
    }

    public static bool operator ==(Id<TEntityType> left, Id<TEntityType> right) => left.Equals(right);

    public static bool operator !=(Id<TEntityType> left, Id<TEntityType> right) => !(left == right);

    public static bool operator <(Id<TEntityType> left, Id<TEntityType> right) => -1 == left.CompareTo(right);

    public static bool operator <=(Id<TEntityType> left, Id<TEntityType> right) => 0 >= left.CompareTo(right);

    public static bool operator >(Id<TEntityType> left, Id<TEntityType> right) => 1 == left.CompareTo(right);

    public static bool operator >=(Id<TEntityType> left, Id<TEntityType> right) => 0 <= left.CompareTo(right);

    public int CompareTo(object? obj)
    {
        if (obj is Id<TEntityType> other) return CompareTo(other);

        return 1;
    }

    public int CompareTo(Id<TEntityType> other)
    {
        if (IsEmpty) return other.IsEmpty ? 0 : -1;
        if (other.IsEmpty) return 1;

        if (!EntityType.Equals(other.EntityType)) return 1;

        return _value.CompareTo(other._value);
    }

    public override bool Equals(object? obj) => obj is Id<TEntityType> other && Equals(other);

    public bool Equals(Id<TEntityType> other)
    {
        return _hashCode == other._hashCode &&  0 == CompareTo(other);
    }

    public override int GetHashCode() => _hashCode;

    public TEntityType EntityType { get; }

    public bool IsEmpty => EntityType is null;

    public override string ToString() => IsEmpty ? "" : $"{EntityType}: {_value}";
}
