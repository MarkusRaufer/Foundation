using Foundation.Collections.Generic;
using Foundation.ComponentModel;

namespace Foundation.DesignPatterns.MutableState
{
    public class Entity 
        : ITypedObject<string>
        , IIdentifiable<Id>
        , IEquatable<Entity>
    {
        private readonly PropertiesState _properties;

        public Entity(string objectType, Id id, PropertiesState properties)
        {
            Id = id;
            ObjectType = objectType;
            _properties = properties;
        }

        public Id Id { get; }

        public string ObjectType { get; }

        public DictionaryValue<string, object?> Properties => _properties.State;

        public override bool Equals(object? obj) => Equals(obj as Entity);
        
        public bool Equals(Entity? other) => other is not null && Id == other.Id;

        public override int GetHashCode() => Id.GetHashCode();
    }
}
