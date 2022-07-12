namespace Foundation.DesignPatterns.Saga;

public interface IRollback<TTransactionState, TRollbackResponse>
{
    TRollbackResponse Rollback(TTransactionState state);
}
