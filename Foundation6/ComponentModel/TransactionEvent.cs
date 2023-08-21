namespace Foundation.ComponentModel;
public abstract record TransactionEvent<TTransactionId, TEventId>(TTransactionId TransactionId, TEventId EventId)
    : ITransactionEvent<TTransactionId, TEventId>
    where TTransactionId : notnull
    where TEventId : notnull;

public abstract record TransactionEvent<TTransactionId, TEventId, TCommandId>(TTransactionId TransactionId, TEventId EventId, TCommandId CommandId)
    : TransactionEvent<TTransactionId, TEventId>(TransactionId, EventId)
    , ITransactionEvent<TTransactionId, TEventId, TCommandId>
    where TTransactionId : notnull
    where TEventId : notnull
    where TCommandId : notnull;
