using Foundation.Collections.Generic;

namespace Foundation.DesignPatterns.PublishSubscribe;

public class SubscriptionContainer<TSubject, TDelegate> 
    : SingleSubscriptionContainer<TSubject, TDelegate>
    , ISubscriptionContainer<TSubject, TDelegate>
    where TSubject : notnull
    where TDelegate : Delegate
{
    private readonly MultiMap<TSubject, TDelegate> _subscriptions;

    public SubscriptionContainer()
    {
        _subscriptions = new MultiMap<TSubject, TDelegate>();
        OnSubscribe = new Event<Action<(TSubject, TDelegate)>>();
    }

    public void Unsubscribe(TSubject subject, TDelegate @delegate)
    {
        _subscriptions.Remove(subject, @delegate);
    }
}
