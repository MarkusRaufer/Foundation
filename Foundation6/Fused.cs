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

/// <summary>
/// Acts like a semiconductor fuse. If blown it won't change the value any longer.
/// </summary>
public struct Fused
{
    public static FusedValue<T> Value<T>(T? value)
    {
        return new FusedValue<T>(value);
    }
}

public struct FusedValue<T>
{
    internal FusedValue(T? value)
    {
        Value = value;
        IsInitialized = true;
    }

    public bool IsInitialized { get; }

    public override string ToString() => $"{Value}";

    public T? Value { get; }
}

public static class FusedValueExtensions
{
    /// <summary>
    /// Blows on condition.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public static Fused<T> BlowIf<T>(this FusedValue<T> value, Func<T?, bool> predicate)
    {
        return new Fused<T>(value.Value, predicate);
    }

    /// <summary>
    /// Blows on value change.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Fused<T> BlowIfChanged<T>(this FusedValue<T> value)
    {
        var initialValue = value;

        return new Fused<T>(value.Value, x =>
        {
            if (!initialValue.IsInitialized) return null == x;
            return !initialValue.Value.EqualsNullable(x);
        });
    }

    public static Fused<T> BlowIfGreater<T>(this FusedValue<T> value, T theshold)
        where T : IComparable<T>
    {
        return new Fused<T>(value.Value, x => x.CompareNullableTo(theshold) == 1);
    }

    public static Fused<T> BlowIfGreaterEqual<T>(this FusedValue<T> value, T theshold)
        where T : IComparable<T>
    {
        return new Fused<T>(value.Value, x => x.CompareNullableTo(theshold) != -1);
    }

    public static Fused<T> BlowIfLess<T>(this FusedValue<T> value, T theshold)
        where T : IComparable<T>
    {
        return new Fused<T>(value.Value, x => x.CompareNullableTo(theshold) == -1);
    }

    public static Fused<T> BlowIfLessEqual<T>(this FusedValue<T> value, T theshold)
        where T : IComparable<T>
    {
        return new Fused<T>(value.Value, x => x.CompareNullableTo(theshold) != 1);
    }
}

/// <summary>
/// This structure reacts like a fuse. If the fuse has blown, the value won't change any longer.
/// </summary>
/// <typeparam name="T"></typeparam>
public struct Fused<T> : IEquatable<Fused<T>>
{
    private readonly Func<T?, bool> _predicate;
    private T? _value;

    public Fused(T? seed, Func<T?, bool> predicate)
    {
        _value = seed;
        _predicate = predicate.ThrowIfNull();
        IsBlown = false;
    }

    public static bool operator ==(Fused<T> left, Fused<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Fused<T> left, Fused<T> right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj) => obj is Fused<T> other && Equals(other);

    public bool Equals(Fused<T> other)
    {
        if (Value is null) return other.Value is null;

        return Value.Equals(other.Value);
    }

    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public bool IsBlown { get; private set; }

    public override string ToString() => $"Value:{Value}, IsBlown:{IsBlown}";

    public T? Value
    {
        get { return _value; }
        set
        {
            if (IsBlown) return;
            IsBlown = _predicate(value);
            _value = value;
        }
    }
}

