namespace Foundation;

using Foundation.Collections.Generic;

public abstract class TimeDef : IEquatable<TimeDef>
{
    private int _hashCode;

    public static bool operator ==(TimeDef? lhs, TimeDef? rhs)
    {
        if (lhs is null) return rhs is null;

        return lhs.Equals(rhs);
    }

    public static bool operator !=(TimeDef? lhs, TimeDef? rhs)
    {
        return !(lhs == rhs);
    }

    protected virtual int CreateHashCode()
    {
        return 0;
    }


    public override bool Equals(object? obj) => obj is TimeDef other && Equals(other);

    public abstract bool Equals(TimeDef? other);

    public override int GetHashCode()
    {
        if (0 == _hashCode)
            _hashCode = CreateHashCode();

        return _hashCode;
    }

    public sealed class And : TimeDef
    {
        public And(TimeDef lhs, TimeDef rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        protected override int CreateHashCode() => System.HashCode.Combine(nameof(And), Lhs, Rhs);

        public override bool Equals(TimeDef? other)
        {
            return other is And @and && Lhs.Equals(@and.Lhs) && Rhs.Equals(@and.Rhs);
        }

        public TimeDef Lhs { get; }

        public TimeDef Rhs { get; }

        public override string ToString() => $"{nameof(And)}({Lhs}, {Rhs})";
    }

    public sealed class DateTimeSpan : TimeDef
    {
        public DateTimeSpan(DateTime from, DateTime to)
        {
            From = from;
            To = to;
        }

        public override bool Equals(TimeDef? other)
        {
            return other is DateTimeSpan ts && From.Equals(ts.From) && To.Equals(ts.To);
        }

        public DateTime From { get; }

        protected override int CreateHashCode() => System.HashCode.Combine(nameof(DateTimeSpan), From, To);

        public DateTime To { get; }

        public override string ToString() => $"{nameof(DateTimeSpan)}({nameof(From)}: {From}, {nameof(To)}: {To})";
    }

    public sealed class Day : TimeDef
    {
        private readonly int[] _values;

        public Day(params int[] dayOfMonth)
        {
            _values = dayOfMonth;
        }

        protected override int CreateHashCode()
        {
            var hb = HashCode.CreateBuilder();
            hb.AddObject(nameof(Day));
            hb.AddObjects(Values);

            return hb.GetHashCode();
        }

        public override bool Equals(TimeDef? other)
        {
            return other is Day day && Values.IsEqualTo(day.Values);
        }

        public override string ToString() => $"{nameof(Day)}({string.Join(", ", _values)})";

        public IEnumerable<int> Values => _values;
    }

    public sealed class Days : TimeDef
    {
        public Days(int quantity)
        {
            Value = quantity;
        }

        protected override int CreateHashCode() => System.HashCode.Combine(nameof(Days), Value);

        public override bool Equals(TimeDef? other) => other is Days days && Value.Equals(days.Value);

        public override string ToString() => $"{nameof(Days)}({Value})";

        public int Value { get; }
    }

    public sealed class Difference : TimeDef
    {
        public Difference(TimeDef lhs, TimeDef rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        protected override int CreateHashCode() => System.HashCode.Combine(nameof(Difference), Lhs, Rhs);
        public override bool Equals(TimeDef? other)
        {
            return other is Difference diff && Lhs.Equals(diff.Lhs) && Rhs.Equals(diff.Rhs);
        }

        public TimeDef Lhs { get; }

        public TimeDef Rhs { get; }

        public override string ToString() => $"{nameof(Difference)}({Lhs}, {Rhs})";
    }

    public sealed class Hour : TimeDef
    {
        private readonly int[] _values;

        public Hour(params int[] hourOfDay)
        {
            _values = hourOfDay;
        }

        protected override int CreateHashCode()
        {
            var hb = HashCode.CreateBuilder();
            hb.AddObject(nameof(Hour));
            hb.AddObjects(Values);

            return hb.GetHashCode();
        }
        public override bool Equals(TimeDef? other)
        {
            return other is Hour hour && Values.IsEqualTo(hour.Values);
        }

        public override string ToString() => $"{nameof(Hour)}({string.Join(", ", _values)})";

        public IEnumerable<int> Values => _values;
    }

    public sealed class Hours : TimeDef
    {
        private readonly int _value;

        public Hours(int quantity)
        {
            _value = quantity;
        }

        protected override int CreateHashCode() => System.HashCode.Combine(nameof(Hours), Value);
        public override bool Equals(TimeDef? other)
        {
            return other is Hours hours && Value.Equals(hours.Value);
        }

        public override string ToString() => $"{nameof(Hours)}({Value})";

        public int Value => _value;
    }

    public sealed class Minute : TimeDef
    {
        private readonly int[] _values;

        public Minute(params int[] minuteOfHour)
        {
            _values = minuteOfHour;
        }

        protected override int CreateHashCode()
        {
            var hb = HashCode.CreateBuilder();
            hb.AddObject(nameof(Minute));
            hb.AddObjects(Values);

            return hb.GetHashCode();
        }
        public override bool Equals(TimeDef? other)
        {
            return other is Minute minute && Values.IsEqualTo(minute.Values);
        }

        public override string ToString() => $"{nameof(Minute)}({string.Join(", ", _values)})";

        public IEnumerable<int> Values => _values;
    }

    public sealed class Minutes : TimeDef
    {
        public Minutes(int quantity)
        {
            Value = quantity;
        }

        protected override int CreateHashCode() => System.HashCode.Combine(nameof(Minutes), Value);
        public override bool Equals(TimeDef? other)
        {
            return other is Minutes minutes && Value.Equals(minutes.Value);
        }

        public override string ToString() => $"{nameof(Minutes)}({Value})";

        public int Value { get; }
    }

    public sealed class Month : TimeDef
    {
        private readonly Foundation.Month[] _values;

        public Month(params Foundation.Month[] monthOfYear)
        {
            _values = monthOfYear;
        }

        protected override int CreateHashCode()
        {
            var hb = HashCode.CreateBuilder();
            hb.AddObject(nameof(Month));
            hb.AddObjects(Values);

            return hb.GetHashCode();
        }
        public override bool Equals(TimeDef? other)
        {
            return other is Month month && Values.IsEqualTo(month.Values);
        }

        public override string ToString() => $"{nameof(Month)}({string.Join(", ", _values)})";

        public IEnumerable<Foundation.Month> Values => _values;
    }

    public sealed class Months : TimeDef
    {
        public Months(int quantity)
        {
            Value = quantity;
        }

        protected override int CreateHashCode() => System.HashCode.Combine(nameof(Months), Value);
        public override bool Equals(TimeDef? other)
        {
            return other is Months months && Value.Equals(months.Value);
        }

        public override string ToString() => $"{nameof(Months)}({Value})";

        public int Value { get; }
    }

    public sealed class Not : TimeDef
    {
        public Not(TimeDef timeDef)
        {
            TimeDef = timeDef;
        }

        protected override int CreateHashCode() => System.HashCode.Combine(nameof(Not), TimeDef);
        public override bool Equals(TimeDef? other)
        {
            return other is Not @not && TimeDef.Equals(@not.TimeDef);
        }

        public TimeDef TimeDef { get; }

        public override string ToString() => $"{nameof(Not)}({TimeDef})";
    }

    public sealed class Or : TimeDef
    {
        public Or(TimeDef lhs, TimeDef rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        protected override int CreateHashCode() => System.HashCode.Combine(nameof(Or), Lhs, Rhs);

        public override bool Equals(TimeDef? other)
        {
            return other is Or @or && Lhs.Equals(@or.Lhs) && Rhs.Equals(@or.Rhs);
        }
        public TimeDef Lhs { get; }

        public TimeDef Rhs { get; }

        public override string ToString() => $"{nameof(Or)}({Lhs}, {Rhs})";
    }

    public sealed class Timespan : TimeDef
    {
        public Timespan(TimeOnly from, TimeOnly to)
        {
            From = from;
            To = to;
        }

        public TimeOnly From { get; }

        protected override int CreateHashCode() => System.HashCode.Combine(nameof(Timespan), From, To);

        public override bool Equals(TimeDef? other)
        {
            return other is Timespan ts && From.Equals(ts.From) && To.Equals(ts.To);
        }

        public TimeOnly To { get; }

        public override string ToString() => $"{nameof(Timespan)}({nameof(From)}: {From}, {nameof(To)}: {To})";
    }

    public sealed class Union : TimeDef
    {
        public Union(TimeDef lhs, TimeDef rhs)
        {
            Lhs = lhs;
            Rhs = rhs;
        }

        protected override int CreateHashCode() => System.HashCode.Combine(nameof(Union), Lhs, Rhs);

        public override bool Equals(TimeDef? other)
        {
            return other is Union union && Lhs.Equals(union.Lhs) && Rhs.Equals(union.Rhs);
        }

        public TimeDef Lhs { get; }

        public TimeDef Rhs { get; }

        public override string ToString() => $"{nameof(Union)}({Lhs}, {Rhs})";
    }

    public sealed class Weekday : TimeDef
    {
        private readonly DayOfWeek[] _values;

        public Weekday(params DayOfWeek[] weekday)
        {
            _values = weekday;
        }

        protected override int CreateHashCode()
        {
            var hb = HashCode.CreateBuilder();
            hb.AddObject(nameof(Weekday));
            hb.AddObjects(Values);

            return hb.GetHashCode();
        }
        public override bool Equals(TimeDef? other)
        {
            return other is Weekday weekday && Values.IsEqualTo(weekday.Values);
        }
        public override string ToString() => $"{nameof(Weekday)}({string.Join(", ", _values)})";

        public IEnumerable<DayOfWeek> Values => _values;
    }

    public sealed class WeekOfMonth : TimeDef
    {
        private readonly int[] _values;

        public WeekOfMonth(DayOfWeek start, params int[] weekOfMonth)
        {
            _values = weekOfMonth;
            WeekStartsWith = start;
        }

        protected override int CreateHashCode()
        {
            var hb = HashCode.CreateBuilder();
            hb.AddObject(nameof(WeekOfMonth));
            hb.AddObject(WeekStartsWith);
            hb.AddObjects(Values);

            return hb.GetHashCode();
        }
        public override bool Equals(TimeDef? other)
        {
            return other is WeekOfMonth weekOfMonth
                && WeekStartsWith.Equals(weekOfMonth.WeekStartsWith)
                && Values.IsEqualTo(weekOfMonth.Values);
        }

        public override string ToString() => $"{nameof(WeekOfMonth)}({WeekStartsWith}, {string.Join(", ", _values)})";

        public IEnumerable<int> Values => _values;

        public DayOfWeek WeekStartsWith { get; }
    }

    public sealed class Weeks : TimeDef
    {
        private readonly int _value;

        public Weeks(int quantity, DayOfWeek weekStartsWith)
        {
            _value = quantity;
            WeekStartsWith = weekStartsWith;
        }

        protected override int CreateHashCode() => System.HashCode.Combine(nameof(Weeks), Value, WeekStartsWith);
        public override bool Equals(TimeDef? other)
        {
            return other is Weeks weeks
                && Value.Equals(weeks.Value)
                && WeekStartsWith.Equals(weeks.WeekStartsWith);
        }

        public override string ToString() => $"{nameof(Weeks)}({Value})";

        public int Value => _value;

        public DayOfWeek WeekStartsWith { get; }
    }

    public sealed class Year : TimeDef
    {
        private readonly int[] _values;

        public Year(params int[] year)
        {
            _values = year;
        }

        protected override int CreateHashCode()
        {
            var hb = HashCode.CreateBuilder();
            hb.AddObject(nameof(Year));
            hb.AddObjects(Values);

            return hb.GetHashCode();
        }
        public override bool Equals(TimeDef? other)
        {
            return other is Year year && Values.IsEqualTo(year.Values);
        }

        public override string ToString() => $"{nameof(Year)}({string.Join(", ", _values)})";

        public IEnumerable<int> Values => _values;
    }

    public sealed class Years : TimeDef
    {
        public Years(int quantity)
        {
            Value = quantity;
        }

        protected override int CreateHashCode() => System.HashCode.Combine(nameof(Years), Value);
        public override bool Equals(TimeDef? other)
        {
            return other is Years years && Value.Equals(years.Value);
        }
        public override string ToString() => $"{nameof(Years)}({Value})";

        public int Value { get; }
    }
}

