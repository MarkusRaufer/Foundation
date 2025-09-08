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
using System.Runtime.CompilerServices;

public static class Result
{
    /// <summary>
    /// Returns an error result.
    /// </summary>
    /// <param name="error">The error value.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<Error> Error(Error error)
    {
        error.ThrowIfNull();
        return new Result<Error>(error);
    }

    /// <summary>
    /// Returns an error result where the error is an exception.
    /// </summary>
    /// <param name="exception"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<Exception> Error(Exception exception)
    {
        exception.ThrowIfNull();
        return new Result<Exception>(exception);
    }

    /// <summary>
    /// Returns an error result.
    /// </summary>
    /// <typeparam name="TOk">Type of ok value.</typeparam>
    /// <param name="error">The error value.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOk, Error> Error<TOk>(Error error)
    {
        error.ThrowIfNull();
        return new Result<TOk, Error>(Option.None<TOk>(), error);
    }

    /// <summary>
    /// Returns an error result where the error is an exception.
    /// </summary>
    /// <typeparam name="TOk">The type of the ok value.</typeparam>
    /// <param name="exception">The exception of the error.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOk, Exception> Error<TOk>(Exception exception)
    {
        exception.ThrowIfNull();
        return new Result<TOk, Exception>(Option.None<TOk>(), exception);
    }

    /// <summary>
    ///  Returns an error result.
    /// </summary>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <param name="error">The error value.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TError> Error<TError>(TError error)
    {
        error.ThrowIfNull();
        return new Result<TError>(error);
    }

    /// <summary>
    ///  Returns an error result.
    /// </summary>
    /// <typeparam name="TOk">Type of ok value.</typeparam>
    /// <typeparam name="TError">Type of error.</typeparam>
    /// <param name="error">Error value.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOk, TError> Error<TOk, TError>(TError error)
    {
        error.ThrowIfNull();
        return new Result<TOk, TError>(Option.None<TOk>(), error);
    }

    /// <summary>
    /// Creates a <see cref="Result{TOk, Error}"/> from an <see cref="Option{TOk}"/>.
    /// </summary>
    /// <typeparam name="TOk">The type of ok.</typeparam>
    /// <param name="func">Returns an <see cref="Option{TOk}"/></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOk, Error> FromOption<TOk>(Func<Option<TOk>> func)
    {
        return FromOption(func, () => new Error("incompatible value", $"the value is not of type {typeof(TOk)}"));
    }

    /// <summary>
    /// Creates a <see cref="Result{TOk, TError}"/> from an <see cref="Option{TOk}"/>.
    /// </summary>
    /// <typeparam name="TOk">The type of ok.</typeparam>
    /// <typeparam name="TError">The error type.</typeparam>
    /// <param name="func">Returns an <see cref="Option{TOk}"/></param>
    /// <param name="error">Returns an error.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOk, TError> FromOption<TOk, TError>(Func<Option<TOk>> func, Func<TError> error)
    {
        func.ThrowIfNull();
        error.ThrowIfNull();

        var option = func();
        if (option.TryGet(out var ok)) return Result.Ok<TOk, TError>(ok);

        var errorArgument = nameof(error);
        if (error() is not TError err) throw new ArgumentException($"{errorArgument} is not of type {typeof(TError).FullName}");

        return Result.Error<TOk, TError>(err);
    }

    /// <summary>
    /// Returns an Ok result if it is of type <typeparamref name="TOk"/> otherwise it returns an Error result.
    /// </summary>
    /// <typeparam name="TOk">Typeof of ok value.</typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOk, Error> Maybe<TOk>(object? value)
    {
        return Maybe<TOk, Error>(value, () => new Error("incompatible value", $"the value is not of type {typeof(TOk)}"));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOk, Error> Maybe<TOk>(object? value, Func<Error> error)
    {
        if (value is TOk ok) return Result.Ok<TOk, Error>(ok);

        return Result.Error<TOk, Error>(error());
    }

    /// <summary>
    /// Returns an Ok result if it is of type <typeparamref name="TOk"/> otherwise it returns an Error result.
    /// </summary>
    /// <typeparam name="TOk"></typeparam>
    /// <param name="value">The value to be checked and returned if it is of type <typeparamref name="TOk"/>"/>.</param>
    /// <param name="error"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOk, Exception> Maybe<TOk>(object? value, Func<Exception> error)
    {
        if (value is TOk ok) return Result.Ok<TOk, Exception>(ok);

        var errorArgument = nameof(error);
        if (error() is not Exception err) throw new ArgumentException($"{errorArgument} is not of type {typeof(Exception).FullName}");

        return Result.Error<TOk>(err);
    }

    /// <summary>
    /// Returns an Ok result if it is of type <typeparamref name="TOk"/> otherwise it returns a result of type TError.
    /// </summary>
    /// <typeparam name="TOk">Typeof of ok value.</typeparam>
    /// <typeparam name="TError">Type of error.</typeparam>
    /// <param name="value">The value to be checked.</param>
    /// <param name="error">The error value.</param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOk, TError> Maybe<TOk, TError>(object? value, Func<TError> error)
    {
        if (value is TOk ok) return Result.Ok<TOk, TError>(ok);

        var errorArgument = nameof(error);
        if (error() is not TError err) throw new ArgumentException($"{errorArgument} is not of type {typeof(TError).FullName}");

        return Result.Error<TOk, TError>(err);
    }

    /// <summary>
    /// Returns an ok result where ok is a boolean with value true.
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<Error> Ok() => new ();

    /// <summary>
    /// Returns an ok result where ok is a boolean with value true.
    /// </summary>
    /// <typeparam name="TError">Type of error</typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TError> Ok<TError>() => new ();

    /// <summary>
    /// Returns an ok result where ok is of type <typeparamref name="TOk"/> and IsOk is true./>.
    /// </summary>
    /// <typeparam name="TOk">The type of the ok value.</typeparam>
    /// <param name="value">the ok value.</param>
    /// <returns>A valid result.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOk, Error> Ok<TOk>(TOk value)
    {
        value.ThrowIfNull();

        return new Result<TOk, Error>(Option.Some(value), null);
    }

    /// <summary>
    /// Returns an ok result where ok is of type <typeparamref name="TOk"/>, error is of type <typeparamref name="TError"/> and IsOk is true./>.
    /// </summary>
    /// <typeparam name="TOk">The type of the ok value.</typeparam>
    /// <param name="value">the ok value.</param>
    /// <returns>A valid result.</returns>
    /// 
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOk, TError> Ok<TOk, TError>(TOk value)
    {
        value.ThrowIfNull();
        return new Result<TOk, TError>(Option.Some(value), default);
    }

    /// <summary>
    /// Returns an ok result where <typeparamref name="TOk"/> is optional. IsOk is always true and OK is <see cref="Option.None{TOk}"/>
    /// </summary>
    /// <typeparam name="TOk">The type of the ok value.</typeparam>
    /// <returns>A valid result that has no value.</returns>
    public static Result<Option<TOk>, TError> OkNone<TOk, TError>()
    {
        return new Result<Option<TOk>, TError>(Option.None<TOk>(), default);
    }

    /// <summary>
    /// Returns an ok result where <paramref name="value"/> is optional. IsOk is always true. If <paramref name="value"/> is null OK is <see cref="Option.None{TOk}"/>
    /// </summary>
    /// <typeparam name="TOk">The type of the ok value.</typeparam>
    /// <param name="value">the ok value.</param>
    /// <returns>A valid result that can have a value.</returns>
    public static Result<Option<TOk>, TError> OkOrNone<TOk, TError>(TOk? value)
    {
        return value is not null
            ? new Result<Option<TOk>, TError>(Option.Some(value), default)
            : new Result<Option<TOk>, TError>(Option.None<TOk>(), default);
    }
}

/// <summary>
/// This is a result that can be only Error.
/// </summary>
/// <typeparam name="TError"></typeparam>
public readonly struct Result<TError>
    : IResult<TError>
    , IEquatable<Result<TError>>
{
    private readonly TError _error;
    private readonly bool _isError;

    internal Result(TError error)
    {
        _error = error.ThrowIfNull();
        _isError = true;
    }

    public static implicit operator Result<TError>(TError error) => Result.Error<TError>(error);

    /// <summary>
    /// Checks equality of two results.
    /// </summary>
    /// <param name="left">A result.</param>
    /// <param name="right">A result.</param>
    /// <returns>True if both results are equal.</returns>
    public static bool operator ==(Result<TError> left, Result<TError> right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Checks unequality of two results.
    /// </summary>
    /// <param name="left">A result.</param>
    /// <param name="right">A result.</param>
    /// <returns>True if both results are not equal.</returns>
    public static bool operator !=(Result<TError> left, Result<TError> right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Result<TError> other && Equals(other);

    public bool Equals(Result<TError> other)
    {
        if (IsOk) return other.IsOk;

        return !other.IsOk
            && EqualityComparer<TError>.Default.Equals(_error, other._error);
    }

    /// <inheritdoc/>
#if NETSTANDARD2_0
    public override int GetHashCode() => IsOk 
                                         ? typeof(Result<TError>).GetHashCode()
                                         : Foundation.HashCode.FromObject(typeof(Result<TError>), _error);
#else
    public override int GetHashCode() => IsOk 
                                         ? typeof(Result<TError>).GetHashCode()
                                         : System.HashCode.Combine(typeof(Result<TError>), _error);
#endif
    /// <summary>
    /// Is true if Result has a value <see cref="IsOk"/> otherwise false;
    /// </summary>
    public bool IsOk => !_isError;

    public override string ToString()
        => IsOk ? $"IsOk: {IsOk}" : $"IsOk: {IsOk}, Error: {_error}";

    /// <summary>
    /// Returns true if result contains an error.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public bool TryGetError([NotNullWhen(true)] out TError? error)
    {
        if(!IsOk)
        {
            error = _error;
            return _error is not null;
        }

        error = default;
        return false;
    }
}

[Serializable]
public readonly struct Result<TOk, TError>
    : IResult<TOk, TError>
    , IEquatable<Result<TOk, TError>>
{
    private readonly int _hashCode;
    private readonly Option<TOk> _ok;
    private readonly TError? _error;

    internal Result(Option<TOk> ok, TError? error)
    {
        if (ok.IsNone && error is null) throw new ArgumentNullException(nameof(error));

        _ok = ok;
        _error = error;

#if NETSTANDARD2_0
        _hashCode = ok.IsSome
            ? Foundation.HashCode.FromObject(typeof(Result<TOk, TError>), ok.OrThrow())
            : Foundation.HashCode.FromObject(typeof(Result<TOk, TError>), error);
#else
        _hashCode = ok.IsSome
            ? System.HashCode.Combine(typeof(Result<TOk, TError>), ok.OrThrow())
            : System.HashCode.Combine(typeof(Result<TOk, TError>), error);
#endif
    }

    public static implicit operator Result<TOk, TError>(TError error) => Result.Error<TOk, TError>(error);

    public static implicit operator Result<TOk, TError>(TOk ok) => Result.Ok<TOk, TError>(ok);

    public static bool operator ==(Result<TOk, TError> left, Result<TOk, TError> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Result<TOk, TError> left, Result<TOk, TError> right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
        => obj is Result<TOk, TError> other && Equals(other);

    public bool Equals(Result<TOk, TError> other)
    {
        if (IsOk)
        {
            if (!other.IsOk) return false;
            return _ok.Equals(other._ok);
        }
        
        if(other.IsOk) return false;

        return _error.EqualsNullable(other._error);
    }

    public override int GetHashCode() => _hashCode;

    public bool IsOk => _ok.IsSome;

    public override string ToString()
    {
        return IsOk
            ? $"IsOk: {IsOk}, Ok: {_ok.OrThrow()}"
            : $"IsOk: {IsOk}, Error: {_error}";
    }

    /// <summary>
    /// Returns true if result contains an error otherwise returns false and error has the default value.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public bool TryGetError([NotNullWhen(true)] out TError? error)
    {
        if(!IsOk)
        {
            if(_error is not null)
            {
                error = _error;
                return true;
            }
        }

        error = default;
        return false;
    }

    /// <summary>
    /// Returns true if result contains a value otherwise false and ok is set to the default value.
    /// </summary>
    /// <param name="ok"></param>
    /// <returns></returns>
    public bool TryGetOk([NotNullWhen(true)] out TOk? ok)
    {
        if (IsOk)
        {
            ok = _ok.OrThrow();
            return true;
        }

        ok = default;
        return false;
    }
}
