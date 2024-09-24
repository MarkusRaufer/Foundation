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
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Foundation.Text.Json.Serialization;

public class ObjectJsonConverter : JsonConverter<object?>
{
    private readonly TypeCode _integerFormat;
    private readonly TypeCode _floatFormat;
    private static readonly QuantityConverter _quantityConverter = new();
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { Converters = { _quantityConverter } };

#if NET6_0_OR_GREATER
#endif

    private readonly Regex? _dateTimeRegex = null;
    private readonly Regex? _timeSpanRegex = null;
    private readonly bool _supportDateOnly;
    private readonly bool _supportTimeOnly;
    private readonly bool _supportTimeSpan;

    public ObjectJsonConverter(
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

    private static Result<TimeSpan, string?> GetTimeSpan(ref Utf8JsonReader reader, Regex? regex)
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
            JsonTokenType.Number when TryGetInteger(ref reader, _integerFormat, out var intValue) => intValue,
            JsonTokenType.Number when TryGetFloat(ref reader, _floatFormat, out var floatValue) => floatValue,
            JsonTokenType.StartObject when TryGetQuantity(ref reader, out var quantity) => quantity,
            JsonTokenType.String when _supportTimeSpan && TryGetTimeSpan(ref reader, out var timeSpan) => timeSpan,
            JsonTokenType.String when TryGetDateTime(ref reader, _dateTimeRegex, out var dateTime) => dateTime,
            JsonTokenType.String when _supportDateOnly && TryGetDateOnly(ref reader, out var dateOnly) => dateOnly,
            JsonTokenType.String when _supportTimeOnly && TryGetTimeOnly(ref reader, out var timeOnly) => timeOnly,
            JsonTokenType.String => reader.GetString(),
            _ => JsonDocument.ParseValue(ref reader).RootElement.Clone()
        };
    }

    private static bool TryGetDateOnly(ref Utf8JsonReader reader, out DateOnly? dateOnly)
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

    private static bool TryGetDateTime(ref Utf8JsonReader reader, Regex? dateTimeRegex, out DateTime? dateTime)
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

    private static bool TryGetFloat(ref Utf8JsonReader reader, TypeCode typeCode, out object? number)
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

    private static bool TryGetInteger(ref Utf8JsonReader reader, TypeCode typeCode, out object? number)
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

    private static bool TryGetQuantity(ref Utf8JsonReader reader, out Quantity? quantity)
    {
        quantity = _quantityConverter.Read(ref reader, typeof(Quantity?), _jsonSerializerOptions);
        return quantity is not null;
    }

    private static bool TryGetTimeOnly(ref Utf8JsonReader reader, out TimeOnly? timeOnly)
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

    private static bool TryGetTimeSpan(ref Utf8JsonReader reader, out TimeSpan? timeSpan)
    {
        var str = reader.GetString();
        if (string.IsNullOrWhiteSpace(str))
        {
            timeSpan = default;
            return false;
        }

        if (Iso8601Period.TryParse(str.AsSpan(), out var value))
        {
            timeSpan = value;
            return true;
        }
        timeSpan = null;
        return false;
    }

    public override void Write(Utf8JsonWriter writer, object? value, JsonSerializerOptions options)
    {
        switch(value)
        {
            case TimeSpan timeSpan: writer.WriteValue(timeSpan.ToIso8601Period()); break;
            case Quantity quantity: _quantityConverter.Write(writer, quantity, options); break;
            default: writer.WriteValue(value); break;
        }
    }
}
