namespace Foundation.ComponentModel;

using System.Collections.Specialized;
using System.ComponentModel;

public interface INotifyObjectChanged
{
    event ObjectChangedEventHandler ObjectChanged;
}

public enum ObjectChangedAction
{
    CollectionChanged,
    ObjectChanged,
    PropertyChanged
}

public delegate void ObjectChangedEventHandler(object? sender, ObjectChangedEventArgs e);

public class ObjectChangedEventArgs : EventArgs
{
    public ObjectChangedEventArgs(object? changedCollection, NotifyCollectionChangedEventArgs? args)
    {
        ChangedCollection = changedCollection.ThrowIfNull();
        NotifyCollectionChangedEventArgs = args.ThrowIfNull();

        Action = ObjectChangedAction.CollectionChanged;
    }

    public ObjectChangedEventArgs(object? changedObject, object? changedCollection, NotifyCollectionChangedEventArgs? args)
    {
        ChangedObject = changedObject.ThrowIfNull();
        ChangedCollection = changedCollection.ThrowIfNull();
        NotifyCollectionChangedEventArgs = args.ThrowIfNull();

        Action = ObjectChangedAction.CollectionChanged;
    }

    public ObjectChangedEventArgs(PropertyChangedEventArgs? args)
    {
        PropertyChangedEventArgs = args.ThrowIfNull();

        Action = ObjectChangedAction.PropertyChanged;
    }

    public ObjectChangedAction Action { get; set; }
    public object? ChangedCollection { get; set; }
    public object? ChangedObject { get; set; }
    public NotifyCollectionChangedEventArgs? NotifyCollectionChangedEventArgs { get; set; }
    public PropertyChangedEventArgs? PropertyChangedEventArgs { get; set; }
}

