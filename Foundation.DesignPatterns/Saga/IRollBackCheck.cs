namespace Foundation.DesignPatterns.Saga;

public interface IRollBackCheck<TTransactionResponse>
{
    bool RequiresRollback(TTransactionResponse response);
}
