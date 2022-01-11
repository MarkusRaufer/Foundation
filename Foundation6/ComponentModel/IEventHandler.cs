namespace Foundation.ComponentModel;

public interface IEventHandler
{
    void HandleEvent<TEvent>(TEvent @event);
}

public interface IEventHandler<TEvent>
{
    void HandleEvent(TEvent @event);
}

