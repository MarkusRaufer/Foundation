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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Foundation;

/// <summary>
/// An if statement that returns a value.
/// </summary>
public static class If
{
    public static IIfElse<T> True<T>(Func<bool> predicate, Func<T> selector)
    {
        predicate.ThrowIfNull();
        selector.ThrowIfNull();
        return new IfElse<T>(predicate(), selector);
    }

#if NETSTANDARD2_0
    public static IIfType<T> Type<T>(object obj)
    {
        if(obj is T t)
        {
            T selector() => t;
            return new IfType<T>(true, selector);
        }
        
        return new IfType<T>(false, null);
    }
#else
    public static IIfType<T> Type<T>([AllowNull] object obj)
    {
        if (obj is T t)
        {
            T selector() => t;
            return new IfType<T>(true, selector);
        }

        return new IfType<T>(false, null);
    }
#endif

#if NETSTANDARD2_0
    public static IIfType<T, TResult> Type<T, TResult>(object obj, Func<T, TResult> selector)
    {
        (bool isOfType, T? value) = obj is T t ? (true, t) : (false, default);

        return new IfType<T, TResult>(isOfType, value, selector);
    }
#else
    public static IIfType<T, TResult> Type<T, TResult>([AllowNull] object obj, Func<T, TResult> selector)
    {
        (bool isOfType, T? value) = obj is T t ? (true, t) : (false, default);

        return new IfType<T, TResult>(isOfType, value, selector);
    }
#endif

    public static IIfValue<T> Value<T>(T? value)
    {
        return new IfValue<T>(value);
    }

    public static IIfValue<T> Value<T>(Func<T?> selector)
    {
        return new IfValue<T>(selector());
    }
}

internal record IfElse<T>(bool IsPredicateTrue, Func<T>? Selector) : IIfElse<T>
{
    public T Else(Func<T> selector)
    {
        if (IsPredicateTrue && Selector is null) throw new ArgumentNullException(nameof(Selector));
        
        return IsPredicateTrue ? Selector!() : selector();
    }

    public IIfElse<T> ElseIf(Func<bool> predicate, Func<T> selector)
    {
        predicate.ThrowIfNull();
        selector.ThrowIfNull();

        return IsPredicateTrue
            ? new IfElse<T>(IsPredicateTrue, Selector)
            : new IfElse<T>(predicate(), selector);
    }
}

internal record IfElse<T, TResult>(bool IsPredicateTrue, T? Value, Func<T, TResult> Selector)
    : IIfElse<T, TResult>
{
    public TResult Else(Func<T, TResult> selector)
    {
        Value.ThrowIfNull();

        return IsPredicateTrue ? Selector(Value!) : selector(Value!);
    }

    public IIfElse<T, TResult> ElseIf(Func<T, bool> predicate, Func<T, TResult> selector)
    {
        predicate.ThrowIfNull();
        selector.ThrowIfNull();

        var isPredicateTrue = Value is not null && predicate(Value);

        return new IfElse<T, TResult>(IsPredicateTrue: isPredicateTrue, Value, selector); ;
    }
}

internal record IfType<T>(bool IsOfType, Func<T>? Selector) : IIfType<T>
{
#if NETSTANDARD2_0

    public IIfElse<T> ElseIfType(object obj)
    {
        if (IsOfType && null != Selector) return new IfElse<T>(true, Selector);

        if (obj is T t)
        {
            T selector() => t;
            return new IfElse<T>(true, selector);
        }

        return new IfElse<T>(false, null);
    }
#else
    public IIfElse<T> ElseIfType([AllowNull] object obj)
    {
        if (IsOfType && null != Selector) return new IfElse<T>(true, Selector);

        if (obj is T t)
        {
            T selector() => t;
            return new IfElse<T>(true, selector);
        }

        return new IfElse<T>(false, null);
    }
#endif
}

internal record IfType<T, TResult>(bool IsOfType, T? Value, Func<T, TResult> Selector) : IIfType<T, TResult>
{
#if NETSTANDARD2_0
    public IIfElse<T, TResult> ElseIfType(object obj, Func<T, TResult> selector)
    {
        if (IsOfType) return new IfElse<T, TResult>(true, Value, Selector);

        (bool isTrue, T? value) = obj is T t ? (true, t) : (false, default);
        return new IfElse<T, TResult>(isTrue, value, selector);
    }
#else
    public IIfElse<T, TResult> ElseIfType([AllowNull] object obj, Func<T, TResult> selector)
    {
        if (IsOfType) return new IfElse<T, TResult>(true, Value, Selector);

        (bool isTrue, T? value) = obj is T t ? (true, t) : (false, default);
        return new IfElse<T, TResult>(isTrue, value, selector);
    }
#endif
}

internal record IfValue<T>(T? Value) : IIfValue<T>
{
    public IIfElse<T, TResult> Is<TResult>(Func<T, bool> predicate, Func<T, TResult> selector)
    {
        predicate.ThrowIfNull();

        var isPredicateTrue = Value is not null && predicate(Value);
        return new IfElse<T, TResult>(isPredicateTrue, Value, selector);
    }

    public IIfElse<TResult> NotNull<TResult>(Func<T, TResult> selector)
    {
        selector.ThrowIfNull();
        
        Func<TResult>? resultSelector = Value is not null
            ? () => selector(Value)
            : null;

        return new IfElse<TResult>(Value is not null, resultSelector);
    }
}

public interface IIfElse<T>
{
    T Else(Func<T> selector);
    IIfElse<T> ElseIf(Func<bool> predicate, Func<T> selector);
}

public interface IIfElse<T, TResult>
{
    TResult Else(Func<T, TResult> selector);
    IIfElse<T, TResult> ElseIf(Func<T, bool> predicate, Func<T, TResult> selector);
}

//public interface IIfNotNullThen<T>
//{
//    IIfNotNullElse<TResult> Then<TResult>(Func<T, TResult?> selector);
//}

//public interface IIfNotNullElse<TResult>
//{
//    TResult? Else(Func<TResult?> selector);
//}
public interface IIfType<T>
{
#if NETSTANDARD2_0
    IIfElse<T> ElseIfType(object value);
#else
    IIfElse<T> ElseIfType([AllowNull] object value);
#endif
}

public interface IIfType<T, TResult>
{
#if NETSTANDARD2_0
    IIfElse<T, TResult> ElseIfType(object value, Func<T, TResult> selector);
#else
    IIfElse<T, TResult> ElseIfType([AllowNull] object value, Func<T, TResult> selector);
#endif
}

public interface IIfValue<T>
{
    IIfElse<T, TResult> Is<TResult>(Func<T, bool> predicate, Func<T, TResult> selector);
    IIfElse<TResult> NotNull<TResult>(Func<T, TResult> selector);
}

