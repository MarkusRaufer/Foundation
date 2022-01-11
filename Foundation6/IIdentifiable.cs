namespace Foundation;

using System.Diagnostics.CodeAnalysis;

public interface IIdentifiable<out T> where T : notnull
{
    [NotNull]
    T Id { get; }
}
