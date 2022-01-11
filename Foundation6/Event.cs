namespace Foundation;

/// <summary>
/// This event automatically removes all subscribers on Dispose().
/// </summary>
/// <typeparam name="TDelegate"></typeparam>
public class Event<TDelegate> : IDisposable
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

    public void Publish(params object?[] args)
    {
        foreach (var subscribtion in _subscribtions.Value)
        {
            subscribtion.DynamicInvoke(args);
        }
    }

    public IDisposable Subscribe(TDelegate @delegate)
    {
        var disposable = new Disposable(() => _subscribtions.Value.Remove(@delegate));

        if (Contains(@delegate)) return disposable;

        _subscribtions.Value.Add(@delegate);
        return disposable;
    }

    public int SubscribtionCount => _subscribtions.Value.Count;

    public bool Unsubscribe(TDelegate @delegate)
    {
        return _subscribtions.Value.Remove(@delegate);
    }

    public void UnsubscribeAll() => _subscribtions.Value.Clear();
}
