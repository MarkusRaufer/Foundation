using System.Diagnostics.CodeAnalysis;

namespace Foundation;

public static class CorrelatedResult
{
    public static CorrelatedResult<Guid, TOk, TError> Error<TOk, TError>(Guid id, TError error)
    {
        return new CorrelatedResult<Guid, TOk, TError>(id, error);
    }

    public static CorrelatedResult<TId, TOk, TError> Error<TId, TOk, TError>(TId id, TError error)
        where TId : notnull
    {
        return new CorrelatedResult<TId, TOk, TError>(id, error);
    }

    public static CorrelatedResult<Guid, TOk, TError> Ok<TOk, TError>(Guid id, TOk ok)
    {
        return new CorrelatedResult<Guid, TOk, TError>(id, ok);
    }

    public static CorrelatedResult<TId, TOk, TError> Ok<TId, TOk, TError>(TId id, TOk ok)
        where TId : notnull
    {
        return new CorrelatedResult<TId, TOk, TError>(id, ok);
    }
}


public struct CorrelatedResult<TCorrelationId, TOk, TError>
    : ICorrelatedResult<TCorrelationId, TOk, TError>
    where TCorrelationId : notnull
{
    private readonly Result<TOk, TError> _result;

    internal CorrelatedResult(TCorrelationId id, TOk ok)
    {
        CorrelationId = id;

        _result = Result.Ok<TOk, TError>(ok.ThrowIfNull());
    }

    internal CorrelatedResult(TCorrelationId id, TError error)
    {
        CorrelationId = id;
        _result = Result.Error<TOk, TError>(error);
    }

    public TCorrelationId CorrelationId { get; }

    public bool IsOk => _result.IsOk;

    public override string ToString() => $"CorrelationId: {CorrelationId} Result: {_result}";

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

