namespace Foundation;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Types are mutually exclusive. Just one of the types is set.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
public class OneOf<T1, T2>
{
    public OneOf(T1 t1) : this(Opt.Some(t1), Opt.None<T2>())
    {
        t1.ThrowIfNull(nameof(t1));
        OrdinalIndex = 1;
    }

    public OneOf(T2 t2) : this(Opt.None<T1>(), Opt.Some(t2))
    {
        t2.ThrowIfNull(nameof(t2));
        OrdinalIndex = 2;
    }

    public OneOf(Opt<T1> t1, Opt<T2> t2)
    {
        Item1 = t1;
        Item2 = t2;
    }

    public Opt<T1> Item1 { get; }
    public Opt<T2> Item2 { get; }

    public int OrdinalIndex { get; protected set; }

    public virtual bool Try<T>([MaybeNullWhen(false)] out T value)
    {
        if (0 == OrdinalIndex)
        {
            value = default;
            return false;
        }

        T? setValue<TItem>(TItem item)
        {
            return item is T t ? t : default;
        }

        value = OrdinalIndex switch
        {
            1 => setValue(Item1.ValueOrThrow()),
            2 => setValue(Item2.ValueOrThrow()),
            _ => default
        };

        return null != value;
    }
}

/// <summary>
/// Types are mutually exclusive. Just one of the types is set.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
public class OneOf<T1, T2, T3> : OneOf<T1, T2>
{
    public OneOf(T3 t3) : this(Opt.None<T1>(), Opt.None<T2>(), Opt.Some(t3))
    {
        t3.ThrowIfNull(nameof(t3));
    }

    public OneOf(Opt<T1> t1, Opt<T2> t2, Opt<T3> t3) : base(t1, t2)
    {
        Item3 = t3;
        OrdinalIndex = 3;
    }

    public Opt<T3> Item3 { get; }

    public override bool Try<T>([MaybeNullWhen(false)] out T value)
    {
        if (0 == OrdinalIndex)
        {
            value = default;
            return false;
        }

        if (3 == OrdinalIndex)
        {
            value = Item3.ValueOrThrow() is T t ? t : default;
            return null != value;
        }
        return base.Try(out value);
    }
}

/// <summary>
/// Types are mutually exclusive. Just one of the types is set.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
public class OneOf<T1, T2, T3, T4> : OneOf<T1, T2, T3>
{
    public OneOf(T4 t4) : this(Opt.None<T1>(), Opt.None<T2>(), Opt.None<T3>(), Opt.Some(t4))
    {
        t4.ThrowIfNull(nameof(t4));
    }

    public OneOf(Opt<T1> t1, Opt<T2> t2, Opt<T3> t3, Opt<T4> t4) : base(t1, t2, t3)
    {
        Item4 = t4;
        OrdinalIndex = 4;
    }

    public Opt<T4> Item4 { get; }

    public override bool Try<T>([MaybeNullWhen(false)] out T value)
    {
        if (0 == OrdinalIndex)
        {
            value = default;
            return false;
        }

        if (4 == OrdinalIndex)
        {
            value = Item4.ValueOrThrow() is T t ? t : default;
            return null != value;
        }
        return base.Try(out value);
    }
}

/// <summary>
/// Types are mutually exclusive. Just one of the types is set.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
/// <typeparam name="T4"></typeparam>
/// <typeparam name="T5"></typeparam>
public class OneOf<T1, T2, T3, T4, T5> : OneOf<T1, T2, T3, T4>
{
    public OneOf(T5 t5) : this(Opt.None<T1>(), Opt.None<T2>(), Opt.None<T3>(), Opt.None<T4>(), Opt.Some(t5))
    {
        t5.ThrowIfNull(nameof(t5));
    }

    public OneOf(Opt<T1> t1, Opt<T2> t2, Opt<T3> t3, Opt<T4> t4, Opt<T5> t5) : base(t1, t2, t3, t4)
    {
        Item5 = t5;
        OrdinalIndex = 5;
    }

    public Opt<T5> Item5 { get; }

    public override bool Try<T>([MaybeNullWhen(false)] out T value)
    {
        if (0 == OrdinalIndex)
        {
            value = default;
            return false;
        }

        if (5 == OrdinalIndex)
        {
            value = Item5.ValueOrThrow() is T t ? t : default;
            return null != value;
        }
        return base.Try(out value);
    }
}
