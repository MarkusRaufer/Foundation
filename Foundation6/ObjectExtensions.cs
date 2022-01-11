namespace Foundation;

using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

public static class ObjectExtensions
{
    public static T AssignIfNull<T>(this object? obj, [DisallowNull] Func<T> creator)
    {
        if (obj is T t) return t;

        creator.ThrowIfNull(nameof(creator));

        return creator();
    }

    /// <summary>
    /// Converts an object to a target type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T? ConvertTo<T>(this object? obj)
    {
        if (null == obj) return default;

        if (obj is T target) return target;

        var targetType = typeof(T);
        var objectType = obj.GetType();

        {
            if (TypeDescriptor.GetConverter(obj) is TypeConverter converter)
            {
                if (converter.CanConvertTo(targetType) && converter.ConvertTo(obj, targetType) is T t) return t;
            }
        }

        {
            if (TypeDescriptor.GetConverter(targetType) is TypeConverter converter)
            {
                if (converter.CanConvertFrom(objectType) && converter.ConvertFrom(obj) is T t) return t;
            }
        }

        return default;
    }

    public static object? ConvertTo(this object? obj, [DisallowNull] Type targetType)
    {
        targetType.ThrowIfNull(nameof(targetType));

        if (null == obj) return null;

        var objectType = obj.GetType();
        if (objectType == targetType) return obj;

        var converter = TypeDescriptor.GetConverter(obj);
        if (null == converter) return null;

        if (converter.CanConvertTo(targetType))
        {
            var converted = converter.ConvertTo(obj, targetType);
            if (converted is not null) return converted;
        }

        converter = TypeDescriptor.GetConverter(targetType);
        if (null == converter) return null;

        if (converter.CanConvertFrom(objectType))
        {
            var converted = converter.ConvertFrom(obj);
            if (converted is not null) return converted;
        }

        return null;
    }

    public static object? DynamicCastTo(this object? obj, params Type[] typeArgs)
    {
        if (null == obj) return null;

        var castMethod = typeof(ObjectExtensions).GetMethod("CastTo")?.MakeGenericMethod(typeArgs);
        return castMethod?.Invoke(null, new[] { obj });
    }

    /// <summary>
    /// Compares two objects. Treats like Object.Equals, but allows that objA is null.
    /// </summary>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    public static bool EqualsNullable(this object? lhs, object? rhs)
    {
        if (ReferenceEquals(lhs, rhs)) return true;

        if (null == lhs) return null == rhs;

        return lhs.Equals(rhs);
    }

    /// <summary>
    /// Casts dynamically to a generic type. The first type must be a generic type. eg: List<>.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="typeArguments"></param>
    /// <returns></returns>
    public static object? GenericDynamicCastTo(this object? obj, params Type[] typeArguments)
    {
        if (null == obj) return null;

        var genericType = obj.GetType().CreateGenericType(typeArguments);

        return DynamicCastTo(obj, genericType);
    }

    public static int GetInstanceHashCode(this object? obj) => RuntimeHelpers.GetHashCode(obj);

    public static int GetNullableHashCode(this object? obj) => (null == obj) ? 0 : obj.GetHashCode();

    /// <summary>
    /// Checks if an object is of a generic type.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="type">e.g. IList<> (without definded generic parameter.</param>
    /// <returns></returns>
    public static bool IsOfGenericType(this object obj, [DisallowNull] Type type)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));

        var objType = obj.GetType();
        if (!objType.IsGenericType) return false;

        var generic = objType.GetGenericTypeDefinition();
        return generic == type;
    }

    public static T ThrowIf<T>(this T? obj, [DisallowNull] Func<T, bool> predicate, [DisallowNull] string name)
    {
        if (null == obj) throw new ArgumentNullException(name);

        return predicate(obj) ? obj : throw new ArgumentNullException(name);
    }

    [return: NotNull]
    public static T ThrowIfNull<T>(this T? obj, [DisallowNull] string name) => obj ?? throw new ArgumentNullException(name);

    /// <summary>
    /// transforms an object to a boolean if it is convertible.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="anyValue">if true, it tries to convert the object to byte, number or string and checks if it has the value 0 respectively True or False.</param>
    /// <returns></returns>
    public static bool ToBool(this object? obj, bool anyValue = false)
    {
        if (null == obj) return false;

        if (obj is bool boolean) return boolean;
        if (!anyValue) return false;

        return obj switch
        {
            byte b => 0 != b,
            decimal m => 0 != m,
            double d => 0 != d,
            float f => 0 != f,
            Int16 i16 => 0 != i16,
            UInt16 ui16 => 0 != ui16,
            Int32 i32 => 0 != i32,
            UInt32 ui32 => 0 != ui32,
            Int64 i64 => 0 != i64,
            UInt64 ui64 => 0 != ui64,
            string str => bool.TryParse(str, out bool result) && result,
            _ => false
        };
    }

    public static byte[]? ToByteArray(this object? obj, Encoding? encoding = null)
    {
        return obj switch
        {
            byte b => new byte[] { b },
            char c => BitConverter.GetBytes(c),
            DateTime dt => BitConverter.GetBytes(dt.Ticks),
            decimal m => m.ToByteArray(),
            double d => BitConverter.GetBytes(d),
            float f => BitConverter.GetBytes(f),
            Int16 i16 => BitConverter.GetBytes(i16),
            Int32 i32 => BitConverter.GetBytes(i32),
            Int64 i64 => BitConverter.GetBytes(i64),
            UInt16 ui16 => BitConverter.GetBytes(ui16),
            UInt32 ui32 => BitConverter.GetBytes(ui32),
            UInt64 ui64 => BitConverter.GetBytes(ui64),
            SByte sb => BitConverter.GetBytes(sb),
            string str => null != encoding ? encoding.GetBytes(str) : Encoding.UTF8.GetBytes(str),
            _ => default
        };
    }

    public static DateTime? ToDateTime(this object? value)
    {
        return value switch
        {
            DateTime dt => dt,
            int i => new DateTime(i),
            long l => new DateTime(l),
            _ => null
        };
    }

    public static DateTime? ToDateTime(this object? value, DateTimeKind kind)
    {
        return value switch
        {
            DateTime dt => new DateTime(dt.Ticks, kind),
            int i => new DateTime(i, kind),
            long l => new DateTime(l, kind),
            _ => null
        };
    }

    public static Opt<T> ToOpt<T>(this object? obj) => (obj is T value) ? Opt.Some(value) : Opt.None<T>();

    public static Opt<T> ToOpt<T>(this T? obj) => null == obj ? Opt.None<T>() : Opt.Some(obj);

    public static string ToStringOrEmpty(this object? obj)
    {
        if (obj is null) return String.Empty;

        return obj.ToString() ?? String.Empty;
    }

    public static T ValueOr<T>(this object? value, T or) => value is T t ? t : or;
}

