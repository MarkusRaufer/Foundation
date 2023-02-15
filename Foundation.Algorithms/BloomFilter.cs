using System.Security.Cryptography;

namespace Foundation.Algorithms;

public abstract class BloomFilter<T>
{
    private readonly int[] _bits;
    private readonly int _hashFunctions;
    private readonly HashAlgorithm _hashAlgorithm;

    public BloomFilter(int size, int hashFunctions, HashAlgorithm hashAlgorithm)
    {
        _bits = new int[size];
        _hashFunctions = hashFunctions;
        _hashAlgorithm = hashAlgorithm;
    }

    public void Add(T item)
    {
        var data = GetHash(item);
        for (int i = 0; i < _hashFunctions; i++)
        {
            int index = GetIndex(data, i, _bits.Length);
            _bits[index] = 1;
        }
    }

    public bool Contains(T item)
    {
        var data = GetHash(item);
        for (int i = 0; i < _hashFunctions; i++)
        {
            int index = GetIndex(data, i, _bits.Length);
            if (_bits[index] == 0)
            {
                return false;
            }
        }
        return true;
    }

    public abstract byte[] GetBytes(T item);

    private byte[] GetHash(T item)
    {
        ArgumentNullException.ThrowIfNull(item);

        var bytes = GetBytes(item);
        return _hashAlgorithm.ComputeHash(bytes);
    }

    private int GetIndex(byte[] data, int hashFunctionIndex, int size)
    {
        int hash = 0;
        for (int i = hashFunctionIndex; i < data.Length; i += _hashFunctions)
        {
            hash = (hash * 31) + data[i];
        }
        return Math.Abs(hash % size);
    }
}