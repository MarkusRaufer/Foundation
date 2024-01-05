using System.Text.Json;

namespace Foundation.Text.Json;

public static class Utf8JsonWriterExtensions
{
    public static void WriteValue(this Utf8JsonWriter writer, object value)
    {
        if (null == value) return;

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
