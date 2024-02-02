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
﻿namespace Foundation;

public struct ErrorException<TError, TException>
    : IEquatable<ErrorException<TError, TException>>
    where TException : Exception
{
    private int _hashCode;

    public ErrorException(TError error, TException exception)
    {
        Error = error.ThrowIfNull();
        Exception = exception.ThrowIfNull();
        _hashCode = 0;
    }
    public static bool operator ==(ErrorException<TError, TException> left, ErrorException<TError, TException> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(ErrorException<TError, TException> left, ErrorException<TError, TException> right)
    {
        return !(left == right);
    }

    public TError Error { get; }
    public TException Exception { get; }

    public override bool Equals(object? obj) => obj is ErrorException<TError, TException> other && Equals(other);

    public bool Equals(ErrorException<TError, TException> other)
    {
        if (!IsInitalized) return !other.IsInitalized;

        return EqualityComparer<TError>.Default.Equals(Error, other.Error) &&
               EqualityComparer<TException>.Default.Equals(Exception, other.Exception);
    }

    public override int GetHashCode()
    {
        if (!IsInitalized) return 0;

        if (0 == _hashCode) _hashCode = System.HashCode.Combine(Error, Exception);

        return _hashCode;
    }

    public bool IsInitalized => null != Exception;

    public override string ToString() => $"Error:{Error}, Exception:{Exception}";
}
