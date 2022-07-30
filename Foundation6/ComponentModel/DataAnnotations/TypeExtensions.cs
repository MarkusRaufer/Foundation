using System.Reflection;

namespace Foundation.ComponentModel.DataAnnotations;

public static class TypeExtensions
{
    public static PropertyInfo? GetPropertyFromAttribute(this Type type, Type attributeType, bool inherit = true)
    {
        type.ThrowIfNull();
        attributeType.ThrowIfNull();

        foreach(var property in type.GetProperties())
        {
            var attribute = property.GetCustomAttributes(attributeType, inherit);
            if (0 < attribute.Length) return property;
        }

        return default;
    }
}
