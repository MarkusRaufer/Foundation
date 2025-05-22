using System.Collections;

namespace Foundation.Collections.Generic;

/// <summary>
/// This queue has a limited maximum size. If maximum reached first element is removed before a new element is enqueued.
/// </summary>
/// <typeparam name="T"></typeparam>
public class LimitedSizeQueue<T> : IReadOnlyCollection<T>
{
    private readonly LinkedList<T> _elements = [];
    private readonly int _size;

    public LimitedSizeQueue(int size)
    {
        _size = size;    
    }

    public T Dequeue()
    {
        if (_elements.First == null) throw new InvalidOperationException("queue is empty");
        return _elements.First.Value;
    }

    public void Enqueue(T element)
    {
        if(_elements.Count == _size) _elements.RemoveFirst();
        _elements.AddLast(element);
    }

    public int Count => _elements.Count;

    public IEnumerator<T> GetEnumerator() => _elements.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _elements.GetEnumerator();

    public bool TryDequeue(out T? element)
    {
        if (_elements.First == null)
        {
            element = default;
            return false;
        }

        element = _elements.First.Value;
        _elements.RemoveFirst();
        return true;
    }
}
