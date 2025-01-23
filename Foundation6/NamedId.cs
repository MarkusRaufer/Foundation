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

using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

/// <summary>
/// This is a named identifier. The name and value are used for equality and comparison.
/// </summary>
public readonly struct NamedId
    : IComparable<NamedId>
    , IEquatable<NamedId>
{
    private readonly IComparable _comparable;
    private readonly string _name;
    private readonly object _value;

    #region operator overloads
    public static implicit operator string(NamedId identifier) => identifier.ToString();

    public static bool operator ==(NamedId lhs, NamedId rhs) => lhs.Equals(rhs);

    public static bool operator !=(NamedId lhs, NamedId rhs) => !(lhs == rhs);

    public static bool operator <(NamedId lhs, NamedId rhs) => lhs.CompareTo(rhs) < 0;

    public static bool operator <=(NamedId lhs, NamedId rhs) => lhs.CompareTo(rhs) is (<= 0);

    public static bool operator >(NamedId lhs, NamedId rhs) => lhs.CompareTo(rhs) > 0;

    public static bool operator >=(NamedId lhs, NamedId rhs) => lhs.CompareTo(rhs) is (>= 0);
    #endregion

    public int CompareTo(NamedId other)
    {
        var cmp = Name.CompareNullableTo(other.Name);
        if(cmp != 0) return cmp;

        return _comparable.CompareToNullable(other._value);
    }

    [JsonIgnore]
    public static readonly NamedId Empty;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is NamedId other && Equals(other);

    public bool Equals(NamedId other)
    {
        if (IsEmpty) return other.IsEmpty;
        if (other.IsEmpty) return false;

        return GetHashCode() == other.GetHashCode() && Name == other.Name && _value.EqualsNullable(other._value);
    }

#if NETSTANDARD2_0
    public override int GetHashCode() => HashCode.FromObject(Name, Value);

#else
    public override int GetHashCode() => System.HashCode.Combine(Name, Value);
#endif

    [JsonIgnore]
    public bool IsEmpty => _name is null;

    public string Name
    {
        get => _name;
        init => _name = value.ThrowIfNullOrWhiteSpace();
    }

    public static NamedId New(string name) => new() { Name = name, Value = Guid.NewGuid() };

    public static NamedId NewId(string name, object value) => new() { Name = name, Value = value };

    public static NamedId<T> New<T>(string name, T value) where T : notnull, IComparable<T>, IEquatable<T>
        => new() { Name = name, Value = value };

    public override string ToString() => $"{Name}:{Value}";

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

public readonly struct NamedId<T> 
    : IComparable<NamedId<T>>
    , IEquatable<NamedId<T>>
    where T : notnull, IComparable<T>, IEquatable<T>
{
    private readonly string _name;

    #region operator overloads
    public static implicit operator string(NamedId<T> identifier) => identifier.ToString();

    public static bool operator ==(NamedId<T> lhs, NamedId<T> rhs) => lhs.Equals(rhs);

    public static bool operator !=(NamedId<T> lhs, NamedId<T> rhs) => !(lhs == rhs);

    public static bool operator <(NamedId<T> lhs, NamedId<T> rhs) => lhs.CompareTo(rhs) < 0;

    public static bool operator <=(NamedId<T> lhs, NamedId<T> rhs) => lhs.CompareTo(rhs) is (<= 0);

    public static bool operator >(NamedId<T> lhs, NamedId<T> rhs) => lhs.CompareTo(rhs) > 0;

    public static bool operator >=(NamedId<T> lhs, NamedId<T> rhs) => lhs.CompareTo(rhs) is (>= 0);
    #endregion

    public readonly int CompareTo(NamedId<T> other)
    {
        var cmp = Name.CompareNullableTo(other.Name);
        if (cmp !=0) return cmp;

        return Value.CompareNullableTo(other.Value);
    }

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is NamedId<T> other && Equals(other);

    public bool Equals(NamedId<T> other)
    {
        if (IsEmpty) return other.IsEmpty;
        if (other.IsEmpty) return false;

        return GetHashCode() == other.GetHashCode() && Name == other.Name && Value.Equals(other.Value);
    }

#if NETSTANDARD2_0
    public override int GetHashCode() => HashCode.FromObject(Name, Value);
#else
    public override int GetHashCode() => System.HashCode.Combine(Name, Value);
#endif

    [JsonIgnore]
    public readonly bool IsEmpty => string.IsNullOrEmpty(Name);

    public string Name
    {
        get => _name;
        init => _name = value.ThrowIfNullOrWhiteSpace();
    }

    public override string ToString() => $"{Name}:{Value}";

    public T Value { get; init; }
}
