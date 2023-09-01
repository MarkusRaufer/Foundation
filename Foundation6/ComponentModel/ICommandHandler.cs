namespace Foundation.ComponentModel;

public interface ICommandHandler
{
    void HandleCommand<TCommand>(TCommand command);
}

public interface ICommandHandler<TCommand>
{
    void HandleCommand(TCommand command);
}

public interface ICommandHandler<TCommand, TEvent>
{
    TEvent HandleCommand(TCommand command);
}