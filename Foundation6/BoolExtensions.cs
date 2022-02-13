using System.Runtime.CompilerServices;

namespace Foundation;

public static class BoolExtensions
{
    public static bool ThrowIfFalse(this bool value, [CallerArgumentExpression("value")] string name = "")
    {
        if (value) return value;
        throw new ArgumentNullException(name);
    }
}
