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
/// This class can be used to avoid state.
/// </summary>
public static class Scope
{
    /// <summary>
    /// Returns a value from a scope. This can be used to avoid state.
    /// This enables e.g. return a value from an if or foreach statement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ret"></param>
    /// <returns></returns>
    public static T Returns<T>(Func<T> ret)
    {
        ret.ThrowIfNull();
        return ret();
    }

    /// <summary>
    /// Returns a value asynchronous from a scope. This can be used to avoid state.
    /// This enables e.g. return a value from an if or foreach statement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ret"></param>
    /// <returns></returns>
    public static async Task<T> ReturnsAsync<T>(Func<Task<T>> ret)
    {
        ret.ThrowIfNull();
        return await ret();
    }

    /// <summary>
    /// Returns a list of values from a scope. This can be used to avoid state. This enables e.g. return values from an if or foreach statement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ret"></param>
    /// <returns></returns>
    public static IEnumerable<T> ReturnsMany<T>(Func<IEnumerable<T>> ret)
    {
        ret.ThrowIfNull();
        return ret();
    }

    /// <summary>
    /// Returns an nullable from a scope. This can be used to avoid state. This enables e.g. return a value from an if or foreach statement.
    /// </summary>
    /// </summary>
    /// <typeparam name="TOk"></typeparam>
    /// <param name="ret"></param>
    /// <returns></returns>
    public static T? ReturnsNullable<T>(Func<T?> ret)
    {
        ret.ThrowIfNull();
        return ret();
    }

    /// <summary>
    /// Returns an option from a scope. This can be used to avoid state. This enables e.g. return a value from an if or foreach statement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="ret"></param>
    /// <returns></returns>
    public static Option<T> ReturnsOption<T>(Func<Option<T>> ret)
    {
        ret.ThrowIfNull();
        return ret();
    }

    /// <summary>
    /// Returns an option from a scope. If decision true ret is executed and returned.
    /// This can be used to avoid state. This enables e.g. return a value from an if or foreach statement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="decision">If true ret is executed and returned.</param>
    /// <param name="ret"></param>
    /// <returns></returns>
    public static Option<T> ReturnsOption<T>(Func<bool> decision, Func<T> ret)
    {
        ret.ThrowIfNull();

        if (decision()) return Option.Maybe(ret());
        return Option.None<T>();
    }

    /// <summary>
    ///  Returns a <see cref="Result<typeparamref name="TOk"/>,<typeparamref name="TError"/>"/> from a scope.
    ///  This can be used to avoid state. This enables e.g. return a value from an if or foreach statement.
    /// </summary>
    /// <typeparam name="TOk"></typeparam>
    /// <typeparam name="TError"></typeparam>
    /// <param name="ret"></param>
    /// <returns></returns>
    public static Result<TOk, TError> ReturnsResult<TOk, TError>(Func<Result<TOk, TError>> ret)
    {
        ret.ThrowIfNull();
        return ret();
    }
}
