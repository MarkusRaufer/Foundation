using Foundation.ComponentModel;
using System.Collections;
using System.Collections.Specialized;

namespace Foundation.Collections.ObjectModel
{
    public class ObservableCollectionDecorator<T>
        : ICollection<T>
        , IMutable
        , INotifyCollectionChanged
    {
        #region events
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        #endregion events

        private readonly ICollection<T> _collection;

        public ObservableCollectionDecorator(ICollection<T> collection, bool collectionChangedEventEnabled = true)
        {
            _collection = collection.ThrowIfNull();
            CollectionChangedEventEnabled = collectionChangedEventEnabled;
        }

        public void Add(T item)
        {
            _collection.Add(item);

            IsDirty = true;
            if (CollectionChangedEventEnabled && CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        public void Clear()
        {
            if (0 == _collection.Count) return;

            _collection.Clear();
            IsDirty = true;
            if (CollectionChangedEventEnabled && null != CollectionChanged)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public bool CollectionChangedEventEnabled { get; set; }

        public bool Contains(T item) => _collection.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

        public int Count => _collection.Count;

        public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool IsDirty { get; set; }

        public bool IsReadOnly => _collection.IsReadOnly;

        public bool Remove(T item)
        {
            if (!_collection.Remove(item))
                return false;

            IsDirty = true;
            if (CollectionChangedEventEnabled && null != CollectionChanged)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));

            return true;
        }
    }
}
