namespace Foundation.DesignPatterns.Saga;

public interface IIdentifiableTransactionCompensation<TId, TTransactionState, TTransactionResponse, TCompensationResponse>
    : ITransactionCompensation<TTransactionState, TTransactionResponse, TCompensationResponse>
    , IIdentifiable<TId>
    where TId : notnull
{
}
