namespace Foundation.DesignPatterns.Saga;

public interface ITransactionRollback<TTransactionState, TTransactionResponse, TRollbackResponse>
    : IRollBackCheck<TTransactionResponse>
    , IRollback<TTransactionState, TRollbackResponse>
{
}
