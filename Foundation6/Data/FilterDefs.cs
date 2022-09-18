using static Foundation.Data.FilterDef;

namespace Foundation.Data;

public static class FilterDefs
{
    public static IEnumerable<string> FilterDefNames
    {
        get
        {
            yield return nameof(EqualsToValue);
            yield return nameof(StringStartsWith);
        }
    }
}
