using Foundation.ComponentModel;
using System.Collections.Specialized;

namespace Foundation.Collections.Specialized
{
    public static class CollectionEventExtensions
    {
        public static NotifyCollectionChangedEventArgs ToNotifyCollectionChangedEventArgs<T>(this CollectionEvent<T> @event)
        {
            return @event.Action switch
            {
                CollectionAction.Add => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, @event.Element),
                CollectionAction.Clear => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset),
                CollectionAction.Remove => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, @event.Element),
                //CollectionAction.Replaced => new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, @event.Element),
                _ => throw new NotImplementedException()
            };
        }
    }
}
