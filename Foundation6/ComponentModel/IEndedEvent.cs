namespace Foundation.ComponentModel;

public interface IEndedEvent<TEventId> : IEvent<TEventId>
{
    DateTime EndedOn { get; }
}
