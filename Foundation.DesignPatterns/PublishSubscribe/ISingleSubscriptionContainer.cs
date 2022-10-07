namespace Foundation.DesignPatterns.PublishSubscribe;

public interface ISingleSubscriptionContainer<TSubject, TDelegate>
    where TSubject : notnull
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
    void Unsubscribe(TSubject subject);
}
