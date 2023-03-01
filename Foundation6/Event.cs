namespace Foundation;

/// <summary>
/// This event automatically removes all subscribers on Dispose().
/// </summary>
/// <typeparam name="TDelegate"></typeparam>
public sealed class Event<TDelegate> : IDisposable
    where TDelegate : Delegate
{
    private readonly Lazy<ICollection<TDelegate>> _subscribtions
        = new(() => new List<TDelegate>());

    private bool _disposing;

    ~Event()
    {
        Dispose();
    }

    public bool Contains(TDelegate @delegate) => _subscribtions.Value.Contains(@delegate);

    public void Dispose()
    {
        if (!_disposing)
        {
            _disposing = true;
            UnsubscribeAll();
        }
        GC.SuppressFinalize(this);
    }

    public Action<TDelegate>? OnSubscribe { get; set; }
    public Action<TDelegate>? OnUnsubscribe { get; set; }

    public void Publish(params object?[] args)
    {
        foreach (var subscribtion in _subscribtions.Value.ToArray())
        {
            subscribtion.DynamicInvoke(args);
        }
    }

    public IDisposable Subscribe(TDelegate @delegate)
    {
        var disposable = new Disposable(() => Unsubscribe(@delegate));

        if (Contains(@delegate)) return disposable;

        _subscribtions.Value.Add(@delegate);

        OnSubscribe?.Invoke(@delegate);

        return disposable;
    }

    public int SubscribtionCount => _subscribtions.Value.Count;

    public bool Unsubscribe(TDelegate @delegate)
    {
        var removed = _subscribtions.Value.Remove(@delegate);

        OnUnsubscribe?.Invoke(@delegate);

        return removed;
    }

    public void UnsubscribeAll() => _subscribtions.Value.Clear();
}
