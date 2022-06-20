namespace Foundation;

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
