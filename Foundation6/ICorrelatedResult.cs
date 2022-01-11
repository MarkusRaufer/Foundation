namespace Foundation;

public interface ICorrelatedResult<TCorrelationId, TOk, TError>
    : IResult<TOk, TError>
    , ICorrelatable<TCorrelationId>
    where TCorrelationId : notnull
{
}
