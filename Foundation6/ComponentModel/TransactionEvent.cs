namespace Foundation.ComponentModel;
public abstract record TransactionEvent<TTransactionId>(TTransactionId EventId)
    : EventBase<TTransactionId>(EventId)
    , ITransactionEvent<TTransactionId>
    where TTransactionId : notnull;

public abstract record TransactionEvent<TTransactionId, TCommandId>(TTransactionId EventId, TCommandId CommandId)
    : EventBase<TTransactionId, TCommandId>(EventId, CommandId)
    , ITransactionEvent<TTransactionId>
    where TTransactionId : notnull
    where TCommandId : notnull;
