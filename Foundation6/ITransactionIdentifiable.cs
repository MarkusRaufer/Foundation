namespace Foundation;

public interface ITransactionIdentifiable<out T>
{
    T TransactionIdentifier { get; }
}

