namespace Foundation;

public abstract class DisparateTupleBase<TTuple> : IEquatable<DisparateTupleBase<TTuple>>
    where TTuple : notnull
{
    protected DisparateTupleBase(TTuple tuple)
    {
        Tuple = tuple.ThrowIfNull();
    }

    public override bool Equals(object? obj) => Equals(obj as DisparateTupleBase<TTuple>);

    public bool Equals(DisparateTupleBase<TTuple>? other)
    {
        return null != other && Tuple.Equals(other);
    }

    public abstract Option<T> Get<T>();

    public override int GetHashCode() => Tuple.GetHashCode();

    public override string ToString() => $"{Tuple}";

    protected TTuple Tuple { get; }
}

public class DisparateTuple<T1> : DisparateTupleBase<Tuple<T1>>
{
    public DisparateTuple(T1 value) : base(System.Tuple.Create(value))
    {
    }

    public override Option<T> Get<T>()
    {
        if (Tuple.Item1 is T item1) return Option.Some(item1);

        return Option.None<T>();
    }
}

public class DisparateTuple<T1, T2> : DisparateTupleBase<Tuple<T1, T2>>
{
    public DisparateTuple(T1 item1, T2 item2) : base(System.Tuple.Create(item1, item2))
    {
        item1.ThrowIfNull();
        item2.ThrowIfNull();

        if (!TypeHelper.AreAllDifferent(typeof(T1), typeof(T2))) throw new ArgumentException("types of input arguments must be different");
    }

    public override Option<T> Get<T>()
    {
        if (Tuple.Item1 is T item1) return Option.Some(item1);
        if (Tuple.Item2 is T item2) return Option.Some(item2);

        return Option.None<T>();
    }
}

public class DisparateTuple<T1, T2, T3> : DisparateTupleBase<Tuple<T1, T2, T3>>
{
    public DisparateTuple(T1 item1, T2 item2, T3 item3) : base(System.Tuple.Create(item1, item2, item3))
    {
        item1.ThrowIfNull();
        item2.ThrowIfNull();
        item3.ThrowIfNull();

        if (!TypeHelper.AreAllDifferent(typeof(T1), typeof(T2), typeof(T3))) throw new ArgumentException("types of input arguments must be different");
    }

    public override Option<T> Get<T>()
    {
        if (Tuple.Item1 is T item1) return Option.Some(item1);
        if (Tuple.Item2 is T item2) return Option.Some(item2);
        if (Tuple.Item3 is T item3) return Option.Some(item3);

        return Option.None<T>();
    }
}

public class DisparateTuple<T1, T2, T3, T4> : DisparateTupleBase<Tuple<T1, T2, T3, T4>>
{
    public DisparateTuple(T1 item1, T2 item2, T3 item3, T4 item4) : base(System.Tuple.Create(item1, item2, item3, item4))
    {
        item1.ThrowIfNull();
        item2.ThrowIfNull();
        item3.ThrowIfNull();
        item4.ThrowIfNull();

        if (!TypeHelper.AreAllDifferent(typeof(T1), typeof(T2), typeof(T3), typeof(T4))) throw new ArgumentException("types of input arguments must be different");
    }

    public override Option<T> Get<T>()
    {
        if (Tuple.Item1 is T item1) return Option.Some(item1);
        if (Tuple.Item2 is T item2) return Option.Some(item2);
        if (Tuple.Item3 is T item3) return Option.Some(item3);
        if (Tuple.Item4 is T item4) return Option.Some(item4);

        return Option.None<T>();
    }
}

public class DisparateTuple<T1, T2, T3, T4, T5> : DisparateTupleBase<Tuple<T1, T2, T3, T4, T5>>
{
    public DisparateTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5) : base(System.Tuple.Create(item1, item2, item3, item4, item5))
    {
        item1.ThrowIfNull();
        item2.ThrowIfNull();
        item3.ThrowIfNull();
        item4.ThrowIfNull();
        item5.ThrowIfNull();

        if (!TypeHelper.AreAllDifferent(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5))) throw new ArgumentException("types of input arguments must be different");
    }

    public override Option<T> Get<T>()
    {
        if (Tuple.Item1 is T item1) return Option.Some(item1);
        if (Tuple.Item2 is T item2) return Option.Some(item2);
        if (Tuple.Item3 is T item3) return Option.Some(item3);
        if (Tuple.Item4 is T item4) return Option.Some(item4);
        if (Tuple.Item5 is T item5) return Option.Some(item5);

        return Option.None<T>();
    }
}

public class DisparateTuple<T1, T2, T3, T4, T5, T6> : DisparateTupleBase<Tuple<T1, T2, T3, T4, T5, T6>>
{
    public DisparateTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6) : base(System.Tuple.Create(item1, item2, item3, item4, item5, item6))
    {
        item1.ThrowIfNull();
        item2.ThrowIfNull();
        item3.ThrowIfNull();
        item4.ThrowIfNull();
        item5.ThrowIfNull();
        item6.ThrowIfNull();

        if (!TypeHelper.AreAllDifferent(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6))) throw new ArgumentException("types of input arguments must be different");
    }

    public override Option<T> Get<T>()
    {
        if (Tuple.Item1 is T item1) return Option.Some(item1);
        if (Tuple.Item2 is T item2) return Option.Some(item2);
        if (Tuple.Item3 is T item3) return Option.Some(item3);
        if (Tuple.Item4 is T item4) return Option.Some(item4);
        if (Tuple.Item5 is T item5) return Option.Some(item5);
        if (Tuple.Item6 is T item6) return Option.Some(item6);

        return Option.None<T>();
    }
}

public class DisparateTuple<T1, T2, T3, T4, T5, T6, T7> : DisparateTupleBase<Tuple<T1, T2, T3, T4, T5, T6, T7>>
{
    public DisparateTuple(T1 item1, T2 item2, T3 item3, T4 item4, T5 item5, T6 item6, T7 item7)
        : base(System.Tuple.Create(item1, item2, item3, item4, item5, item6, item7))
    {
        item1.ThrowIfNull();
        item2.ThrowIfNull();
        item3.ThrowIfNull();
        item4.ThrowIfNull();
        item5.ThrowIfNull();
        item6.ThrowIfNull();
        item7.ThrowIfNull();

        if (!TypeHelper.AreAllDifferent(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7))) throw new ArgumentException("types of input arguments must be different");
    }

    public override Option<T> Get<T>()
    {
        if (Tuple.Item1 is T item1) return Option.Some(item1);
        if (Tuple.Item2 is T item2) return Option.Some(item2);
        if (Tuple.Item3 is T item3) return Option.Some(item3);
        if (Tuple.Item4 is T item4) return Option.Some(item4);
        if (Tuple.Item5 is T item5) return Option.Some(item5);
        if (Tuple.Item6 is T item6) return Option.Some(item6);
        if (Tuple.Item7 is T item7) return Option.Some(item7);

        return Option.None<T>();
    }
}