namespace Foundation;

public static class TimedRc
{
    /// <summary>
    /// Creates a timed reference counter object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rc"></param>
    /// <returns></returns>
    public static TimedRc<T> New<T>(Rc<T> rc) where T : notnull, IEquatable<T>
    {
        return new TimedRc<T>(rc);
    }
}

public struct TimedRc<T>
    : IEquatable<TimedRc<T>>
    , IOptionalComparable<TimedRc<T>>
    where T : notnull, IEquatable<T>
{
    private readonly Rc<T> _rc;

    public TimedRc(Rc<T> rc) : this(rc, DateTime.Now)
    {
    }

    public TimedRc(Rc<T> rc, DateTime dt)
    {
        _rc = rc.IsEmpty ? throw new ArgumentNullException(nameof(rc)) : rc;
        TimeStamp = dt;
    }

    public static bool operator ==(TimedRc<T> left, TimedRc<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TimedRc<T> left, TimedRc<T> right)
    {
        return !(left == right);
    }

    public int Counter => _rc.Counter;

    public override bool Equals(object? obj) => obj is TimedRc<T> other && Equals(other);

    public bool Equals(TimedRc<T> other)
    {
        if (!_rc.Equals(other._rc)) return false;

        return TimeStamp.Equals(other.TimeStamp);
    }

    public Opt<T> Get()
    {
        TimeStamp = DateTime.Now;

        return _rc.Get();
    }

    public override int GetHashCode() => System.HashCode.Combine(_rc, TimeStamp);

    public bool IsEmpty => _rc.IsEmpty;

    public Opt<int> OptionalCompareTo(TimedRc<T> other)
    {
        if (IsEmpty) return Opt.None<int>();

        var result = _rc.OptionalCompareTo(other._rc);
        if (result.IsNone || 0 != result.ValueOrThrow()) return result;

        return TimeStamp.CompareTo(other.TimeStamp);
    }

    public void Reset()
    {
        TimeStamp = DateTime.Now;
        _rc.Reset();
    }

    public DateTime TimeStamp { get; private set; }

    public override string ToString()
    {
        return $"{_rc}, TimeStamp: {TimeStamp}";
    }
}

