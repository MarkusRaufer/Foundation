#if NET6_0_OR_GREATER

using System.Text.Json.Serialization;
using System.Text.Json;

namespace Foundation.Text.Json;

public static class JsonSerializerOptionsExtensions
{
    public static JsonSerializerOptions AddConverter<TConverter>(this JsonSerializerOptions options, TConverter converter)
        where TConverter : JsonConverter
    {
        options.Converters.Add(converter);
        return options;
    }
}
#endif
