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

using Foundation.Collections.Generic;

public static class ExceptionExtensions
{
    /// <summary>
    /// Returns the exception and all inner exceptions.
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public static IEnumerable<Exception> Flatten(this Exception? e)
    {
        if (null == e) yield break;

        yield return e;
        foreach (var ex in Flatten(e.InnerException))
        {
            yield return ex;
        }
    }

    /// <summary>
    /// Returns the exception message and all inner exception messages.
    /// </summary>
    /// <param name="exception"></param>
    /// <param name="separator">The separator for each exception message.</param>
    /// <returns></returns>
    public static string FlattenedMessages(this Exception? exception, string separator = ", \n")
    {
        return Flatten(exception).Select(e => e.Message)
                                 .ToReadableString(separator);
    }
}
