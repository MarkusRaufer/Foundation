namespace Foundation;

using System.Diagnostics.CodeAnalysis;
using System.Text;

public struct Id
    : IComparable
    , IComparable<Id>
    , IEquatable<Id>
    , IIncompatibleComparable
    , IUninitializedComparable
{
    private readonly int _hashCode;
    internal Id(Type? entityType, [DisallowNull] IComparable value)
    {
        Value = value.ThrowIfNull();
        EntityType = entityType ?? typeof(object);

        CompareValueIfEmpty = -1;
        CompareValueIfIncompatible = 1;

        _hashCode = HashCode.FromObject(EntityType, Value);
    }

    public static bool operator ==(Id lhs, Id rhs) => lhs.Equals(rhs);

    public static bool operator !=(Id lhs, Id rhs) => !lhs.Equals(rhs);

    public static bool operator <(Id lhs, Id rhs) => -1 == lhs.CompareTo(rhs);

    public static bool operator <=(Id lhs, Id rhs) => 0 >= lhs.CompareTo(rhs);

    public static bool operator >(Id lhs, Id rhs) => 1 == lhs.CompareTo(rhs);

    public static bool operator >=(Id lhs, Id rhs) => 0 <= lhs.CompareTo(rhs);

    public int CompareTo(Id other)
    {
        if (IsEmpty) return other.IsEmpty ? 0 : CompareValueIfEmpty;
        if (!EntityType.Equals(other.EntityType)) return CompareValueIfIncompatible;

        if (other.IsEmpty) return CompareValueIfEmpty * -1;

        return Value.CompareTo(other.Value);
    }

    public int CompareTo(object? obj)
    {
        if (obj is Id other) return CompareTo(other);

        return CompareValueIfIncompatible;
    }

    /// <summary>
    /// With this value you can decide if empty Ids are smaller or greater. The value can be -1 or 1. 
    /// Means that in a sorted list empty Ids appear at the beginning or at the end of the list. 
    /// </summary>
    public int CompareValueIfEmpty { get; set; }

    /// <summary>
    /// With this value you can decide if incompatible Ids are smaller or greater. The value can be -1 or 1. 
    /// Means that in a sorted list incompatible Ids appear at the beginning or at the end of the list.
    /// </summary>
    public int CompareValueIfIncompatible { get; set; }

    public Type EntityType { get; }

    public override bool Equals(object? obj) => obj is Id other && Equals(other);

    public bool Equals(Id other) => CompareTo(other) == 0;

    public override int GetHashCode() => _hashCode;

    public bool IsEmpty => EntityType is null;

    public static Id New(ByteString value) => New(typeof(object), value);
    public static Id New(byte[] value) => New(typeof(object), value);
    public static Id New(Guid value) => New(typeof(object), value);
    public static Id New(string value) => New(typeof(object), value, Encoding.Unicode);
    public static Id New(string value, Encoding encoding) => New(typeof(object), value, encoding);

    public static Id New(Type entityType, ByteString value) => new Id(entityType, value);
    public static Id New(Type entityType, byte[] value) => new Id(entityType, ByteString.CopyFrom(value));
    public static Id New(Type entityType, Guid value) => New(entityType, value.ToByteArray());
    public static Id New(Type entityType, string value) => New(entityType, value, Encoding.Unicode);
    public static Id New(Type entityType, string value, Encoding encoding) => new(entityType, ByteString.FromString(value, encoding));

    public static Id New<TValue>(TValue value)
         where TValue : struct, IComparable, IComparable<TValue>, IConvertible, IEquatable<TValue>, IFormattable
        => New(typeof(object), value);

    public static Id New<TValue>(Type? entityType, TValue value)
         where TValue : struct, IComparable, IComparable<TValue>, IConvertible, IEquatable<TValue>, IFormattable
        => new(entityType, value);

    public static Id<TEntity> New<TEntity>(ByteString value) => new(value);
    public static Id<TEntity> New<TEntity>(byte[] value) => new(ByteString.CopyFrom(value));
    public static Id<TEntity> New<TEntity>(Guid value) => New<TEntity>(value.ToByteArray());
    public static Id<TEntity> New<TEntity>(string value) => New<TEntity>(value, Encoding.Unicode);
    public static Id<TEntity> New<TEntity>(string value, Encoding encoding) => new(ByteString.FromString(value, encoding));

    /// <summary>
    /// Attention boxing happens with value.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Id<TEntity> New<TEntity>(IComparable value) => new(value);

    /// <summary>
    /// No boxing happens.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Id<TEntity> New<TEntity, TValue>(TValue value)
         where TValue : struct, IComparable, IComparable<TValue>, IConvertible, IEquatable<TValue>, IFormattable => new(value);

    public override string ToString() => IsEmpty ? "" : $"{Value}";

    public IComparable Value { get; }
}

/// <summary>
/// Boxing happens with value.
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public struct Id<TEntity>
    : IComparable
    , IComparable<Id<TEntity>>
    , IEquatable<Id<TEntity>>
    , IIncompatibleComparable
    , IUninitializedComparable
{
    private readonly int _hashCode;

    internal Id(IComparable value)
    {
        Value = value.ThrowIfNull();
        CompareValueIfEmpty = -1;
        CompareValueIfIncompatible = 1;

        _hashCode = HashCode.FromObject(typeof(TEntity), Value);
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

        return CompareValueIfIncompatible;
    }

    public int CompareTo(Id<TEntity> other)
    {
        if (IsEmpty) return other.IsEmpty ? 0 : CompareValueIfEmpty;
        if (!EntityType.Equals(other.EntityType)) return CompareValueIfIncompatible;

        if (other.IsEmpty) return CompareValueIfEmpty * -1;
        return Value.CompareTo(other.Value);
    }

    public override bool Equals(object? obj) => obj is Id<TEntity> other && Equals(other);

    public bool Equals(Id<TEntity> other) => 0 == CompareTo(other);

    public int CompareValueIfEmpty { get; set; }

    public int CompareValueIfIncompatible { get; set; }

    public override int GetHashCode() => _hashCode;

    public Type EntityType => typeof(TEntity);

    public bool IsEmpty => EntityType is null;

    public override string ToString() => IsEmpty ? "" : $"{Value}";

    public IComparable Value { get; }
}

