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

public static class FuncHelper
{
    public static Func<object, object?> CreateObjectFunc<T, TResult>(Func<T, TResult> func)
    {
        func.ThrowIfNull();

        return x => x is T t ? func(t) : null;
    }

    public static Func<object, bool> CreateObjectPredicate<T>(Func<T, bool> func)
    {
        return t => func((T)t);
    }

    public static Func<string, object>? StringToScalarValueConverter(Type type)
    {
        type.ThrowIfNull();

        if (!type.IsScalar()) return null;

        return Type.GetTypeCode(type) switch
        {
            TypeCode.Boolean => str => bool.TryParse(str, out bool result) ? result : default,
            TypeCode.Byte => str => byte.TryParse(str, out byte result) ? result : default,
            TypeCode.Char => str => char.TryParse(str, out char result) ? result : default,
            TypeCode.DateTime => str => DateTime.TryParse(str, out DateTime result) ? result : default,
            TypeCode.Decimal => str => decimal.TryParse(str, out decimal result) ? result : default,
            TypeCode.Double => str => double.TryParse(str, out double result) ? result : default,
            TypeCode.Int16 => str => short.TryParse(str, out short result) ? result : default,
            TypeCode.Int32 => str => int.TryParse(str, out int result) ? result : default,
            TypeCode.Int64 => str => long.TryParse(str, out long result) ? result : default,
            TypeCode.SByte => str => SByte.TryParse(str, out SByte result) ? result : default,
            TypeCode.Single => str => Single.TryParse(str, out Single result) ? result : default,
            TypeCode.String => str => str,
            TypeCode.UInt16 => str => ushort.TryParse(str, out ushort result) ? result : default,
            TypeCode.UInt32 => str => uint.TryParse(str, out uint result) ? result : default,
            TypeCode.UInt64 => str => ulong.TryParse(str, out ulong result) ? result : default,
            _ => null
        };
    }
}

