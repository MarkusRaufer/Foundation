namespace Foundation;

/// <summary>
/// If you need a property that can only be set internally from the same assembly
/// after initialization you can use this class in combination with required properties with init setter.
/// If you want to set the value after initialization you can use the <see cref="Internal{T}.Value"/> property.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
/// <param name="value">The initial value.</param>
public class Internal<T>(T value) : IEquatable<Internal<T>>
{
    public static implicit operator Internal<T>(T value) => new (value);

    public override bool Equals(object? obj) => Equals(obj as Internal<T>);

    public bool Equals(Internal<T>? other)
    {
        return other is not null && Value.EqualsNullable(other.Value);
    }

    public override int GetHashCode() => Value.GetNullableHashCode();

    public override string ToString() => $"{Value}";

    public T Value { get; internal set; } = value.ThrowIfNull();
}
