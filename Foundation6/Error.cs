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
/// This is a generic Error type. Use this if a method can return different kind of errors like exceptions, error messages...
/// </summary>
/// <param name="Id"></param>
/// <param name="Message"></param>
/// <param name="InnerErrors"></param>
public sealed record Error(string Id, string Message, Error[]? InnerErrors = null)
{
    /// <summary>
    /// Creates an Error instance from an exception.
    /// </summary>
    /// <param name="exception">The exception and all inner exceptions are fetched and created as a list of Errors.</param>
    /// <returns>An Error where the Id is the exception type.</returns>
    public static Error FromException(Exception exception)
    {
        var id = exception.GetType().Name;

        var innerErrors = exception.Flatten()
                                   .Select(x => FromException(x))
                                   .ToArray();

        return new Error(id, exception.Message, innerErrors);
    }

    /// <summary>
    /// Creates an Error instance from a list of exceptions.
    /// </summary>
    /// <param name="id">The error id which is typically an exception type name.</param>
    /// <param name="message">The error message.</param>
    /// <param name="exceptions">A list of exceptions and all their inner exceptions.</param>
    /// <returns>An Error where the Id is the exception type.</returns>
    public static Error FromExceptions(string id, string message, IEnumerable<Exception> exceptions)
    {
        var errors = exceptions.Select(FromException).ToArray();
        return new Error(id, message, errors);
    }

    /// <summary>
    /// Creates an Error instance from a specific type.
    /// </summary>
    /// <typeparam name="T">The type of the error.</typeparam>
    /// <param name="error">The error instance.</param>
    /// <param name="selector">A selector for the error message.</param>
    /// <returns>An Error where the Id is the error type.</returns>
    public static Error FromType<T>(T error, Func<T, string> selector)
    {
        var id = error.ThrowIfNull().GetType().Name;

        return new Error(id, selector(error));
    }
}
