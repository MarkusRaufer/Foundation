namespace Foundation;

/// <summary>
/// Decorator to make an object identifiable.
/// </summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
/// <typeparam name="TSubject">Type of the object.</typeparam>
/// <param name="Id">The identifier of the object.</param>
/// <param name="Subject">The object to be identified.</param>
public record Identifiable<TId, TSubject>(TId Id, TSubject Subject) : IIdentifiable<TId>;
