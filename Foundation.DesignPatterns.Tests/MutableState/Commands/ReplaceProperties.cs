using Foundation.Collections.Generic;

namespace Foundation.DesignPatterns.Tests.MutableState.Commands;

public record ReplaceProperties(string ObjectType, Id EntityId, DictionaryValue<string, object?> Properties) : PropertiesCommand(ObjectType, EntityId, Properties);
