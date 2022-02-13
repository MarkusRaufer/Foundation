namespace Foundation.Test.Collections.Generic;

using Foundation;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

public class TestEnumerator<T> : IEnumerator<T>
{
    private bool _disposing;
    private readonly IEnumerator<T> _enumerator;

    public TestEnumerator(IEnumerator<T> enumerator)
    {
        _enumerator = enumerator.ThrowIfNull();
    }

    ~TestEnumerator()
    {
        Dispose(false);
    }

    public T Current
    {
        get 
        { 
            var item = _enumerator.Current;
            OnCurrent.Publish(item);
            return item;
        }
    }

    [MaybeNull]
    object IEnumerator.Current
    {
        get
        {
            var item = _enumerator.Current;

            if (item is T t) OnCurrent.Publish(t);
            else OnCurrent.Publish(item);
            
            return item;
        }
    }

    public void Dispose() => Dispose(true);

    private void Dispose(bool disposing)
    {
        if (_disposing) return;
        _disposing = disposing;

        OnCurrent.Dispose();
        OnMoveNext.Dispose();
        OnReset.Dispose();

        _enumerator.Dispose();
        GC.SuppressFinalize(this);
    }

    public bool MoveNext()
    {
        var hasNext = _enumerator.MoveNext();
        OnMoveNext.Publish(hasNext);
        return hasNext;
    }

    public Event<Action<T>> OnCurrent { get; } = new Event<Action<T>>();

    public Event<Action<bool>> OnMoveNext { get; } = new Event<Action<bool>>();

    public Event<Action> OnReset { get; } = new Event<Action>();

    public void Reset()
    {
        _enumerator.Reset();
        OnReset.Publish();
    }
}

