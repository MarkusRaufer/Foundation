namespace Foundation.DesignPatterns.PublishSubscribe;

public interface ISubscriptionContainer<TSubject, TDelegate>
    : ISingleSubscriptionContainer<TSubject, TDelegate>
    where TSubject : notnull
    where TDelegate : Delegate
{
    /// <summary>
    /// removes the subscription of the message.
    /// </summary>
    /// <param name="subject">subject of subscription.</param>
    /// <param name="delegate">delegate, which should be deregistered.</param>
    void Unsubscribe(TSubject subject, TDelegate @delegate);
}

public interface ISubscriptionContainer<TMessage>
{
    IDisposable Subscribe(Action<TMessage> action);
    IDisposable Unsubscribe(Action<TMessage> action);
}
