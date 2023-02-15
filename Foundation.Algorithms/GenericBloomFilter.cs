using System.Security.Cryptography;
using System.Text;

namespace Foundation.Algorithms;

public class GenericBloomFilter<T> : BloomFilter<T>
{
    public GenericBloomFilter(int size, int hashFunctions, HashAlgorithm hashAlgorithm)
        : base(size, hashFunctions, hashAlgorithm)
    {
    }

    public override byte[] GetBytes(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        return Encoding.UTF8.GetBytes(item.ToString()!);
    }
}