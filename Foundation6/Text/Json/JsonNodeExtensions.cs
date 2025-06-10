using System.Text.Json.Nodes;

namespace Foundation.Text.Json;

public static class JsonNodeExtensions
{
    public static Option<T> GetValueAsOption<T>(this JsonNode? jsonNode)
    {
        if (jsonNode is null) return Option.None<T>();
        return Option.Some(jsonNode.GetValue<T>());
    }

    public static T? GetNullableValue<T>(this JsonNode? jsonNode)
    {
        if (jsonNode is null) return default;
        return jsonNode.GetValue<T>();
    }
}
