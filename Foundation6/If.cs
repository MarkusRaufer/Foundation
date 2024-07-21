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

//internal record IfThen(bool IsPredicateTrue) : IIfThen
//{
//    public IIfElse<T> Then<T>(Func<T> selector)
//    {
//        selector.ThrowIfNull();

//        var value = IsPredicateTrue ? selector() : default;

//        return new IfElse<T>(value, IsPredicateTrue);
//    }
//}

//internal record IfThen<T>(T? Value, bool IsPredicateTrue) : IIfTrue<T>
//{
//    public IIfElse<T, TResult> Then<TResult>(Func<T, TResult?> selector)
//    {
//        selector.ThrowIfNull();

//        var result = IsPredicateTrue && Value is not null
//            ? selector(Value)
//            : default;

//        return new IfElse<T, TResult>(Value, result, IsPredicateTrue);
//    }
//}

//internal record IfTrue<T>(bool IsPredicateTrue, Func<T> Selector) : IIfTrue<T>
//{
//    public T Else<TResult>(Func<T> selector)
//    {
//        selector.ThrowIfNull();

//        return IsPredicateTrue ? Selector() :selector();
//    }

//    public IIfTrue<T> ElseIf(Func<bool> predicate, Func<T> selector)
//    {
//        return new IfTrue<T>(predicate(), selector);
//    }
//}

//internal record IfNotNullThen<T>(T? Value, bool IsPredicateTrue) : IIfNotNullThen<T>
//{
//    public IIfNotNullElse<TResult> Then<TResult>(Func<T, TResult?> selector)
//    {
//        selector.ThrowIfNull();

//        return new IfNotNullElse<T, TResult>(Value, selector, IsPredicateTrue);
//    }
//}

//internal record IfNotNullElse<T, TResult>(T? Value, Func<T, TResult?> Selector, bool IsPredicateTrue) : IIfNotNullElse<TResult>
//{
//    public TResult? Else(Func<TResult?> selector)
//    {
//        selector.ThrowIfNull();

//        if (IsPredicateTrue && Value is not null) return Selector(Value);
        
//        return selector();
//    }
//}

//public interface IIfThen
//{
//    IIfElse<T> Then<T>(Func<T> selector);
//}

//public interface IIfTrue<T>
//{
//    T Else<TResult>(Func<T> selector);
//    IIfElse<T> ElseIf(Func<bool> predicate, Func<T> selector);
//}

//public interface IIfTrue<T, TResult>
//{
//    IIfElse<T, TResult> Else(Func<T, TResult?> selector);
//}

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

public interface IIfValue<T>
{
    IIfElse<T, TResult> Is<TResult>(Func<T, bool> predicate, Func<T, TResult> selector);
    IIfElse<TResult> NotNull<TResult>(Func<T, TResult> selector);
}

