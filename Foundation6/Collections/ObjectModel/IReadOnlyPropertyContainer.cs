namespace Foundation.Collections.ObjectModel
{
    public interface IReadOnlyPropertyContainer : IReadOnlyPropertyContainer<IReadOnlyPropertyCollection>
    {
    }

    public interface IReadOnlyPropertyContainer<TPropertyCollection>
        where TPropertyCollection : IReadOnlyPropertyCollection
    {
        TPropertyCollection Properties { get; }
    }

    public interface IReadOnlyPropertyContainer<TProperty, TPropertyCollection>
        where TPropertyCollection : IReadOnlyPropertyCollection<TProperty>
    {
        TPropertyCollection Properties { get; }
    }
}
