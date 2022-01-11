namespace Foundation;

public record struct Quantity(string Unit, decimal Value)
{
    public bool IsEmpty => 0 == GetHashCode();
}

public record struct Quantity<TValue>(string Unit, TValue Value)
{
    public bool IsEmpty => 0 == GetHashCode();
}

public record struct Quantity<TUnit, TValue>(TUnit Unit, TValue Value)
{
    public bool IsEmpty => 0 == GetHashCode();
}

