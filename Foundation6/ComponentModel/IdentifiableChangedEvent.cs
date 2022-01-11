namespace Foundation.ComponentModel;

using System.Diagnostics.CodeAnalysis;

public record class IdentifiableChangedEvent<TId>(TId Id, PropertyChangedEvent PropertyChanged) 
                    : IIdentifiable<TId>
                    where TId : notnull;

public record class IdentifiableChangedEvent<TObjectType, TId>(TObjectType ObjectType, TId Id, PropertyChangedEvent PropertyChanged)
                    : IdentifiableChangedEvent<TId>(Id, PropertyChanged)
                    where TId : notnull;

//public class IdentifiableChangedEvent<TId>
//    : Identifiable<TId>
//    , IIdentifiable<TId>
//    , IEquatable<IdentifiableChangedEvent<TId>>
//    where TId : notnull
//{
//    public IdentifiableChangedEvent(TId id, PropertyChangedEvent propertyChanged) : base(id)
//    {
//        PropertyChanged = propertyChanged.ThrowIfNull(nameof(propertyChanged));
//    }

//    [NotNull]
//    public PropertyChangedEvent PropertyChanged { get; }

//    public override bool Equals(object? obj) => Equals(obj as IdentifiableChangedEvent<TId>);

//    public bool Equals(IdentifiableChangedEvent<TId>? other)
//    {
//        return null != other && base.Equals(other) && PropertyChanged.Equals(other.PropertyChanged);
//    }

//    public override int GetHashCode() => System.HashCode.Combine(Id, PropertyChanged);

//    public override string ToString() => $"{Id} => {PropertyChanged}";
//}

//public class IdentifiableChangedEvent<TObjectType, TId>
//    : IdentifiableChangedEvent<TId>
//    , ITypedObject<TObjectType>
//    , IEquatable<IdentifiableChangedEvent<TObjectType, TId>>
//    where TId : notnull
//    where TObjectType : notnull
//{
//    public IdentifiableChangedEvent(
//        TObjectType objectType,
//        TId id,
//        PropertyChangedEvent propertyChanged)
//        : base(id, propertyChanged)
//    {
//        ObjectType = objectType.ThrowIfNull(nameof(objectType));
//    }

//    public override bool Equals(object? obj) => Equals(obj as IdentifiableChangedEvent<TObjectType, TId>);

//    public bool Equals(IdentifiableChangedEvent<TObjectType, TId>? other)
//    {
//        return null != other
//            && ObjectType.Equals(other.ObjectType)
//            && base.Equals(other);

//    }

//    public override int GetHashCode() => System.HashCode.Combine(ObjectType, Id, PropertyChanged);

//    [NotNull]
//    public TObjectType ObjectType { get; }

//    public override string ToString() => $"{ObjectType}, {Id} => {PropertyChanged}";
//}

