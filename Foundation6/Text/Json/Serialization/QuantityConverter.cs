using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundation.Text.Json.Serialization;

public class QuantityConverter : JsonConverter<Quantity?>
{
    public override Quantity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject) return null;

        if (!reader.GetProperty().TryGetOk(out var unitProperty) || unitProperty.Value is not string unit) return null;

        if(!reader.GetProperty().TryGetOk(out var valueProperty) || valueProperty.Value is not decimal value) return null;

        reader.Read();

        return Quantity.New(unit, value);
    }

    public override void Write(Utf8JsonWriter writer, Quantity? value, JsonSerializerOptions options)
    {
        if (!value.HasValue) return;

        writer.WriteStartObject();

        writer.WritePropertyName(nameof(Quantity.Unit));
        writer.WriteStringValue(value.Value.Unit);

        writer.WritePropertyName(nameof(Quantity.Value));
        writer.WriteNumberValue(value.Value.Value);

        writer.WriteEndObject();
    }
}
