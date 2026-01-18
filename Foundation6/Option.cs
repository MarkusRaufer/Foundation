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
﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Foundation;

public static class Option
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Maybe<T>(T? value) => new(value);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> MaybeOfType<T>(object? value) => value is T t ? new Option<T>(t) : Option.None<T>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> MaybeOfType<T>(object? value, Func<T, bool> predicate)
        => value is T t && predicate(t) ? new Option<T>(t) : Option.None<T>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> MaybeOfType<T, TResult>(T? value) => value is TResult result ? Option.Some(result) : Option.None<TResult>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> MaybeOfType<T, TResult>(T? value, Func<T, bool> predicate)
        => value is not null && predicate(value) && value is TResult result ? Option.Some(result) : Option.None<TResult>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> MaybeOfType<T, TResult>(T? value, Func<TResult, bool> predicate)
        => value is TResult result && predicate(result) ? Option.Some(result) : Option.None<TResult>();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> None<T>() => Option<T>.None;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Some<T>(T value) => value is not null ? new Option<T>(value) : throw new ArgumentNullException(nameof(value));
}


[Serializable]
public readonly struct Option<T>
    : IEquatable<Option<T>>
    , ISerializable
{
    private readonly T? _value;

    public Option(T? value)
    {
        IsSome = value is not null;
        _value = value;
    }

    public Option(SerializationInfo info, StreamingContext context)
    {
        if (info.GetValue(nameof(IsSome), typeof(bool)) is not bool isSome)
        {
            IsSome = false;
            _value = default;
            return;
        }

        IsSome = isSome;

        if (!IsSome || info.GetValue(nameof(T), typeof(T)) is not T value)
        {
            IsSome = false;
            _value = default;
            return;
        }
        _value = value;
    }

    public static implicit operator Option<T>(T obj) => Option.Maybe(obj);

    public static implicit operator Option<object>(Option<T> option)
    {
        if (option.TryGet(out var value) && value is not null) return Option.Some<object>(value);
        return Option.None<object>();
    }

    public static bool operator ==(Option<T> lhs, Option<T> rhs) => lhs.Equals(rhs);

    public static bool operator ==(Option<T> lhs, T rhs) => lhs.TryGet(out var l) && l.Equals(rhs);

    public static bool operator ==(T lhs, Option<T> rhs) => rhs == lhs;

    public static bool operator !=(Option<T> lhs, Option<T> rhs) => !(lhs == rhs);

    public static bool operator !=(Option<T> lhs, T rhs) => !(lhs == rhs);

    public static bool operator !=(T lhs, Option<T> rhs) => rhs != lhs;

    public override bool Equals(object? obj) => obj is Option<T> other && Equals(other);

    public bool Equals(Option<T> other)
    {
        if (!IsSome) return !other.IsSome;
        if(!other.IsSome) return false;

        return GetHashCode() == other.GetHashCode() && _value!.Equals(other._value);
    }

    public override int GetHashCode()
    {
        return IsSome ? _value!.GetHashCode() : 0;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(IsSome), IsSome);
        if(IsSome) info.AddValue(nameof(_value), _value);
    }

    public bool IsNone => !IsSome;

    public bool IsSome { get; }

    public override string ToString() => IsSome ? $"Some({_value})" : "None";

    /// <summary>
    /// If returning true it has a value otherwise it returns false.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGet([NotNullWhen(true)] out T? value)
    {
        value = _value;

        return IsSome;
    }

    internal static readonly Option<T> None = new();
}