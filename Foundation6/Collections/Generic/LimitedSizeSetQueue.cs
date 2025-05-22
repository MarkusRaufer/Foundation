using System.Collections;

namespace Foundation.Collections.Generic;

/// <summary>
/// This queue has a limited maximum size. The queue allows only unique elements like a set.
/// If maximum reached the same element will be removed before a the new element is added at the end.
/// If the element does not exist the first element is removed before the new element is added at the end.
/// </summary>
/// <typeparam name="T"></typeparam>
public class LimitedSizeSetQueue<T> : IReadOnlyCollection<T>
{
    private readonly LinkedList<T> _elements = [];
    private readonly int _size;
    
    public LimitedSizeSetQueue(int size)
    {
        _size = size;
    }

    public int Count => _elements.Count;

    public T Dequeue()
    {
        if (_elements.First == null) throw new InvalidOperationException("queue is empty");
        return _elements.First.Value;
    }

    /// <summary>
    /// Adds the element at the end of the queue. If <paramref name="element"/> exists it will be removed before it will be added.
    /// </summary>
    /// <param name="element"></param>
    public void Enqueue(T element)
    {
        if (_elements.Count == _size)
        {
            var existing = _elements.Find(element);
            
            if (null == existing) _elements.RemoveFirst();
            else _elements.Remove(existing);
        }
        _elements.AddLast(element);
    }


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
