namespace Foundation.ComponentModel;

/// <summary>
/// Contract of a factory that uses a delegate to create an id.
/// </summary>
/// <typeparam name="TFunc">Should be a functor that returns a value.</typeparam>
public interface IDelegateIdFactory<TFunc>
    where TFunc : Delegate
{
    TFunc CreateId { get; }
}

/// <summary>
/// Contract of a factory that uses a delegate to create an id. The lambda expects an input value to create an id.
/// </summary>
/// <typeparam name="TIdSelector">An input value to create an id.</typeparam>
/// <typeparam name="TId">Type of the id.</typeparam>
public interface IDelegateIdFactory<TIdSelector, TId> : IDelegateIdFactory<Func<TIdSelector, TId>>
{
}

/// <summary>
/// Contract of a factory that uses a delegate to create an id. The lambda expects an input value to create an id.
/// </summary>
/// <typeparam name="TFactoryId">The identifier of the factory.</typeparam>
/// <typeparam name="TIdSelector">An input value to create an id.</typeparam>
/// <typeparam name="TId">Type of the id.</typeparam>
public interface IDelegateIdFactory<TFactoryId, TIdSelector, TId> 
    : IDelegateIdFactory<TIdSelector, TId>
    , IIdentifiableFactory<TFactoryId>
{
}
