namespace Foundation.ComponentModel;

public interface IModifyEvent<TEventId> : IEvent<TEventId>
{
    DateTime ModifiedOn { get; }
}
