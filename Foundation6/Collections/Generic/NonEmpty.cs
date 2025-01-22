using System.Collections;

namespace Foundation.Collections.Generic;

public static class NonEmpty
{
    public static NonEmpty<TCollection> New<TCollection>(TCollection collection)
        where TCollection : IEnumerable 
        => new(collection);
}

public record NonEmpty<TCollection>
    where TCollection : IEnumerable
{
    private readonly TCollection _collection;

    public NonEmpty(TCollection collection)
    {
        collection.ThrowIfNull();
        if (!collection.AnyObject()) throw new ArgumentException($"{nameof(collection)} must not be empty");

        _collection = collection;
    }

    public static implicit operator TCollection(NonEmpty<TCollection> collection) => collection._collection;
}
