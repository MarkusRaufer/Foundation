namespace Foundation.ComponentModel;

using System.Diagnostics.CodeAnalysis;

public class EntityChangedEvent<TId>
    : IndexedIdentifiable<string, TId>
    , IIdentifiable<TId>
    , IPropertyChangedContainer
    , IEquatable<EntityChangedEvent<TId>>
    where TId : notnull
{
    public EntityChangedEvent(KeyValue<string, TId> identifier, PropertyChangedEvent propertyChanged) 
        : base(identifier)
    {
        PropertyChanged = propertyChanged.ThrowIfNull();
    }

    public TId Id => Identifier.Value;

    [NotNull]
    public PropertyChangedEvent PropertyChanged { get; }

    public override bool Equals(object? obj) => Equals(obj as EntityChangedEvent<TId>);

    public bool Equals(EntityChangedEvent<TId>? other)
    {
        return null != other && base.Equals(other) && PropertyChanged.Equals(other.PropertyChanged);
    }

    public override int GetHashCode() => System.HashCode.Combine(Identifier, PropertyChanged);

    public override string ToString() => $"{Identifier} => {PropertyChanged}";
}

public class EntityChangedEvent<TObjectType, TId>
    : EntityChangedEvent<TId>
    , ITypedObject<TObjectType>
    , IEquatable<EntityChangedEvent<TObjectType, TId>>
    where TId : notnull
    where TObjectType : notnull
{
    public EntityChangedEvent(
        TObjectType objectType,
        KeyValue<string, TId> identifier,
        PropertyChangedEvent propertyChanged)
        : base(identifier, propertyChanged)
    {
        ObjectType = objectType.ThrowIfNull();
    }

    public override bool Equals(object? obj) => Equals(obj as EntityChangedEvent<TObjectType, TId>);

    public bool Equals(EntityChangedEvent<TObjectType, TId>? other)
    {
        return null != other
            && ObjectType.Equals(other.ObjectType)
            && base.Equals(other);
            
    }

    public override int GetHashCode() => System.HashCode.Combine(ObjectType, Identifier, PropertyChanged);

    [NotNull]
    public TObjectType ObjectType { get; }

    public override string ToString() => $"{ObjectType}, {Identifier} => {PropertyChanged}";
}

