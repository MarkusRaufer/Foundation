namespace Foundation;

using System.Diagnostics;

public static class Result
{
    public static Result<Exception> Error(Exception error)
    {
        error.ThrowIfNull();
        return new Result<Exception>(error);
    }

    public static Result<TOk, Exception> Error<TOk>(Exception error)
    {
        error.ThrowIfNull();
        return new Result<TOk, Exception>(Option.None<TOk>(), Option.Some(error));
    }

    public static Result<TOk, TError> Error<TOk, TError>(TError error)
    {
        error.ThrowIfNull();
        return new Result<TOk, TError>(Option.None<TOk>(), Option.Some(error));
    }

    public static Result<Exception> Ok()
    {
        return new Result<Exception>();

    }

    public static Result<TOk, Exception> Ok<TOk>(TOk value)
    {
        value.ThrowIfNull();

        return new Result<TOk, Exception>(Option.Some(value), Option.None<Exception>());
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
    private readonly bool _isError;

    internal Result(TError error)
    {
        Error = error.ThrowIfNull();
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

    public TError Error { get; }

    public override bool Equals(object? obj) => obj is Result<TError> other && Equals(other);

    public bool Equals(Result<TError> other)
    {
        if (IsOk) return other.IsOk;

        return !other.IsOk
            && EqualityComparer<TError>.Default.Equals(Error, other.Error);
    }

    public override int GetHashCode() => IsOk 
                                         ? typeof(Result<TError>).GetHashCode()
                                         : System.HashCode.Combine(typeof(Result<TError>), Error);

    /// <summary>
    /// Is true if Result has a value <see cref="IsOk"/> otherwise false;
    /// </summary>
    public bool IsOk => !_isError;

    public override string ToString()
        => IsOk ? $"IsOk: {IsOk}" : $"IsOk: {IsOk}, Error: {Error}";
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
        if (IsOk) return other.IsOk && EqualityComparer<TOk>.Default.Equals(Ok, other.Ok);

        return !other.IsOk && EqualityComparer<TError>.Default.Equals(Error, other.Error);
    }

    public TError Error
    {
        get
        {
            if (IsOk) throw new ArgumentException("The result conains no error");
            return _error.OrThrow();
        }
    }

    public override int GetHashCode() => _hashCode;

    public bool IsOk => _ok.IsSome;

    public TOk Ok
    {
        get
        {
            if (!IsOk) throw new ArgumentException("Result contains an error");

            return _ok.OrThrow();
        }
    }

    public override string ToString()
    {
        return IsOk
            ? $"IsOk: {IsOk}, Ok: {Ok}"
            : $"IsOk: {IsOk}, Error: {Error}";
    }
}
