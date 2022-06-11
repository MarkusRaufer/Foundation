namespace Foundation;

public static class TypeResult
{
    public static TypedResult<T, Exception> Error<T>(Exception error)
    {
        return new TypedResult<T, Exception>(Opt.Some(error));
    }

    public static TypedResult<T, TError> Error<T, TError>(TError error)
    {
        return new TypedResult<T, TError>(Opt.Some(error));
    }

    public static TypedResult<T, Exception> Ok<T>()
    {
        return new TypedResult<T, Exception>(Opt.None<Exception>());
    }

    public static TypedResult<T, TError> Ok<T, TError>()
    {
        return new TypedResult<T, TError>(Opt.None<TError>());
    }
}

public struct TypedResult<T, TError>
    : IResult<Type, TError>
    , IEquatable<TypedResult<T, TError>>
{
    private readonly Opt<TError> _error;

    public TypedResult(Opt<TError> error)
    {
        _error = error;
        IsInitialized = true;
    }

    public static bool operator ==(TypedResult<T, TError> left, TypedResult<T, TError> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TypedResult<T, TError> left, TypedResult<T, TError> right)
    {
        return !(left == right);
    }


    public TError Error => _error.OrThrow();

    public bool IsError => _error.IsSome;

    public bool IsInitialized { get; }

    public bool IsOk => !IsError;

    public override bool Equals(object? obj) => obj is TypedResult<T, TError> other && Equals(other);

    public bool Equals(TypedResult<T, TError> other)
    {
        if (IsOk) return other.IsOk && Ok.Equals(other.Ok);

        return other.IsError && Error!.Equals(other.Error);
    }

    public override int GetHashCode()
    {
        if (IsOk) return System.HashCode.Combine(nameof(TypedResult<T, TError>), Ok);

        return System.HashCode.Combine(nameof(TypedResult<T, TError>), Error);
    }

    public Type Ok => typeof(T);

    public override string ToString()
    {
        return IsOk ? $"IsOk: {IsOk}, Ok: {Ok.FullName}"
                    : $"IsOk: {IsOk}, Error: {Error}";
    }
}

