namespace Foundation.ComponentModel;

public record PropertyChangedEvent(string Name, object? Value, PropertyChangedState ChangedState) : IPropertyChangedStateEvent;

public record PropertyChangedEvent<TObjectType>(TObjectType ObjectType, string Name, object? Value, PropertyChangedState ChangedState) 
    : PropertyChangedEvent(Name, Value, ChangedState)
    , ITypedObject<TObjectType>;
