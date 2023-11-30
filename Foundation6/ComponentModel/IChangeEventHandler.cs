namespace Foundation.ComponentModel;

public interface IChangeEventHandler<TEvent>
{
    void ApplyEvent(TEvent ev);
}
