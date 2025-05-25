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
﻿using System.Data;

namespace Foundation.Data;

public static class TypeExtensions
{
    public static DbType ToDbType(this Type type)
    {
        return type switch
        {
            Type t when t == typeof(bool) => DbType.Boolean,
            Type t when t == typeof(byte) => DbType.Byte,
            Type t when t == typeof(sbyte) => DbType.SByte,
            Type t when t == typeof(DateOnly) => DbType.Date,
            Type t when t == typeof(DateTime) => DbType.DateTime,
            Type t when t == typeof(DateTimeOffset) => DbType.DateTimeOffset,
            Type t when t == typeof(decimal) => DbType.Decimal,
            Type t when t == typeof(double) => DbType.Double,
            Type t when t == typeof(Guid) => DbType.Guid,
            Type t when t == typeof(Int16) => DbType.Int16,
            Type t when t == typeof(Int32) => DbType.Int32,
            Type t when t == typeof(Int64) => DbType.Int64,
            Type t when t == typeof(Single) => DbType.Single,
            Type t when t == typeof(string) => DbType.String,
            Type t when t == typeof(TimeOnly) => DbType.Time,
            Type t when t == typeof(UInt16) => DbType.UInt16,
            Type t when t == typeof(UInt32) => DbType.UInt32,
            Type t when t == typeof(UInt64) => DbType.UInt64,
            _ => DbType.Binary,
        };
    }
}
