using Foundation.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Text.Json;

public static class Json
{
    public static string Properties(IEnumerable<(string name, object? value)> properties)
    {
        var sb = new StringBuilder();
        foreach (var (name, value) in properties.AfterEach(() => sb.Append(',')))
        {
            sb.Append(Property(name, value));
        }
        return sb.ToString();
    }

    public static string Properties(IEnumerable<KeyValuePair<string, object?>> properties)
    {
        var sb = new StringBuilder();

        foreach (var (name, value) in properties.AfterEach(() => sb.Append(',')))
        {
            sb.Append(Property(name, value));
        }
        return sb.ToString();
    }

    public static string Property<T>(string name, T? value) => $"{name.ToJson()}:{ToJson(value)}";

    public static string ToJson(this string str) => $@"""{str}""";

    public static string ToJson<T>(this T? value)
    {
        if (value is null) return "null";

        var type = value.GetType();

        var scalarType = TypeHelper.GetScalarType(type);
        if (scalarType is null) return "null";

        return scalarType switch
        {
            { IsPrimitive: true } => $"{value}",
            Type _ when scalarType == typeof(DateTime) => $"{value:yyyy-MM-ddTHH:mm:ss}",
            Type _ when scalarType == typeof(DateOnly) => $"{value:yyyy-MM-dd}",
            Type _ when scalarType == typeof(decimal) => string.Create(CultureInfo.InvariantCulture, $"{value}"),
            Type _ when scalarType == typeof(Guid) => $"\"{value}\"",
            Type _ when scalarType == typeof(string) => $"\"{value}\"",
            Type _ when scalarType == typeof(TimeOnly) => $"\"{value:HH:mm:ss}\"",
            Type _ when scalarType == typeof(TimeSpan) => $"\"{value}\"",
            _ => "null"
        };
    }
}
