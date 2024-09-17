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
﻿using System.Runtime.CompilerServices;

namespace Foundation;
public static class IdExtensions
{
    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if <paramref name="id"/> is empty.
    /// </summary>
    /// <param name="id">The identifier which should be checked.</param>
    /// <param name="paramName">The name of the caller.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Is thrown if IsEmpty of id returns true.</exception>
    public static Id ThrowIfEmpty(this Id id, [CallerArgumentExpression(nameof(id))] string paramName = "")
    {
        return ThrowIfEmpty(id, () => throw new ArgumentNullException(paramName));
    }

    /// <summary>
    /// Throws an <see cref="Exception"/> if <paramref name="id"/> is empty.
    /// </summary>
    /// <param name="id">The identifier which should be checked.</param>
    /// <param name="exeption">The exception which is thrown on IsEmtpy is true.</param>
    /// <returns></returns>
    public static Id ThrowIfEmpty(this Id id, Func<Exception> exeption)
    {
        return id.IsEmpty ? throw exeption() : id;
    }
}
