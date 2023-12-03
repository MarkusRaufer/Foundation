using Foundation.Collections.Generic;

namespace Foundation.DesignPatterns.Tests.MutableState.Events;

public abstract record PropertiesEvent(string ObjectType, Id EntityId, DictionaryValue<string, object?> Properties);
