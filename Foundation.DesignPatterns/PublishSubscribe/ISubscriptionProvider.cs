namespace Foundation.DesignPatterns.PublishSubscribe;

public interface ISubscriptionProvider<TSubject, TDelegate>
    where TDelegate : Delegate
{
    IEnumerable<TDelegate> GetSubscriptions(TSubject subject);
}

public interface ISubscriptionProvider<TMessage>
{
    IEnumerable<Action<TMessage>> Subscriptions { get; }
}
