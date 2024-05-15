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
#if NET6_0_OR_GREATER﻿
using System.Text.Json;

namespace Foundation.Text.Json;

public static class Utf8JsonWriterExtensions
{
    public static void WriteValue(this Utf8JsonWriter writer, object? value)
    {
        if (null == value)
        {
            writer.WriteNullValue();
            return;
        }
        var type = value.GetType();
        var typeCode = Type.GetTypeCode(type);

        switch (typeCode)
        {
            case TypeCode.Boolean: writer.WriteBooleanValue((bool)value); return;
            case TypeCode.DateTime: writer.WriteStringValue((DateTime)value); return;
            case TypeCode.Decimal: writer.WriteNumberValue((decimal)value); return;
            case TypeCode.Double: writer.WriteNumberValue((double)value); return;
            case TypeCode.Int16: writer.WriteNumberValue((Int16)value); return;
            case TypeCode.Int32: writer.WriteNumberValue((int)value); return;
            case TypeCode.Int64: writer.WriteNumberValue((long)value); return;
            case TypeCode.UInt16: writer.WriteNumberValue((UInt16)value); return;
            case TypeCode.UInt32: writer.WriteNumberValue((uint)value); return;
            case TypeCode.UInt64: writer.WriteNumberValue((ulong)value); return;
            case TypeCode.Single: writer.WriteNumberValue((float)value); return;
            case TypeCode.String: writer.WriteStringValue((string)value); return;
        }

        switch(value)
        {
            case DateOnly x: writer.WriteStringValue($"{x:yyyy-MM-dd}"); return;
            case Guid x: writer.WriteStringValue(x); return;
            case TimeOnly x: writer.WriteStringValue($"{x:HH:mm:ss}"); return;
        };
        writer.WriteStringValue($"{value}");
    }
}
#endif