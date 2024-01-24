using System.Collections;

namespace Foundation.ComponentModel;

public static class CollectionTransactionProvider
{
    /// <summary>
    /// Creates a new <see cref="ListTransactionProvider{T}"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static CollectionTransactionProvider<T> New<T>(IList<T> list) => new(list);
}

/// <summary>
/// This decorates a collection as <see cref="ITransactionProvider{TTransaction}"/>
/// </summary>
/// <typeparam name="T">The type of the collection elements.</typeparam>
public class CollectionTransactionProvider<T> 
    : ICollection<T>
    , ITransactionProvider<ITransaction<Guid>>
{
    private IDisposable? _commited;
    private readonly List<ActionEvent> _events = new();
    private readonly ICollection<T> _collection;

    #region events
    private record ActionEvent(CollectionAction Action);
    private record ValueEvent(CollectionAction Action, T Value) : ActionEvent(Action);
    #endregion events

    public CollectionTransactionProvider(ICollection<T> collection)
    {
        _collection = collection.ThrowIfNull();
    }

    public ITransaction<Guid> BeginTransaction()
    {
        _events.Clear();

        var transaction = new Transaction();
        _commited = transaction.Committed.Subscribe(OnTransactionCommitted);

        return transaction;
    }

    public int Count => _collection.Count;

    public bool IsReadOnly => _collection.IsReadOnly;

    public void Add(T item) => _events.Add(new ValueEvent(CollectionAction.Add, item));

    public void Clear() => _events.Add(new ActionEvent(CollectionAction.Clear));

    public bool Contains(T item) => _collection.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

    private void OnTransactionCommitted()
    {
        foreach(var @event in _events)
        {
            switch(@event)
            {
                case ValueEvent e when e.Action == CollectionAction.Add: _collection.Add(e.Value); break;
                case ActionEvent e when e.Action == CollectionAction.Clear: _collection.Clear(); break;
                case ValueEvent e when e.Action == CollectionAction.Remove: _collection.Remove(e.Value); break;
            }
        }

        _commited?.Dispose();
    }

    public bool Remove(T item)
    {
        if (_collection.Contains(item))
        {
            _events.Add(new ValueEvent(CollectionAction.Remove, item));
            return true;
        }

        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();
}
