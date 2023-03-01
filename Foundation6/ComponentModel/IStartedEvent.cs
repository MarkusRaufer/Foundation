namespace Foundation.ComponentModel;

public interface IStartedEvent<TEventId> : IEvent<TEventId>
{
    DateTime StartedOn { get; }
}
