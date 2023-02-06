namespace Foundation.Collections.Generic
{
    /// <summary>
    /// Represents a value with an ordinal position of a list.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="Position">The ordinal position in a list.</param>
    /// <param name="Value">The positioned value.</param>
    public record struct Ordinal<T>(int Position, T Value) : IComparable<Ordinal<T>>
    {
        public int CompareTo(Ordinal<T> other) => Position.CompareTo(other.Position);
    }
}
