namespace Foundation.ComponentModel;

public interface IPropertyChangedEvent<TEventId, TPropertyChanged> : IEvent<TEventId>
    where TPropertyChanged : IPropertyChanged
{
    TPropertyChanged PropertyChanged { get; }
}
