namespace Foundation.Text.Json.Serialization;

public sealed class PrivateFieldConverterFactory(params IEnumerable<Type> types) 
    : GenericConverterFactory(typeof(PrivateFieldConverter<>), types)
{
}
