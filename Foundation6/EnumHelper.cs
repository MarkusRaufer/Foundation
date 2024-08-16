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
public static class EnumHelper
{
    /// <summary>
    /// Returns a string of an enum value.
    /// </summary>
    /// <typeparam name="T">Type of the enum value.</typeparam>
    /// <param name="value">The enum value.</param>
    /// <param name="nameAsValue">If true the name of the enum value is returned.
    /// If false the value representing an enum name is returned as string.
    /// </param>
    /// <returns>The enum value.</returns>
    public static string ToString<T>(T value, bool nameAsValue) where T : Enum
    {
        return nameAsValue ? $"{value}" : $"{Convert.ChangeType(value, Type.GetTypeCode(typeof(T)))}";
    }
}
