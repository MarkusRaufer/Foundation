namespace Foundation.ComponentModel;

public record TransactionStarted(Guid EventId, DateTime StartedOn) : TransactionStarted<Guid>(EventId, StartedOn);

public record TransactionStarted<TTransactionId>(TTransactionId EventId, DateTime StartedOn) 
    : TransactionEvent<TTransactionId>(EventId)
    , IStartedEvent<TTransactionId>
    where TTransactionId : notnull;
