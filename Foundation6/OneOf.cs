namespace Foundation;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// Types are mutually exclusive. Just one of the types is set.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
public class OneOf<T1, T2>
{
    protected OneOf()
    {
    }

    public OneOf(T1 t1)
    {
        t1.ThrowIfNull();
        Item1 = Opt.Some(t1);

        OrdinalIndex = 1;
    }

    public OneOf(T2 t2)
    {
        t2.ThrowIfNull();
        Item2 = Opt.Some(t2);

        OrdinalIndex = 2;
    }

    public Opt<T1> Item1 { get; }
    public Opt<T2> Item2 { get; }

    public int OrdinalIndex { get; protected set; }

    public virtual bool Try<T>([MaybeNullWhen(false)] out T? value)
    {
        if (0 == OrdinalIndex)
        {
            value = default;
            return false;
        }

        Opt<T> setValue<TItem>(TItem item)
        {
            return item is T t ? Opt.Some(t) : Opt.None<T>();
        }

        var foundValue = OrdinalIndex switch
        {
            1 => setValue(Item1.ValueOrThrow()),
            2 => setValue(Item2.ValueOrThrow()),
            _ => Opt.None<T>()
        };

        value = foundValue.IsSome ? foundValue.ValueOrThrow() : default;
        return foundValue.IsSome;
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
    protected OneOf()
    {
    }

    public OneOf(T1 t1) : base(t1)
    {
    }

    public OneOf(T2 t2) : base(t2)
    {
    }

    public OneOf(T3 t3)
    {
        t3.ThrowIfNull();
        Item3 = Opt.Some(t3);

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
            if (Item3.ValueOrThrow() is T t)
            {
                value = t;
                return true;
            }
            value = default;
            return false;
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
    protected OneOf()
    {
    }

    public OneOf(T1 t1) : base(t1)
    {
    }

    public OneOf(T2 t2) : base(t2)
    {
    }

    public OneOf(T3 t3) : base(t3)
    {
    }

    public OneOf(T4 t4)
    {
        t4.ThrowIfNull();
        Item4 = Opt.Some(t4);

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
            if (Item4.ValueOrThrow() is T t)
            {
                value = t;
                return true;
            }
            value = default;
            return false;
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
    public OneOf(T5 t5)
    {
        t5.ThrowIfNull();
        Item5 = Opt.Some(t5);

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
            if (Item5.ValueOrThrow() is T t)
            {
                value = t;
                return true;
            }
            value = default;
            return false;
        }
        return base.Try(out value);
    }
}
