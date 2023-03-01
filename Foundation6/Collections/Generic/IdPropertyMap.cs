using Foundation.ComponentModel;

namespace Foundation.Collections.Generic
{
    public class IdPropertyMap<TId>
        : IdPropertyMap<TId, EntityPropertyValueChanged<Guid, TId, object, PropertyValueChanged<object>>>
        , IIdPropertyMap<TId, EntityPropertyValueChanged<Guid, TId, object, PropertyValueChanged<object>>>
        where TId : notnull
    {
        public IdPropertyMap(TId id) : base(id)
        {
        }

        public IdPropertyMap(TId id, EquatableSortedDictionary<string, object> dictionary)
            : base(id, dictionary)
        {
        }

        public override void HandleEvent(EntityPropertyValueChanged<Guid, TId, object, PropertyValueChanged<object>> @event)
        {
            @event.ThrowIfNull();

            if (!Id.Equals(@event.EntityId)) return;

            switch (@event.PropertyChanged.ChangedState)
            {
                case PropertyChangedState.Added: Add(@event.PropertyChanged.PropertyName, @event.PropertyChanged.Value); break;
                case PropertyChangedState.Removed: Remove(@event.PropertyChanged.PropertyName); break;
                case PropertyChangedState.Replaced: this[@event.PropertyChanged.PropertyName] = @event.PropertyChanged.Value!; break;
            };
        }
        protected override EntityPropertyValueChanged<Guid, TId, object, PropertyValueChanged<object>> CreateChangedEvent(string propertyName, object? value, PropertyChangedState state)
        {
            return new EntityPropertyValueChanged<Guid, TId, object, PropertyValueChanged<object>>(Guid.NewGuid(), Id, new PropertyValueChanged<object>(propertyName, state, value));
        }
    }



    public abstract class IdPropertyMap<TObjectType, TId, TEvent>
        : IdPropertyMap<TId, TEvent>
        , IIdPropertyMap<TObjectType, TId, TEvent>
        where TId : notnull
        where TEvent : IEntityEvent<Guid, TId>, ITypedObject<TObjectType>
    {
        public IdPropertyMap(TObjectType objectType, TId id)
            : this(objectType, id, new EquatableSortedDictionary<string, object>())
        {
        }

        public IdPropertyMap(TObjectType objectType, TId id, EquatableSortedDictionary<string, object> dictionary)
            : base(id, dictionary)
        {
            ObjectType = objectType.ThrowIfNull();
        }

        public abstract override void HandleEvent(TEvent @event);

        public TObjectType ObjectType { get; }

        public override string ToString() => $"{ObjectType}, {Id}";
    }

    public abstract class IdPropertyMap<TId, TEvent>
        : PropertyMap<TEvent>
        , IIdPropertyMap<TId, TEvent>
        where TId : notnull
        where TEvent : IEntityEvent<Guid, TId>
    {
        public IdPropertyMap(TId id) : this(id, new EquatableSortedDictionary<string, object>())
        {
        }

        public IdPropertyMap(TId id, EquatableSortedDictionary<string, object> dictionary)
            : base(dictionary)
        {
            Id = id;
        }

        public abstract override void HandleEvent(TEvent @event);

        public TId Id { get; }

        public override string ToString() => $"{Id}";
    }
}
