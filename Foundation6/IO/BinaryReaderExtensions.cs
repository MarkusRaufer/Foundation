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
using Foundation.Reflection;
using System.Reflection;

namespace Foundation.IO
{
    public static class BinaryReaderExtensions
    {
        public static DateOnly ReadDateOnly(this BinaryReader reader)
        {
            var dateTime = reader.ReadDateTime();
            return DateOnly.FromDateTime(dateTime);
        }

        public static DateTime ReadDateTime(this BinaryReader reader)
        {
            var ticks = reader.ReadInt64();
            return new DateTime(ticks);
        }

        public static IEnumerable<(MemberInfo member, object value)> ReadFromMembers(
            this BinaryReader reader,
            IEnumerable<MemberInfo> members)
        {
            foreach (var member in members)
            {
                var memberType = member.GetMemberType();
                if (memberType is null) throw new ArgumentException($"member {member} not found");

                var value = reader.ReadSystemType(memberType);
                yield return (member, value);
            }
        }

        public static IEnumerable<(PropertyInfo member, object value)> ReadFromProperties(
            this BinaryReader reader,
            IEnumerable<PropertyInfo> properties)
        {
            foreach (var property in properties)
            {
                var value = reader.ReadSystemType(property.PropertyType);
                yield return (property, value);
            }
        }

        /// <summary>
        /// Reads from a type which is included in the System namespace. E.g. bool, byte, char, ...
        /// </summary>
        /// <param name="reader">An instance of a <see cref="BinaryReader"/></param>
        /// <param name="type">Type is included in the System namesapce.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static object ReadSystemType(this BinaryReader reader, Type type)
        {
            return type switch
            {
                Type _ when type == typeof(bool) =>  reader.ReadBoolean(),
                Type _ when type == typeof(byte) => reader.ReadByte(),
                Type _ when type == typeof(char) => reader.ReadChar(),
                Type _ when type == typeof(decimal) => reader.ReadDecimal(),
                Type _ when type == typeof(double) => reader.ReadDouble(),
                Type _ when type == typeof(float) => reader.ReadSingle(),
                Type _ when type == typeof(int) => reader.ReadInt32(),
                Type _ when type == typeof(long) => reader.ReadInt64(),
                Type _ when type == typeof(sbyte) => reader.ReadSByte(),
                Type _ when type == typeof(short) => reader.ReadInt16(),
                Type _ when type == typeof(string) => reader.ReadString(),
                Type _ when type == typeof(uint) => reader.ReadUInt32(),
                Type _ when type == typeof(ulong) => reader.ReadUInt64(),
                Type _ when type == typeof(ushort) => reader.ReadUInt16(),
                // --> custom types
                Type _ when type == typeof(DateOnly) => reader.ReadDateOnly(),
                Type _ when type == typeof(TimeOnly) => reader.ReadTimeOnly(),
                Type _ when type == typeof(DateTime) => reader.ReadDateTime(),
                Type _ when type == typeof(Guid) => reader.ReadGuid(),
                // <-- custom types
                Type _ => throw new NotImplementedException($"{type}")
            };
        }

        public static IEnumerable<(Type type, object value)> ReadSystemTypes(
            this BinaryReader reader,
            IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                yield return (type, reader.ReadSystemType(type));
            }
        }

        public static Guid ReadGuid(this BinaryReader reader)
        {
            var bytes = reader.ReadBytes(16);
            return new Guid(bytes);
        }

        public static TimeOnly ReadTimeOnly(this BinaryReader reader)
        {
            var ticks = reader.ReadInt64();
            return new TimeOnly(ticks);
        }

        /// <summary>
        /// Reads the values from stream and sets the values of the properties of obj.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="obj"></param>
        public static void ReadToObject(this BinaryReader reader, object obj)
        {
            obj.ThrowIfNull();

            ReadToObject(reader, obj, obj.GetType().GetMembers());
        }

        /// <summary>
        /// Reads the values from stream and sets the values of the properties of obj.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="obj">An instance of an object.</param>
        /// <param name="memberNames">The names of the objects properties.</param>
        public static void ReadToObject(this BinaryReader reader, object obj, IEnumerable<string> memberNames)
        {
            var type = obj.ThrowIfNull().GetType();

            var members = memberNames.FilterMap(name => type.GetMember(name).FirstAsOption());
            ReadToObject(reader, obj, members);
        }

        /// <summary>
        /// Reads the values from stream and sets the values of the properties of obj.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="obj"></param>
        /// <param name="members"></param>
        public static void ReadToObject<T>(this BinaryReader reader, T? obj, IEnumerable<MemberInfo> members)
        {
            obj.ThrowIfNull();

            foreach (var (member, value) in ReadFromMembers(reader, members))
            {
                ReflectionHelper.SetValue(obj, member, value);
            }
        }

        public static IEnumerable<(MemberInfo member, object value)> ReadTypeMembers(this BinaryReader reader, Type type)
        {
            reader.ThrowIfNull();
            type.ThrowIfNull();

            foreach (var (member, value) in ReadFromMembers(reader, type.GetMembers()))
            {
                yield return (member, value);
            }
        }

        public static IEnumerable<(PropertyInfo property, object value)> ReadTypeProperties(this BinaryReader reader, Type type)
        {
            reader.ThrowIfNull();
            type.ThrowIfNull();

            foreach (var (property, value) in ReadFromProperties(reader, type.GetProperties()))
            {
                yield return (property, value);
            }
        }
    }
}
