namespace Foundation;

public interface IIdentifierFactory : IResetable
{
    Identifier CreateIdentifier();
}

public interface IIdentifierFactory<T>
    where T : struct, IComparable<T>, IEquatable<T>
{
    Identifier<T> CreateIdentifier();

}
public interface IIdentifierFactory<TEntity, T>
    where TEntity : IEquatable<TEntity>
    where T : struct, IComparable<T>, IEquatable<T>
{
    Identifier<TEntity, T> CreateIdentifier();
}

