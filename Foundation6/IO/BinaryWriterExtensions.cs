using Foundation.Collections.Generic;
using System.Reflection;

namespace Foundation.IO
{
    public static class BinaryWriterExtensions
    {
        public static Result<bool, NotImplementedException> Write(this BinaryWriter writer, object? value)
        {
            switch (value)
            {
                case bool boolean: writer.Write(boolean); break;
                case byte byteValue: writer.Write(byteValue); break;
                case byte[] bytes: writer.Write(bytes); break;
                case char ch: writer.Write(ch); break;
                case char[] chars: writer.Write(chars); break;
                case DateTime dt: writer.Write(dt); break;
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
                default: return Result.Error<bool, NotImplementedException>(new NotImplementedException($"type {value?.GetType()} of {nameof(value)}"));
            }

            return Result.Ok<bool, NotImplementedException>(true);
        }

        public static void Write(this BinaryWriter writer, DateTime dt)
        {
            writer.Write(dt.Ticks);
        }

        public static Result<bool, NotImplementedException> WriteObject(this BinaryWriter writer, object obj)
        {
            var type = obj.GetType();
            return WriteObject(writer, obj, type.GetMembers());
        }

        /// <summary>
        /// Writes all members of obj.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="obj">The object including the values.</param>
        /// <param name="memberNames">The names of the members.</param>
        /// <returns></returns>
        public static Result<bool, NotImplementedException> WriteObject(this BinaryWriter writer, object obj, IEnumerable<string> memberNames)
        {
            var type = obj.GetType();
            return WriteObject(writer, obj, memberNames.FilterMap(name => type.GetMember(name).FirstAsOpt()));
        }

        /// <summary>
        /// Writes all members of obj.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="obj">The object including the values.</param>
        /// <param name="members">The members of the object.</param>
        /// <returns></returns>
        public static Result<bool, NotImplementedException> WriteObject(this BinaryWriter writer, object? obj, IEnumerable<MemberInfo> members)
        {
            foreach (var member in members)
            {
                var value = member switch
                {
                    FieldInfo fi => fi.GetValue(obj),
                    PropertyInfo pi => pi.GetValue(obj),
                    _ => null
                };

                var result = writer.Write(value);
                if (result.IsError) return result;
            }

            return Result.Ok<bool, NotImplementedException>(true);
        }
    }
}
