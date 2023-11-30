namespace Foundation.ComponentModel;

public interface IRecoveryEventHandler<TEvent>
{
    void RecoverFromEvent(TEvent ev);
}
