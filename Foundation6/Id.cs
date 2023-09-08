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

    public static Id New<TValue>(TValue value)
         where TValue : struct, IComparable, IComparable<TValue>, IEquatable<TValue>, IFormattable
        => New(typeof(object), value);

    public static Id New<TEntity>(byte[] value) => new (typeof(object), ByteString.CopyFrom(value));

    public static Id New<TValue>(Type entityType, TValue value)
         where TValue : struct, IComparable, IComparable<TValue>, IEquatable<TValue>, IFormattable
        => new(entityType, value);

    //public static Id<TEntity> New<TEntity>(byte[] value) => new (ByteString.CopyFrom(value));

    /// <summary>
    /// Attention boxing happens with value.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Id<TEntity> New<TEntity>(IComparable value) where TEntity : notnull
         => new(value);

    /// <summary>
    /// No boxing happens.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Id<TEntity> New<TEntity, TValue>(TValue value)
        where TValue : struct, IComparable, IComparable<TValue>, IEquatable<TValue>, IFormattable
        where TEntity : notnull
        => new(value);

    public override string ToString() => IsEmpty ? "" : $"{_value}";
}

/// <summary>
/// Typed identifier. Boxing happens with value.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public readonly struct Id<TEntity>
    : IComparable
    , IComparable<Id<TEntity>>
    , IEquatable<Id<TEntity>>
    where TEntity : notnull
{
    private readonly int _hashCode;
    private readonly IComparable _value;

    internal Id(IComparable value)
    {
        _value = value.ThrowIfNull();

        EntityType = typeof(TEntity);

        _hashCode = System.HashCode.Combine(EntityType, _value);
    }

    public static bool operator ==(Id<TEntity> left, Id<TEntity> right) => left.Equals(right);

    public static bool operator !=(Id<TEntity> left, Id<TEntity> right) => !(left == right);

    public static bool operator <(Id<TEntity> left, Id<TEntity> right) => -1 == left.CompareTo(right);

    public static bool operator <=(Id<TEntity> left, Id<TEntity> right) => 0 >= left.CompareTo(right);

    public static bool operator >(Id<TEntity> left, Id<TEntity> right) => 1 == left.CompareTo(right);

    public static bool operator >=(Id<TEntity> left, Id<TEntity> right) => 0 <= left.CompareTo(right);

    public int CompareTo(object? obj)
    {
        if (obj is Id<TEntity> other) return CompareTo(other);

        return 1;
    }

    public int CompareTo(Id<TEntity> other)
    {
        if (IsEmpty) return other.IsEmpty ? 0 : -1;
        if (other.IsEmpty) return 1;

        if (!EntityType.Equals(other.EntityType)) return 1;

        return _value.CompareTo(other._value);
    }

    public override bool Equals(object? obj) => obj is Id<TEntity> other && Equals(other);

    public bool Equals(Id<TEntity> other)
    {
        return _hashCode == other._hashCode &&  0 == CompareTo(other);
    }

    public override int GetHashCode() => _hashCode;

    public Type EntityType { get; }

    public bool IsEmpty => EntityType is null;

    public override string ToString() => IsEmpty ? "" : $"{_value}";
}
