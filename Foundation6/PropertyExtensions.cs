
namespace Foundation;

using System.Diagnostics.CodeAnalysis;

public static class PropertyExtensions
{
    public static KeyValue<string, object?> ToKeyValue([DisallowNull] this Property property)
    {
        return new KeyValue<string, object?>(property.Name, property.Value);
    }

    public static KeyValuePair<string, object?> ToKeyValuePair([DisallowNull] this Property property)
    {
        return new KeyValuePair<string, object?>(property.Name, property.Value);
    }
}

