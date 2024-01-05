using Foundation.Collections.Generic;
using Foundation.ComponentModel;

namespace Foundation.DesignPatterns.MutableState;

public class Entity : Entity<Id>
{
    public Entity(string objectType, Id id, PropertiesState properties)
        : base(objectType, id, properties)
    {
    }
}

public class Entity<TId>
    : ITypedObject<string>
    , IIdentifiable<TId>
    , IEquatable<Entity<TId>>
    where TId : notnull, IEquatable<TId>
{
    private readonly PropertiesState _properties;

    public Entity(string objectType, TId id, PropertiesState properties)
    {
        ObjectType = objectType.ThrowIfNullOrWhiteSpace();
        Id = id;
         _properties = properties.ThrowIfNull();
    }

    public TId Id { get; }

    public string ObjectType { get; }

    public DictionaryValue<string, object?> Properties => _properties.State;

    public override bool Equals(object? obj) => Equals(obj as Entity<TId>);
    
    public bool Equals(Entity<TId>? other) => other is not null
                                           && GetHashCode() == other.GetHashCode()
                                           && ObjectType.Equals(other.ObjectType)
                                           && Id.Equals(other.Id);

    public override int GetHashCode() => System.HashCode.Combine(ObjectType, Id);

    public override string ToString() => $"{Id}";
}
