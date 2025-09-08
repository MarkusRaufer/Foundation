using System.Text.Json;
using System.Text.Json.Serialization;

namespace Foundation.Text.Json.Serialization;

public class GenericConverterFactory : JsonConverterFactory
{
    private readonly Type _converterType;
    private readonly Type[] _types;

    public GenericConverterFactory(Type converter, params IEnumerable<Type> types)
    {
        _converterType = converter.ThrowIfNull();
        _types = [.. types];
    }

    public override bool CanConvert(Type typeToConvert) => Array.IndexOf(_types, typeToConvert) != -1;

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        var convType = _converterType.MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(convType)!;
    }
}