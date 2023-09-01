namespace Foundation;

public interface ICorrelatable<TId>
{
    TId CorrelationId { get; }
}
