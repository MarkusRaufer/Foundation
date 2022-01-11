namespace Foundation;

public class HashHelper
{
    public static Func<string, int> CreateStringPrefixHashCodeFunction(int length)
    {
        return s => s[..length].GetHashCode();
    }
}
