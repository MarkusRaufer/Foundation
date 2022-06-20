namespace Foundation;

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Serialization;

/// <summary>
/// This identifier is ideal for rapid prototyping, if you do not want to commit to a specific type yet.
/// </summary>
[Serializable]
public readonly struct Identifier
    : IIdentifier
    , IComparable<Identifier>
    , IEquatable<Identifier>
    , IUninitializedComparable
    , ISerializable
{
    private readonly bool _isInitialized;
    #region ctors

    internal Identifier(IComparable value)
    {
        Value = value.ThrowIfNull();

        CompareValueIfEmpty = -1;
        _isInitialized = true;
    }

    public Identifier(SerializationInfo info, StreamingContext context)
    {
        CompareValueIfEmpty = (info.GetValue(nameof(CompareValueIfEmpty), typeof(int)) is int compareValue)
            ? compareValue
            : -1;

        if (info.GetValue(nameof(Value), typeof(IComparable)) is not IComparable comparable)
            throw new ArgumentNullException(nameof(Value));

        Value = comparable;
        _isInitialized = true;
    }
    #endregion ctors

    #region operator overloads

    public static implicit operator string(Identifier identifier) => identifier.ToString();

    public static bool operator ==(Identifier lhs, Identifier rhs) => lhs.Equals(rhs);

    public static bool operator !=(Identifier lhs, Identifier rhs) => !(lhs == rhs);

    public static bool operator <(Identifier lhs, Identifier rhs) => -1 == lhs.CompareTo(rhs);

    public static bool operator <=(Identifier lhs, Identifier rhs) => 0 >= lhs.CompareTo(rhs);

    public static bool operator >(Identifier lhs, Identifier rhs) => 1 == lhs.CompareTo(rhs);

    public static bool operator >=(Identifier lhs, Identifier rhs) => 0 <= lhs.CompareTo(rhs);
    #endregion operator overloads

    public int CompareTo(object? obj)
    {
        if (obj is Identifier other) return CompareTo(other);

        return CompareValueIfEmpty;
    }

    public int CompareTo(Identifier other)
    {
        if (IsEmpty) return other.IsEmpty ? 0 : CompareValueIfEmpty;
        if (other.IsEmpty) return CompareValueIfEmpty * -1;

        return Value.CompareTo(other.Value);
    }

    public int CompareValueIfEmpty { get; init; }

    public static readonly Identifier Empty = new();

    public override bool Equals(object? obj) => obj is Identifier other && Equals(other);

    public bool Equals(Identifier other)
    {
        if (IsEmpty) return other.IsEmpty;

        return !other.IsEmpty && Value.Equals(other.Value);
    }

    public override int GetHashCode() => IsEmpty ? 0 : Value.GetHashCode();

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(CompareValueIfEmpty), CompareValueIfEmpty);
        info.AddValue(nameof(Value), Value);
    }

    public bool IsEmpty => !_isInitialized;

    public static Identifier New<T>(T value)
        where T : IComparable
    {
        return new Identifier(value);
    }

    public override string ToString() => $"{Value}";

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var provider = formatProvider ?? CultureInfo.InvariantCulture;

        return string.IsNullOrEmpty(format) ? $"{Value}" : string.Format(provider, format, Value);
    }

    public T ToValue<T>() where T : struct
    {
        if (IsEmpty) return default;
        return (T)Value;
    }

    [NotNull]
    public IComparable Value { get; }
}

[Serializable]
public struct Identifier<T>
    : IIdentifier<T>
    , IComparable<Identifier<T>>
    , IEquatable<Identifier<T>>
    , IUninitializedComparable
    , ISerializable
    where T : struct, IComparable<T>, IEquatable<T>
{
    private readonly bool _isInitialized;

    #region ctors

    internal Identifier(T value)
    {
        Value = value.ThrowIfNull();
        CompareValueIfEmpty = -1;
        _isInitialized = true;
    }

    public Identifier(SerializationInfo info, StreamingContext context)
    {
        CompareValueIfEmpty = (info.GetValue(nameof(CompareValueIfEmpty), typeof(int)) is int compareValue)
            ? compareValue
            : -1;

        if (info.GetValue(nameof(Value), typeof(T)) is not T value)
            throw new ArgumentNullException(nameof(Value));

        Value = value;
        _isInitialized = true;
    }
    #endregion ctors

    #region operator overloads
    public static implicit operator string(Identifier<T> identifier) => $"{identifier}";

    public static bool operator ==(Identifier<T> lhs, Identifier<T> rhs) => lhs.Equals(rhs);

    public static bool operator !=(Identifier<T> lhs, Identifier<T> rhs) => !(lhs == rhs);

    public static bool operator <(Identifier<T> lhs, Identifier<T> rhs) => 1 == lhs.CompareTo(rhs);

    public static bool operator <=(Identifier<T> lhs, Identifier<T> rhs) => 0 >= lhs.CompareTo(rhs);

    public static bool operator >(Identifier<T> lhs, Identifier<T> rhs) => 1 == lhs.CompareTo(rhs);

    public static bool operator >=(Identifier<T> lhs, Identifier<T> rhs) => 0 <= lhs.CompareTo(rhs);
    #endregion

    public int CompareTo(object? obj)
    {
        if (obj is Identifier<T> other) return CompareTo(other);

        return CompareValueIfEmpty;
    }

    public int CompareTo(Identifier<T> other)
    {
        if (IsEmpty) return other.IsEmpty ? 0 : CompareValueIfEmpty;
        if (other.IsEmpty) return CompareValueIfEmpty * -1;

        return Value.CompareTo(other.Value);
    }

    public int CompareValueIfEmpty { get; init; }

    public static Identifier<T> Empty => new();

    public override bool Equals(object? obj) => obj is Identifier<T> other && Equals(other);

    public bool Equals(Identifier<T> other)
    {
        if (IsEmpty) return other.IsEmpty;

        return !other.IsEmpty && Value.Equals(other.Value);
    }

    public override int GetHashCode() => IsEmpty ? 0 : Value.GetHashCode();

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(CompareValueIfEmpty), CompareValueIfEmpty);
        info.AddValue(nameof(Value), Value);
    }

    public bool IsEmpty => !_isInitialized;

    public static Identifier<T> New(T value) => new(value);

    public override string ToString() => $"{Value}";

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var provider = formatProvider ?? CultureInfo.InvariantCulture;

        return string.IsNullOrEmpty(format) ? $"{Value}" : string.Format(provider, format, Value);
    }

    public T Value { get; }
}

[DebuggerDisplay("Entity: {EntityType}, Value: {Value}")]
[Serializable]
public struct Identifier<TEntityType, T>
    : IIdentifier<T>
    , IComparable<Identifier<TEntityType, T>>
    , IEquatable<Identifier<TEntityType, T>>
    , IIncompatibleComparable
    , IUninitializedComparable
    , ISerializable
    where TEntityType : IEquatable<TEntityType>
    where T : struct, IComparable<T>, IEquatable<T>
{
    private int _hashCode;
    private readonly bool _isInitialized;

    #region ctors

    internal Identifier(TEntityType entityType, T value)
    {
        EntityType = entityType.ThrowIfNull();
        Value = value.ThrowIfNull();

        CompareValueIfEmpty = -1;
        CompareValueIfIncompatible = 1;

        _hashCode = System.HashCode.Combine(EntityType, Value);

        _isInitialized = true;
    }

    public Identifier(SerializationInfo info, StreamingContext context)
    {
        CompareValueIfEmpty = (info.GetValue(nameof(CompareValueIfEmpty), typeof(int)) is int ifEmpty)
            ? ifEmpty
            : -1;

        CompareValueIfIncompatible = (info.GetValue(nameof(CompareValueIfIncompatible), typeof(int)) is int ifIncompatible)
            ? ifIncompatible
            : 1;

        if (info.GetValue(nameof(EntityType), typeof(TEntityType)) is not TEntityType entityType)
            throw new ArgumentNullException(nameof(EntityType));

        EntityType = entityType;

        if (info.GetValue(nameof(Value), typeof(T)) is not T value)
            throw new ArgumentNullException(nameof(Value));

        Value = value;

        _hashCode = System.HashCode.Combine(EntityType, Value);
        _isInitialized = true;
    }
    #endregion ctors

    #region operator overloads
    public static implicit operator string(Identifier<TEntityType, T> identifier) => $"{identifier}";

    public static bool operator ==(Identifier<TEntityType, T> lhs, Identifier<TEntityType, T> rhs) => lhs.Equals(rhs);

    public static bool operator !=(Identifier<TEntityType, T> lhs, Identifier<TEntityType, T> rhs) => !(lhs == rhs);

    public static bool operator <(Identifier<TEntityType, T> lhs, Identifier<TEntityType, T> rhs) => -1 == lhs.CompareTo(rhs);

    public static bool operator <=(Identifier<TEntityType, T> lhs, Identifier<TEntityType, T> rhs) => 0 >= lhs.CompareTo(rhs);

    public static bool operator >(Identifier<TEntityType, T> lhs, Identifier<TEntityType, T> rhs) => 1 == lhs.CompareTo(rhs);

    public static bool operator >=(Identifier<TEntityType, T> lhs, Identifier<TEntityType, T> rhs) => 0 <= lhs.CompareTo(rhs);
    #endregion

    public int CompareTo(object? obj)
    {
        if (obj is Identifier<TEntityType, T> other) return CompareTo(other);

        return -1;
    }

    public int CompareTo(Identifier<TEntityType, T> other)
    {
        if (IsEmpty) return other.IsEmpty ? 0 : CompareValueIfEmpty;
        if (other.IsEmpty) return CompareValueIfEmpty * -1;

        if (EntityType.Equals(other.EntityType)) return CompareValueIfIncompatible;

        return Value.CompareTo(other.Value);
    }

    public int CompareValueIfEmpty { get; init; }

    public int CompareValueIfIncompatible { get; init; }

    public static Identifier<TEntityType, T> Empty => new();

    public TEntityType EntityType { get; }

    public override bool Equals(object? obj) => obj is Identifier<TEntityType, T> other && Equals(other);

    public bool Equals(Identifier<TEntityType, T> other)
    {
        if (IsEmpty) return other.IsEmpty;

        return !other.IsEmpty && Value.Equals(other.Value);
    }

    public override int GetHashCode() => _hashCode;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(CompareValueIfEmpty), CompareValueIfEmpty);
        info.AddValue(nameof(CompareValueIfIncompatible), CompareValueIfIncompatible);

        info.AddValue(nameof(EntityType), EntityType);
        info.AddValue(nameof(Value), Value);
    }

    public bool IsEmpty => !_isInitialized;

    public static Identifier<TEntityType, T> New(TEntityType entityType, T value) => new(entityType, value);

    public override string ToString() => $"Entity: {EntityType}, Value: {Value}";

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var provider = formatProvider ?? CultureInfo.InvariantCulture;

        return string.IsNullOrEmpty(format)
            ? $"Entity: {EntityType}, Value: {Value}"
            : string.Format(provider, format, Value);
    }

    public T Value { get; }
}


