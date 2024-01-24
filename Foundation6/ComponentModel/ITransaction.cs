namespace Foundation.ComponentModel;

/// <summary>
/// Contract of an identifiable transaction.
/// </summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
public interface ITransaction<TId> : IDisposable
{
    /// <summary>
    /// Commit changes of the transaction.
    /// </summary>
    public void Commit();

    /// <summary>
    /// If true the transaction contains changes.
    /// </summary>
    bool HasChanges { get; }

    /// <summary>
    /// The identifier of the transaction.
    /// </summary>
    TId TransactionId { get; }
}

/// <summary>
/// Contract of an identifiable transaction.
/// </summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
/// <typeparam name="TDelegate">The type of the delegate which is called on Commit.</typeparam>
public interface ITransaction<TId, TDelegate> : ITransaction<TId>
    where TDelegate : Delegate
{
    /// <summary>
    /// Event is called when the transaction Commit was called.
    /// </summary>
    Event<TDelegate> Committed { get; }
}
