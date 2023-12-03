using Foundation.ComponentModel;

namespace Foundation.DesignPatterns.MutableState;

public interface IMutableState<TState, TCommand, TEvent>
    : ICommandHandler<TCommand, TEvent>
    , IChangeEventHandler<TEvent>
{
    TState State { get; }
}
