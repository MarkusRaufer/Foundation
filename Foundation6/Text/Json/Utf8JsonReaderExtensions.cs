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
//#if NET6_0_OR_GREATER

using System.Text.Json;

namespace Foundation.Text.Json;

public static class Utf8JsonReaderExtensions
{
    public static Result<KeyValuePair<string, object?>, Error> GetProperty(this ref Utf8JsonReader reader)
    {
        if (reader.TokenType != JsonTokenType.PropertyName) reader.Read();

        if (reader.TokenType != JsonTokenType.PropertyName)
        {
            var error = new Error($"{nameof(JsonTokenType)}", $"expected {nameof(JsonTokenType.PropertyName)}");
            return Result.Error<KeyValuePair<string, object?>>(error);
        }

        var name = reader.GetString();
        if (null == name)
        {
            return Result.Error<KeyValuePair<string, object?>>(new Error("property name", "property has no name"));
        }

        if (!reader.Read())
        {
            return Result.Error<KeyValuePair<string, object?>>(new Error("property value", $"property {name} has no value"));
        }

        var value = reader.GetValue(reader.TokenType);

        return Result.Ok(new KeyValuePair<string, object?>(name, value));
    }

    public static Result<KeyValuePair<string, object?>, Error> GetProperty(this ref Utf8JsonReader reader, Type type)
    {
        if (reader.TokenType != JsonTokenType.PropertyName)
        {
            var error = new Error($"{nameof(JsonTokenType)}", $"expected {nameof(JsonTokenType.PropertyName)}");
            return Result.Error<KeyValuePair<string, object?>>(error);
        }

        var name = reader.GetString();
        if (null == name)
        {
            return Result.Error<KeyValuePair<string, object?>>(new Error("property name", "property has no name"));
        }
        
        if (!reader.Read() || !reader.TokenType.IsValue())
        {
            return Result.Error<KeyValuePair<string, object?>>(new Error("property value", $"property {name} has no value"));
        }

        var value = reader.GetValue(type);
        return Result.Ok(new KeyValuePair<string, object?>(name, value));
    }

    public static Result<object?, Error> GetValue(this Utf8JsonReader reader)
    {
        if (!reader.TokenType.IsValue())
        {
            return Result.Error<object?>(new Error("property value", "property value not found"));
        }

        switch (reader.TokenType)
        {
            case JsonTokenType.False: return Result.Ok<object?>(false);
            case JsonTokenType.Null: return Result.Ok<object?>(default);
            case JsonTokenType.Number:
                if (reader.TryGetInt64(out var i64)) return Result.Ok<object?>(i64);
                if (reader.TryGetDecimal(out var real)) return Result.Ok<object?>(real);

                return Result.Error<object?>(new Error("property value", "elegal number format"));
            case JsonTokenType.String: return Result.Ok<object?>(reader.GetString());
            case JsonTokenType.True: return Result.Ok<object?>(true);
            default: return Result.Error<object?>(new Error("property value", "format not supported"));
        }
    }

    public static object? GetValue(this Utf8JsonReader reader, JsonTokenType tokenType)
    {
        switch(tokenType)
        {
            case JsonTokenType.False: return false;
            case JsonTokenType.True: return true;
            case JsonTokenType.Null: return default;
            case JsonTokenType.Number:
                if (reader.TryGetInt64(out var i64)) return i64;
                if (reader.TryGetDecimal(out var real)) return real;
                return null;
            case JsonTokenType.String: return reader.GetString();
            default: return default;
        };
    }

    public static object? GetValue(this Utf8JsonReader reader, TypeCode typeCode)
    {
        return typeCode switch
        {
            TypeCode.Boolean => reader.GetBoolean(),
            TypeCode.Byte => reader.GetByte(),
            TypeCode.Char => reader.GetString(),
            TypeCode.DateTime => reader.GetDateTime(),
            TypeCode.Decimal => reader.GetDecimal(),
            TypeCode.Double => reader.GetDouble(),
            TypeCode.Int16 => reader.GetInt16(),
            TypeCode.Int32 => reader.GetInt32(),
            TypeCode.Int64 => reader.GetInt64(),
            TypeCode.UInt16 => reader.GetUInt16(),
            TypeCode.UInt32 => reader.GetUInt32(),
            TypeCode.UInt64 => reader.GetUInt64(),
            TypeCode.SByte => reader.GetSByte(),
            TypeCode.Single => reader.GetSingle(),
            TypeCode.String => reader.GetString(),
            _ => default,
        };
    }

    public static object? GetValue(this Utf8JsonReader reader, Type type)
    {
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Boolean: return reader.GetBoolean();
            case TypeCode.Byte: return reader.GetByte();
            case TypeCode.Char: return reader.GetString();
            case TypeCode.DateTime: return reader.GetDateTime();
            case TypeCode.Decimal: return reader.GetDecimal();
            case TypeCode.Double: return reader.GetDouble();
            case TypeCode.Int16: return reader.GetInt16();
            case TypeCode.Int32: return reader.GetInt32();
            case TypeCode.Int64: return reader.GetInt64();
            case TypeCode.UInt16: return reader.GetUInt16();
            case TypeCode.UInt32: return reader.GetUInt32();
            case TypeCode.UInt64: return reader.GetUInt64();
            case TypeCode.SByte: return reader.GetSByte();
            case TypeCode.Single: return reader.GetSingle();
            case TypeCode.String: return reader.GetString();
        }

        switch(type)
        {
#if NET6_0_OR_GREATER
            case Type _ when type == typeof(DateOnly):
                {
                    var str = reader.GetString();
                    if(str is null) return null;

                    if (DateTime.TryParse(str, out var dt)) return dt.ToDateOnly();
                    return null;
                }
#endif
            case Type _ when type == typeof(Guid):
                {
                    var guid = reader.GetGuid();
                    return guid;
                }
#if NET6_0_OR_GREATER
            case Type _ when type == typeof(TimeOnly):
                {
                    var str = reader.GetString();
                    if (str is null) return null;

                    if (DateTime.TryParse(str, out var dt)) return dt.ToTimeOnly();
                    return null;
                }
#endif
        };

        return reader.GetString();
    }

    public static object? GetValue(this Utf8JsonReader reader, string typeName)
    {
        switch (typeName)
        {
            case "System.Boolean": return reader.GetBoolean();
            case "System.Byte": return reader.GetByte();
            case "System.Char": return reader.GetString();
#if NET6_0_OR_GREATER
            case "System.DateOnly":
                {
                    var str = reader.GetString();
                    if (str is null) return null;

                    if (DateTime.TryParse(str, out var dt)) return dt.ToDateOnly();
                    return null;
                }
#endif
            case "System.DateTime": return reader.GetDateTime();
            case "System.Decimal": return reader.GetDecimal();
            case "System.Double": return reader.GetDouble();
            case "System.Guid": return reader.GetGuid();
            case "System.Int16": return reader.GetInt16();
            case "System.Int32": return reader.GetInt32();
            case "System.Int64": return reader.GetInt64();
            case "System.UInt16": return reader.GetUInt16();
            case "System.UInt32": return reader.GetUInt32();
            case "System.UInt64": return reader.GetUInt64();
            case "System.SByte": return reader.GetSByte();
            case "System.Single": return reader.GetSingle();
            case "System.String": return reader.GetString();
#if NET6_0_OR_GREATER
            case "System.TimeOnly":
                {
                    var str = reader.GetString();
                    if (str is null) return null;

                    if (DateTime.TryParse(str, out var dt)) return dt.ToTimeOnly();
                    return null;
                }
#endif
        }

        return reader.GetString();
    }
}
