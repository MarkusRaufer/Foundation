namespace Foundation;

/// <summary>
/// This class represent disjunct alternatives of types. Types are mutually exclusive. Just one of the types is set.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
public class OneOf<T1, T2> : IEquatable<OneOf<T1, T2>>
{
    protected OneOf()
    {
    }

    public OneOf(T1 t1)
    {
        t1.ThrowIfNull();
        Item1 = Opt.Some(t1);

        SelectedType = typeof(T1);
        Value = t1!;
    }

    public OneOf(T2 t2)
    {
        t2.ThrowIfNull();
        Item2 = Opt.Some(t2);
        
        SelectedType = typeof(T2);
        Value = t2!;
    }

    public override bool Equals(object? obj) => Equals(obj as OneOf<T1, T2>);

    public bool Equals(OneOf<T1, T2>? other)
    {
        return null != other 
            && SelectedType!.Equals(other.SelectedType)
            && Value!.Equals(other.Value);
    }

    public override int GetHashCode() => System.HashCode.Combine(SelectedType, Value);
    
    public void Invoke(Action<T1> action)
    {
        if(Value is T1 value) action(value);
    }

    public void Invoke(Action<T2> action)
    {
        if (Value is T2 value) action(value);
    }

    public Opt<T1> Item1 { get; }

    public Opt<T2> Item2 { get; }

    public TResult Match<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2)
    {
        return Value switch
        {
            T1 t1 => onT1(t1),
            T2 t2 => onT2(t2),
            _ => throw new NotSupportedException()
        };
    }

    public virtual bool TryGet<T>(out T? value)
    {
        if(Item1.TryGet(out T1? t1) && t1 is T t1Value)
        {
            value = t1Value;
            return true;
        }
        if(Item2.TryGet(out T2? t2) && t2 is T t2Value)
        {
            value = t2Value;
            return true;
        }

        value = default;
        return false;
    }

    public Type? SelectedType { get; protected set; }

    public override string ToString() => SelectedType!.ToString();

    protected object? Value { get; set; }
}

/// <summary>
/// This class represent disjunct alternatives of types. Types are mutually exclusive. Just one of the types is set.
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

        SelectedType = typeof(T3);
        Value = t3!;
    }

    public void Invoke(Action<T3> action)
    {
        if (Value is T3 value) action(value);
    }

    public Opt<T3> Item3 { get; }

    public TResult Match<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2,
        Func<T3, TResult> onT3)
    {
        return Value switch
        {
            T1 t1 => onT1(t1),
            T2 t2 => onT2(t2),
            T3 t3 => onT3(t3),
            _ => throw new NotSupportedException()
        };
    }

    public override bool TryGet<T>(out T? value) where T : default
    {
        if (Item3.TryGet(out T3? t3) && t3 is T t)
        {
            value = t;
            return true;
        }
        return base.TryGet(out value);
    }
}

/// <summary>
/// This class represent disjunct alternatives of types. Types are mutually exclusive. Just one of the types is set.
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

        SelectedType = typeof(T4);
        Value = t4!;
    }

    public void Invoke(Action<T4> action)
    {
        if (Value is T4 value) action(value);
    }

    public Opt<T4> Item4 { get; }

    public TResult Match<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2,
        Func<T3, TResult> onT3,
        Func<T4, TResult> onT4)
    {
        return Value switch
        {
            T1 t1 => onT1(t1),
            T2 t2 => onT2(t2),
            T3 t3 => onT3(t3),
            T4 t4 => onT4(t4),
            _ => throw new NotSupportedException()
        };
    }

    public override bool TryGet<T>(out T? value) where T : default
    {
        if (Item4.TryGet(out T4? t4) && t4 is T t)
        {
            value = t;
            return true;
        }
        return base.TryGet(out value);
    }
}

/// <summary>
/// This class represent disjunct alternatives of types. Types are mutually exclusive. Just one of the types is set.
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

        SelectedType = typeof(T5);
        Value = t5!;
    }

    public void Invoke(Action<T5> action)
    {
        if (Value is T5 value) action(value);
    }

    public Opt<T5> Item5 { get; }

    public TResult Match<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2,
        Func<T3, TResult> onT3,
        Func<T4, TResult> onT4,
        Func<T5, TResult> onT5)
    {
        return Value switch
        {
            T1 t1 => onT1(t1),
            T2 t2 => onT2(t2),
            T3 t3 => onT3(t3),
            T4 t4 => onT4(t4),
            T5 t5 => onT5(t5),
            _ => throw new NotSupportedException()
        };
    }

    public override bool TryGet<T>(out T? value) where T : default
    {
        if (Item5.TryGet(out T5? t5) && t5 is T t)
        {
            value = t;
            return true;
        }
        return base.TryGet(out value);
    }
}
