namespace Foundation.ComponentModel;

public interface ITransactionEvent<TTransactionId, TEventId> : IEvent<TEventId>
{
    TTransactionId TransactionId { get; }
}

public interface ITransactionEvent<TTransactionId, TEventId, TCommandId> 
    : IEvent<TEventId, TCommandId>
    , ITransactionEvent<TTransactionId, TEventId>
{
}
