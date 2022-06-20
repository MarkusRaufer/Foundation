namespace Foundation;

public interface IIdentifiable<out T> where T : notnull
{
    T Id { get; }
}
