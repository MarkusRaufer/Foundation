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

using System.Text.Json.Serialization;

/// <summary>
/// This is a named identifier. The name and value are used for equality and comparison.
/// </summary>
/// <param name="Name">The name of the identifier.</param>
/// <param name="Value">The value of the identifier.</param>
public record struct NamedId(string Name, Id Value) : IComparable<NamedId>
{
    /// <summary>
    /// The default <paramref name="Name"/> for a named identifier.
    /// </summary>
    public static readonly string DefaultName = "Id";

    public static bool operator <(NamedId left, NamedId right) => left.CompareTo(right) < 0;
    public static bool operator >(NamedId left, NamedId right) => left.CompareTo(right) > 0;

    public static bool operator <=(NamedId left, NamedId right) => left.CompareTo(right) <= 0;

    public static bool operator >=(NamedId left, NamedId right) => left.CompareTo(right) >= 0;

    /// <summary>
    /// Compares this instance of <see cref="NamedId"/> with the other instance.
    /// </summary>
    /// <param name="other">The other to compare with.</param>
    /// <returns><see cref="IComparable"/></returns>

    public readonly int CompareTo(NamedId other)
    {
        if (Name != other.Name) return 1;

        return Value.CompareTo(other.Value);
    }

    /// <summary>
    /// An empty instance of NamedId.
    /// </summary>
    [JsonIgnore]
    public static readonly NamedId Empty;

    /// <summary>
    /// Returns true when Value is empty otherwise false.
    /// </summary>
    [JsonIgnore]
    public readonly bool IsEmpty => Value.IsEmpty;

    /// <summary>
    /// Returns a new instance of <see cref="NamedId"/> with default name Id and value is a Guid.
    /// </summary>
    /// <returns></returns>
    public static NamedId New() => new(DefaultName, Id.New());

    /// <summary>
    /// Returns a new instance of <see cref="NamedId"/> with default name Id.
    /// </summary>
    /// <param name="value">The value of the identifer mut not be emtpy.</param>
    /// <returns></returns>
    public static NamedId New(Id value) => new(DefaultName, value.ThrowIfEmpty());

    /// <summary>
    /// Returns a new instance of <see cref="NamedId"/> with default name Id.
    /// </summary>
    /// <typeparam name="T">The type of the identifier value.</typeparam>
    /// <param name="value">The value of the identifier.</param>
    /// <returns></returns>
    public static NamedId New<T>(T value) where T : notnull => new(DefaultName, Id.New(value));

    /// <summary>
    /// Returns a new instance of <see cref="NamedId"/>.
    /// </summary>
    /// <param name="name">The name of the identifier.</param>
    /// <param name="value">The value of the identifier.</param>
    /// <returns></returns>
    public static NamedId New(string name, Id value) => new(name.ThrowIfNullOrWhiteSpace(), value.ThrowIfEmpty());

    /// <summary>
    /// Returns a new instance of <see cref="NamedId"/>.
    /// </summary>
    /// <param name="name">The name of the identifier.</param>
    /// <param name="value">The value of the identifier.</param>
    /// <returns></returns>
    public static NamedId New<T>(string name, T value) where T : notnull => new(name.ThrowIfNullOrWhiteSpace(), Id.New(value));
}

/// <summary>
/// This is a named identifier. The name and value are used for equality and comparison.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="Name">The name of the identifier.</param>
/// <param name="Value">The value of the identifier.</param>
public record struct NamedId<T>(string Name, T Value) : IComparable<NamedId<T>>
    where T : notnull, IComparable<T>
{
    public static bool operator <(NamedId<T> left, NamedId<T> right) => left.Name == right.Name && left.Value.CompareTo(right.Value) == -1;
    public static bool operator >(NamedId<T> left, NamedId<T> right) => left.Name == right.Name && left.Value.CompareTo(right.Value) == 1;

    public static bool operator <=(NamedId<T> left, NamedId<T> right) => left.Name == right.Name && left.Value.CompareTo(right.Value) is -1 or 0;

    public static bool operator >=(NamedId<T> left, NamedId<T> right) => left.Name == right.Name && left.Value.CompareTo(right.Value) is 0 or 1;

    /// <summary>
    /// Compares this instance of <see cref="NamedId{T}"/> with the other instance.
    /// </summary>
    /// <param name="other">The other to compare with.</param>
    /// <returns><see cref="IComparable"/></returns>
    public readonly int CompareTo(NamedId<T> other)
    {
        if (IsEmpty) return other.IsEmpty ? 0 : -1;
        if (other.IsEmpty) return 1;

        if (Name != other.Name) return 1;
        return Value.CompareTo(other.Value);
    }

    /// <summary>
    /// An empty instance of NamedId.
    /// </summary>
    [JsonIgnore] public static readonly NamedId Empty;

    /// <summary>
    /// Returns true when Value is empty otherwise false.
    /// </summary>
    [JsonIgnore] public readonly bool IsEmpty => Value is null;

    /// <summary>
    /// Creates a new <see cref="NamedId{T}"/>.
    /// </summary>
    /// <param name="name">The name of the identifier.</param>
    /// <param name="value">The value of the identifier. The value must not be null.</param>
    /// <exception cref="ArgumentNullException">If value of identifer is null.</exception>
    /// <returns></returns>
    public static NamedId<T> New(string name, T value) => new(name.ThrowIfNullOrWhiteSpace(), value.ThrowIfNull());
}
