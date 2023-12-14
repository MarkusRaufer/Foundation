namespace Foundation;

using System.Globalization;
using System.Runtime.Serialization;

/// <summary>
/// This is a named identifier. The name and value are used for equality and comparison.
/// </summary>
[Serializable]
public readonly struct NamedId
    : IIdentifier
    , IComparable<NamedId>
    , IEquatable<NamedId>
    , ISerializable
{
    private readonly int _hashCode;
    private readonly bool _isInitialized;
    private readonly IComparable _value;

    #region ctors

    internal NamedId(string name, IComparable value)
    {
        Name = name.ThrowIfNullOrEmpty();
        _value = value.ThrowIfNull();

        _hashCode = System.HashCode.Combine(Name, _value);

        _isInitialized = true;
    }

    public NamedId(SerializationInfo info, StreamingContext context)
    {
        if (info.GetValue(nameof(Name), typeof(Type)) is not string name)
            throw new ArgumentNullException(nameof(Name));

        if (info.GetValue(nameof(_value), typeof(IComparable)) is not IComparable comparable)
            throw new ArgumentNullException(nameof(_value));

        _value = comparable;
        Name = name.ThrowIfNullOrEmpty();

        _hashCode = System.HashCode.Combine(Name, _value);
        _isInitialized = true;
    }
    #endregion ctors

    #region operator overloads

    public static implicit operator string(NamedId identifier) => identifier.ToString();

    public static bool operator ==(NamedId lhs, NamedId rhs) => lhs.Equals(rhs);

    public static bool operator !=(NamedId lhs, NamedId rhs) => !(lhs == rhs);

    public static bool operator <(NamedId lhs, NamedId rhs) => -1 == lhs.CompareTo(rhs);

    public static bool operator <=(NamedId lhs, NamedId rhs) => 0 >= lhs.CompareTo(rhs);

    public static bool operator >(NamedId lhs, NamedId rhs) => 1 == lhs.CompareTo(rhs);

    public static bool operator >=(NamedId lhs, NamedId rhs) => 0 <= lhs.CompareTo(rhs);
    #endregion operator overloads

    public int CompareTo(object? obj)
    {
        if (obj is NamedId other) return CompareTo(other);

        return 1;
    }

    public int CompareTo(NamedId other)
    {
        if (IsEmpty) return other.IsEmpty ? 0 : -1;
        if (other.IsEmpty) return 1;

        if (!Name.Equals(other.Name)) return 1;

        return _value.CompareTo(other._value);
    }

    public static readonly NamedId Empty = new();

    public override bool Equals(object? obj) => obj is NamedId other && Equals(other);

    public bool Equals(NamedId other)
    {
        return _hashCode == other._hashCode  && 0 == CompareTo(other);
    }

    public override int GetHashCode() => _hashCode;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(Name), Name);
        info.AddValue(nameof(_value), _value);
    }

    public bool IsEmpty => !_isInitialized;

    public string Name { get; }

    public static NamedId New(string name) => new (name, Guid.NewGuid());

    public static NamedId New(string name, IComparable value) => new (name, value);

    public static NamedId New(string name, byte[] value) => new (name, ByteString.CopyFrom(value));

    public static NamedId<T> New<T>(string name, T value) where T : struct, IComparable<T>, IEquatable<T>
        => new(name, value);

    public override string ToString() => $"{Name}:{_value}";

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var provider = formatProvider ?? CultureInfo.InvariantCulture;

        return string.IsNullOrEmpty(format) ? $"{Name}:{_value}" : string.Format(provider, format, _value);
    }

    public string ValueToString() => $"{_value}";

    public string ValueToString(string? format, IFormatProvider? formatProvider)
    {
        var provider = formatProvider ?? CultureInfo.InvariantCulture;

        return string.IsNullOrEmpty(format) ? $"{_value}" : string.Format(provider, format, _value);
    }
}

/// <summary>
/// This is a named identifier. The name and value are used for equality and comparison.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public readonly struct NamedId<T>
    : IComparable<NamedId<T>>
    , IEquatable<NamedId<T>>
    , ISerializable
    where T : struct, IComparable<T>, IEquatable<T>
{
    private readonly int _hashCode;
    private readonly bool _isInitialized;
    private readonly T _value;

    #region ctors

    internal NamedId(string name, T value)
    {
        Name = name.ThrowIfNullOrEmpty();
        _value = value.ThrowIfNull();

        _hashCode = System.HashCode.Combine(Name, value);

        _isInitialized = true;
    }

    public NamedId(SerializationInfo info, StreamingContext context)
    {
        if (info.GetValue(nameof(Name), typeof(T)) is not string name)
            throw new ArgumentNullException(nameof(_value));

        if (info.GetValue(nameof(_value), typeof(T)) is not T value)
            throw new ArgumentNullException(nameof(_value));

        Name = name;
        _value = value;

        _hashCode = System.HashCode.Combine(Name, _value);
        _isInitialized = true;
    }
    #endregion ctors

    #region operator overloads
    public static implicit operator string(NamedId<T> identifier) => $"{identifier}";

    public static bool operator ==(NamedId<T> lhs, NamedId<T> rhs) => lhs.Equals(rhs);

    public static bool operator !=(NamedId<T> lhs, NamedId<T> rhs) => !(lhs == rhs);

    public static bool operator <(NamedId<T> lhs, NamedId<T> rhs) => -1 == lhs.CompareTo(rhs);

    public static bool operator <=(NamedId<T> lhs, NamedId<T> rhs) => 0 >= lhs.CompareTo(rhs);

    public static bool operator >(NamedId<T> lhs, NamedId<T> rhs) => 1 == lhs.CompareTo(rhs);

    public static bool operator >=(NamedId<T> lhs, NamedId<T> rhs) => 0 <= lhs.CompareTo(rhs);
    #endregion

    public int CompareTo(object? obj)
    {
        if (obj is NamedId<T> other) return CompareTo(other);

        return 1;
    }

    public int CompareTo(NamedId<T> other)
    {
        if (IsEmpty) return other.IsEmpty ? 0 : -1;
        if (other.IsEmpty) return 1;

        if (!Name.Equals(other.Name)) return 1;

        return _value.CompareTo(other._value);
    }

    public static NamedId<T> Empty => new();

    public override bool Equals(object? obj) => obj is NamedId<T> other && Equals(other);

    public bool Equals(NamedId<T> other)
    {
        return _hashCode == other._hashCode && 0 == CompareTo(other);
    }

    public override int GetHashCode() => _hashCode;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(Name), Name);
        info.AddValue(nameof(_value), _value);
    }

    public bool IsEmpty => !_isInitialized;

    public string Name { get; }

    public override string ToString() => $"{Name}:{_value}";

    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        var provider = formatProvider ?? CultureInfo.InvariantCulture;

        return string.IsNullOrEmpty(format) ? $"{Name}:{_value}" : string.Format(provider, format, _value);
    }
}

