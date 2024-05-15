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
﻿using System.Diagnostics.CodeAnalysis;

#if NET6_0_OR_GREATER
using System.Text.Json.Serialization;
#endif

namespace Foundation;

/// <summary>
/// A generic identifier which encapsulates the value of the identifier.
/// You can change the type of the identifier value without changing your code.
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

    public int CompareTo(Id other) => _comparable.CompareToNullable(other._value);

    public int CompareTo(object? obj) => obj is Id other ? CompareTo(other) : 1;

    public static readonly Id Empty;

    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Id other && Equals(other);

    public bool Equals(Id other)
    {
        if(IsEmpty) return other.IsEmpty;

        return _comparable.Equals(other._value);
    }

    public override int GetHashCode() => _value.GetNullableHashCode();

#if NET6_0_OR_GREATER
    [JsonIgnore]
#endif
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
