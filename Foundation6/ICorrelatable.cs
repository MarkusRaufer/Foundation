namespace Foundation;

public interface ICorrelatable<TId>
    where TId : notnull
{
    TId CorrelationId { get; }
}
