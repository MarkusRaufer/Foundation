namespace Foundation.Reflection;

using System.Reflection;
public static class MemberInfoExtensions
{
    public static Type? GetMemberType(this MemberInfo memberInfo)
    {
        return memberInfo switch
        {
            EventInfo ei => ei.EventHandlerType,
            FieldInfo fi => fi.FieldType,
            MethodInfo mi => mi.ReturnType,
            PropertyInfo pi => pi.PropertyType,
            _ => null
        };
    }

#nullable enable
    public static object? GetValue(this MemberInfo memberInfo, object? obj)
    {
        memberInfo.ThrowIfNull();
        if (null == obj) return null;

        return memberInfo switch
        {
            FieldInfo fi => fi.GetValue(obj),
            MethodInfo mi => mi.GetValue(obj),
            PropertyInfo pi => pi.GetValue(obj),
            _ => null
        };
    }
#nullable restore

    public static void SetValue(this MemberInfo memberInfo, object obj, object value)
    {
        if (memberInfo is PropertyInfo pi)
        {
            pi.SetValue(obj, value);
            return;
        }

        if (memberInfo is FieldInfo fi)
        {
            fi.SetValue(obj, value);
            return;
        }

        if (memberInfo is MethodInfo mi)
        {
            mi.SetValue(obj, value);
            return;
        }
    }
}

