using Foundation.Collections.Generic;
using System.Collections;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace Foundation.Collections.Immutable;

/// <summary>
/// Represents a collection of values that are sorted by the values
/// and are accessible by value and by index.
/// </summary>
/// <typeparam name="T">T should implement IComparable<typeparamref name="T"/> or use your own IComparer<typeparamref name="T"/></typeparam>
public class ImmutableSortedList<T> : IImmutableList<T>
{
    private readonly SortedList<T> _list;

    public ImmutableSortedList() : this(ImmutableList<T>.Empty)
    {
    }

    public ImmutableSortedList(IComparer<T> comparer)
    {
        _list = new SortedList<T>(comparer);
    }

    /// <summary>
    /// Constructor expects a collection of items.
    /// </summary>
    /// <param name="collection">The collection can be unsorted.</param>
    public ImmutableSortedList(IEnumerable<T> collection, bool isSorted = false)
    {
        _list = new SortedList<T>(collection, isSorted);
    }

    public ImmutableSortedList(IComparer<T> comparer, IEnumerable<T> collection)
    {
        _list = new SortedList<T>(comparer, collection);
    }

    private ImmutableSortedList(SortedList<T> list)
    {
        _list = list;
    }

    /// <inheritdoc/>
    public T this[int index] => _list[index];

    /// <summary>
    /// Adds an element into the <see cref="SortedList{T}"./>
    /// </summary>
    /// <param name="item"></param>
    public IImmutableList<T> Add(T item)
    {
        var list = new SortedList<T>(_list, true)
        {
            item
        };

        return new ImmutableSortedList<T>(list, true);
    }


    public IImmutableList<T> AddRange(IEnumerable<T> items)
    {
        var list = new SortedList<T>(_list, true);
        list.AddRange(items);

        return new ImmutableSortedList<T>(list, true);
    }

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.BinarySearch(T)"/>
    /// </summary>
    public int BinarySearch(T item) => _list.BinarySearch(item);

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.BinarySearch(T, IComparer{T}?)"/>
    /// </summary>
    public int BinarySearch(T item, IComparer<T>? comparer) => _list.BinarySearch(item, comparer);

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.BinarySearch(int, int, T, IComparer{T}?)"/>
    /// </summary>
    public int BinarySearch(int index, int count, T item, IComparer<T>? comparer)
        => _list.BinarySearch(index, count, item, comparer);

    /// <inheritdoc/>
    public IImmutableList<T> Clear() => new ImmutableSortedList<T>();

    /// <inheritdoc/>
    public bool Contains(T item) => _list.Contains(item);

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
        => _list.CopyTo(array, arrayIndex);

    /// <inheritdoc/>
    public int Count => _list.Count;

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.Find(Predicate{T})"/>
    /// </summary>
    public T? Find(Predicate<T> match) => _list.Find(match);

    /// <summary>
    /// Retrieves all the elements that match the conditions defined by the specified lambda.
    /// </summary>
    /// <param name="list">The list to execute the lambda.</param>
    /// <param name="lambda">Filter for FindAll method.</param>
    /// <returns></returns>
    public IImmutableList<T> FindAll(LambdaExpression lambda)
        => new ImmutableSortedList<T>(_list.FindAll(lambda), true);

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.FindAll(Predicate{T})"/>
    /// </summary>
    public IImmutableList<T> FindAll(Predicate<T> match) => new ImmutableSortedList<T>(_list.FindAll(match), true);

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.FindIndex(Predicate{T})"/>
    /// </summary>
    public int FindIndex(Predicate<T> match) => _list.FindIndex(match);

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.FindIndex(int, Predicate{T})"/>
    /// </summary>
    public int FindIndex(int startIndex, Predicate<T> match)
        => _list.FindIndex(startIndex, match);

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.FindIndex(int, int, Predicate{T})"/>
    /// </summary>
    public int FindIndex(int startIndex, int count, Predicate<T> match)
        => _list.FindIndex(startIndex, count, match);

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.FindLast(Predicate{T})"/>
    /// </summary>
    public T? FindLast(Predicate<T> match) => _list.FindLast(match);

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.FindLastIndex(Predicate{T})"/>
    /// </summary>
    public int FindLastIndex(Predicate<T> match) => _list.FindLastIndex(match);

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.FindLastIndex(int, Predicate{T})"/>
    /// </summary>
    public int FindLastIndex(int startIndex, Predicate<T> match)
        => _list.FindLastIndex(startIndex, match);

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.FindLastIndex(int, int, Predicate{T})"/>
    /// </summary>
    public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        => _list.FindLastIndex(startIndex, count, match);

    /// <summary>
    /// Returs the first element if list is not empty.
    /// </summary>
    public T? First => 0 == _list.Count ? default : _list[0];

    /// <summary>
    /// Returns the index of the first element. If <see cref="SortedList{T}"/> is empty -1 is returned.
    /// </summary>
    /// <returns>Index greater or equal 0 or -1 if emtpy.</returns>
    public int FirstIndex() => 0 == _list.Count ? -1 : 0;

    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.GetRange(int, int)"/>
    /// </summary>
    public IImmutableList<T> GetRange(int index, int count)
    {
        var list = _list.GetRange(index, count);
        return new ImmutableSortedList<T>(list, true);
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
    public IImmutableList<T> GetViewBetween(T? lowerValue, T? upperValue)
    {
        var list = _list.GetViewBetween(lowerValue, upperValue);

        return new ImmutableSortedList<T>(list, true);
    }

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.IndexOf(T)"/>
    /// </summary>
    public int IndexOf(T item) => _list.IndexOf(item);

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.IndexOf(T, int)"/>
    /// </summary>
    public int IndexOf(T item, int index) => _list.IndexOf(item, index);

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.IndexOf(T, int, int)"/>
    /// </summary>
    public int IndexOf(T item, int index, int count) => _list.IndexOf(item, index, count);


    public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
    {
        index.ThrowIfOutOfRange(() => 0 > index);
        if(equalityComparer is null) throw new ArgumentNullException(nameof(equalityComparer));

        var end = index + count;
        for (var i = index; i < end; i++)
        {
            if (equalityComparer.Equals(item, _list[i])) return i;
        }
        return -1;
    }


    IImmutableList<T> IImmutableList<T>.Insert(int index, T element)
    {
        var list = new SortedList<T>(_list, true)
        {
            element
        };
        return new ImmutableSortedList<T>(list);
    }

    IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items)
    {
        var list = new SortedList<T>(_list, true);
        list.AddRange(items);

        return new ImmutableSortedList<T>(list);
    }

    /// <inheritdoc/>
    public bool IsReadOnly => true;

    /// <summary>
    /// Returns the last element of the list if not empty.
    /// </summary>
    public T? Last => 0 == _list.Count ? default : _list[LastIndex()];

    /// <summary>
    /// Returns the index of the last element. If <see cref="SortedList{T}"/> is empty -1 is returned.
    /// </summary>
    /// <returns>Index greater or equal 0 or -1 if emtpy.</returns>
    public int LastIndex() => _list.Count - 1;

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.LastIndexOf(T)"/>
    /// </summary>
    public int LastIndexOf(T item) => _list.LastIndexOf(item);

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.LastIndexOf(T, int)"/>
    /// </summary>
    public int LastIndexOf(T item, int index) => _list.LastIndexOf(item, index);

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.LastIndexOf(T, int, int)"/>
    /// </summary>
    public int LastIndexOf(T item, int index, int count) => _list.LastIndexOf(item, index, count);

    public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
    {
        if(equalityComparer is null) throw new ArgumentNullException(nameof(equalityComparer));

        var end = index + count;

        var lastIndex = -1;
        for(int i = 0; i < end; i++)
        {
            if (equalityComparer.Equals(item, _list[i])) lastIndex = i;
        }
        return lastIndex;
    }

    public ImmutableSortedList<T> Remove(T item)
    {
        return new ImmutableSortedList<T>(new SortedList<T>(_list.Ignore(item)));
    }

    public IImmutableList<T> Remove(T value, IEqualityComparer<T>? equalityComparer)
    {
        if (equalityComparer is null) throw new ArgumentNullException(nameof(equalityComparer));

        return new ImmutableSortedList<T>(_list.Ignore(x => equalityComparer.Equals(value, x)), true);
    }

    public IImmutableList<T> RemoveAll(Predicate<T> match)
    {
        if(match is null) throw new ArgumentNullException(nameof(match));

        return new ImmutableSortedList<T>(_list.Ignore(x => match(x)), true);
    }

    IImmutableList<T> IImmutableList<T>.RemoveAt(int index)
    {
        return new ImmutableSortedList<T>(_list.Ignore(new[] { index }), true);
    }

    public IImmutableList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
    {
        if (equalityComparer is null) throw new ArgumentNullException(nameof(equalityComparer));

        var list = new SortedList<T>();
        foreach (var element in _list)
        {
            if (items.Any(x => equalityComparer.Equals(x, element))) continue;

            list.Add(element);
        }

        return new ImmutableSortedList<T>(list, true);
    }

    public IImmutableList<T> RemoveRange(int index, int count)
    {
        var indices = Enumerable.Range(index, count).ToArray();

        return new ImmutableSortedList<T>(_list.Ignore(indices));
    }

    public IImmutableList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.Reverse()"/>
    /// </summary>
    public IImmutableList<T> Reverse()
    {
        var list = new SortedList<T>(_list, true);
        list.Reverse();
        return new ImmutableSortedList<T>(list, true);
    }


    /// <summary>
    /// <see cref="ImmutableSortedList{T}.Reverse(int, int)"/>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public IImmutableList<T> Reverse(int index, int count) => throw new NotImplementedException();

    public IImmutableList<T> SetItem(int index, T value)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.TrueForAll(Predicate{T})"/>
    /// </summary>
    /// <param name="match"></param>
    /// <returns></returns>
    public bool TrueForAll(Predicate<T> match) => _list.TrueForAll(match);
}
