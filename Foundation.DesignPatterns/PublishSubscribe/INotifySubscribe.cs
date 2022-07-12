namespace Foundation.DesignPatterns.PublishSubscribe;

public interface INotifySubscribe<TDelegate>
    where TDelegate : Delegate
{
    public Event<TDelegate> OnSubscribe { get; set; }
}
