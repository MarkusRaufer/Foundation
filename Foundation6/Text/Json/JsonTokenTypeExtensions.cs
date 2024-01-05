using System.Text.Json;

namespace Foundation.Text.Json;

public static class JsonTokenTypeExtensions
{
    public static bool IsValue(this JsonTokenType tokenType)
    {
        return tokenType switch
        {
            JsonTokenType.Number or JsonTokenType.String => true,
            _ => false,
        };
    }
}
