using System.Diagnostics.CodeAnalysis;

namespace Foundation;

public struct Fused
{
    public static FusedValue<T> Value<T>([DisallowNull] T value)
    {
        return new FusedValue<T>(value);
    }
}

public struct FusedValue<T>
{
    internal FusedValue([DisallowNull] T value)
    {
        Value = value.ThrowIfNull();
        IsInitialized = true;
    }

    public bool IsInitialized { get; }

    public T Value { get; }
}

public static class FusedValueExtensions
{
    public static Fused<T> BlowIf<T>(this FusedValue<T> value, [DisallowNull] Func<T, bool> predicate)
    {
        return new Fused<T>(value.Value, predicate);
    }

    public static Fused<T> BlowIfChanged<T>(this FusedValue<T> value)
        where T : IComparable<T>
    {
        var initialValue = value;
        return new Fused<T>(value.Value, x =>
        {
            if (initialValue.IsInitialized) return null == x;
            return !initialValue.Value.Equals(x);
        });
    }

    public static Fused<T> BlowIfGreater<T>(this FusedValue<T> value, T theshold)
        where T : IComparable<T>
    {
        return new Fused<T>(value.Value, x => x.CompareTo(theshold) == 1);
    }

    public static Fused<T> BlowIfGreaterEqual<T>(this FusedValue<T> value, T theshold)
        where T : IComparable<T>
    {
        return new Fused<T>(value.Value, x => x.CompareTo(theshold) != -1);
    }

    public static Fused<T> BlowIfLess<T>(this FusedValue<T> value, T theshold)
        where T : IComparable<T>
    {
        return new Fused<T>(value.Value, x => x.CompareTo(theshold) == -1);
    }

    public static Fused<T> BlowIfLessEqual<T>(this FusedValue<T> value, T theshold)
        where T : IComparable<T>
    {
        return new Fused<T>(value.Value, x => x.CompareTo(theshold) != 1);
    }
}

public struct Fused<T>
{
    private readonly Func<T, bool> _predicate;
    private T _value;

    public Fused(T? value, [DisallowNull] Func<T, bool> predicate)
    {
        _value = value.ThrowIfNull();
        _predicate = predicate.ThrowIfNull();
        IsBlown = false;
    }

    public bool IsBlown { get; private set; }

    public T Value
    {
        get { return _value; }
        set
        {
            if (IsBlown) return;
            IsBlown = _predicate(value);
            _value = value;
        }
    }
}

