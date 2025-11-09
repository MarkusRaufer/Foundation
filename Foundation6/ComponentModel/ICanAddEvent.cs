namespace Foundation.ComponentModel;

public interface ICanAddEvent<TEvent>
{
    void AddEvent(TEvent @event);
}
