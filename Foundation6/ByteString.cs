namespace Foundation;

using System.Collections;
using System.Runtime.Serialization;
using System.Text;
using Foundation.Collections.Generic;

[Serializable]
public sealed class ByteString
    : ICloneable
    , IComparable
    , IComparable<ByteString>
    , IEnumerable<byte>
    , IEquatable<ByteString>
    , ISerializable
{
    private readonly byte[] _bytes;
    private readonly IComparer<ByteString> _comparer;
    private readonly int _hashCode;

    public ByteString(byte[] bytes)
    {
        _bytes = bytes;
        _comparer = ByteStringComparer.Default;
        _hashCode = HashCode.FromObjects(_bytes);
    }

    public ByteString(SerializationInfo info, StreamingContext context)
    {
        var value = info.GetValue(nameof(_bytes), typeof(byte[]));

        _bytes = (value is byte[] bytes) ? bytes : Array.Empty<byte>();

        _hashCode = HashCode.FromObjects(_bytes);

        _comparer = ByteStringComparer.Default;
    }

    public static bool operator ==(ByteString lhs, ByteString rhs)
    {
        if (ReferenceEquals(lhs, rhs)) return true;

        if (lhs is null) return rhs is null;

        return lhs.Equals(rhs);
    }

    public static bool operator !=(ByteString lhs, ByteString rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator <(ByteString lhs, ByteString rhs)
    {
        if (lhs is null) return rhs is not null;

        return lhs.CompareTo(rhs) == -1;
    }

    public static bool operator <=(ByteString lhs, ByteString rhs)
    {
        if (lhs is null) return true;

        var compare = lhs.CompareTo(rhs);
        return -1 == compare || 0 == compare;
    }

    public static bool operator >(ByteString lhs, ByteString rhs)
    {
        if (lhs is null) return rhs is not null;

        return lhs.CompareTo(rhs) == 1;
    }

    public static bool operator >=(ByteString lhs, ByteString rhs)
    {
        if (lhs is null) return rhs is null;

        var compare = lhs.CompareTo(rhs);
        return 1 == compare || 0 == compare;
    }

    public static implicit operator ByteString(byte[] bytes) => CopyFrom(bytes);

    public static implicit operator byte[](ByteString byteString) => byteString.ToByteArray();

    public byte this[int index] => _bytes[index];

    public object Clone()
    {
        return CopyFrom(_bytes);
    }

    public int CompareTo(ByteString? other)
    {
        return _comparer.Compare(this, other);
    }

    public int CompareTo(object? obj)
    {
        return CompareTo(obj as ByteString);
    }

    public static ByteString Concat(params ByteString[] byteStrings)
    {
        var bytes = Enumerable.Empty<byte>();

        foreach (var byteString in byteStrings)
        {
            bytes = bytes.Concat(byteString.ToByteArray());
        }

        return new ByteString(bytes.ToArray());
    }

    public static ByteString CopyFrom(params byte[] bytes)
    {
        return new ByteString((byte[])bytes.Clone());
    }

    public static ByteString CopyFrom(ReadOnlySpan<byte> bytes)
    {
        return new ByteString(bytes.ToArray());
    }

    public static ByteString Empty { get; } = new ByteString(Array.Empty<byte>());

    public override bool Equals(object? obj)
    {
        if (obj is ByteString other)
            return Equals(other);


        return false;
    }

    public bool Equals(ByteString? other)
    {
        return CompareTo(other) == 0;
    }

    public static ByteString FromBase64String(string base64)
    {
        return string.IsNullOrEmpty(base64) ? Empty : new ByteString(Convert.FromBase64String(base64));
    }

    public static ByteString FromString(string text)
    {
        return FromString(text, Encoding.Unicode);
    }

    public static ByteString FromString(string text, Encoding encoding)
    {
        return new ByteString(encoding.GetBytes(text));
    }

    public static ByteString FromUtf8String(string text)
    {
        return string.IsNullOrEmpty(text) ? Empty : FromString(text, Encoding.UTF8);
    }

    public IEnumerator<byte> GetEnumerator() => _bytes.GetEnumerator<byte>();

    IEnumerator IEnumerable.GetEnumerator() => _bytes.GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(_bytes), _bytes);
    }

    public bool IsEmpty => 0 == Length;

    public int Length => _bytes.Length;

    public string ToBase64String() => Convert.ToBase64String(_bytes);

    public byte[] ToByteArray() => (byte[])_bytes.Clone();

    public ReadOnlySpan<byte> ToReadOnlySpan() => new(_bytes);

    public string ToString(Encoding encoding)
    {
        return encoding.GetString(_bytes, 0, _bytes.Length);
    }

    public override string ToString()
    {
        return ToString(Encoding.Unicode);
    }

    public string ToUtf8String()
    {
        return ToString(Encoding.UTF8);
    }
}

