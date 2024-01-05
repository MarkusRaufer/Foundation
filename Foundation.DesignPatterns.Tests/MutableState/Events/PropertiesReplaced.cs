using Foundation;
using Foundation.Collections.Generic;

namespace Foundation.DesignPatterns.Tests.MutableState.Events;

public record PropertiesReplaced(string ObjectType, Id EntityId, DictionaryValue<string, object?> Properties) : PropertiesEvent(ObjectType, EntityId, Properties);
