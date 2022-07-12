namespace Foundation.DesignPatterns.Saga;

public interface ITransactionAsyncRollback<TTransactionState, TTransactionResponse, TRollbackResponse>
    : IRollBackCheck<TTransactionResponse>
    , IAsyncRollback<TTransactionState, TRollbackResponse>
{
}
