using System.Diagnostics.CodeAnalysis;

namespace Foundation;

public static class FuncHelper
{
    public static Func<object, object?> CreateObjectFunc<T, TResult>([DisallowNull] Func<T, TResult>? func)
    {
        ArgumentNullException.ThrowIfNull(func, nameof(func));

        return x => x is T t ? func(t) : null;
    }

    public static Func<object, bool> CreateObjectPredicate<T>([DisallowNull] Func<T, bool> func)
    {
        return t => func((T)t);
    }

    public static Func<string, object>? StringToScalarValueConverter([DisallowNull] Type type)
    {
        type.ThrowIfNull(nameof(type));

        if (!type.IsScalar()) return null;

        return Type.GetTypeCode(type) switch
        {
            TypeCode.Boolean => str => bool.TryParse(str, out bool result) ? result : default,
            TypeCode.Byte => str => byte.TryParse(str, out byte result) ? result : default,
            TypeCode.Char => str => char.TryParse(str, out char result) ? result : default,
            TypeCode.DateTime => str => DateTime.TryParse(str, out DateTime result) ? result : default,
            TypeCode.Decimal => str => decimal.TryParse(str, out decimal result) ? result : default,
            TypeCode.Double => str => double.TryParse(str, out double result) ? result : default,
            TypeCode.Int16 => str => short.TryParse(str, out short result) ? result : default,
            TypeCode.Int32 => str => int.TryParse(str, out int result) ? result : default,
            TypeCode.Int64 => str => long.TryParse(str, out long result) ? result : default,
            TypeCode.SByte => str => SByte.TryParse(str, out SByte result) ? result : default,
            TypeCode.Single => str => Single.TryParse(str, out Single result) ? result : default,
            TypeCode.String => str => str,
            TypeCode.UInt16 => str => ushort.TryParse(str, out ushort result) ? result : default,
            TypeCode.UInt32 => str => uint.TryParse(str, out uint result) ? result : default,
            TypeCode.UInt64 => str => ulong.TryParse(str, out ulong result) ? result : default,
            _ => null
        };
    }
}

