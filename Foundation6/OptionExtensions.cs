using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Foundation;

public static class OptionExtensions
{
    /// <summary>
    /// Compares two optionals.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CompareTo<T>(this Option<T> lhs, Option<T> rhs)
        where T : IComparable<T>
    {
        if(lhs.IsNone) return rhs.IsNone ? 0 : - 1;
        if(rhs.IsNone) return 1;

        return lhs.Value!.CompareTo(rhs.Value);
    }

    /// <summary>
    /// Calls <paramref name="some"/> if IsSome is true. Calls <paramref name="none"/> if IsSome is false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <param name="some"></param>
    /// <param name="none"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Invoke<T>(
        this Option<T> option,
        Action<T> some,
        Action none)
    {
        some.ThrowIfNull();
        none.ThrowIfNull();

        if (option.IsSome)
        {
            some.Invoke(option.Value!);
            return true;
        }

        none.Invoke();

        return false;
    }

    /// <summary>
    /// Calls <paramref name="some"/> if IsSome is true. Calls <paramref name="none"/> if IsSome is false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="option"></param>
    /// <param name="some"></param>
    /// <param name="none"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TResult Match<T, TResult>(
        this Option<T> option, 
        Func<T, TResult> some, 
        Func<TResult> none)
        where TResult : notnull
    {
        some.ThrowIfNull();
        none.ThrowIfNull();

        return option.IsSome ? some(option.Value!) : none();
    }

    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unit OnNone<T>(this Option<T> option, Action action)
    {
        if (option.IsNone) action.Invoke();

        return new Unit();
    }

    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Unit OnSome<T>(this Option<T> option, Action<T> action)
    {
        if (option.IsSome) action.Invoke(option.Value!);

        return new Unit();
    }

    /// <summary>
    /// If lhs has a value lhs is returned otherwise rhs is returned.
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    /// <param name="lhs">The left option containing a possible value.</param>
    /// <param name="rhs">The right option containing a possible value.</param>
    /// <returns></returns>
    public static Option<T> Or<T>(this Option<T> lhs, Option<T> rhs)
    {
        return lhs.IsSome ? lhs : rhs;
    }

    /// <summary>
    /// Returns the value if IsSome is true or returns <paramref name="none"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <param name="none"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Or<T>(this Option<T> option, T none)
    {
        return option.IsSome ? option.Value! : none.ThrowIfNull();
    }

    /// <summary>
    /// Returns value if IsNone or calls <paramref name="none"/>;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <param name="none"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Or<T>(this Option<T> option, Func<T> none)
    {
        none.ThrowIfNull();

        if(option.IsSome) return option.Value!;

        var value = none();

        return value.ThrowIf(() => null == value, $"{nameof(none)} returned null");
    }

    /// <summary>
    /// Throws NullReferenceException if IsSome is false or returns a value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T OrThrow<T>(this Option<T> option)
    {
        return OrThrow(option, () => new NullReferenceException(nameof(option)));
    }

    /// <summary>
    /// Throws an exception if IsSome is false or returns a value. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TException">exception which will be thrown when IsSome is false.</typeparam>
    /// <param name="option"></param>
    /// <param name="exception"></param>
    /// <returns></returns>
    [return: NotNull]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T OrThrow<T, TException>(this Option<T> option, Func<TException> exception)
        where TException : Exception
    {
        if (option.IsSome) return option.Value!;

        throw exception();
    }

    /// <summary>
    /// If returning true it has a value otherwise it returns false.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="option">The option containing the possible value.</param>
    /// <param name="value">The value if the option has some value.</param>
    /// <returns></returns>
    public static bool TryGet<T>(this Option<T> option, out T? value)
    {
        if(option.IsSome)
        {
            value = option.OrThrow();
            return true;
        }

        value = default;
        return false;
    }
}

