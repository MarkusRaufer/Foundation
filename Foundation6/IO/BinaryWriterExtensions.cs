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
﻿using Foundation.Collections.Generic;
using System.Reflection;

namespace Foundation.IO
{
    public static class BinaryWriterExtensions
    {
        public static void Write(this BinaryWriter writer, object? value)
        {
            switch (value)
            {
                case bool boolean: writer.Write(boolean); break;
                case byte byteValue: writer.Write(byteValue); break;
                case byte[] bytes: writer.Write(bytes); break;
                case char ch: writer.Write(ch); break;
                case char[] chars: writer.Write(chars); break;
                case decimal decimalValue: writer.Write(decimalValue); break;
                case double doubleValue: writer.Write(doubleValue); break;
                case float floatValue: writer.Write(floatValue); break;
                case int i32: writer.Write(i32); break;
                case long i64: writer.Write(i64); break;
                case sbyte sbyteValue: writer.Write(sbyteValue); break;
                case short shortValue: writer.Write(shortValue); break;
                case string str: writer.Write(str); break;
                case uint u32: writer.Write(u32); break;
                case ulong u64: writer.Write(u64); break;
                case ushort u16: writer.Write(u16); break;
                // --> custom types
                case DateOnly dateOnly: writer.Write(dateOnly); break;
                case TimeOnly timeOnly: writer.Write(timeOnly); break;
                case DateTime dt: writer.Write(dt); break;
                case Guid guid: writer.Write(guid); break;
                // <-- custom types
                default:
                   throw new NotSupportedException($"{nameof(value)} of type {value?.GetType()}");
            }
        }

        public static void Write(this BinaryWriter writer, DateOnly date)
        {
            writer.Write(date.ToDateTime().Ticks);
        }

        public static void Write(this BinaryWriter writer, DateTime dt)
        {
            writer.Write(dt.Ticks);
        }

        public static void Write(this BinaryWriter writer, Guid guid)
        {
            writer.Write(guid.ToByteArray());
        }

#if NET6_0_OR_GREATER
        public static void Write(this BinaryWriter writer, TimeOnly time)
        {
            writer.Write(time.Ticks);
        }
#endif

        public static void WriteObject(this BinaryWriter writer, object obj)
        {
            var type = obj.GetType();
            WriteObject(writer, obj, type.GetMembers().Where(mi => mi is FieldInfo or PropertyInfo));
        }

        /// <summary>
        /// Writes all members of obj.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="obj">The object including the values.</param>
        /// <param name="memberNames">The names of the members.</param>
        /// <returns></returns>
        public static void WriteObject(
            this BinaryWriter writer,
            object obj,
            IEnumerable<string> memberNames)
        {
            var type = obj.GetType();
            WriteObject(writer, obj, memberNames.FilterMap(name => type.GetMember(name).FirstAsOption()));
        }

        /// <summary>
        /// Writes all members of obj.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="obj">The object including the values.</param>
        /// <param name="members">The members of the object.</param>
        /// <returns></returns>
        public static void WriteObject(
            this BinaryWriter writer, object? obj,
            IEnumerable<MemberInfo> members)
        {
            foreach (var member in members)
            {
                var value = member switch
                {
                    FieldInfo fi => fi.GetValue(obj),
                    PropertyInfo pi => pi.GetValue(obj),
                    _ => throw new NotSupportedException($"{member}")
                };

                writer.Write(value);
            }
        }
    }
}
