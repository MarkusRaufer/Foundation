namespace Foundation.DesignPatterns.Saga;

public interface IRollbackProvider<TId, TStrategy>
{
    TStrategy GetRollbackStrategy(TId strategyId);
}

public interface IRollbackProvider<TId, TTransactionState, TTransactionResponse, TRollbackResponse>
    : IRollbackProvider<TId, IIdentifiableTransactionRollback<TId, TTransactionState, TTransactionResponse, TRollbackResponse>>
{
}
