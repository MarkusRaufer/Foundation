namespace Foundation;

public static class Identifiable
{
    /// <summary>
    /// Factory method of Identifiable<typeparamref name="TId"/>, <typeparamref name="TSubject"/>.
    /// </summary>
    /// <typeparam name="TId"></typeparam>
    /// <typeparam name="TSubject"></typeparam>
    /// <param name="id"></param>
    /// <param name="subject"></param>
    /// <returns></returns>
    public static Identifiable<TId, TSubject> New<TId, TSubject>(TId id, TSubject subject) => new(id, subject);
}

/// <summary>
/// Decorator to make an object identifiable.
/// </summary>
/// <typeparam name="TId">The type of the identifier.</typeparam>
/// <typeparam name="TSubject">Type of the object.</typeparam>
/// <param name="Id">The identifier of the object.</param>
/// <param name="Subject">The object to be identified.</param>
public record Identifiable<TId, TSubject>(TId Id, TSubject Subject) : IIdentifiable<TId>;
