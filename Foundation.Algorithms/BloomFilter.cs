// The MIT License (MIT)
//
// Copyright (c) 2020 Markus Raufer
//
// All rights reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
﻿using System.Security.Cryptography;

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