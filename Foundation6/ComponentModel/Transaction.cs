namespace Foundation.ComponentModel;

public class Transaction : Transaction<TransactionAction, ITransactionControl>
{
    public Transaction() : base()
    {
    }
}

public class Transaction<TAction, TChangeTuple>
    : Transaction<TAction, TChangeTuple, Action<IList<TChangeTuple>>>
    where TChangeTuple : ITransactionControl<TAction>
{
    public Transaction() : base()
    {
    }

    public override void Commit() => Committed?.Publish(Actions);
}

/// <summary>
/// With this class you can record changes and on <see cref="Commit"/> the event <see cref="Committed"/> is called.
/// </summary>
/// <typeparam name="TAction">This value is needed to describe what kind of action is executed.</typeparam>
/// <typeparam name="TChangeTuple">This tuple should contain the TAction and the value changes.</typeparam>
/// <typeparam name="TDelegate">This is the delegate which will be called on <see cref="Commit"/></typeparam>
public abstract class Transaction<TAction, TChangeTuple, TDelegate> : ITransaction
    where TChangeTuple : ITransactionControl<TAction>
    where TDelegate : Delegate
{
    private bool _disposed;

    public Transaction()
    {
        Actions = new List<TChangeTuple>();
        Committed = new Event<TDelegate>();
    }

    ~Transaction()
    {
        Dispose(false);
    }

    protected IList<TChangeTuple> Actions { get; }

    public void Add(TChangeTuple change) => Actions.Add(change);

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
                Actions.Clear();

            }
            _disposed = true;
        }
    }
}