namespace Foundation;

public static class TimedRc
{
    /// <summary>
    /// Creates a timed reference counter. The timestamp is <see cref="DateTimeKind.Utc"./>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rc"></param>
    /// <returns></returns>
    public static TimedRc<T> New<T>(Rc<T> rc) where T : notnull, IEquatable<T>
    {
        return new TimedRc<T>(rc);
    }

    /// <summary>
    /// Creates a timed reference counter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rc"></param>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static TimedRc<T> New<T>(Rc<T> rc, DateTime timestamp) where T : notnull, IEquatable<T>
    {
        return new TimedRc<T>(rc, timestamp);
    }
}

/// <summary>
/// A timed reference counter.
/// </summary>
/// <typeparam name="T"></typeparam>
public struct TimedRc<T>
    : IEquatable<TimedRc<T>>
    , IComparable<TimedRc<T>>
    where T : notnull, IEquatable<T>
{
    private readonly Rc<T> _rc;

    public TimedRc(Rc<T> rc, DateTimeKind kind = DateTimeKind.Utc)
    {
        _rc = rc.IsEmpty ? throw new ArgumentNullException(nameof(rc)) : rc;
        Timestamp = DateTimeHelper.Now(kind);
    }

    public TimedRc(Rc<T> rc, DateTime timestamp)
    {
        _rc = rc.IsEmpty ? throw new ArgumentNullException(nameof(rc)) : rc;
        Timestamp = timestamp;
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

        return Timestamp.ValueObjectEquals(other.Timestamp);
    }

    /// <summary>
    /// Timestamp is changed on call.
    /// </summary>
    /// <returns></returns>
    public Opt<T> Get()
    {
        if (IsEmpty) return Opt.None<T>();

        Timestamp = DateTimeHelper.Now(Timestamp.Kind);

        return _rc.Get();
    }

    public override int GetHashCode() => System.HashCode.Combine(_rc, Timestamp);

    public bool IsEmpty => _rc.IsEmpty;

    public int CompareTo(TimedRc<T> other)
    {
        var timeComparision = Timestamp.CompareTo(other.Timestamp);
        if (0 != timeComparision) return timeComparision;

        return _rc.CompareTo(other._rc);
    }

    public void Reset()
    {
        if (IsEmpty) return;

        Timestamp = DateTimeHelper.Now(Timestamp.Kind);
        _rc.Reset();
    }

    public DateTime Timestamp { get; private set; }

    public override string ToString()
    {
        return $"{_rc}, TimeStamp: {Timestamp}";
    }
}

