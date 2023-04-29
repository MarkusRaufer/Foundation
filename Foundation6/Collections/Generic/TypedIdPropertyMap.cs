using Foundation.ComponentModel;

namespace Foundation.Collections.Generic
{
    public class TypedIdPropertyMap<TId>
        : TypedIdPropertyMap<string, TId, EntityChanged<Guid, string, TId, PropertyValueChanged>>
        , IIdPropertyMap<string, TId, EntityChanged<Guid, string, TId, PropertyValueChanged>>
        where TId : notnull
    {
        public TypedIdPropertyMap(string objectType, TId id) : this(objectType, id, new EquatableSortedDictionary<string, object>())
        {
        }

        public TypedIdPropertyMap(string objectType, TId id, EquatableSortedDictionary<string, object> dictionary)
            : base(objectType, id, dictionary)
        {
        }

        public override void HandleEvent(EntityChanged<Guid, string, TId, PropertyValueChanged> @event)
        {
            @event.ThrowIfNull();

            if (!Id.Equals(@event.EntityId)) return;

            switch (@event.ChangedState.ActionState)
            {
                case CollectionActionState.Added: Add(@event.ChangedState.PropertyName, @event.ChangedState.Value); break;
                case CollectionActionState.Removed: Remove(@event.ChangedState.PropertyName); break;
                case CollectionActionState.Replaced: this[@event.ChangedState.PropertyName] = @event.ChangedState.Value!; break;
            };
        }
        protected override EntityChanged<Guid, string, TId, PropertyValueChanged> CreateChangedEvent(string propertyName, object? value, CollectionActionState actionState)
        {
            return new (Guid.NewGuid(), ObjectType, Id, new PropertyValueChanged(propertyName, actionState, value));
        }
    }

    public abstract class TypedIdPropertyMap<TObjectType, TId, TEvent>
        : IdPropertyMap<TId, TEvent>
        , IIdPropertyMap<TObjectType, TId, TEvent>
        where TId : notnull
        where TEvent : IEntityEvent<Guid, TId>, ITypedObject<TObjectType>
    {
        public TypedIdPropertyMap(TObjectType objectType, TId id)
            : this(objectType, id, new EquatableSortedDictionary<string, object>())
        {
        }

        public TypedIdPropertyMap(TObjectType objectType, TId id, EquatableSortedDictionary<string, object> dictionary)
            : base(id, dictionary)
        {
            ObjectType = objectType.ThrowIfNull();
        }

        public abstract override void HandleEvent(TEvent @event);

        public TObjectType ObjectType { get; }

        public override string ToString() => $"{nameof(ObjectType)}: {ObjectType}, {base.ToString()}";
    }
}
