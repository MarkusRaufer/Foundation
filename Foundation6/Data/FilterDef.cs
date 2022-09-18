namespace Foundation.Data;

public record FilterDef
{
    public record EqualsToValue(KeyValuePair<string, object?>[] KeyValues) : FilterDef;
    public record StringStartsWith(KeyValuePair<string, string>[] KeyValues) : FilterDef;
}
