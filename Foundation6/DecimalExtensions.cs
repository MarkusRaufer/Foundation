namespace Foundation;

public static class DecimalExtensions
{
    public static byte[] ToByteArray(this decimal d)
    {
        return BitConverterExt.GetBytes(d).ToArray();
    }
}

