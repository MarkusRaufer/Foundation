using System.Diagnostics.CodeAnalysis;

namespace Foundation;

public static class OptExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <param name="valueOnNull">In a sorted list it defines if the null values appear at the beginning -1 (Microsoft standard) or at the end 1.</param>
    /// <returns></returns>
    public static int CompareTo<T>(this Opt<T> lhs, Opt<T> rhs, int valueOnNull = -1)
        where T : IComparable<T>
    {
        if (lhs.IsNone) return rhs.IsNone ? 0 : valueOnNull;
        if (rhs.IsNone) return valueOnNull * -1;

        return lhs.Value!.CompareTo(rhs.Value);
    }

    public static bool Invoke<T>(this Opt<T> option, Action<T> action)
    {
        if (option.IsSome)
        {
            action(option.Value!);
            return true;
        }

        return false;
    }

    public static Opt<TResult> Map<T, TResult>(this Opt<T> option, [DisallowNull] Func<T, TResult> func)
    {
        if (option.IsSome)
        {
            return Opt.Some(func(option.Value!));
        }

        return Opt.None<TResult>();
    }

    public static bool IsSome<T>(this Opt<T> option, out T? value)
    {
        if (option.IsSome)
        {
            value = option.Value!;
            return true;
        }

        value = default;
        return false;
    }

    public static bool TryGetValue<T>(this Opt<T> option, out T? value)
    {
        if (option.IsSome)
        {
            value = option.Value;
            return true;
        }

        value = default;
        return false;
    }

    [return: NotNull]
    public static T ValueOrElse<T>(this Opt<T> option, [DisallowNull] T value)
    {
        return (option.IsSome) ? option.Value! : value.ThrowIfNull(nameof(value));
    }

    [return: NotNull]
    public static T ValueOrThrow<T>(this Opt<T> option)
    {
        return ValueOrThrow(option, () => new NullReferenceException(nameof(option)));
    }

    [return: NotNull]
    public static T ValueOrThrow<T, TException>(this Opt<T> option, [DisallowNull] Func<TException> exception)
        where TException : Exception
    {
        if (option.IsSome) return option.Value!;

        throw exception();
    }
}

