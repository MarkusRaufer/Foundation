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
/// Provides static factory methods for creating instances of the NullableResult type with either a successful value or
/// an error result.
/// </summary>
/// <remarks>This class is intended to simplify the creation of NullableResult objects by clearly indicating
/// whether the result represents a successful value or an error. It is commonly used in scenarios where an operation
/// may succeed with a value or fail with an error, and where the absence of a value is a valid outcome.</remarks>
public static class NullableResult
{
    /// <summary>
    /// Creates a new instance of the <see cref="NullableResult{T, TError}"/> class representing a failed result with the specified error.
    /// </summary>
    /// <typeparam name="TOk">The type of the successful result value.</typeparam>
    /// <param name="error">The error associated with the failed result.</param>
    /// <returns>A <see cref="NullableResult{T, TError}"/> representing the failed result.</returns>
    /// <returns></returns>
    public static NullableResult<TOk, Error> Error<TOk>(Error error) => new(error);

    /// <summary>
    /// Creates a new NullableResult instance that represents an error state with the specified error value.
    /// </summary>
    /// <typeparam name="TOk">The type of the successful result value.</typeparam>
    /// <typeparam name="TError">The type of the error value.</typeparam>
    /// <param name="error">The error value to associate with the result. This value represents the failure state.</param>
    /// <returns>A NullableResult instance containing the specified error value and no successful result.</returns>
    public static NullableResult<TOk, TError> Error<TOk, TError>(TError error) => new(error);

    /// <summary>
    /// Creates a successful NullableResult containing the specified value.
    /// </summary>
    /// <typeparam name="TOk">The type of the value to be wrapped in the result.</typeparam>
    /// <param name="ok">The value to be wrapped as a successful result. Can be null if TOk is a reference or nullable type.</param>
    /// <returns>A NullableResult containing the specified value as a successful result.</returns>
    public static NullableResult<TOk, Error> Ok<TOk>(TOk? ok) => new(ok);

    /// <summary>
    /// Creates a new successful result containing the specified value.
    /// </summary>
    /// <typeparam name="TOk">The type of the value representing a successful result.</typeparam>
    /// <typeparam name="TError">The type of the value representing an error result.</typeparam>
    /// <param name="ok">The value to store in the successful result. Can be null if the type permits null values.</param>
    /// <returns>A NullableResult containing the specified successful value.</returns>
    public static NullableResult<TOk, TError> Ok<TOk, TError>(TOk? ok) => new(ok);
}

/// <summary>
/// This result type allows null as a valid result ok.
/// </summary>
/// <typeparam name="TOk">The type of the result ok.</typeparam>
/// <typeparam name="TError">The type of the error ok.</typeparam>
public struct NullableResult<TOk, TError> : IResult<TOk, TError>
{
    private readonly TError? _error;
    private readonly TOk? _ok;

    /// <summary>
    /// Sets the result ok.
    /// </summary>
    /// <param name="value">The ok of the result.</param>
    public NullableResult(TOk? value)
    {
        IsOk = true;
        _ok = value;
    }

    /// <summary>
    /// Initializes a new instance of the NullableResult class that represents a failed result with the specified error.
    /// </summary>
    /// <param name="error">The error ok associated with the failed result. Cannot be null.</param>
    public NullableResult(TError error)
    {
        _error = error.ThrowIfNull();
        IsOk = false;
    }

    /// <summary>
    /// Converts a ok of type <typeparamref name="TOk"/> to a <see cref="NullableResult{T, TError}"/> representing a
    /// successful result.
    /// </summary>
    /// <remarks>This implicit conversion allows a ok of type <typeparamref name="TOk"/> to be used where a
    /// <see cref="NullableResult{T, TError}"/> is expected, treating the ok as a successful result. If <paramref
    /// name="value"/> is null and <typeparamref name="TOk"/> is a reference type, the resulting <see
    /// cref="NullableResult{T, TError}"/> will represent a null ok.</remarks>
    /// <param name="value">The ok to convert to a successful <see cref="NullableResult{T, TError}"/>.</param>
    public static implicit operator NullableResult<TOk, TError>(TOk value) => new(value);

    /// <summary>
    /// Inidcates that a ok is set and no error. The ok can be null.
    /// </summary>
    public bool IsOk { get; }

    /// <summary>
    /// Returns the ok if IsOk is true, otherwise returns false.
    /// </summary>
    /// <param name="ok"></param>
    /// <returns></returns>
    public readonly bool TryGetOk(out TOk? ok)
    {
        ok = default;
        if (!IsOk) return false;

        ok = _ok;
        return true;
    }

    /// <summary>
    /// Attempts to retrieve the error ok if <see cref="IsOk"/> is false.
    /// </summary>
    /// <param name="error">When this method returns, contains the error ok if the result is an error; otherwise, the default ok for
    /// the type.</param>
    /// <returns>true if the result is an error and the error ok was retrieved; otherwise, false.</returns>
    public readonly bool TryGetError(out TError? error)
    {
        error = default;
        if (IsOk) return false;

        if (_error is TError err)
        {
            error = err;
            return true;
        }
            
        return false;
    }
}
