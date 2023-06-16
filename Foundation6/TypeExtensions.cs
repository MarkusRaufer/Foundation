namespace Foundation;

using System.Collections;
using System.Text;

public static class TypeExtensions
{
    public static int Compare(this Type type, Type other, Func<Type, IComparable> selector)
    {
        return selector(type).CompareTo(selector(other));
    }

    public static int Compare<T>(this Type type, Type other, Func<Type, T> selector)
        where T : IComparable<T>
    {
        return selector(type).CompareTo(selector(other));
    }

    public static Type CreateGenericType(this Type objectType, params Type[] genericTypeArguments)
    {
        if (!objectType.IsGenericType)
            throw new ArgumentException("type is not a generic type");

        if (null == genericTypeArguments || genericTypeArguments.Length == 0)
            throw new ArgumentException("genericTypeArguments must have at least 1 type argument");

        return objectType.MakeGenericType(genericTypeArguments);
    }

    public static Type? GetAssignableInterface<T>(this Type self)
    {
        var interfaceType = typeof(T);
        return GetAssignableInterface(self, interfaceType);
    }

    public static Type? GetAssignableInterface(this Type self, Type interfaceType)
    {
        foreach (var type in self.GetInterfaces())
        {
            if (HasInterface(type, interfaceType))
                return type;
        }

        return null;
    }

    public static object? GetDefault(this Type type)
    {
        if (type.IsValueType)
            return Activator.CreateInstance(type);

        return null;
    }

    public static IEnumerable<Type> GetGenericInterfaceTypeArguments(this Type self, Type interfaceType)
    {
        foreach (var type in self.GetInterfaces())
        {
            if (type.IsGenericType)
            {
                var genericTypeDef = type.GetGenericTypeDefinition();
                if (genericTypeDef == interfaceType)
                {
                    var arg = type.GenericTypeArguments[0];
                    yield return arg;
                }
            }
        }
    }

    /// <summary>
    /// Returns the name of the type without generics.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetNameWithoutGenericArity(this Type type)
    {
        var sb = new StringBuilder();
        foreach (var ch in type.Name)
        {
            if ('`' == ch) break;

            sb.Append(ch);
        }
        return sb.ToString();
    }

    public static IEnumerable<Type> GetNestedParentTypes(this Type? type)
    {
        if (null == type) throw new ArgumentNullException(nameof(type));
        if (!type.IsNested) yield break;

        if (type.DeclaringType is Type declType)
            yield return declType;

        foreach (var parent in GetNestedParentTypes(type.DeclaringType))
        {
            yield return parent;
        }
    }

    public static bool HasInterface<T>(this Type type)
    {
        return HasInterface(type, typeof(T));
    }

    public static bool HasInterface(this Type type, Type @interface)
    {
        return @interface.IsAssignableFrom(type);
    }

    /// <summary>
    /// Returns true, if the object obj implements the generic interface of type type.
    /// </summary>
    /// <param name="type">Type of the object, which implements the generic interface.</param>
    /// <param name="interfaceType">Type of the generic interface. E.g. typeof(IEquatable{})</param>
    /// <returns></returns>
    public static bool ImplementsGenericInterface(this Type type, Type interfaceType)
    {
        return type.GetInterfaces()
                   .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == interfaceType);
    }

    public static bool IsAction(Type? type)
    {
        if (null == type) return false;

        if (type == typeof(Action)) return true;

        var generic = type switch
        {
            Type { IsGenericTypeDefinition: true } => type,
            Type { IsGenericType: true } => type.GetGenericTypeDefinition(),
            _ => null
        };

        return generic == typeof(Action<>);
    }

    public static bool IsEnumerable(this Type type)
    {
        return (typeof(string) != type) && typeof(IEnumerable).IsAssignableFrom(type);
    }

    public static bool IsLambda(this Type? type)
    {
        var generic = type switch
        {
            Type { IsGenericTypeDefinition: true } => type,
            Type { IsGenericType: true } => type.GetGenericTypeDefinition(),
            _ => null
        };

        if (generic == null) return false;
        if (generic.Namespace != "System") return false;
        if (generic.BaseType != typeof(MulticastDelegate)) return false;
        return generic.Name.StartsWith("Func`");
    }

    public static bool IsNumeric(this Type? type)
    {
        switch (Type.GetTypeCode(type))
        {
            case TypeCode.Byte:
            case TypeCode.Decimal:
            case TypeCode.Double:
            case TypeCode.Int16:
            case TypeCode.Int32:
            case TypeCode.Int64:
            case TypeCode.UInt16:
            case TypeCode.UInt32:
            case TypeCode.UInt64:
            case TypeCode.SByte:
            case TypeCode.Single:
                return true;
            default:
                return false;
        }
    }

    public static bool IsScalar(this Type? type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return type.IsPrimitive || TypeHelper.ScalarTypes(true).Any(x => x == type);
    }

    public static bool IsScalarArrayType(this Type? type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (!type.IsArray) return false;

        return type.GetElementType().IsScalar();
    }

    public static bool IsScalarEnumerableType(this Type? type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (!type.IsEnumerable()) return false;
        if (!type.IsGenericType) return false;

        return type.GenericTypeArguments.All(t => t.IsScalar());
    }

    public static string ToGenericsString(this Type? type)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (!type.IsGenericTypeDefinition && !type.IsGenericType)
            return type.ToString();

        var name = type.Name.Split('`').First();
        var sb = new StringBuilder();
        sb.Append(name);
        sb.Append('<');
        var args = type.GetGenericArguments();

        var i = 0;
        foreach (var argType in args)
        {
            if (i > 0)
                sb.Append(',');

            sb.Append(argType.Name);
            i++;
        }
        sb.Append('>');
        return sb.ToString();
    }
}

