using System.Diagnostics.CodeAnalysis;

namespace Foundation;

public static class IdResult
{
    public static IdResult<Guid, TOk, TError> Error<TOk, TError>(Guid id, TError error)
    {
        return new IdResult<Guid, TOk, TError>(id, error);
    }

    public static IdResult<TId, TOk, TError> Error<TId, TOk, TError>(TId id, TError error)
        where TId : notnull
    {
        return new IdResult<TId, TOk, TError>(id, error);
    }

    public static IdResult<Guid, TOk, TError> Ok<TOk, TError>(Guid id, TOk ok)
    {
        return new IdResult<Guid, TOk, TError>(id, ok);
    }

    public static IdResult<TId, TOk, TError> Ok<TId, TOk, TError>(TId id, TOk ok)
        where TId : notnull
    {
        return new IdResult<TId, TOk, TError>(id, ok);
    }
}

/// <summary>
/// This is an identifiable result. Sometimes necessary int concurrent environments.
/// </summary>
/// <typeparam name="TId"></typeparam>
/// <typeparam name="TOk"></typeparam>
/// <typeparam name="TError"></typeparam>
public readonly struct IdResult<TId, TOk, TError>
    : IIdResult<TId, TOk, TError>
    , IEquatable<IdResult<TId, TOk, TError>>
    , IIdentifiable<TId>
    where TId : notnull
{
    private readonly Result<TOk, TError> _result;

    internal IdResult(TId id, TOk ok)
    {
        Id = id;

        _result = Result.Ok<TOk, TError>(ok.ThrowIfNull());
    }

    internal IdResult(TId id, TError error)
    {
        Id = id;
        _result = Result.Error<TOk, TError>(error);
    }


    public static bool operator ==(IdResult<TId, TOk, TError> left, IdResult<TId, TOk, TError> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(IdResult<TId, TOk, TError> left, IdResult<TId, TOk, TError> right)
    {
        return !(left == right);
    }

    public TId Id { get; }

    public override int GetHashCode() => System.HashCode.Combine(Id, _result);

    public bool IsOk => _result.IsOk;

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is IdResult<TId, TOk, TError> other && Equals(other);

    public bool Equals(IdResult<TId, TOk, TError> other)
    {
        if (!Id.Equals(other.Id)) return false;

        return IsOk
            ? TryGetOk(out var ok) && other.TryGetOk(out var otherOk) && ok.Equals(otherOk)
            : TryGetError(out var error) && other.TryGetError(out var otherError) && error.Equals(otherError);
    }

    public override string ToString() => $"Id: {Id} Result: {_result}";

    public bool TryGetError([NotNullWhen(true)] out TError? error)
    {
        if (!IsOk) return _result.TryGetError(out error);

        error = default;
        return false;
    }

    public bool TryGetOk([NotNullWhen(true)] out TOk? ok)
    {

        if (IsOk) return _result.TryGetOk(out ok);

        ok = default;
        return false;
    }
}

