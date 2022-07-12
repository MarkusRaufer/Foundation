namespace Foundation.DesignPatterns.PublishSubscribe;

public interface ISubscriptionContainer<TSubject, TDelegate>
    where TDelegate : Delegate
{
    /// <summary>
    /// subscribes a delegate to a message type.
    /// </summary>
    /// <param name="subject">subject of subscription.</param>
    /// <param name="delegate">delegate, which should be executed.</param>
    /// <returns></returns>
    IDisposable Subscribe(TSubject subject, TDelegate @delegate);

    /// <summary>
    /// removes the subscription of the message.
    /// </summary>
    /// <param name="subject">subject of subscription.</param>
    /// <param name="delegate">delegate, which should be deregistered.</param>
    void Unsubscribe(TSubject subject, TDelegate @delegate);
    void Unsubscribe(TSubject subject);
    void UnsubscribeAll();
}

public interface ISubscriptionContainer<TMessage>
{
    IDisposable Subscribe(Action<TMessage> action);
    IDisposable Unsubscribe(Action<TMessage> action);
}
