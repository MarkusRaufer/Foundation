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
using System.Text.Json.Serialization;

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
    private readonly IComparable _value;

    public static bool operator ==(Id lhs, Id rhs) => lhs.Equals(rhs);

    public static bool operator !=(Id lhs, Id rhs) => !(lhs == rhs);

    public static bool operator <(Id lhs, Id rhs) => lhs.CompareTo(rhs) == -1;

    public static bool operator <=(Id lhs, Id rhs) => lhs.CompareTo(rhs) <= 0;

    public static bool operator >(Id lhs, Id rhs) => lhs.CompareTo(rhs) == 1;

    public static bool operator >=(Id lhs, Id rhs) => lhs.CompareTo(rhs) >= 0;

    /// <summary>
    /// Compares <see cref="Id"/> instance with other <see cref="Id"/> instannce.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(Id other)
    {
        if (IsEmpty) return other.IsEmpty ? 0 : -1;
        if (other.IsEmpty) return 1;
        if (!Type.Equals(other.Type)) return 1;

        return _value.CompareTo(other._value);
    }

    /// <summary>
    ///  Compares <see cref="Id"/> instance with other object.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public int CompareTo(object? obj) => obj is Id other ? CompareTo(other) : 1;

    /// <summary>
    /// An empty instance of <see cref="Id"/>.
    /// </summary>
    public static readonly Id Empty;

    /// <summary>
    /// Verifies if this instance of <see cref="Id"/> equals to the other object.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Id other && Equals(other);

    /// <summary>
    /// Verifies if this instance of <see cref="Id"/> equals to the other <see cref="Id"/>.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(Id other)
    {
        if(IsEmpty) return other.IsEmpty;

        return _value.Equals(other._value);
    }

    public override int GetHashCode() => _value.GetNullableHashCode();

    /// <summary>
    /// Returns a new Id with an incremented value if value is a number otherwise a new Id with the same value.
    /// </summary>
    /// <returns></returns>
    public Option<Id> Inc()
    {
        var option = ObjectHelper.Increment(_value);
        
        return option.TryGet(out var value) 
            ? Option.Some(Id.New(value))
            : Option.None<Id>();
    }

    [JsonIgnore]
    public readonly bool IsEmpty => _value is null;

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
            Type = value.GetType();
            _value = cmp;
        }
    }
}
