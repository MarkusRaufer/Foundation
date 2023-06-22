namespace Foundation.ComponentModel;

public class Transaction<TChange> : Transaction<Guid, TChange>
{
    public Transaction() : this(Guid.NewGuid())
    {
    }

    public Transaction(Guid transactionId) : base(transactionId)
    {
    }
}

public class Transaction<TId, TChange>
    : Transaction<TId, TChange, Action<IList<TChange>>>
{
    public Transaction(TId transactionId) : base(transactionId)
    {
    }

    public override void Commit() => Committed?.Publish(Changes);
}

/// <summary>
/// With this class you can record changes and on <see cref="Commit"/> the event <see cref="Committed"/> is called.
/// </summary>
/// <typeparam name="TAction">This value is needed to describe what kind of action is executed.</typeparam>
/// <typeparam name="TChangeTuple">This tuple should contain the TAction and the value changes.</typeparam>
/// <typeparam name="TDelegate">This is the delegate which will be called on <see cref="Commit"/></typeparam>
public abstract class Transaction<TId, TChange, TDelegate> : ITransaction<TId>
    where TDelegate : Delegate
{
    private bool _disposed;

    public Transaction(TId transactionId)
    {
        TransactionId = transactionId.ThrowIfNull();

        Changes = new List<TChange>();
        Committed = new Event<TDelegate>();
    }

    ~Transaction()
    {
        Dispose(false);
    }

    public void Add(TChange change) => Changes.Add(change);

    protected IList<TChange> Changes { get; }

    public virtual void Commit()
    {
    }

    public Event<TDelegate> Committed { get; }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Committed.Dispose();
                Changes.Clear();

            }
            _disposed = true;
        }
    }

    public bool HasChanges() => 0 < Changes.Count;

    public TId TransactionId { get; }
}