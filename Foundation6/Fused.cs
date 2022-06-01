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

    public override string ToString() => $"{Value}";
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
            if (!initialValue.IsInitialized) return null == x;
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

/// <summary>
/// This structure reacts like a fuse. If the fuse has blown, the value won't change any longer.
/// </summary>
/// <typeparam name="T"></typeparam>
public struct Fused<T> : IEquatable<Fused<T>>
{
    private readonly Func<T, bool> _predicate;
    private T _value;

    public Fused(T? seed, [DisallowNull] Func<T, bool> predicate)
    {
        _value = seed.ThrowIfNull();
        _predicate = predicate.ThrowIfNull();
        IsBlown = false;
    }

    public static bool operator ==(Fused<T> left, Fused<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Fused<T> left, Fused<T> right)
    {
        return !(left == right);
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if(obj is Fused<T> other) return Equals(other);

        return false;
    }

    public bool Equals(Fused<T> other)
    {
        if (Value is null) return other.Value is null;
        return Value.Equals(other.Value);
    }

    public override int GetHashCode() => Value?.GetHashCode() ?? 0;

    public bool IsBlown { get; private set; }

    public override string ToString() => $"Value:{Value}, IsBlown:{IsBlown}";

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

