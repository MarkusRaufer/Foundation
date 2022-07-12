namespace Foundation.DesignPatterns.PublishSubscribe;

public interface IPublisher
{
    void Publish<TMessage>(TMessage message);
}

public interface IPublisher<in TMessage>
{
    void Publish(TMessage message);
}
