namespace Foundation;

public static class BitConverterExt
{
    public static IEnumerable<byte> GetBytes(decimal value)
    {
        return decimal.GetBits(value).SelectMany(i => BitConverter.GetBytes(i));
    }

    public static decimal ToDecimal(byte[] bytes)
    {
        if (16 != bytes.Length) throw new ArgumentOutOfRangeException(nameof(bytes), "A decimal must be created from exactly 16 bytes");

        //make an array to convert back to int32's
        var bits = new Int32[4];
        for (int i = 0; i <= 15; i += 4)
        {
            //convert every 4 bytes into an int32
            bits[i / 4] = BitConverter.ToInt32(bytes, i);
        }

        return new decimal(bits);
    }
}

