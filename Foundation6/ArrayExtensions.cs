namespace Foundation;

using System.Runtime.CompilerServices;

public static class ArrayExtensions
{
    public static IEnumerable<T> AsEnumerable<T>(params T[] items)
    {
        return items;
    }

    public static bool EqualsArray<T>(this T[] lhs, T[] rhs)
    {
        if (lhs is null) return rhs is null;

        if (rhs is null || lhs.Length != rhs.Length) return false;
        for (var i = 0; i < lhs.Length; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(lhs[i], rhs[i])) return false;
        }

        return true;
    }

    public static bool EqualsArray(this byte[] lhs, byte[] rhs)
    {
        if (lhs is null) return rhs is null;

        if (rhs is null || lhs.Length != rhs.Length) return false;
        for (var i = 0; i < lhs.Length; i++)
        {
            if (lhs[i] != rhs[i]) return false;
        }

        return true;
    }

    public static IEnumerator<T> GetEnumerator<T>(this T[] array)
    {
        return array.AsEnumerable().GetEnumerator();
    }

    /// <summary>
    /// Creates a new array and prepends <paramref name="elem"/> to the new array.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="elem"></param>
    /// <returns></returns>
    public static T[] Prepend<T>(this T[] array, T elem)
    {
        array.ThrowIfNull();

        var newValues = new T[array.Length + 1];
        newValues[0] = elem;

        Array.Copy(array, 0, newValues, 1, array.Length);

        return newValues;
    }

    public static T[] ThrowIfNullOrEmpty<T>(this T[] array, [CallerArgumentExpression("array")] string name = "")
    {
        if (null == array) throw new ArgumentNullException(name);
        if (0 == array.Length) throw new ArgumentOutOfRangeException(name, "must contain at least one element");
        return array;
    }
}

