namespace Foundation.ComponentModel;

public interface INotifyChanged<T> : INotifyChanged<T, Action<T>>
{
}

public interface INotifyChanged<T, TDelegate>
    where TDelegate : Delegate
{
    Event<TDelegate> OnChanged { get; }
}
