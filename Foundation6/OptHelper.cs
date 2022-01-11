namespace Foundation;

public class OptHelper
{
    public static Opt<object> CreateOptFromObject(object? obj)
    {
        if (obj is null) return Opt.None<object>();

        var type = obj.GetType();
        if (type != typeof(Opt<>)) return Opt.None<object>();

        var isSomeProp = type.GetProperty(nameof(Opt<object>.IsSome));
        if (null == isSomeProp) return Opt.None<object>();

        if (isSomeProp.GetValue(obj) is not bool isSome || !isSome) return Opt.None<object>();

        var valueProp = type.GetProperty(nameof(Opt<object>.ValueAsObject));
        if (null == valueProp) return Opt.None<object>();

        var value = valueProp.GetValue(obj);
        return Opt.Maybe(value);
    }
}

