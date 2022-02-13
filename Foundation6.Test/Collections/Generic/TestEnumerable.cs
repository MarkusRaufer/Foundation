namespace Foundation.Test.Collections.Generic;

using System.Collections;

public class TestEnumerable<T> 
    : IEnumerable<T>
    , IDisposable
{
    private bool _disposing;
    private readonly IEnumerable<T> _items;

    public TestEnumerable(IEnumerable<T> items)
    {
        _items = items.ThrowIfNull();
    }

    ~TestEnumerable()
    {
        Dispose(false);
    }

    public void Dispose() => Dispose(true);

    private void Dispose(bool disposing)
    {
        if (_disposing) return;
        _disposing = disposing;

        OnCurrent.Dispose();
        OnMoveNext.Dispose();
        OnReset.Dispose();

        GC.SuppressFinalize(this);
    }

    public Event<Action<T>> OnCurrent { get; } = new Event<Action<T>>();

    private void _OnCurrent(T item)
    {
        OnCurrent.Publish(item);
    }

    public Event<Action<bool>> OnMoveNext { get; } = new Event<Action<bool>>();

    private void _OnMoveNext(bool hasNext)
    {
        OnMoveNext.Publish(hasNext);
    }

    public Event<Action> OnReset { get; } = new Event<Action>();

    private void _OnReset()
    {
        OnReset.Publish();
    }

    public IEnumerator<T> GetEnumerator()
    {
        var enumerator = new TestEnumerator<T>(_items.GetEnumerator());

        enumerator.OnCurrent.Subscribe(_OnCurrent);
        enumerator.OnMoveNext.Subscribe(_OnMoveNext);
        enumerator.OnReset.Subscribe(_OnReset);

        return enumerator;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        var enumerator = new TestEnumerator<T>(_items.GetEnumerator());

        enumerator.OnCurrent.Subscribe(_OnCurrent);
        enumerator.OnMoveNext.Subscribe(_OnMoveNext);
        enumerator.OnReset.Subscribe(_OnReset);

        return enumerator;
    }
}

