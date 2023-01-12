namespace Foundation.ComponentModel;

public interface ICommandHandler<TCommand>
{
    void HandleCommand(TCommand command);
}
