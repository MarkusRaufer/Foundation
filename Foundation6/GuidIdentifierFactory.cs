namespace Foundation;

public class GuidIdentifierFactory : IIdentifierFactory
{
    public Identifier CreateEmptyIdentifier()
    {
        return Identifier.New(Guid.Empty);
    }

    public Identifier CreateIdentifier()
    {
        return Identifier.New(Guid.NewGuid());
    }

    public Identifier CreateIdentifier(Guid id) => Identifier.New(id);

    public Identifier CreateIdentifierFromString(string value)
    {
        value.ThrowIfNullOrEmpty(nameof(value));

        var id = Guid.Parse(value);
        return Identifier.New(id);
    }

    public void Reset()
    {
    }
}

