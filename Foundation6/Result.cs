namespace Foundation;

using System.Diagnostics;

public static class Result
{
    public static Result<Exception> Error(Exception error)
    {
        return new Result<Exception>(error);
    }

    public static Result<TOk, Exception> Error<TOk>(Exception error)
    {
        return new Result<TOk, Exception>(Opt.None<TOk>(), Opt.Some(error));
    }

    public static Result<TOk, TError> Error<TOk, TError>(TError error)
    {
        return new Result<TOk, TError>(Opt.None<TOk>(), Opt.Some(error));
    }

    public static Result<Exception> Ok()
    {
        return new Result<Exception>();

    }

    public static Result<TOk, Exception> Ok<TOk>(TOk value)
    {
        if (ReferenceEquals(null, value)) throw new ArgumentNullException(nameof(value));
        return new Result<TOk, Exception>(Opt.Some(value), Opt.None<Exception>());
    }

    public static Result<TOk, TError> Ok<TOk, TError>(TOk value)
    {
        return new Result<TOk, TError>(Opt.Some(value), Opt.None<TError>());
    }
}

public struct Result<TError>
    : IBooleanResult<TError>
    , IEquatable<Result<TError>>
{
    internal Result(TError error)
    {
        Error = error;
        IsError = true;
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

        return IsError && other.IsError
            && EqualityComparer<TError>.Default.Equals(Error, other.Error);
    }

    public override int GetHashCode() => System.HashCode.Combine(IsOk, IsError, Error);

    public bool IsError { get; }

    public bool IsOk => !IsError;

    public override string ToString() => IsOk ? $"IsOk: {IsOk}" : $"IsOk: {IsOk}, Error: {Error}";
}

[DebuggerDisplay("IsOk={IsOk}")]
[Serializable]
public struct Result<TOk, TError>
    : IResult<TOk, TError>
    , IEquatable<Result<TOk, TError>>
{
    private int _hashCode;
    private readonly Opt<TOk> _ok;
    private readonly Opt<TError> _error;

    //TODO: remove Opt from error.
    internal Result(Opt<TOk> ok, Opt<TError> error)
    {
        if (ok.IsSome ^ error.IsNone)
            throw new ArgumentException($"{nameof(ok)} and {nameof(error)} are disjoint.");

        _ok = ok;
        _error = error;

        _hashCode = ok.IsSome
            ? System.HashCode.Combine(typeof(TOk), ok.ValueOrThrow())
            : System.HashCode.Combine(typeof(TError), error.ValueOrThrow());
    }

    public static bool operator ==(Result<TOk, TError> left, Result<TOk, TError> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Result<TOk, TError> left, Result<TOk, TError> right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj) => obj is Result<TOk, TError> other && Equals(other);

    public bool Equals(Result<TOk, TError> other)
    {
        if (IsOk) return other.IsOk && EqualityComparer<TOk>.Default.Equals(Ok, other.Ok);

        return other.IsError && EqualityComparer<TError>.Default.Equals(Error, other.Error);
    }

    public TError Error
    {
        get
        {
            if (IsOk) throw new ArgumentException("The result conains no error");
            return _error.ValueOrThrow();
        }
    }

    public override int GetHashCode() => _hashCode;

    public bool IsError => !IsOk;

    public bool IsOk => _ok.IsSome;

    public TOk Ok
    {
        get
        {
            if (IsError)
                throw new ArgumentException("Ok is not valid because the result contains an error");

            return _ok.ValueOrThrow();
        }
    }

    public override string ToString()
    {

        return IsOk
            ? $"IsOk: {IsOk}, Ok: {Ok}"
            : $"IsOk: {IsOk}, Error: {Error}";
    }
}

