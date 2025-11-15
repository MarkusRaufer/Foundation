namespace Foundation.Collections.Generic;

public static class StringExtensions
{
    public static KeyValuePair<string, object?> ToKeyValue(this string str, object? value)
        => new (str, value);
}
