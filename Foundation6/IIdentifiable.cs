namespace Foundation;

/// <summary>
/// Contract for an object that can be identified by Id.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IIdentifiable<out T> where T : notnull
{
    T Id { get; }
}
