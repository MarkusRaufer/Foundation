namespace Foundation;

public interface IIdentifier
    : IComparable
    , IFormattable
{
    bool IsEmpty { get; }
}

public interface IIdentifier<T> : IIdentifier
    where T : IComparable<T>, IEquatable<T>
{
}
