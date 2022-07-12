namespace Foundation.DesignPatterns.Saga;

public interface ITransactionCompensation<TTransactionState, TTransactionResponse, TCompensationResponse>
    : ICompensableFailureCheck<TTransactionResponse>
    , ICompensable<TTransactionState, TCompensationResponse>
{
}
