namespace Foundation.ComponentModel;

public static class PropertyExtensions
{
    public static KeyValuePair<string, object?> ToKeyValuePair(this Property property)
    {
        return new KeyValuePair<string, object?>(property.Name, property.Value);
    }
}

