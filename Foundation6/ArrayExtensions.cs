namespace Foundation;

using Foundation.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

public static class ArrayExtensions
{
    public static bool EqualsArray<T>(this T[] lhs, [DisallowNull] T[] rhs)
    {
        if (lhs is null) return rhs is null;

        if (lhs.Length != rhs.Length) return false;
        for (var i = 0; i < lhs.Length; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(lhs[i], rhs[i])) return false;
        }

        return true;
    }

    public static IEnumerator<T> GetEnumerator<T>(this T[] array)
    {
        return new ArrayEnumerator<T>(array);
    }

    public static T[] ThrowIfNullOrEmpty<T>(this T[] array, [CallerArgumentExpression("array")] string name = "")
    {
        if (null == array) throw new ArgumentNullException(name);
        if (0 == array.Length) throw new ArgumentOutOfRangeException(name, "must contain at least one element");
        return array;
    }
}

