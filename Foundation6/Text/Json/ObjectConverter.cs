using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundation.Text.Json;

public class ObjectConverter : JsonConverter<object>
{
    private static Type _objectType = typeof(object);

    public override bool HandleNull => base.HandleNull;

    public override bool CanConvert(Type typeToConvert)
    {
        if (typeToConvert == _objectType) return true;
        return base.CanConvert(typeToConvert);
    }

    public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(typeToConvert == typeof(object))
        {
            return reader.TokenType switch
            {
                JsonTokenType.Null => null,
                JsonTokenType.None => null,
                JsonTokenType.False => reader.GetBoolean(),
                JsonTokenType.True => reader.GetBoolean(),
                JsonTokenType.String => reader.GetString(),
                JsonTokenType.Number => reader.GetInt32(),
                _ => Read(ref reader, typeToConvert, options)
            };
        }

        return Read(ref reader, typeToConvert, options);
    }
 
    public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
