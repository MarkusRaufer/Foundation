namespace Foundation.Collections.Generic;

public static class HashChainElement
{
    public static HashChainElement<TPayload, THash> New<TPayload, THash>(
        TPayload payload, 
        Func<TPayload, THash> getHash,
        Option<THash> prevElementHash)
        where TPayload : notnull
        where THash : notnull
    {
        return new HashChainElement<TPayload, THash>(payload, getHash, prevElementHash);
    }
}

/// <summary>
/// This immutable type has a payload and the hashcode of the previous element in a hash chain.
/// </summary>
/// <typeparam name="TPayload"></typeparam>
public class HashChainElement<TPayload, THash>
    : IEquatable<HashChainElement<TPayload, THash>>
    where TPayload : notnull
    where THash : notnull
{
    public HashChainElement(TPayload payload, Func<TPayload, THash> getHash, Option<THash> previousElementHash)
    {
        Payload = payload.ThrowIfNull();

        Hash = getHash(Payload);

        PreviousElementHash = previousElementHash;
    }

    public override bool Equals(object? obj) => Equals(obj as HashChainElement<TPayload, THash>);

    public bool Equals(HashChainElement<TPayload, THash>? other)
    {
        return null != other
            && PreviousElementHash.Equals(other.PreviousElementHash)
            && Payload.Equals(other.Payload);
    }

    public override int GetHashCode() => Hash.GetHashCode();

    public THash Hash { get; }
    public TPayload Payload { get; }

    /// <summary>
    /// This hash code must not be 0 except of the first element of a hash chain.
    /// </summary>
    public Option<THash> PreviousElementHash { get; }

    public override string ToString()
    {
        return $"Payload:{Payload}, Hash:{Hash}, PreviousElementHash:{PreviousElementHash}";
    }
}