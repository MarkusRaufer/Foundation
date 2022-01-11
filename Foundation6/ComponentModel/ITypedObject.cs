namespace Foundation.ComponentModel;

/// <summary>
/// The type of a object like invoice, order, ...
/// </summary>
/// <typeparam name="TObjectType"></typeparam>
public interface ITypedObject<TObjectType>
{
    TObjectType ObjectType { get; }
}

