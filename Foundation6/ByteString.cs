// The MIT License (MIT)
//
// Copyright (c) 2020 Markus Raufer
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
﻿namespace Foundation;

using Foundation.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

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

    [JsonConstructor]
    public ByteString(byte[] bytes) : this (bytes, ByteStringComparer.Default)
    {
    }

    public ByteString(byte[] bytes, IComparer<ByteString> comparer)
    {
        _bytes = bytes;
        _comparer = comparer.ThrowIfNull();
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

    public static bool operator !=(ByteString lhs, ByteString rhs) => !(lhs == rhs);

    public static bool operator <(ByteString lhs, ByteString rhs)
    {
        if (lhs is null) return rhs is not null;
        
        return lhs.CompareTo(rhs) < 0;
    }

    public static bool operator <=(ByteString lhs, ByteString rhs)
    {
        if (lhs is null) return true;

        return lhs.CompareTo(rhs) is (<= 0);
    }

    public static bool operator >(ByteString lhs, ByteString rhs)
    {
        if (lhs is null) return false;

        return lhs.CompareTo(rhs) > 0;
    }

    public static bool operator >=(ByteString lhs, ByteString rhs)
    {
        if (lhs is null) return rhs is null;

        return lhs.CompareTo(rhs) is (>= 0);
    }

    public static implicit operator ByteString(byte[] bytes) => CopyFrom(bytes);

    public static implicit operator byte[](ByteString byteString) => byteString.ToByteArray();

    public byte this[int index] => _bytes[index];

    public ReadOnlySpan<byte> AsSpan() => new(_bytes);

    public object Clone()
    {
        return CopyFrom(_bytes);
    }

    /// <summary>
    /// Determines the relative order of the sequences being compared by comparing the elements using IComparable{T}.CompareTo(T).
    /// </summary>
    /// <param name="other">The other ByteString which should be compared.</param>
    /// <returns></returns>
    public int CompareTo(ByteString? other) => other is null ? 1 : _comparer.Compare(this, other);

    /// <summary>
    /// Determines the relative order of the sequences being compared by comparing the elements using IComparable.CompareTo(obj).
    /// </summary>
    /// <param name="other">If other is a ByteString it will be compared otherwise it returns 1.</param>
    /// <returns></returns>
    public int CompareTo(object? obj) => CompareTo(obj as ByteString);

    public static ByteString CopyFrom(params byte[] bytes) => new ByteString((byte[])bytes.Clone());

    public static ByteString CopyFrom(ReadOnlySpan<byte> bytes) => new(bytes.ToArray());

    [JsonIgnore]
    public static ByteString Empty { get; } = new ByteString(Array.Empty<byte>());

    public override bool Equals(object? obj) => Equals(obj as ByteString);

    public bool Equals(ByteString? other)
    {
        return GetHashCode() == other.GetNullableHashCode() && CompareTo(other) == 0;
    }

    public static ByteString FromBase64String(string base64)
    {
        return string.IsNullOrEmpty(base64) ? Empty : new (Convert.FromBase64String(base64));
    }

    public static ByteString FromString(string text) => FromString(text, Encoding.Unicode);

    public static ByteString FromString(string text, Encoding encoding) => new (encoding.GetBytes(text));

    public static ByteString FromUtf8String(string text) => string.IsNullOrEmpty(text) ? Empty : FromString(text, Encoding.UTF8);

    public IEnumerator<byte> GetEnumerator() => _bytes.GetEnumerator<byte>();

    IEnumerator IEnumerable.GetEnumerator() => _bytes.GetEnumerator();

    public override int GetHashCode() => _hashCode;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(_bytes), _bytes);
    }

    [JsonIgnore]
    public bool IsEmpty => 0 == Length;

    [JsonIgnore]
    public int Length => _bytes.Length;

    public int ToBase64CharArray(int offsetIn, int length, char[] outArray, int offsetOut)
        => Convert.ToBase64CharArray(_bytes, offsetIn, length, outArray, offsetOut);

    public int ToBase64CharArray(char[] outArray) =>
        Convert.ToBase64CharArray(_bytes, 0,
                                  _bytes.Length,
                                  outArray.ThrowIf(() => outArray.Length != _bytes.Length,
                                  () => new ArgumentOutOfRangeException(
                                            nameof(outArray),
                                            $"must have at least the size of {nameof(Length)} {Length} but was {outArray.Length}")),
                                  0);
                                                   

    public string ToBase64String() => Convert.ToBase64String(_bytes);

    public byte[] ToByteArray() => (byte[])_bytes.Clone();

    public IEnumerable<byte> ToBytes() => _bytes;

    public string ToString(Encoding encoding) => encoding.GetString(_bytes, 0, _bytes.Length);

    public override string ToString() => ToString(Encoding.Unicode);

    public string ToUtf8String() => ToString(Encoding.UTF8);
}
