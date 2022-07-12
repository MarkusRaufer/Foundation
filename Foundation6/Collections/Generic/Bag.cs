using System.Collections;

namespace Foundation.Collections.Generic
{
    /// <summary>
    /// Same as Collection<typeparamref name="T"/> but faster on adding items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Bag<T> : ICollection<T>
    {
        private readonly ICollection<T> _collection;

        public Bag() : this(new List<T>())
        {
        }

        public Bag(ICollection<T> collection)
        {
            _collection = collection.ThrowIfNull();
        }

        public Bag(IEnumerable<T> collection) : this(new List<T>(collection))
        {
        }

        public Bag(int capacity) : this(new List<T>(capacity))
        {
        }

        public int Count => _collection.Count;

        public bool IsReadOnly => _collection.IsReadOnly;

        public void Add(T item) => _collection.Add(item);

        public void Clear() => _collection.Clear();

        public bool Contains(T item) => _collection.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

        public bool Remove(T item) => _collection.Remove(item);

        IEnumerator IEnumerable.GetEnumerator() => _collection.GetEnumerator();
    }
}
