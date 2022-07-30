namespace Foundation;

/// <summary>
/// The compares compatible types only. If types are incompatible it returns None.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IOptionalComparable<T>
{
    /// <summary>
    /// The compares compatible types only. If types are incompatible it returns None.
    /// </summary>
    /// <param name="other"></param>
    /// <returns>None, other is incompatible and can't be compared.</returns>
    Option<int> OptionalCompareTo(T other);
}
