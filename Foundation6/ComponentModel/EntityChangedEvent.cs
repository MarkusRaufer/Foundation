namespace Foundation.ComponentModel;

public record class EntityChangedEvent<TId>(KeyValue<string, TId> Identifier, PropertyChangedEvent PropertyChanged)
    : IIndexedIdentifiable<string, TId>
    , IPropertyChangedContainer;

public record class EntityChangedEvent<TObjectType, TId>(
    TObjectType ObjectType, 
    KeyValue<string, TId> Identifier, 
    PropertyChangedEvent PropertyChanged)
    : EntityChangedEvent<TId>(Identifier, PropertyChanged)
    , ITypedObject<TObjectType>;
