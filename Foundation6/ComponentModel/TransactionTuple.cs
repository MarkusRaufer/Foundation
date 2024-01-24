namespace Foundation.ComponentModel;

public record TransactionControl(TransactionAction Action) 
    : TransactionControl<TransactionAction>(Action);

public record TransactionControl<TAction>(TAction Action) : ITransactionControl<TAction>;

public static class TransactionTuple
{
    public static TransactionTuple<TChange> New<TChange>(TransactionAction Action, TChange Change)
        => new(Action, Change);

    public static TransactionTuple<TAction, TChange> New<TAction, TChange>(TAction Action, TChange Change)
        => new(Action, Change);
}

public record TransactionTuple<TChange>(TransactionAction Action, TChange Change)
    : TransactionControl(Action);

public record TransactionTuple<TAction, TChange>(TAction Action, TChange Change)
    : TransactionControl<TAction>(Action)
    , ITransactionTuple<TAction, TChange>;
