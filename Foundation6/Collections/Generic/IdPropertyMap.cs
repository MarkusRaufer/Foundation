using Foundation.ComponentModel;

namespace Foundation.Collections.Generic
{
    public class IdPropertyMap<TId>
        : IdPropertyMap<TId, EntityChanged<Guid, TId, PropertyValueChanged>>
        , IIdPropertyMap<TId, EntityChanged<Guid, TId, PropertyValueChanged>>
        where TId : notnull
    {
        public IdPropertyMap(TId id) : base(id)
        {
        }

        public IdPropertyMap(TId id, EquatableSortedDictionary<string, object> dictionary)
            : base(id, dictionary)
        {
        }

        public override void HandleEvent(EntityChanged<Guid, TId, PropertyValueChanged> @event)
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
        protected override EntityChanged<Guid, TId, PropertyValueChanged> CreateChangedEvent(string propertyName, object? value, CollectionActionState actionState)
        {
            return new (Guid.NewGuid(), Id, new PropertyValueChanged(propertyName, actionState, value));
        }
    }

    public abstract class IdPropertyMap<TId, TEvent>
        : PropertyMap<TEvent>
        , IIdPropertyMap<TId, TEvent>
        , IEquatable<IIdentifiable<TId>>
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

        public override bool Equals(object? obj) => Equals(obj as IIdentifiable<TId>);

        public bool Equals(IIdentifiable<TId>? other) => null != other && Id.Equals(other.Id);

        public override int GetHashCode() => Id.GetHashCode();

        public abstract override void HandleEvent(TEvent @event);

        public TId Id { get; }

        public override string ToString() => $"Id: {Id}, {base.ToString()}";
    }
}
