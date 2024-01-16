using System.Collections;
using System.Runtime.InteropServices;

namespace Foundation;

public static class BitConverterExt
{
    /// <summary>
    /// Creates a list of bytes from <paramref name="value"/>
    /// </summary>
    /// <param name="value"></param>
    /// <returns>a byte array with the converted decimal value.</returns>
    public static byte[] GetBytes(decimal value)
    {
        ReadOnlySpan<int> bits = decimal.GetBits(value);
        var bytes = MemoryMarshal.Cast<int, byte>(bits);

        return bytes.ToArray();
    }

    /// <summary>
    /// Creates a decimal value from a list of <paramref name="bytes"/>.
    /// </summary>
    /// <param name="bytes">Size must be exactly 16 bytes.</param>
    /// <returns></returns>
    public static decimal ToDecimal(byte[]? bytes)
    {
        bytes = bytes.ThrowIfNull();
        bytes.ThrowIf(() => 16 != bytes!.Length, "A decimal must be created from exactly 16 bytes");

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

