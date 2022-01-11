namespace Foundation.Reflection;

using System.Collections;
using System.Reflection;

public static class PropertyInfoExtension
{
    public static bool IsEnumerable(this PropertyInfo propertyInfo)
    {
        return ((typeof(string) != propertyInfo.PropertyType) &&
                typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType));
    }

    public static bool IsInherited(this PropertyInfo propertyInfo)
    {
        return propertyInfo.ReflectedType != propertyInfo.DeclaringType;
    }
}
