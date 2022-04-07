namespace Foundation.Collections.Generic;

public static class HashChainElement
{
    public static HashChainElement<TPayload> New<TPayload>(TPayload payload, int prevElementHash = 0)
        where TPayload : notnull
    {
        return new HashChainElement<TPayload>(payload, prevElementHash);
    }
}

/// <summary>
/// This immutable type has a payload and the hashcode of the previous element in a hash chain.
/// </summary>
/// <typeparam name="TPayload"></typeparam>
public class HashChainElement<TPayload>
    : IEquatable<HashChainElement<TPayload>>
{
    private readonly int _hashCode;

    //This should be used on the first element means when no previous element exists.
    public HashChainElement(TPayload payload) : this(payload, 0)
    {
    }

    public HashChainElement(TPayload payload, int previousElementHash)
    {
        Payload = payload.ThrowIfNull();

        PreviousElementHash = previousElementHash;

        var hcb = HashCode.CreateBuilder();
        hcb.AddObject(Payload);
        hcb.AddHashCode(PreviousElementHash);

        _hashCode = hcb.GetHashCode();
    }

    public TPayload Payload { get; }

    /// <summary>
    /// This hash code must not be 0 except of the first element of a hash chain.
    /// </summary>
    public int PreviousElementHash { get; }

    public override bool Equals(object? obj) => Equals(obj as HashChainElement<TPayload>);

    public bool Equals(HashChainElement<TPayload>? other)
    {
        return null != other 
            && PreviousElementHash == other.PreviousElementHash
            && EqualityComparer<TPayload>.Default.Equals(Payload, other.Payload);
    }

    public override int GetHashCode() => _hashCode;
}