namespace Foundation;

public static class ObjectHelper
{
    public static IEnumerable<object> SortByHashCode(params object[] objects)
    {
        var d = new SortedDictionary<int, object>();
        foreach (var obj in objects)
            d.Add(obj.GetHashCode(), obj);

        return d.Select(pair => pair.Value);
    }
}

