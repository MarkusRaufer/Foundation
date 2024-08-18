using System.Runtime.CompilerServices;

namespace Foundation;
public static class IdExtensions
{
    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if <paramref name="id"/> is empty.
    /// </summary>
    /// <param name="id">The identifier which should be checked.</param>
    /// <param name="paramName">The name of the caller.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">Is thrown if IsEmpty of id returns true.</exception>
    public static Id ThrowIfEmpty(this Id id, [CallerArgumentExpression(nameof(id))] string paramName = "")
    {
        return ThrowIfEmpty(id, () => throw new ArgumentNullException(paramName));
    }

    /// <summary>
    /// Throws an <see cref="Exception"/> if <paramref name="id"/> is empty.
    /// </summary>
    /// <param name="id">The identifier which should be checked.</param>
    /// <param name="exeption">The exception which is thrown on IsEmtpy is true.</param>
    /// <returns></returns>
    public static Id ThrowIfEmpty(this Id id, Func<Exception> exeption)
    {
        return id.IsEmpty ? throw exeption() : id;
    }
}
