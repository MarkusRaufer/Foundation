#if NET6_0_OR_GREATER
using System;
using System.Dynamic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundation.Text.Json.Serialization;
public class ExpandoObjectConverter : JsonConverter<ExpandoObject>
{
    public override ExpandoObject? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, ExpandoObject value, JsonSerializerOptions options)
    {
        WriteExpandoObject(writer, value, options);
    }

    private void WriteExpandoObject(Utf8JsonWriter writer, ExpandoObject obj, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        foreach (var kvp in obj)
        {
            writer.WritePropertyName(kvp.Key);
            if (kvp.Value is ExpandoObject xpando) WriteExpandoObject(writer, xpando, options);
            else JsonSerializer.Serialize(writer, kvp.Value, options);
        }
        writer.WriteEndObject();
    }
}
#endif
