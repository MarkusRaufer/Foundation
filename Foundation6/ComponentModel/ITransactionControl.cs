namespace Foundation.ComponentModel;

public interface ITransactionControl : ITransactionControl<TransactionAction>
{
}

public interface ITransactionControl<TAction>
{
    TAction Action { get; init; }
}
