namespace Foundation.ComponentModel;

public interface ICreateEvent<TEventId> : IEvent<TEventId>
{
    DateTime CreatedOn { get; }
}
