namespace Foundation.ComponentModel;

public interface ITransactionTuple<TChange>
    : ITransactionTuple<TransactionAction, TChange>
    , ITransactionControl
{
}

public interface ITransactionTuple<TAction, TChange> : ITransactionControl<TAction>
{
    TChange Change { get; init; }
}