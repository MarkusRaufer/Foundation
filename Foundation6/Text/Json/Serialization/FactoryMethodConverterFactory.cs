namespace Foundation.Text.Json.Serialization;

public sealed class FactoryMethodConverterFactory(params IEnumerable<Type> types) 
    : GenericConverterFactory(typeof(FactoryMethodConverter<>), types)
{
}