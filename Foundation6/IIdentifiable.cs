namespace Foundation;

/// <summary>
/// Contract for an object that can be identified by EntityId.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IIdentifiable<out T>
{
    T Id { get; }
}
