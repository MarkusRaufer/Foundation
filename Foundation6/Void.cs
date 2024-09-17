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
public static class Void
{
    /// <summary>
    /// Enables a void method to return always a value.
    /// </summary>
    /// <typeparam name="T">The type of the returned value.</typeparam>
    /// <param name="value">The value which will be returned after execution.</param>
    /// <param name="action">The action which should be executed.</param>
    /// <returns>The func.</returns>
    public static T Returns<T>(T value, Action action)
    {
        action.ThrowIfNull();
        action();
        return value;
    }

    /// <summary>
    /// Enables a void method to return always a value.
    /// </summary>
    /// <typeparam name="T">The type of the returned value.</typeparam>
    /// <param name="func">The delegate which will return a value after execution.</param>
    /// <param name="action">The action which should be executed.</param>
    /// <returns>The func.</returns>
    public static T Returns<T>(Func<T> func, Action action)
    {
        action.ThrowIfNull();
        action();
        return func();
    }

    /// <summary>
    /// Enables a void method to return always a value.
    /// </summary>
    /// <typeparam name="T">The type of the returned value.</typeparam>
    /// <param name="func">The delegate which will return a value after execution.</param>
    /// <param name="action">The action which should be executed.</param>
    /// <returns>The func.</returns>
    public static T Returns<T>(Func<T> func, Action<T> action)
    {
        action.ThrowIfNull();
        var value = func();
        action(value);
        return value;
    }
}
