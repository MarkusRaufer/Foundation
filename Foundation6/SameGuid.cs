using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Foundation;

/// <summary>
/// Creates the same Guid when using the same seed value.
/// </summary>
public static class SameGuid
{
    /// <summary>
    /// Creates a same Guids if using same seed value.
    /// This is needed for tests.
    /// </summary>
    /// <param name="seed">The value to create a Guid.</param>
    /// <returns>A Guid value.</returns>
    public static Guid New(object seed)
    {
#if NET6_0_OR_GREATER
            byte[] hash = MD5.HashData(Encoding.Default.GetBytes($"{seed}"));
#else
        using MD5 md5 = MD5.Create();

        byte[] hash = md5.ComputeHash(Encoding.Default.GetBytes($"{seed}"));
#endif
        return new Guid(hash);
    }
}
