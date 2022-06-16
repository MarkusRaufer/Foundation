using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Foundation;

public static class OptExtensions
{
    /// <summary>
    /// Compares two optionals.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lhs"></param>
    /// <param name="rhs"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CompareTo<T>(this Opt<T> lhs, Opt<T> rhs)
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
    public static bool Match<T>(
        this Opt<T> option,
        Action<T>? some = null,
        Action? none = null)
    {
        if (option.IsSome)
        {
            some?.Invoke(option.Value!);
            return true;
        }

        none?.Invoke();

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
        this Opt<T> option, 
        [DisallowNull] Func<T, TResult> some, 
        [DisallowNull] Func<TResult> none)
        where TResult : notnull
    {
        some.ThrowIfNull();
        none.ThrowIfNull();

        return option.IsSome ? some(option.Value!) : none();
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
    public static T Or<T>(this Opt<T> option, [DisallowNull] T none)
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
    public static T Or<T>(this Opt<T> option, [DisallowNull] Func<T> none)
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
    public static T OrThrow<T>(this Opt<T> option)
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
    public static T OrThrow<T, TException>(this Opt<T> option, [DisallowNull] Func<TException> exception)
        where TException : Exception
    {
        if (option.IsSome) return option.Value!;

        throw exception();
    }
}

