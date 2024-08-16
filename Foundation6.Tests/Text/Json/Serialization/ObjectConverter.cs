#if NET6_0_OR_GREATER
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Foundation.Text.Json.Serialization.Tests;

public class ObjectConverter : JsonConverter<object?>
{
    private readonly TypeCode _integerFormat;
    private readonly TypeCode _floatFormat;
    private readonly bool _supportDateOnly;
    private readonly bool _supportTimeOnly;
    private readonly bool _supportTimeSpan;

    private readonly Regex? _dateTimeRegex;
    private readonly Regex? _timeSpanRegex;

    public ObjectConverter(
        TypeCode integerFormat = TypeCode.Int32,
        TypeCode floatFormat = TypeCode.Double,
        bool supportDateOnly = true,
        bool supportTimeOnly = true,
        bool supportTimeSpan = true)
    {
        _integerFormat = integerFormat;
        _floatFormat = floatFormat;
        _supportDateOnly = supportDateOnly;
        _supportTimeOnly = supportTimeOnly;
        _supportTimeSpan = supportTimeSpan;

        if (_supportDateOnly) _dateTimeRegex = new Regex(RegularExpressions.DateTimeExpression);
        if (_supportTimeSpan) _timeSpanRegex = new Regex(RegularExpressions.TimeSpanExpression);
    }

    private static Result<TimeSpan, string?> GetTimeSpan(Utf8JsonReader reader, Regex? regex)
    {
        var str = reader.GetString();
        if (null == regex) return Result.Error<TimeSpan, string?>(str);

        if (string.IsNullOrEmpty(str)) return Result.Error<TimeSpan, string?>(str);
        if (!regex.IsMatch(str)) return Result.Error<TimeSpan, string?>(str);

        if (TimeSpan.TryParse(str, out var value)) return Result.Ok<TimeSpan, string?>(value);

        return Result.Error<TimeSpan, string?>(str);
    }

    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            JsonTokenType.Number when TryGetInteger(reader, _integerFormat, out var intValue) => intValue,
            JsonTokenType.Number when TryGetFloat(reader, _floatFormat, out var floatValue) => floatValue,
            JsonTokenType.String when TryGetDateTime(reader, _dateTimeRegex, out var dateTime) => dateTime,
            JsonTokenType.String when _supportDateOnly && TryGetDateOnly(reader, out var dateOnly) => dateOnly,
            JsonTokenType.String when _supportTimeOnly && TryGetTimeOnly(reader, out var timeOnly) => timeOnly,
            JsonTokenType.String => GetTimeSpan(reader, _timeSpanRegex).Either(x => (object)x, x => x),
            _ => JsonDocument.ParseValue(ref reader).RootElement.Clone()
        };
    }

    private static bool TryGetDateOnly(Utf8JsonReader reader, out DateOnly? dateOnly)
    {
        var str = reader.GetString();
        if (DateOnly.TryParse(str, out var value))
        {
            dateOnly = value;
            return true;
        }
        dateOnly = null;
        return false;
    }

    private static bool TryGetDateTime(Utf8JsonReader reader, Regex? dateTimeRegex, out DateTime? dateTime)
    {
        if (null == dateTimeRegex)
        {
            if (reader.TryGetDateTime(out var dt))
            {
                dateTime = dt;
                return true;
            }
            dateTime = null;
            return false;
        }

        var str = reader.GetString();
        if (string.IsNullOrEmpty(str) || !dateTimeRegex.IsMatch(str))
        {
            dateTime = null;
            return false;
        }

        if (DateTime.TryParse(str, out var value))
        {
            dateTime = value;
            return true;
        }
        dateTime = null;
        return false;
    }

    private static bool TryGetFloat(Utf8JsonReader reader, TypeCode typeCode, out object? number)
    {
        switch (typeCode)
        {
            case TypeCode.Decimal:
                if (reader.TryGetDecimal(out var decimalValue))
                {
                    number = decimalValue;
                    return true;
                }
                break;
            case TypeCode.Double:
                if (reader.TryGetDouble(out var doubleValue))
                {
                    number = doubleValue;
                    return true;
                }
                break;
        }
        number = default;
        return false;
    }

    private static bool TryGetInteger(Utf8JsonReader reader, TypeCode typeCode, out object? number)
    {
        switch (typeCode)
        {
            case TypeCode.Int16:
                if (reader.TryGetInt16(out var int16))
                {
                    number = int16;
                    return true;
                }
                break;
            case TypeCode.Int32:
                if (reader.TryGetInt32(out var int32))
                {
                    number = int32;
                    return true;
                }
                break;
            case TypeCode.Int64:
                if (reader.TryGetInt64(out var int64))
                {
                    number = int64;
                    return true;
                }
                break;
            case TypeCode.UInt16:
                if (reader.TryGetInt16(out var uInt16))
                {
                    number = uInt16;
                    return true;
                }
                break;
            case TypeCode.UInt32:
                if (reader.TryGetInt32(out var uInt32))
                {
                    number = uInt32;
                    return true;
                }
                break;
            case TypeCode.UInt64:
                if (reader.TryGetUInt64(out var uInt64))
                {
                    number = uInt64;
                    return true;
                }
                break;
        }
        number = default;
        return false;
    }

    private static bool TryGetTimeOnly(Utf8JsonReader reader, out TimeOnly? timeOnly)
    {
        var str = reader.GetString();
        if (TimeOnly.TryParse(str, out var value))
        {
            timeOnly = value;
            return true;
        }
        timeOnly = null;
        return false;
    }

    public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
    {
        writer.WriteValue(value);
    }
}
#endif
