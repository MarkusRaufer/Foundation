namespace Foundation.DesignPatterns.Saga;

public interface ICompensableFailureCheck<TTransactionResponse>
{
    bool IsFailureCompensable(TTransactionResponse response);
}
