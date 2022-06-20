using Foundation.ComponentModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;

namespace Foundation.Collections.Specialized
{
    public class NotifyCollectionChangedDecorator<T> : INotifyCollectionChanged
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private readonly ICollectionChanged<T> _collectionChanged;

        public NotifyCollectionChangedDecorator(ICollectionChanged<T> collectionChanged)
        {
            _collectionChanged = collectionChanged.ThrowIfNull();
            _collectionChanged.CollectionChanged.Subscribe(OnCollectionChanged);
        }

        private void OnCollectionChanged(CollectionEvent<T> @event)
        {
            CollectionChanged?.Invoke(_collectionChanged, @event.ToNotifyCollectionChangedEventArgs());
        }
    }
}
