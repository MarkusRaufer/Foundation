using Foundation.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundation.Text.Json.Serialization;

public class NamedIdJsonConverter : JsonConverter<NamedId>
{
    private readonly TypeJsonConverter _typeJsonConverter;

    public NamedIdJsonConverter() : this(new TypeJsonConverter())
    {
    }

    public NamedIdJsonConverter(TypeJsonConverter typeJsonConverter)
    {
        _typeJsonConverter = typeJsonConverter.ThrowIfNull();
    }

    public override NamedId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject) return NamedId.Empty;
        if (!reader.Read()) throw new JsonException(nameof(NamedId));

        // Name property
        var result = reader.GetProperty(typeof(string));
        if (!result.TryGetOk(out var nameProperty) || nameProperty.Value is not string name)
        {
            throw new JsonException(nameof(NamedId));
        }

        if (!reader.Read()) throw new JsonException(nameof(NamedId));

        // Type property
        var type = _typeJsonConverter.Read(ref reader, typeToConvert, options);
        if (null == type) throw new JsonException(nameof(Id));

        // Value property
        if (!reader.Read() || reader.TokenType != JsonTokenType.PropertyName) throw new JsonException(nameof(NamedId));

        var valuePropertyResult = reader.GetProperty(type);
        if (!valuePropertyResult.TryGetOk(out var valueProperty) || valueProperty.Value is null)
        {
            throw new JsonException(nameof(NamedId));
        }

        reader.Read();

        return NamedId.NewId(name, valueProperty.Value);
    }

    public override void Write(Utf8JsonWriter writer, NamedId id, JsonSerializerOptions options)
    {
        if (id.IsEmpty) return;

        writer.WriteStartObject();

        // Name property
        writer.WritePropertyName(nameof(NamedId.Name));
        writer.WriteStringValue(id.Name);

        // Type property
        writer.WritePropertyName(nameof(NamedId.Type));

        // Type property value
        _typeJsonConverter.Write(writer, id.Type, options);

        // Value property
        writer.WritePropertyName(nameof(NamedId.Value));
        writer.WriteValue(id.Value);

        writer.WriteEndObject();
    }
}
