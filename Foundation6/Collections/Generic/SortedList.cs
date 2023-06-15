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

    /// <summary>
    /// Constructor expects a collection of items.
    /// </summary>
    /// <param name="collection">The collection can be unsorted.</param>
    public SortedList(IEnumerable<T> collection)
    {
        _list = new List<T>();
        foreach (T item in collection.OrderBy(x => x))
        {
            _list.Add(item);
        }
    }

    /// <inheritdoc/>
    public T this[int index] => _list[index];

    /// <summary>
    /// Adds an element into the <see cref="SortedList{T}"./>
    /// </summary>
    /// <param name="item"></param>
    public void Add(T item)
    {
        var index = _list.BinarySearch(item);
        if (0 > index) index = ~index;

        _list.Insert(index, item);
    }

    /// <summary>
    /// <see cref="List{T}.BinarySearch(T)"/>
    /// </summary>
    public int BinarySearch(T item) => _list.BinarySearch(item);

    /// <summary>
    /// <see cref="List{T}.BinarySearch(T, IComparer{T}?)"/>
    /// </summary>
    public int BinarySearch(T item, IComparer<T>? comparer) => _list.BinarySearch(item, comparer);

    /// <summary>
    /// <see cref="List{T}.BinarySearch(int, int, T, IComparer{T}?)"/>
    /// </summary>
    public int BinarySearch(int index, int count, T item, IComparer<T>? comparer)
        => _list.BinarySearch(index, count, item, comparer);

    /// <inheritdoc/>
    public void Clear() => _list.Clear();

    /// <inheritdoc/>
    public bool Contains(T item) => _list.BinarySearch(item) > -1;

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public int Count => _list.Count;

    /// <summary>
    /// <see cref="List{T}.Find(Predicate{T})"/>
    /// </summary>
    public T? Find(Predicate<T> match) => _list.Find(match);

    /// <summary>
    /// <see cref="List{T}.FindAll(Predicate{T})"/>
    /// </summary>
    public List<T> FindAll(Predicate<T> match) => _list.FindAll(match);

    /// <summary>
    /// <see cref="List{T}.FindIndex(Predicate{T})"/>
    /// </summary>
    public int FindIndex(Predicate<T> match) => _list.FindIndex(match);

    /// <summary>
    /// <see cref="List{T}.FindIndex(int, Predicate{T})"/>
    /// </summary>
    public int FindIndex(int startIndex, Predicate<T> match) => _list.FindIndex(startIndex, match);

    /// <summary>
    /// <see cref="List{T}.FindIndex(int, int, Predicate{T})"/>
    /// </summary>
    public int FindIndex(int startIndex, int count, Predicate<T> match) => _list.FindIndex(startIndex, count, match);

    /// <summary>
    /// <see cref="List{T}.FindLast(Predicate{T})"/>
    /// </summary>
    public T? FindLast(Predicate<T> match) => _list.FindLast(match);

    /// <summary>
    /// <see cref="List{T}.FindLastIndex(Predicate{T})"/>
    /// </summary>
    public int FindLastIndex(Predicate<T> match) => _list.FindLastIndex(match);

    /// <summary>
    /// <see cref="List{T}.FindLastIndex(int, Predicate{T})"/>
    /// </summary>
     public int FindLastIndex(int startIndex, Predicate<T> match) => _list.FindLastIndex(startIndex, match);

    /// <summary>
    /// <see cref="List{T}.FindLastIndex(int, int, Predicate{T})"/>
    /// </summary>
    public int FindLastIndex(int startIndex, int count, Predicate<T> match) => _list.FindLastIndex(startIndex, count, match);

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

    /// <summary>
    /// <see cref="List{T}.GetRange(int, int)"/>
    /// </summary>
    public SortedList<T> GetRange(int index, int count)
    {
        return new SortedList<T>
        {
            _list = _list.GetRange(index, count)
        };
    }

    /// <summary>
    /// Returns a view of a subset in a <see cref="SortedList{T}"/>.
    /// </summary>
    /// <param name="lowerValue">The lowest desired value in the view.
    /// If lowerValue is smaller than the minimum value, the first element is taken.
    /// If lowerValue does not exist, the next higher value is taken.
    /// </param>
    /// <param name="upperValue">The highest desired value in the view.
    /// If upperValue is greater than the maximum value, the last element is taken.
    /// If upperValue does not exist, the next lower value is taken.
    /// </param>
    /// <returns>A subset view that contains only the values in the specified range.</returns>
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

            if (index <= upperIndex)
            {
                upperIndex = index;

                var valueAtIndex = _list[upperIndex];
                if (!upper.Equals(valueAtIndex)) --upperIndex;
            }
        }

        return GetRange(lowerIndex, (upperIndex - lowerIndex) + 1);
    }

    /// <summary>
    /// <see cref="List{T}.IndexOf(T)"/>
    /// </summary>
    public int IndexOf(T item) => _list.IndexOf(item);

    /// <summary>
    /// <see cref="List{T}.IndexOf(T, int)"/>
    /// </summary>
    public int IndexOf(T item, int index) => _list.IndexOf(item, index);

    /// <summary>
    /// <see cref="List{T}.IndexOf(T, int, int)"/>
    /// </summary>
    public int IndexOf(T item, int index, int count) => _list.IndexOf(item, index, count);

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <summary>
    /// <see cref="List{T}.LastIndexOf(T)"/>
    /// </summary>
    public int LastIndexOf(T item) => _list.LastIndexOf(item);

    /// <summary>
    /// <see cref="List{T}.LastIndexOf(T, int)"/>
    /// </summary>
    public int LastIndexOf(T item, int index) => _list.LastIndexOf(item, index);

    /// <summary>
    /// <see cref="List{T}.LastIndexOf(T, int, int)"/>
    /// </summary>
    public int LastIndexOf(T item, int index, int count) => _list.LastIndexOf(item, index, count);

    /// <inheritdoc/>
    public bool Remove(T item) => _list.Remove(item);

    /// <inheritdoc/>
    public void RemoveAt(int index) => _list.RemoveAt(index);

    /// <summary>
    /// <see cref="List{T}.Reverse()"/>
    /// </summary>
    public void Reverse() => _list.Reverse();

    /// <summary>
    /// <see cref="List{T}.Reverse(int, int)"/>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public void Reverse(int index, int count) => _list.Reverse(index, count);

    /// <summary>
    /// <see cref="List{T}.TrimExcess"/>
    /// </summary>
    public void TrimExcess() => _list.TrimExcess();

    /// <summary>
    /// <see cref="List{T}.TrueForAll(Predicate{T})"/>
    /// </summary>
    /// <param name="match"></param>
    /// <returns></returns>
    public bool TrueForAll(Predicate<T> match) => _list.TrueForAll(match);
}
