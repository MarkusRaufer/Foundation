using System.Collections;

namespace Foundation.Collections.Generic;

/// <summary>
/// This HashSet counts all items added. If same item is added again the counter is incremented.
/// If an existing item is removed the counter is decremented. If the counter is 0 after removing an item, the item if removed from the HashSet.
/// </summary>
/// <typeparam name="T"></typeparam>
public class CountedHashSet<T> : ICollection<T>
    where T : notnull
{
    private readonly HashSet<Countable<T>> _countables = new ();

    /// <inheritdoc/>
    public int Count => _countables.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => false;

    /// <summary>
    /// if item exists the counter of this item is incremented.
    /// </summary>
    /// <param name="item"></param>
    public void Add(T item)
    {
        var newCountable = Countable.New(item);
        if (_countables.TryGetValue(newCountable, out Countable<T>? countable))
        {
            countable.Inc();
            return;
        }

        _countables.Add(newCountable);
    }

    /// <inheritdoc/>
    public void Clear() => _countables.Clear();

    /// <inheritdoc/>
    public bool Contains(T item) => _countables.Contains(Countable.New(item));

    /// <inheritdoc/>
    public void CopyTo(T[] array, int arrayIndex)
    {
        var it = GetEnumerator();
        for(int i = 0; i < array.Length; i++)
        {
            if (!it.MoveNext()) break;
            array[i] = it.Current;
        }
    }

    /// <summary>
    /// Returns all items with their counters as tuples.
    /// </summary>
    /// <returns></returns>
    public IEnumerable<(T? item, int count)> GetCountedElements() => _countables.Select(x => (x.Value, x.Count));

    /// <summary>
    /// Returns the number of additions of this item.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>The number of additions of the items.</returns>
    public int GetCount(T item) => _countables.TryGetValue(Countable.New(item), out Countable<T>? countable) ? countable.Count : 0;

#pragma warning disable CS8619
    /// <inheritdoc/>
    public IEnumerator<T> GetEnumerator() => _countables.Select(x => x.Value).GetEnumerator();
#pragma warning restore CS8619

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => _countables.Select(x => x.Value).GetEnumerator();

    /// <summary>
    /// If item exists the counter of this item is decremented.
    /// If the counter is 0 after removing an item, the item if removed from the HashSet.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool Remove(T item)
    {
        var countable = Countable.New(item);
        if (_countables.TryGetValue(countable, out Countable<T>? found))
        {
            found.Dec();
            if (0 == found.Count) _countables.Remove(countable);
            return true;
        }
        return false;
    }
}
