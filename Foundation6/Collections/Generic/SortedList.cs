using System.Collections;

namespace Foundation.Collections.Generic;

public class SortedList<T>
    : ICollection<T>
    , IReadOnlyList<T>
{
    private List<T> _list;

    public SortedList()
    {
        _list = new List<T>();
    }

    public SortedList(int capacity)
    {
        _list = new List<T>(capacity);
    }

    public SortedList(IEnumerable<T> collection)
    {
        _list = new List<T>();
        foreach (T item in collection.OrderBy(x => x))
        {
            _list.Add(item);
        }
    }

    public T this[int index] => _list[index];

    public void Add(T item)
    {
        var index = _list.BinarySearch(item);
        if (0 > index) index = ~index;

        _list.Insert(index, item);
    }

    public int BinarySearch(T item) => _list.BinarySearch(item);

    public void Clear() => _list.Clear();

    public bool Contains(T item) => _list.BinarySearch(item) > -1;

    public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

    public int Count => _list.Count;

    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

    public SortedList<T> GetRange(int index, int count)
    {
        return new SortedList<T>
        {
            _list = _list.GetRange(index, count)
        };
    }

    public SortedList<T> GetViewBetween(T? lowerValue, T? upperValue)
    {
        var lowerIndex = 0;
        var upperIndex = _list.Count - 1;

        if (lowerValue is T lower)
        {
            var index = _list.BinarySearch(lower);
            if (0 > index) index = ~index;

            lowerIndex = index;
        }

        if (upperValue is T upper)
        {
            var index = _list.BinarySearch(upper);
            if (0 > index) index = ~index;

            if (index < upperIndex) upperIndex = index;
        }

        return GetRange(lowerIndex, (upperIndex - lowerIndex) + 1);
    }

    public int IndexOf(T item) => _list.IndexOf(item);

    public bool IsReadOnly => false;

    public bool Remove(T item) => _list.Remove(item);

    public void RemoveAt(int index) => _list.RemoveAt(index);
}
