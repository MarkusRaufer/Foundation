namespace Foundation;

/// <summary>
/// The optional comparison only compares compatible types.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IOptionalComparable<T>
{
    /// <summary>
    /// The optional comparison can be used to check if values can be compared.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>None, other is incompatible and can't be compared.</returns>
    Opt<int> OptionalCompareTo(T other);
}
