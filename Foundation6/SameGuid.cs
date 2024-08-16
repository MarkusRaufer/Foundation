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
﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Foundation;

/// <summary>
/// Creates the same Guid when using the same seed value.
/// </summary>
public static class SameGuid
{
    /// <summary>
    /// Creates a same Guids if using same seed value.
    /// This is needed for tests.
    /// </summary>
    /// <param name="seed">The value to create a Guid.</param>
    /// <returns>A Guid value.</returns>
    public static Guid New(object seed)
    {
#if NET6_0_OR_GREATER
            byte[] hash = MD5.HashData(Encoding.Default.GetBytes($"{seed}"));
#else
        using MD5 md5 = MD5.Create();

        byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes($"{seed}"));
#endif
        return new Guid(hash);
    }
}
