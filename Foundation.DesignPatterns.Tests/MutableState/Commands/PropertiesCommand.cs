using Foundation.Collections.Generic;

namespace Foundation.DesignPatterns.Tests.MutableState.Commands;

public abstract record PropertiesCommand(string ObjectType, Id EntityId, DictionaryValue<string, object?> Properties);
