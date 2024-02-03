using Foundation.Collections.Generic;
using Foundation.DesignPatterns.Tests.MutableState.Commands;
using Foundation.DesignPatterns.Tests.MutableState.Events;

namespace Foundation.DesignPatterns.MutableState;

public class PropertiesState : IMutableState<DictionaryValue<string, object?>, PropertiesCommand, PropertiesEvent>
{
    private DictionaryValue<string, object?> _state;

    public PropertiesState(DictionaryValue<string, object?> state)
    {
        _state = state;
    }

    public DictionaryValue<string, object?> State => _state;

    /// <inheritdoc/>
    public void ApplyEvent(PropertiesEvent @event)
    {
        switch (@event)
        {
            case PropertiesReplaced ev: ApplyPropertiesReplaced(ev); break;
        }
    }

    public PropertiesEvent HandleCommand(PropertiesCommand command)
    {
        return command switch
        {
            ReplaceProperties cmd => new PropertiesReplaced(cmd.ObjectType, cmd.EntityId, cmd.Properties),
            _ => throw new NotImplementedException($"{command}")
        };
    }

    private void ApplyPropertiesReplaced(PropertiesReplaced ev)
    {
        _state = _state.Replace(ev.Properties).ToDictionaryValue(x => x.Key, x => x.Value);
    }
}
