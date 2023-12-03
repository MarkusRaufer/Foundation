namespace Foundation;

/// <summary>
/// Type that represent a quantity.
/// </summary>
/// <param name="Unit">The unit of the quantity.</param>
/// <param name="Value">The value of the quantity.</param>
public readonly record struct Quantity(string Unit, decimal Value)
{
    public readonly bool IsEmpty => 0 == GetHashCode();

    public static Quantity New(string unit, decimal value) => new(unit, value);

    public static Quantity<TValue> New<TValue>(string unit, TValue value) => new(unit, value);

    public static Quantity<TUnit, TValue> New<TUnit, TValue>(TUnit unit, TValue value) => new(unit, value);
}

/// <summary>
/// Type that represent a quantity.
/// </summary>
/// <typeparam name="TValue">Type of the value of the quantity.</typeparam>
/// <param name="Unit">The unit of the quantity.</param>
/// <param name="Value">The value of the quantity.</param>
public readonly record struct Quantity<TValue>(string Unit, TValue Value)
{
    public readonly bool IsEmpty => 0 == GetHashCode();
}

/// <summary>
/// Type that represent a quantity.
/// </summary>
/// <typeparam name="TUnit">The unit type of the quantity.</typeparam>
/// <typeparam name="TValue">Type of the value of the quantity.</typeparam>
/// <param name="Unit">The unit of the quantity.</param>
/// <param name="Value">The value of the quantity.</param>
public readonly record struct Quantity<TUnit, TValue>(TUnit Unit, TValue Value)
{
    public readonly bool IsEmpty => 0 == GetHashCode();
}

