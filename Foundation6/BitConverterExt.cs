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
﻿using System.Collections;
using System.Runtime.InteropServices;

namespace Foundation;

public static class BitConverterExt
{
    /// <summary>
    /// Creates a list of bytes from <paramref name="value"/>
    /// </summary>
    /// <param name="value"></param>
    /// <returns>a byte array with the converted decimal value.</returns>
    public static byte[] GetBytes(decimal value)
    {
        ReadOnlySpan<int> bits = decimal.GetBits(value);
        var bytes = MemoryMarshal.Cast<int, byte>(bits);

        return bytes.ToArray();
    }

    /// <summary>
    /// Creates a decimal value from a list of <paramref name="bytes"/>.
    /// </summary>
    /// <param name="bytes">Size must be exactly 16 bytes.</param>
    /// <returns></returns>
    public static decimal ToDecimal(byte[]? bytes)
    {
        bytes = bytes.ThrowIfNull();
        bytes.ThrowIf(() => 16 != bytes!.Length, "A decimal must be created from exactly 16 bytes");

        //make an array to convert back to int32's
        var bits = new Int32[4];
        for (int i = 0; i <= 15; i += 4)
        {
            //convert every 4 bytes into an int32
            bits[i / 4] = BitConverter.ToInt32(bytes, i);
        }

        return new decimal(bits);
    }
}

