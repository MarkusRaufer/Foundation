using Foundation.Collections.Generic;
using System.Runtime.Serialization;

namespace Foundation.ComponentModel
{
    /// <summary>
    /// This Equals of this list checks the equality of all elements.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class EquatableList<T> 
        : List<T>
        , IEquatable<EquatableList<T>>
        , ISerializable
    {
        private const string SerializationKey = "items";

        private int _hashCode;

        public EquatableList()
        {
            CreateHashCode();
        }

        public EquatableList(IEnumerable<T> collection)
        {
            foreach(T item in collection)
                base.Add(item);

            CreateHashCode();
        }

        public EquatableList(int capacity) : base(capacity)
        {
            CreateHashCode();
        }

        public EquatableList(SerializationInfo info, StreamingContext context)
        {
            if (info.GetValue(SerializationKey, typeof(List<T>)) is List<T> collection)
            {
                foreach (var item in collection)
                    base.Add(item);
            }

            CreateHashCode();
        }

        public new T this[int index]
        {
            get => base[index];
            set
            {

                var existingValue = base[index];

                if (EqualityComparer<T>.Default.Equals(existingValue, value)) return;

                base[index] = value;

                CreateHashCode();
            }
        }
        
        public new void Add(T item)
        {
            base.Add(item);

            var builder = HashCode.CreateBuilder();
            builder.AddHashCode(_hashCode);
            builder.AddObject(item);

            _hashCode = builder.GetHashCode();
        }

        protected void CreateHashCode()
        {
            var builder = HashCode.CreateBuilder();

            builder.AddObject(typeof(EquatableList<T>));

            builder.AddObjects(this);

            _hashCode = builder.GetHashCode();
        }

        public override bool Equals(object? obj) => Equals(obj as EquatableList<T>);

        public bool Equals(EquatableList<T>? other)
        {
            if (other is null) return false;
            if (_hashCode != other._hashCode) return false;

            return this.IsEqualTo(other);
        }

        public override int GetHashCode() => _hashCode;

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(SerializationKey, (List<T>)this);
        }

        public new bool Remove(T item)
        {
            if(base.Remove(item))
            {
                CreateHashCode();
                return true;
            }

            return false;
        }
    }
}
