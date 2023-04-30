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
                CollectionActionState.Added => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, @event.Element),
                CollectionActionState.Cleared => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset),
                CollectionActionState.Removed => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, @event.Element),
                CollectionActionState.Replaced => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, @event.Element),
                _ => throw new NotImplementedException()
            };
        }
    }
}
