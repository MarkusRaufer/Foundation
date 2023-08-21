namespace Foundation.ComponentModel;

public interface ITransaction : ITransaction<Guid>
{
}

public interface ITransaction<TId> : IDisposable
{
    void Commit();
    bool HasChanges { get; }

    TId TransactionId { get; }
}
