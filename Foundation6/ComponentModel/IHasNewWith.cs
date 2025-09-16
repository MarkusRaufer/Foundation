namespace Foundation.ComponentModel;

/// <summary>
/// Contract of a factory method typically used that allows partial input values to create object.
/// </summary>
/// <typeparam name="TSelf"></typeparam>
/// <typeparam name="TInput"></typeparam>
public interface IHasNewWith<TSelf, TInput>
    where TSelf : IHasNewWith<TSelf, TInput>
{
    TSelf NewWith(TInput input);
}
