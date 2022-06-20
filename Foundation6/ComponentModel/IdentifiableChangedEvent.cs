namespace Foundation.ComponentModel;

public record class IdentifiableChangedEvent<TId>(TId Id, PropertyChangedEvent PropertyChanged) 
                    : IIdentifiable<TId>
                    where TId : notnull;

public record class IdentifiableChangedEvent<TObjectType, TId>(TObjectType ObjectType, TId Id, PropertyChangedEvent PropertyChanged)
                    : IdentifiableChangedEvent<TId>(Id, PropertyChanged)
                    where TId : notnull;
