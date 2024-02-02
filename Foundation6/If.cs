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
/// An if statement that returns a value.
/// </summary>
public static class If
{
    public static IIfThen True(bool isTrue)
    {
        return new IfThen(isTrue);
    }

    public static IIfThen True(Func<bool> predicate)
    {
        return new IfThen(predicate());
    }
    public static IIfValue<T> Value<T>(T? value)
    {
        return new IfValue<T>(value);
    }

    public static IIfValue<T> Value<T>(Func<T?> func)
    {
        return new IfValue<T>(func());
    }
}
internal record IfValue<T>(T? Value) : IIfValue<T>
{
    public IIfThen<T> Is(Func<T, bool> predicate)
    {
        var isTrue = Value is not null && predicate(Value);
        
        return new IfThen<T>(Value, IsPredicateTrue: isTrue);
    }
    public IIfNotNullThen<T> NotNull()
    {
        return new IfNotNullThen<T>(Value, IsPredicateTrue: Value is not null);
    }
}

internal record IfThen(bool IsPredicateTrue) : IIfThen
{
    public IIfElse<T> Then<T>(Func<T> selector)
    {
        var value = IsPredicateTrue ? selector() : default;

        return new IfElse<T>(value, IsPredicateTrue);
    }
}

internal record IfThen<T>(T? Value, bool IsPredicateTrue) : IIfThen<T>
{
    public IIfElse<T, TResult> Then<TResult>(Func<T, TResult?> selector)
    {
        var result = IsPredicateTrue && Value is not null
            ? selector(Value)
            : default;

        return new IfElse<T, TResult>(Value, result, IsPredicateTrue);
    }
}

internal record IfNotNullThen<T>(T? Value, bool IsPredicateTrue) : IIfNotNullThen<T>
{
    public IIfNotNullElse<TResult> Then<TResult>(Func<T, TResult?> selector)
    {
        return new IfNotNullElse<T, TResult>(Value, selector, IsPredicateTrue);
    }
}

internal record IfElse<T>(T? Value, bool IsPredicateTrue) : IIfElse<T>
{
    public T Else(Func<T> selector)
    {
        if (IsPredicateTrue)
        {
            if (Value is T value) return value;
            throw new ArgumentException(nameof(selector));
        }

        return selector();
    }
}

internal record IfElse<T, TResult>(T? Value, TResult? Result, bool IsPredicateTrue) : IIfElse<T, TResult>
{
    public TResult? Else(Func<T, TResult?> selector)
    {
        if (IsPredicateTrue) return Result;

        if (Value is T value) return selector(value);

        return default;
    }
}

internal record IfNotNullElse<T, TResult>(T? Value, Func<T, TResult?> Selector, bool IsPredicateTrue) : IIfNotNullElse<TResult>
{
    public TResult? Else(Func<TResult?> selector)
    {
        if (IsPredicateTrue && Value is not null) return Selector(Value);
        
        return selector();
    }
}

public interface IIfValue<T>
{
    IIfThen<T> Is(Func<T, bool> predicate);
    IIfNotNullThen<T> NotNull();
}

public interface IIfThen
{
    IIfElse<T> Then<T>(Func<T> selector);
}

public interface IIfThen<T>
{
    IIfElse<T, TResult> Then<TResult>(Func<T, TResult?> selector);
}

public interface IIfNotNullThen<T>
{
    IIfNotNullElse<TResult> Then<TResult>(Func<T, TResult?> selector);
}

public interface IIfElse<T>
{
    T Else(Func<T> selector);
}

public interface IIfElse<T, TResult>
{
    TResult? Else(Func<T, TResult?> selector);
}

public interface IIfNotNullElse<TResult>
{
    TResult? Else(Func<TResult?> selector);
}
