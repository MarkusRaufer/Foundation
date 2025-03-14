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
using System.Runtime.CompilerServices;

namespace Foundation;

public static class OptionExtensions
{
    /// <summary>
    /// Compares two optionals.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CompareTo<T>(this Option<T> lhs, Option<T> rhs)
        where T : IComparable<T>
    {
        if(lhs.IsNone) return rhs.IsNone ? 0 : - 1;
        if(rhs.IsNone) return 1;

        if (lhs.TryGet(out T? lhsValue) && rhs.TryGet(out T? rhsValue)) return lhsValue.CompareTo(rhsValue);
        if (lhs.IsNone) return rhs.IsNone ? 0 : -1;
        
        return 1;
    }

    /// <summary>
    /// Calls <paramref name="some"/> if IsSome is true. Calls <paramref name="none"/> if IsSome is false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="option"></param>
    /// <param name="some"></param>
    /// <param name="none"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Either<T, TResult>(
        this Option<T> option,
        Func<T, TResult> some,
        Func<TResult> none)
        where TResult : notnull
    {
        some.ThrowIfNull();
        none.ThrowIfNull();

        return option.TryGet(out T? value) ? some(value!) : none();
    }

    /// <summary>
    /// Calls <paramref name="some"/> if IsSome is true. Calls <paramref name="none"/> if IsSome is false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <param name="some"></param>
    /// <param name="none"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Invoke<T>(
        this Option<T> option,
        Action<T> some,
        Action none)
    {
        some.ThrowIfNull();
        none.ThrowIfNull();

        if (option.TryGet(out T? value))
        {
            some.Invoke(value!);
            return true;
        }

        none.Invoke();
        return false;
    }

    /// <summary>
    /// Calls <paramref name="predicate"/> if <paramref name="option"/> IsSome is true and returns the result of the <paramref name="predicate"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsSomeAndAlso<T>(this Option<T> option, Func<T, bool> predicate)
    {
        return option.TryGet(out var value) && predicate(value);
    }

    /// <summary>
    /// Calls <paramref name="action"/> if <paramref name="option"/> is none.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unit OnNone<T>(this Option<T> option, Action action)
    {
        if (option.IsNone) action.Invoke();

        return new Unit();
    }

    /// <summary>
    /// Calls <paramref name="action"/> if <paramref name="option"/> IsSome is true.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unit OnSome<T>(this Option<T> option, Action<T> action)
    {
        if (option.TryGet(out T? value)) action.Invoke(value!);

        return new Unit();
    }

    /// <summary>
    /// If lhs has a value lhs is returned otherwise rhs is returned.
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="lhs">The left option containing a possible value.</param>
    /// <param name="rhs">The right option containing a possible value.</param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Or<T>(this Option<T> lhs, Option<T> rhs)
    {
        return lhs.IsSome ? lhs : rhs;
    }

    /// <summary>
    /// Returns value if IsNone or calls <paramref name="none"/>;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <param name="none"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Or<T>(this Option<T> option, Func<T> none)
    {
        none.ThrowIfNull();

        if(option.TryGet(out T? value)) return value!;

        value = none();

        return value.ThrowIf(() => null == value, $"{nameof(none)} returned null");
    }

    /// <summary>
    /// Throws NullReferenceException if IsSome is false or returns a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T OrThrow<T>(this Option<T> option)
    {
        return OrThrow(option, () => new NullReferenceException(nameof(option)));
    }

    /// <summary>
    /// Throws an exception if IsSome is false or returns a value. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TException">exception which will be thrown when IsSome is false.</typeparam>
    /// <param name="option"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T OrThrow<T, TException>(this Option<T> option, Func<TException> exception)
        where TException : Exception
    {
        if (option.TryGet(out T? value)) return value!;

        throw exception();
    }

    /// <summary>
    /// Converts an option of type T into an option of type TResult.
    /// If conversion is not possible, it returns None.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="option"></param>
    /// <param name="projection"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<TResult> To<T, TResult>(
        this Option<T> option,
        Func<T, TResult> projection)
    {
        projection.ThrowIfNull();

        if (!option.TryGet(out T? value)) return Option.None<TResult>();

        var result = projection(value!);

        return Option.Maybe(result);
    }

    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<T, Error> ToResult<T>(this Option<T> option, Func<Error> error)
    {
        return option.TryGet(out T? value) ? Result.Ok(value) : Result.Error<T>(error());
    }
}

