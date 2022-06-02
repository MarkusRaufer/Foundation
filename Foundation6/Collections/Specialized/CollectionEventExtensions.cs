using Foundation.ComponentModel;
using System.Collections.Specialized;

namespace Foundation.Collections.Specialized
{
    public static class CollectionEventExtensions
    {
        public static NotifyCollectionChangedEventArgs ToNotifyCollectionChangedEventArgs<T>(this CollectionEvent<T> @event)
        {
            return @event.State switch
            {
                CollectionChangedState.CollectionCleared => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset),
                CollectionChangedState.ElementAdded => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, @event.Element),
                CollectionChangedState.ElementRemoved => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, @event.Element),
                CollectionChangedState.ElementReplaced => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, @event.Element),
                _ => throw new NotImplementedException()
            };
        }
    }
}
