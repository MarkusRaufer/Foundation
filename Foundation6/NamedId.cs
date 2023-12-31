namespace Foundation;

using Foundation.Text.Json.Serialization;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

///// <summary>
///// This is a named identifier. The name and value are used for equality and comparison.
///// </summary>
//[Serializable]
//public readonly struct NamedId
//    : IIdentifier
//    , IComparable<NamedId>
//    , IEquatable<NamedId>
//    , ISerializable
//{
//    private readonly int _hashCode;
//    private readonly bool _isInitialized;

//    #region ctors

//    internal NamedId(string name, ByteString value)
//    {
//        Name = name.ThrowIfNullOrEmpty();
//        Value = value.ThrowIfNull();

//        _hashCode = System.HashCode.Combine(Name, Value);

//        _isInitialized = true;
//    }

//    public NamedId(SerializationInfo info, StreamingContext context)
//    {
//        if (info.GetValue(nameof(Name), typeof(Type)) is not string name)
//            throw new ArgumentNullException(nameof(Name));

//        if (info.GetValue(nameof(Value), typeof(ByteString)) is not ByteString value)
//            throw new ArgumentNullException(nameof(Value));

//        Value = value;
//        Name = name.ThrowIfNullOrEmpty();

//        _hashCode = System.HashCode.Combine(Name, Value);
//        _isInitialized = true;
//    }
//    #endregion ctors

//    #region operator overloads

//    public static implicit operator string(NamedId identifier) => identifier.ToString();

//    public static bool operator ==(NamedId lhs, NamedId rhs) => lhs.Equals(rhs);

//    public static bool operator !=(NamedId lhs, NamedId rhs) => !(lhs == rhs);

//    public static bool operator <(NamedId lhs, NamedId rhs) => -1 == lhs.CompareTo(rhs);

//    public static bool operator <=(NamedId lhs, NamedId rhs) => 0 >= lhs.CompareTo(rhs);

//    public static bool operator >(NamedId lhs, NamedId rhs) => 1 == lhs.CompareTo(rhs);

//    public static bool operator >=(NamedId lhs, NamedId rhs) => 0 <= lhs.CompareTo(rhs);
//    #endregion operator overloads

//    public int CompareTo(object? obj)
//    {
//        if (obj is NamedId other) return CompareTo(other);

//        return 1;
//    }

//    public int CompareTo(NamedId other)
//    {
//        if (IsEmpty) return other.IsEmpty ? 0 : -1;
//        if (other.IsEmpty) return 1;

//        if (!Name.Equals(other.Name)) return 1;

//        return Value.CompareTo(other.Value);
//    }

//    public static readonly NamedId Empty = new();

//    public override bool Equals(object? obj) => obj is NamedId other && Equals(other);

//    public bool Equals(NamedId other)
//    {
//        return _hashCode == other._hashCode && 0 == CompareTo(other);
//    }

//    public override int GetHashCode() => _hashCode;

//    public void GetObjectData(SerializationInfo info, StreamingContext context)
//    {
//        info.AddValue(nameof(Name), Name);
//        info.AddValue(nameof(Value), Value);
//    }

//    public bool IsEmpty => !_isInitialized;

//    public string Name { get; }

//    public static NamedId New(string name) => new(name, Guid.NewGuid().ToByteArray());

//    public static NamedId New(string name, byte[] value) => new(name, ByteString.CopyFrom(value));

//    public static NamedId New(string name, decimal value) => new(name, value.ToByteArray());

//    public static NamedId New(string name, double value) => new(name, BitConverter.GetBytes(value));

//    public static NamedId New(string name, float value) => new(name, BitConverter.GetBytes(value));

//    public static NamedId New(string name, Guid value) => new(name, value.ToByteArray());

//    public static NamedId New(string name, int value) => new(name, BitConverter.GetBytes(value));

//    public static NamedId New(string name, long value) => new(name, BitConverter.GetBytes(value));

//    public static NamedId New(string name, string value) => new(name, value.ThrowIfNullOrWhiteSpace().ToByteArray());

//    public static NamedId New(string name, ulong value) => new(name, BitConverter.GetBytes(value));

//    public static NamedId New(string name, ushort value) => new(name, BitConverter.GetBytes(value));

//    public static NamedId<T> New<T>(string name, T value) where T : struct, IComparable<T>, IEquatable<T>
//        => new(name, value);

//    public override string ToString() => $"{Name}:{Value}";

//    public string ToString(string? format, IFormatProvider? formatProvider)
//    {
//        var provider = formatProvider ?? CultureInfo.InvariantCulture;

//        return string.IsNullOrEmpty(format) ? $"{Name}:{Value}" : string.Format(provider, format, Value);
//    }

//    [JsonConverter(typeof(ByteString))]
//    public ByteString Value { get; }

//    public string ValueToString() => $"{Value}";

//    public string ValueToString(string? format, IFormatProvider? formatProvider)
//    {
//        var provider = formatProvider ?? CultureInfo.InvariantCulture;

//        return string.IsNullOrEmpty(format) ? $"{Value}" : string.Format(provider, format, Value);
//    }
//}

///// <summary>
///// This is a named identifier. The name and value are used for equality and comparison.
///// </summary>
///// <typeparam name="T"></typeparam>
//[Serializable]
//public readonly struct NamedId<T>
//    : IComparable<NamedId<T>>
//    , IEquatable<NamedId<T>>
//    , ISerializable
//    where T : struct, IComparable<T>, IEquatable<T>
//{
//    private readonly int _hashCode;
//    private readonly bool _isInitialized;

//    #region ctors

//    internal NamedId(string name, T value)
//    {
//        Name = name.ThrowIfNullOrEmpty();
//        Value = value.ThrowIfNull();

//        _hashCode = System.HashCode.Combine(Name, Value);

//        _isInitialized = true;
//    }

//    public NamedId(SerializationInfo info, StreamingContext context)
//    {
//        if (info.GetValue(nameof(Name), typeof(T)) is not string name)
//            throw new ArgumentNullException(nameof(Name));

//        if (info.GetValue(nameof(Value), typeof(T)) is not T value)
//            throw new ArgumentNullException(nameof(Value));

//        Name = name;
//        Value = value;

//        _hashCode = System.HashCode.Combine(Name, Value);
//        _isInitialized = true;
//    }
//    #endregion ctors

//    #region operator overloads
//    public static implicit operator string(NamedId<T> identifier) => $"{identifier}";

//    public static bool operator ==(NamedId<T> lhs, NamedId<T> rhs) => lhs.Equals(rhs);

//    public static bool operator !=(NamedId<T> lhs, NamedId<T> rhs) => !(lhs == rhs);

//    public static bool operator <(NamedId<T> lhs, NamedId<T> rhs) => -1 == lhs.CompareTo(rhs);

//    public static bool operator <=(NamedId<T> lhs, NamedId<T> rhs) => 0 >= lhs.CompareTo(rhs);

//    public static bool operator >(NamedId<T> lhs, NamedId<T> rhs) => 1 == lhs.CompareTo(rhs);

//    public static bool operator >=(NamedId<T> lhs, NamedId<T> rhs) => 0 <= lhs.CompareTo(rhs);
//    #endregion

//    public int CompareTo(object? obj)
//    {
//        if (obj is NamedId<T> other) return CompareTo(other);

//        return 1;
//    }

//    public int CompareTo(NamedId<T> other)
//    {
//        if (IsEmpty) return other.IsEmpty ? 0 : -1;
//        if (other.IsEmpty) return 1;

//        if (!Name.Equals(other.Name)) return 1;

//        return Value.CompareTo(other.Value);
//    }

//    public static NamedId<T> Empty => new();

//    public override bool Equals(object? obj) => obj is NamedId<T> other && Equals(other);

//    public bool Equals(NamedId<T> other)
//    {
//        return _hashCode == other._hashCode && 0 == CompareTo(other);
//    }

//    public override int GetHashCode() => _hashCode;

//    public void GetObjectData(SerializationInfo info, StreamingContext context)
//    {
//        info.AddValue(nameof(Name), Name);
//        info.AddValue(nameof(Value), Value);
//    }

//    public bool IsEmpty => !_isInitialized;

//    public string Name { get; }

//    public override string ToString() => $"{Name}:{Value}";

//    public string ToString(string? format, IFormatProvider? formatProvider)
//    {
//        var provider = formatProvider ?? CultureInfo.InvariantCulture;

//        return string.IsNullOrEmpty(format) ? $"{Name}:{Value}" : string.Format(provider, format, Value);
//    }

//    internal T Value { get; }
//}

public readonly record struct NamedId(string Name, [property: JsonConverter(typeof(ByteStringConverter))] ByteString Value)
    : IComparable<NamedId>
{
    #region operator overloads
    public static implicit operator string(NamedId identifier) => $"{identifier}";

    //public static bool operator ==(NamedId lhs, NamedId rhs) => lhs.Equals(rhs);

    //public static bool operator !=(NamedId lhs, NamedId rhs) => !(lhs == rhs);

    public static bool operator <(NamedId lhs, NamedId rhs) => lhs.CompareTo(rhs) < 0;

    public static bool operator <=(NamedId lhs, NamedId rhs) => lhs.CompareTo(rhs) is (<= 0);

    public static bool operator >(NamedId lhs, NamedId rhs) => lhs.CompareTo(rhs) > 0;

    public static bool operator >=(NamedId lhs, NamedId rhs) => lhs.CompareTo(rhs) is (>= 0);
    #endregion

    public int CompareTo(NamedId other)
    {
        var cmp = Name.CompareNullableTo(other.Name);
        if(cmp != 0) return cmp;

        return Value.CompareNullableTo(other.Value);
    }

    [JsonIgnore]
    public bool IsEmpty => string.IsNullOrEmpty(Name);

    public string Name { get; init; } = string.IsNullOrWhiteSpace(Name) ? throw new ArgumentNullException(nameof(Name)) : Name;

    public static NamedId New(string name) => new(name, Guid.NewGuid().ToByteArray());

    public static NamedId New(string name, byte[] value)
        => string.IsNullOrWhiteSpace(name) ? throw new ArgumentNullException(name) : new(name, new ByteString(value));

    public static NamedId New(string name, decimal value) => new(name, value.ToByteArray());

    public static NamedId New(string name, double value) => new(name, BitConverter.GetBytes(value));

    public static NamedId New(string name, float value) => new(name, BitConverter.GetBytes(value));

    public static NamedId New(string name, Guid value) => new(name, value.ToByteArray());

    public static NamedId New(string name, int value) => new(name, BitConverter.GetBytes(value));

    public static NamedId New(string name, long value) => new(name, BitConverter.GetBytes(value));

    public static NamedId New(string name, string value, Encoding? encoding = null) => new(name, value.ThrowIfNullOrWhiteSpace().ToByteArray(encoding));

    public static NamedId New(string name, ulong value) => new(name, BitConverter.GetBytes(value));

    public static NamedId New(string name, ushort value) => new(name, BitConverter.GetBytes(value));

    public static NamedId<T> New<T>(string name, T value) where T : struct, IComparable<T>
        => new(name, value);
}

public record struct NamedId<T>(string Name, T Value) : IComparable<NamedId<T>> where T : IComparable<T>
{
    public readonly int CompareTo(NamedId<T> other)
    {
        var cmp = Name.CompareNullableTo(other.Name);
        if (cmp !=0) return cmp;

        return Value.CompareNullableTo(other.Value);
    }

    [JsonIgnore]
    public readonly bool IsEmpty => string.IsNullOrEmpty(Name);

    public string Name { get; init; } = string.IsNullOrWhiteSpace(Name) ? throw new ArgumentNullException(nameof(Name)) : Name;
}
