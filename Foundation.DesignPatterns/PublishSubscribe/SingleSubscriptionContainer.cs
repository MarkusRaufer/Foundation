using Foundation.Collections.Generic;

namespace Foundation.DesignPatterns.PublishSubscribe;

public class SingleSubscriptionContainer<TSubject, TDelegate> 
    : ISingleSubscriptionContainer<TSubject, TDelegate>
    , IAllSubscriptions
    , ISubscriptionProvider<TSubject, TDelegate>
    , INotifySubscribe<Action<(TSubject, TDelegate)>>
    where TSubject : notnull
    where TDelegate : Delegate
{
    private readonly MultiValueMap<TSubject, TDelegate> _subscriptions;

    public SingleSubscriptionContainer()
    {
        _subscriptions = new MultiValueMap<TSubject, TDelegate>();
        OnSubscribe = new Event<Action<(TSubject, TDelegate)>>();
    }

    public Event<Action<(TSubject, TDelegate)>> OnSubscribe { get; set; }

    public IEnumerable<TDelegate> GetSubscriptions(TSubject subject) => _subscriptions.GetValues(subject)
                                                                                      .ToArray();

    public IDisposable Subscribe(TSubject subject, TDelegate @delegate)
    {
        if (_subscriptions.Contains(subject, @delegate))
            return new Disposable(() => _subscriptions.Remove(subject, @delegate));

        _subscriptions.Add(subject, @delegate);

        OnSubscribe?.Publish((subject, @delegate));

        return new Disposable(() => _subscriptions.Remove(subject, @delegate));
    }

    public void Unsubscribe(TSubject subject)
    {
        _subscriptions.Remove(subject);
    }

    public void UnsubscribeAll()
    {
        _subscriptions.Clear();
    }
}
