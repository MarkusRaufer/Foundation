using Foundation.Linq.Expressions;
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
    private readonly IComparer<T>? _comparer;
    private readonly ImmutableList<T> _list;

    public ImmutableSortedList() : this(ImmutableList<T>.Empty)
    {
    }

    public ImmutableSortedList(IComparer<T> comparer) : this()
    {
        _comparer = comparer.ThrowIfNull();
    }

    /// <summary>
    /// Constructor expects a collection of items.
    /// </summary>
    /// <param name="collection">The collection can be unsorted.</param>
    public ImmutableSortedList(IEnumerable<T> collection)
    {
        _list = ImmutableList.CreateRange(collection.OrderBy(x => x));
    }

    public ImmutableSortedList(IComparer<T> comparer, IEnumerable<T> collection) : this(collection)
    {
        _comparer = comparer.ThrowIfNull();
    }

    public ImmutableSortedList(ImmutableSortedList<T> list)
    {
        list.ThrowIfNull();

        _list = ImmutableList.CreateRange(list);
    }

    /// <inheritdoc/>
    public T this[int index] => _list[index];

    /// <summary>
    /// Adds an element into the <see cref="SortedList{T}"./>
    /// </summary>
    /// <param name="item"></param>
    public IImmutableList<T> Add(T item)
    {
        var index = BinarySearch(item);

        if (0 > index) index = ~index;

        return new ImmutableSortedList<T>(_list.Insert(index, item));
    }


    public IImmutableList<T> AddRange(IEnumerable<T> items) => new ImmutableSortedList<T>(_list.AddRange(items));

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.BinarySearch(T)"/>
    /// </summary>
    public int BinarySearch(T item) => null == _comparer
            ? _list.BinarySearch(item)
            : _list.BinarySearch(item, _comparer);

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
    public IImmutableList<T> Clear() => new ImmutableSortedList<T>(_list.Clear());

    /// <inheritdoc/>
    public bool Contains(T item) => null == _comparer
            ? _list.BinarySearch(item) > -1
            : _list.BinarySearch(item, _comparer) > -1;

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
    {
        if (!lambda.ThrowIfNull().IsPredicate())
            throw new ArgumentOutOfRangeException(nameof(lambda), $"is not a predicate");

        if (1 != lambda.Parameters.Count)
            throw new ArgumentOutOfRangeException(nameof(lambda), $"one parameter expected");

        if (lambda.Parameters.First().Type != typeof(T))
            throw new ArgumentOutOfRangeException(nameof(lambda), $"wrong parameter type");

        var func = (Func<T, bool>)lambda.Compile();
        return FindAll(new Predicate<T>(func));
    }

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.FindAll(Predicate{T})"/>
    /// </summary>
    public IImmutableList<T> FindAll(Predicate<T> match) => new ImmutableSortedList<T>(_list.FindAll(match));

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
        return new ImmutableSortedList<T>(_list.GetRange(index, count));
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
        return _list.IndexOf(item, index, count, equalityComparer);
    }


    IImmutableList<T> IImmutableList<T>.Insert(int index, T element)
    {
        var list = _list.Insert(index, element);
        return list.Sort();
    }

    IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items)
    {
        var list = _list.InsertRange(index, items.OrderBy(x => x));
        return list.Sort();
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
        return _list.LastIndexOf(item, index, count, equalityComparer);
    }

    public ImmutableSortedList<T> Remove(T item) => new (_list.Remove(item));

    public IImmutableList<T> Remove(T value, IEqualityComparer<T>? equalityComparer)
    {
        return new ImmutableSortedList<T>(_list.Remove(value, equalityComparer));
    }

    public IImmutableList<T> RemoveAll(Predicate<T> match)
    {
        return new ImmutableSortedList<T>(_list.RemoveAll(match));
    }

    IImmutableList<T> IImmutableList<T>.RemoveAt(int index)
    {
        return new ImmutableSortedList<T>(_list.RemoveAt(index));
    }

    public IImmutableList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
    {
        return new ImmutableSortedList<T>(_list.RemoveRange(items, equalityComparer));
    }

    public IImmutableList<T> RemoveRange(int index, int count)
    {
        return new ImmutableSortedList<T>(_list.RemoveRange(index, count));
    }

    public IImmutableList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer)
    {
        return new ImmutableSortedList<T>(_list.Replace(oldValue, newValue, equalityComparer).Sort());
    }

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.Reverse()"/>
    /// </summary>
    public IImmutableList<T> Reverse() => new ImmutableSortedList<T>(_list.Reverse());

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.Reverse(int, int)"/>
    /// </summary>
    /// <param name="index"></param>
    /// <param name="count"></param>
    public IImmutableList<T> Reverse(int index, int count) => new ImmutableSortedList<T>(_list.Reverse(index, count));

    public IImmutableList<T> SetItem(int index, T value)
    {
        index.ThrowIfOutOfRange(() => 0 > index);

        if (index == _list.Count - 1) return Add(value);

        var foundIndex = _list.BinarySearch(value);
        if (0 > foundIndex) foundIndex = ~foundIndex;

        var list = _list.SetItem(index, value);
        if (index == foundIndex) return new ImmutableSortedList<T>(list);

        return new ImmutableSortedList<T>(list.Sort());
    }

    /// <summary>
    /// <see cref="ImmutableSortedList{T}.TrueForAll(Predicate{T})"/>
    /// </summary>
    /// <param name="match"></param>
    /// <returns></returns>
    public bool TrueForAll(Predicate<T> match) => _list.TrueForAll(match);
}
