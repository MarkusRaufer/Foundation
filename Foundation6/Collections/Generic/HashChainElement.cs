// The MIT License (MIT)
//
// Copyright (c) 2020 Markus Raufer
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
﻿namespace Foundation.Collections.Generic;

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