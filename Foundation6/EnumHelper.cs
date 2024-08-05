namespace Foundation;
public static class EnumHelper
{
    /// <summary>
    /// Returns a string of an enum value.
    /// </summary>
    /// <typeparam name="T">Type of the enum value.</typeparam>
    /// <param name="value">The enum value.</param>
    /// <param name="nameAsValue">If true the name of the enum value is returned.
    /// If false the value representing an enum name is returned as string.
    /// </param>
    /// <returns>The enum value.</returns>
    public static string ToString<T>(T value, bool nameAsValue) where T : Enum
    {
        return nameAsValue ? $"{value}" : $"{Convert.ChangeType(value, Type.GetTypeCode(typeof(T)))}";
    }
}
