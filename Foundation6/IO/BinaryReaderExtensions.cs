using Foundation.Collections.Generic;
using Foundation.Reflection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Foundation.IO
{
    public static class BinaryReaderExtensions
    {
        public static DateTime ReadDateTime(this BinaryReader reader)
        {
            var ticks = reader.ReadInt64();
            return new DateTime(ticks);
        }

        public static object ReadFromType(this BinaryReader reader, Type type)
        {
            return type switch
            {
                Type _ when type == typeof(bool) =>  reader.ReadBoolean(),
                Type _ when type == typeof(byte) => reader.ReadByte(),
                Type _ when type == typeof(char) => reader.ReadChar(),
                Type _ when type == typeof(DateTime) => reader.ReadDateTime(),
                Type _ when type == typeof(decimal) => reader.ReadDecimal(),
                Type _ when type == typeof(double) => reader.ReadDouble(),
                Type _ when type == typeof(float) => BitConverter.ToSingle(reader.ReadBytes(sizeof(float)), 0),
                Type _ when type == typeof(int) => reader.ReadInt32(),
                Type _ when type == typeof(long) => reader.ReadInt64(),
                Type _ when type == typeof(sbyte) => reader.ReadSByte(),
                Type _ when type == typeof(short) => reader.ReadInt16(),
                Type _ when type == typeof(string) => reader.ReadString(),
                Type _ when type == typeof(uint) => reader.ReadUInt32(),
                Type _ when type == typeof(ulong) => reader.ReadUInt64(),
                Type _ when type == typeof(ushort) => reader.ReadUInt16(),
                Type _ => throw new NotImplementedException($"{type}")
            };
        }

        public static IEnumerable<(Type type, object value)> ReadFromTypes(this BinaryReader reader, params Type[] types)
        {
            foreach (var type in types)
            {
                yield return (type, reader.ReadFromType(type));
            }
        }

        public static void ReadObject(this BinaryReader reader, object obj)
        {
            obj.ThrowIfNull();

            ReadObject(reader, obj, obj.GetType().GetMembers());
        }

        public static void ReadObject(this BinaryReader reader, object obj, IEnumerable<string> memberNames)
        {
            obj.ThrowIfNull();

            var type = obj.GetType();
            var members = memberNames.FilterMap(name => type.GetMember(name).FirstAsOpt());
            ReadObject(reader, obj, members);
        }

        public static void ReadObject(this BinaryReader reader, object obj, IEnumerable<MemberInfo> members)
        {
            obj.ThrowIfNull();

            foreach (var member in members)
            {
                var memberType = member.GetMemberType();
                var value = reader.ReadFromType(memberType);
                ReflectionHelper.SetValue(obj, member, value);
            }
        }
    }
}
