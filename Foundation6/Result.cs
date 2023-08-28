namespace Foundation;

using System.Diagnostics.CodeAnalysis;

public static class Result
{
    public static Result<Error> Error(Error error)
    {
        error.ThrowIfNull();
        return new Result<Error>(error);
    }

    public static Result<Error> Error(Exception exception)
    {
        exception.ThrowIfNull();
        return new Result<Error>(Foundation.Error.FromException(exception));
    }

    public static Result<TOk, Error> Error<TOk>(Error error)
    {
        error.ThrowIfNull();
        return new Result<TOk, Error>(Option.None<TOk>(), Option.Some(error));
    }

    public static Result<TOk, Error> Error<TOk>(Exception exception)
    {
        exception.ThrowIfNull();
        return new Result<TOk, Error>(Option.None<TOk>(), Option.Some(Foundation.Error.FromException(exception)));
    }

    public static Result<TOk, TError> Error<TOk, TError>(TError error)
    {
        error.ThrowIfNull();
        return new Result<TOk, TError>(Option.None<TOk>(), Option.Some(error));
    }

    public static Result<Error> Ok()
    {
        return new Result<Error>();
    }

    public static Result<TOk, Error> Ok<TOk>(TOk value)
    {
        value.ThrowIfNull();

        return new Result<TOk, Error>(Option.Some(value), Option.None<Error>());
    }

    public static Result<TOk, TError> Ok<TOk, TError>(TOk value)
    {
        value.ThrowIfNull();
        return new Result<TOk, TError>(Option.Some(value), Option.None<TError>());
    }
}

/// <summary>
/// This is a result that can be only Error.
/// </summary>
/// <typeparam name="TError"></typeparam>
public readonly struct Result<TError>
    : IResult<TError>
    , IEquatable<Result<TError>>
{
    private readonly TError _error;
    private readonly bool _isError;

    internal Result(TError error)
    {
        _error = error.ThrowIfNull();
        _isError = true;
    }

    public static bool operator ==(Result<TError> left, Result<TError> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Result<TError> left, Result<TError> right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj) => obj is Result<TError> other && Equals(other);

    public bool Equals(Result<TError> other)
    {
        if (IsOk) return other.IsOk;

        return !other.IsOk
            && EqualityComparer<TError>.Default.Equals(_error, other._error);
    }

    public override int GetHashCode() => IsOk 
                                         ? typeof(Result<TError>).GetHashCode()
                                         : System.HashCode.Combine(typeof(Result<TError>), _error);

    /// <summary>
    /// Is true if Result has a value <see cref="IsOk"/> otherwise false;
    /// </summary>
    public bool IsOk => !_isError;

    public override string ToString()
        => IsOk ? $"IsOk: {IsOk}" : $"IsOk: {IsOk}, Error: {_error}";

    /// <summary>
    /// Returns true if result contains an error.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public bool TryGetError(out TError? error)
    {
        if(!IsOk)
        {
            error = _error;
            return true;
        }

        error = default;
        return false;
    }
}

[Serializable]
public readonly struct Result<TOk, TError>
    : IResult<TOk, TError>
    , IEquatable<Result<TOk, TError>>
{
    private readonly int _hashCode;
    private readonly Option<TOk> _ok;
    private readonly Option<TError> _error;

    internal Result(Option<TOk> ok, Option<TError> error)
    {
        if (ok.IsSome ^ error.IsNone)
            throw new ArgumentException($"{nameof(ok)} and {nameof(error)} are disjoint.");

        _ok = ok;
        _error = error;

        _hashCode = ok.IsSome
            ? System.HashCode.Combine(typeof(Result<TOk, TError>), ok.OrThrow())
            : System.HashCode.Combine(typeof(Result<TOk, TError>), error.OrThrow());
    }

    public static bool operator ==(Result<TOk, TError> left, Result<TOk, TError> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Result<TOk, TError> left, Result<TOk, TError> right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
        => obj is Result<TOk, TError> other && Equals(other);

    public bool Equals(Result<TOk, TError> other)
    {
        if (!IsOk)
        {
            if(other.IsOk) return false;
            return _error.Equals(other._error);
        }
        if (!other.IsOk) return false;

        return _ok.Equals(other._ok);
    }

    public override int GetHashCode() => _hashCode;

    public bool IsOk => _ok.IsSome;

    public override string ToString()
    {
        return IsOk
            ? $"IsOk: {IsOk}, Ok: {_ok.OrThrow()}"
            : $"IsOk: {IsOk}, Error: {_error.OrThrow()}";
    }

    /// <summary>
    /// Returns true if result contains an error otherwise returns false and error has the default value.
    /// </summary>
    /// <param name="error"></param>
    /// <returns></returns>
    public bool TryGetError([NotNullWhen(true)] out TError? error)
    {
        if(!IsOk)
        {
            error = _error.OrThrow();
            return true;
        }

        error = default;
        return false;
    }

    /// <summary>
    /// Returns true if result contains a value otherwise false and ok is set to the default value.
    /// </summary>
    /// <param name="ok"></param>
    /// <returns></returns>
    public bool TryGetOk([NotNullWhen(true)] out TOk? ok)
    {
        if (IsOk)
        {
            ok = _ok.OrThrow();
            return true;
        }

        ok = default;
        return false;
    }
}
