namespace Foundation;

using System.Diagnostics;
using System.Runtime.Serialization;

//TODO: implement IComparable and refactore the rest.
[DebuggerDisplay("{Start}-{End}, Duration={Duration}")]
[Serializable]
public struct PeriodOfHours : ISerializable
{
    public enum Direction
    {
        Forward,
        Backward
    }

    public static readonly PeriodOfHours Empty;

    private readonly int _hashCode;

    public PeriodOfHours(TimeOnly start, TimeOnly end)
    {
        if (start > end) throw new ArgumentOutOfRangeException($"{start} must be smaller than {end}");

        Start = start;
        End = end;
        _hashCode = HashCode.FromObject(start, end);
    }

    public PeriodOfHours(SerializationInfo info, StreamingContext context)
    {
        Start = info.GetValue(nameof(Start), typeof(TimeOnly)) is TimeOnly start ? start : default;
        End = info.GetValue(nameof(End), typeof(TimeOnly)) is TimeOnly end ? end : start;

        _hashCode = HashCode.FromObject(Start, End);
    }

    public static bool operator ==(PeriodOfHours lhs, PeriodOfHours rhs) => lhs.Equals(rhs);

    public static bool operator !=(PeriodOfHours lhs, PeriodOfHours rhs) => !lhs.Equals(rhs);

    public static PeriodOfHours operator +(PeriodOfHours lhs, TimeSpan rhs) => New(lhs.Start, lhs.End.Add(rhs));

    public static bool operator <(PeriodOfHours lhs, PeriodOfHours rhs)
    {
        if (lhs.IsEmpty) return false;
        if (rhs.IsEmpty) return true;

        if (lhs.Start < rhs.Start) return true;
        if (lhs.Start > rhs.Start) return false;
        return lhs.End < rhs.End;
    }

    public static bool operator <=(PeriodOfHours lhs, PeriodOfHours rhs)
    {
        return (lhs < rhs) || lhs.Equals(rhs);
    }

    public static bool operator >(PeriodOfHours lhs, PeriodOfHours rhs)
    {
        return !(lhs <= rhs);
    }

    public static bool operator >=(PeriodOfHours lhs, PeriodOfHours rhs)
    {
        return !(lhs < rhs);
    }

    public TimeSpan Duration => End - Start;

    public TimeOnly End { get; }

    public override bool Equals(object? obj) => obj is PeriodOfHours other && Equals(other);

    public bool Equals(PeriodOfHours other)
    {
        return Start.Equals(other.Start) && End.Equals(other.End);
    }

    public override int GetHashCode() => _hashCode;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(Start), Start);
        info.AddValue(nameof(End), End);
    }

    public bool IsEmpty => 0 == _hashCode;

    /// <summary>
    /// Creates a new DayPeriod-Object.
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static PeriodOfHours New(TimeOnly start, TimeOnly end)
    {
        if (start > end) throw new ArgumentException($"{start} must be smaller than {end}");

        return new PeriodOfHours(start, end);
    }

    /// <summary>
    /// Creates a new DayPeriod-Object.
    /// </summary>
    /// <param name="time">The offset of the DayPeriod.</param>
    /// <param name="duration">The duration of the DayPeriod</param>
    /// <param name="direction">The direction which is used for calculation.
    /// If it is Forward then Start is dateTime and End is dateTime + duration.
    /// If it is Backward then End is dateTime and Start is dateTime - duration.</param>
    /// <returns></returns>
    public static PeriodOfHours New(TimeOnly time, TimeSpan duration, Direction direction = Direction.Forward)
    {
        if (Direction.Forward == direction)
            return new PeriodOfHours(time, time.Add(duration));

        return new PeriodOfHours(time.Subtract(duration), time);
    }

    public static PeriodOfHours Parse(string s)
    {
        var splitted = s.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
        if (2 != splitted.Length) throw new ArgumentException("invalid format");

        var start = TimeOnly.Parse(splitted[0]);
        var end = TimeOnly.Parse(splitted[1]);

        return new PeriodOfHours(start, end);
    }

    public TimeOnly Start { get; }

    public override string ToString() => $"{Start}-{End}";
}

