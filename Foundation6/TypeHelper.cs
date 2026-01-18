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
using System.Reflection;

namespace Foundation;

public static class TypeHelper
{
 
    /// <summary>
    /// Checks if all types are different.
    /// </summary>
    /// <param name="types">List of types.</param>
    /// <returns>true if all types are different otherwise false.</returns>
    public static bool AreAllDifferent(params ICollection<Type> types) => types.Count == new HashSet<Type>(types).Count;

    /// <summary>
    /// Checks if all types are equal.
    /// </summary>
    /// <param name="types">List of types</param>
    /// <returns>true if all types are equal otherwise false.</returns>
    public static bool AreAllEqual(params ICollection<Type> types) => new HashSet<Type>(types).Count == 1;

    public static Type? GetFromString(string typeName, params Assembly[] assemblies)
    {
        var type = Type.GetType(typeName);
        if (type is null)
        {
            var span = typeName.AsSpan();
            var index = span.IndexOf('+');
            var name = span[(index + 1)..].ToString();

            type = assemblies.SelectMany(x => x.GetTypes()).Where(x => x.Name == name).FirstOrDefault();
        }
        return type;
    }

    /// <summary>
    /// Returns the type of a nullable primitive type from type typeName.
    /// </summary>
    /// <param typeName="assemblyQualifiedName"></param>
    /// <returns></returns>
    public static Type? GetNullablePrimitiveType(string shortTypeName)
    {
        var fullName = GetNullablePrimitiveTypeFullName(shortTypeName);
        if (null == fullName) return null;

        return Type.GetType(fullName);
    }

    /// <summary>
    /// Returns the full typeName of the nullable primitive type from the short typeName.
    /// </summary>
    /// <param typeName="assemblyQualifiedName"></param>
    /// <returns></returns>
    public static string? GetNullablePrimitiveTypeFullName(string shortTypeName) => shortTypeName switch
    {
        "bool?" => typeof(Boolean?).FullName,
        "byte?" => typeof(Byte?).FullName,
        "char?" => typeof(Char?).FullName,
        "double?" => typeof(Double?).FullName,
        "float?" => typeof(Single?).FullName,
        "int?" => typeof(Int32?).FullName,
        "long?" => typeof(Int64?).FullName,
        "sbyte?" => typeof(SByte?).FullName,
        "short?" => typeof(Int16?).FullName,
        "uint?" => typeof(UInt32?).FullName,
        "ulong?" => typeof(UInt64?).FullName,
        "ushort?" => typeof(UInt16?).FullName,
        _ => null,
    };

    /// <summary>
    /// returns the type from the short typeName. e.g. int?, string?.
    /// </summary>
    /// <param typeName="assemblyQualifiedName">The typeName of the type.</param>
    /// <returns></returns>
    public static Type? GetNullableScalarType(string shortTypeName, bool withNullablePrimitives = true)
    {
        var fullName = GetNullableScalarTypeFullName(shortTypeName, withNullablePrimitives);
        if (null == fullName) return null;

        return Type.GetType(fullName);
    }

    /// <summary>
    /// returns the full name of the type.
    /// </summary>
    /// <param typeName="assemblyQualifiedName">e.g. DateTime?, decimal?, string.</param>
    /// <returns></returns>
    public static string? GetNullableScalarTypeFullName(string shortTypeName, bool withNullablePrimitives = true)
    {
        if (withNullablePrimitives)
        {
            var fullName = GetNullablePrimitiveTypeFullName(shortTypeName);
            if (null != fullName) return fullName;
        }

        return shortTypeName switch
        {
            $"{nameof(DateOnly)}?" => typeof(DateOnly?).FullName,
            $"{nameof(TimeOnly)}?" => typeof(TimeOnly?).FullName,
            $"{nameof(DateTime)}?" => typeof(DateTime?).FullName,
            "decimal?" => typeof(Decimal?).FullName,
            $"{nameof(Guid)}?" => typeof(Guid?).FullName,
            "string" => typeof(String).FullName,
            _ => null,
        };
    }

    /// <summary>
    /// returns the type from the short typeName. e.g. bool, int.
    /// </summary>
    /// <param typeName="assemblyQualifiedName"></param>
    /// <returns></returns>
    public static Type? GetPrimitiveType(string shortTypeName)
    {
        var fullName = GetPrimitiveTypeFullName(shortTypeName);
        if (null == fullName) return null;

        return Type.GetType(fullName);
    }

    /// <summary>
    /// returns the fullname of the type. e.g. int => System.Int32.
    /// </summary>
    /// <param typeName="assemblyQualifiedName">e.g. bool, byte int, long, ...</param>
    /// <returns></returns>
    public static string? GetPrimitiveTypeFullName(string shortTypeName) => shortTypeName switch
    {
        "bool" => typeof(Boolean).FullName,
        "byte" => typeof(Byte).FullName,
        "char" => typeof(Char).FullName,
        "double" => typeof(Double).FullName,
        "float" => typeof(Single).FullName,
        "int" => typeof(Int32).FullName,
        "long" => typeof(Int64).FullName,
        "sbyte" => typeof(SByte).FullName,
        "short" => typeof(Int16).FullName,
        "uint" => typeof(UInt32).FullName,
        "ulong" => typeof (UInt64).FullName,
        "ushort" => typeof(UInt16).FullName,
        _ => null,
    };


    /// <summary>
    /// Returns a parser that parses a string and returns a primitive type value.
    /// </summary>
    /// <param name="type">Type of a primitive type.</param>
    /// <returns>Returns lambda of a string parser.</returns>
    public static Func<string, object>? GetPrimitiveTypeStringParser(Type type)
    {
        return type switch
        {
            Type t when t == typeof(Boolean) => (s) => bool.Parse(s),
            Type t when t == typeof(Byte) => (s) => byte.Parse(s),
            Type t when t == typeof(Char) => (s) => char.Parse(s),
            Type t when t == typeof(Double) => (s) => double.Parse(s),
            Type t when t == typeof(Int16) => (s) => short.Parse(s),
            Type t when t == typeof(Int32) => (s) => int.Parse(s),
            Type t when t == typeof(Int64) => (s) => long.Parse(s),
            Type t when t == typeof(SByte) => (s) => sbyte.Parse(s),
            Type t when t == typeof(Single) => (s) => Single.Parse(s),
            Type t when t == typeof(UInt16) => (s) => ushort.Parse(s),
            Type t when t == typeof(UInt32) => (s) => uint.Parse(s),
            Type t when t == typeof(UInt64) => (s) => ulong.Parse(s),
            _ => null,
        };
    }

    /// <summary>
    /// returns the type from the short typeName. e.g. int, string.
    /// </summary>
    /// <param typeName="assemblyQualifiedName">The typeName of the type.</param>
    /// <returns></returns>
    public static Type? GetScalarType(string shortTypeName)
    {
        return GetScalarType(shortTypeName, true);
    }

    /// <summary>
    /// returns the type from the short typeName. e.g. int, string.
    /// </summary>
    /// <param typeName="assemblyQualifiedName">The typeName of the type.</param>
    /// <param typeName="withPrimitives">Including primitive types.</param>
    /// <returns></returns>
    public static Type? GetScalarType(string shortTypeName, bool withPrimitives)
    {
        var fullName = GetScalarTypeFullName(shortTypeName, withPrimitives);
        if (null == fullName) return null;

        return Type.GetType(fullName);
    }

    /// <summary>
    /// returns the full name of the type. e.g. int => System.Int32.
    /// </summary>
    /// <param typeName="assemblyQualifiedName">e.g. DateTime, decimal, string.</param>
    /// <param name="withPrimitives">If true the primitive types are also included otherwise not.</param>
    /// <returns></returns>
    public static string? GetScalarTypeFullName(string shortTypeName, bool withPrimitives = true)
    {
        if(withPrimitives)
        {
            var fullName = GetPrimitiveTypeFullName(shortTypeName);
            if (null != fullName) return fullName;
        }

        return shortTypeName switch
        {
            nameof(DateOnly) => typeof(DateOnly).FullName,
            nameof(DateTime) => typeof(DateTime).FullName,
            "decimal"  => typeof(Decimal).FullName,
            nameof(Guid) => typeof(Guid).FullName,
            "string" => typeof(String).FullName,
            nameof(TimeOnly) => typeof(TimeOnly).FullName,
            nameof(TimeSpan) => typeof(TimeSpan).FullName,
            _ => null,
        };
    }


    /// <summary>
    /// Returns a parser that parses a string and returns a primitive or scalar type value.
    /// </summary>
    /// <param name="type">Type of a scalar type.</param>
    /// <param name="withPrimitives">If true it includes primitive type string parsers.</param>
    /// <returns>Returns lambda of a string parser.</returns>
    public static Func<string, object>? GetScalarTypeStringParser(Type type, bool withPrimitives = true)
    {
        if (withPrimitives)
        {
            var parser = GetPrimitiveTypeStringParser(type);
            if (parser is not null) return parser;
        }

        return type switch
        {
            Type t when t == typeof(DateOnly) => (s) => DateOnly.Parse(s),
            Type t when t == typeof(DateTime) => (s) => DateTime.Parse(s),
            Type t when t == typeof(decimal) => (s) => decimal.Parse(s),
            Type t when t == typeof(Guid) => (s) => Guid.Parse(s),
            Type t when t == typeof(string) => (s) => s,
            Type t when t == typeof(TimeOnly) => (s) => TimeOnly.Parse(s),
            Type t when t == typeof(TimeSpan) => (s) => TimeSpan.Parse(s),
            _ => null,
        };
    }


    /// <summary>
    /// returns true if type is a scalar or primitive type.
    /// </summary>
    /// <param typeName="type">Type to check if it is scalar or primitive.</param>
    /// <param typeName="withoutPrimitives">If true primitive types are not considered.</param>
    /// <returns></returns>
    public static bool IsScalarType(Type type, bool withoutPrimitives = false)
    {
        return ScalarTypes(withoutPrimitives).Any(x => x == type);
    }

    public static IEnumerable<Type> NullablePrimitiveTypes()
    {
        yield return typeof(Boolean?);
        yield return typeof(Byte?);
        yield return typeof(Char?);
        yield return typeof(Double?);
        yield return typeof(Int16?);
        yield return typeof(Int32?);
        yield return typeof(Int64?);
        yield return typeof(SByte?);
        yield return typeof(Single?);
        yield return typeof(UInt16?);
        yield return typeof(UInt32?);
        yield return typeof(UInt64?);
    }

    public static IEnumerable<string> NullablePrimitiveTypeShortNames()
    {
        yield return "bool?";
        yield return "byte?";
        yield return "char?";
        yield return "double?";
        yield return "float?";
        yield return "int?";
        yield return "long?";
        yield return "sbyte?";
        yield return "short?";
        yield return "uint?";
        yield return "ulong?";
        yield return "ushort?";
    }

    public static IEnumerable<Type> NullableScalarTypes(bool whithoutPrimitives = false)
    {
        if (!whithoutPrimitives)
        {
            foreach (var primitive in NullablePrimitiveTypes())
            {
                yield return primitive;
            }
        }
        yield return typeof(TimeOnly?);
        yield return typeof(DateTime?);
        yield return typeof(decimal?);
        yield return typeof(Guid?);
        yield return typeof(string);
        yield return typeof(DateOnly?);
        yield return typeof(TimeSpan?);
    }

    public static IEnumerable<Type> PrimitiveArrayTypes()
    {
        yield return typeof(Boolean[]);
        yield return typeof(Byte[]);
        yield return typeof(Char[]);
        yield return typeof(Double[]);
        yield return typeof(Int16[]);
        yield return typeof(Int32[]);
        yield return typeof(Int64[]);
        yield return typeof(SByte[]);
        yield return typeof(Single[]);
        yield return typeof(UInt16[]);
        yield return typeof(UInt32[]);
        yield return typeof(UInt64[]);
    }

    public static IEnumerable<Type> PrimitiveEnumerableTypes()
    {
        yield return typeof(IEnumerable<Boolean>);
        yield return typeof(IEnumerable<Byte>);
        yield return typeof(IEnumerable<Char>);
        yield return typeof(IEnumerable<Double>);
        yield return typeof(IEnumerable<Int16>);
        yield return typeof(IEnumerable<Int32>);
        yield return typeof(IEnumerable<Int64>);
        yield return typeof(IEnumerable<SByte>);
        yield return typeof(IEnumerable<Single>);
        yield return typeof(IEnumerable<UInt16>);
        yield return typeof(IEnumerable<UInt32>);
        yield return typeof(IEnumerable<UInt64>);
    }

    public static IEnumerable<Type> PrimitiveTypes()
    {
        yield return typeof(Boolean);
        yield return typeof(Byte);
        yield return typeof(Char);
        yield return typeof(Double);
        yield return typeof(Int16);
        yield return typeof(Int32);
        yield return typeof(Int64);
        yield return typeof(SByte);
        yield return typeof(Single);
        yield return typeof(UInt16);
        yield return typeof(UInt32);
        yield return typeof(UInt64);
    }

    public static IEnumerable<string> PrimitiveTypeShortNames()
    {
        yield return "bool";
        yield return "byte";
        yield return "char";
        yield return "double";
        yield return "float";
        yield return "int";
        yield return "long";
        yield return "sbyte";
        yield return "short";
        yield return "uint";
        yield return "ulong";
        yield return "ushort";
    }

    /// <summary>
    /// Real number types.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<Type> RealNumberTypes()
    {
        yield return typeof(Decimal);
        yield return typeof(Double);
        yield return typeof(Single);
    }

    public static IEnumerable<Type> ScalarArrayTypes(bool whithoutPrimitives = false)
    {
        if(!whithoutPrimitives)
        {
            foreach (var primitive in PrimitiveArrayTypes())
            {
                yield return primitive;
            }
        }
        yield return typeof(TimeOnly[]);
        yield return typeof(DateTime[]);
        yield return typeof(decimal[]);
        yield return typeof(Guid[]);
        yield return typeof(string[]);
        yield return typeof(DateOnly[]);
        yield return typeof(TimeSpan[]);
    }

    public static IEnumerable<Type> ScalarEnumerableTypes(bool whithoutPrimitives = false)
    {
        if(!whithoutPrimitives)
        {
            foreach (var primitive in PrimitiveEnumerableTypes())
            {
                yield return primitive;
            }
        }
        yield return typeof(IEnumerable<DateOnly>);
        yield return typeof(IEnumerable<DateTime>);
        yield return typeof(IEnumerable<decimal>);
        yield return typeof(IEnumerable<Guid>);
        yield return typeof(IEnumerable<string>);
        yield return typeof(IEnumerable<TimeOnly>);
        yield return typeof(IEnumerable<TimeSpan>);
    }

    public static IEnumerable<Type> ScalarTypes(bool whithoutPrimitives = false)
    {
        if(!whithoutPrimitives)
        {
            foreach (var primitive in PrimitiveTypes())
            {
                yield return primitive;
            }
        }
        yield return typeof(DateOnly);
        yield return typeof(DateTime);
        yield return typeof(decimal);
        yield return typeof(Guid);
        yield return typeof(string);
        yield return typeof(TimeOnly);
        yield return typeof(TimeSpan);
    }

    public static IEnumerable<string> ScalarTypeShortNames(bool whithoutPrimitives = false)
    {
        if(!whithoutPrimitives)
        {
            foreach (var typeName in PrimitiveTypeShortNames())
                yield return typeName;
        }

        yield return nameof(DateOnly);
        yield return nameof(TimeOnly);
        yield return nameof(DateTime);
        yield return "decimal";
        yield return nameof(Guid);
        yield return "string";
        yield return nameof(TimeSpan);
    }

    /// <summary>
    /// Returns a string parser that returns an optional primitive value.
    /// </summary>
    /// <param name="type">Type of primitive type.</param>
    /// <returns>An optional primitive value.</returns>
    public static Func<string, Option<object>>? GetPrimitiveTypeOptionalStringParser(Type type)
    {
        return type switch
        {
            Type t when t == typeof(Boolean) => (s) => bool.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(Byte) => (s) => byte.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(Char) => (s) => char.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(Double) => (s) => double.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(Int16) => (s) => short.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(Int32) => (s) => int.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(Int64) => (s) => long.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(SByte) => (s) => sbyte.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(Single) => (s) => float.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(UInt16) => (s) => ushort.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(UInt32) => (s) => uint.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(UInt64) => (s) => ulong.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            _ => null,
        };
    }

    /// <summary>
    /// Returns a string parser if type is a scalar type.
    /// </summary>
    /// <param name="type">Type of scalar type.</param>
    /// <param name="withPrimitives">If true it includes primitive types.</param>
    /// <returns></returns>
    public static Func<string, Option<object>>? GetScalarTypeOptionalStringParser(Type type, bool withPrimitives = true)
    {
        if (withPrimitives)
        {
            var parser = GetPrimitiveTypeOptionalStringParser(type);
            if (parser is not null) return parser;
        }

        return type switch
        {
            Type t when t == typeof(DateOnly) => (s) => DateOnly.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(DateTime) => (s) => DateTime.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(decimal) => (s) => decimal.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(Guid) => (s) => Guid.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(string) => (s) => s is not null ? Option.Some(s) : Option.None<object>(),
            Type t when t == typeof(TimeOnly) => (s) => TimeOnly.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            Type t when t == typeof(TimeSpan) => (s) => TimeSpan.TryParse(s, out var value) ? Option.Some(value) : Option.None<object>(),
            _ => null,
        };
    }
}

