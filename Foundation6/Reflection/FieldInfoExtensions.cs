using System.Collections;
using System.Reflection;

namespace Foundation.Reflection;
public static class FieldInfoExtensions
{
    public static bool IsCollection(this FieldInfo fieldInfo)
    {
        if(fieldInfo is null) return false;

        return typeof(IEnumerable).IsAssignableFrom(fieldInfo.FieldType) && fieldInfo.FieldType != typeof(string);
    }
}
