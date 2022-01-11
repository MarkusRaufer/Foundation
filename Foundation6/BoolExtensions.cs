namespace Foundation;

public static class BoolExtensions
{
    public static bool ThrowIfFalse(this bool value, string name)
    {
        name.ThrowIfNullOrEmpty(nameof(name));

        if (value) return value;
        throw new ArgumentNullException(name);
    }
}
