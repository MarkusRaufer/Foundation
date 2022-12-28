using System.Runtime.CompilerServices;

namespace Foundation;

public class OptionHelper
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<object> CreateOptionFromObject(object? obj)
    {
        if (obj is null) return Option.None<object>();

        var type = obj.GetType();
        if (type != typeof(Option<>)) return Option.None<object>();

        var isSomeProp = type.GetProperty(nameof(Option<object>.IsSome));
        if (null == isSomeProp) return Option.None<object>();

        if (isSomeProp.GetValue(obj) is not bool isSome || !isSome) return Option.None<object>();

        var valueField = type.GetField("_value");
        if (null == valueField) return Option.None<object>();

        var value = valueField.GetValue(obj);
        return Option.Maybe(value);
    }
}

