using System.Data.Common;

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

    /// <summary>
    /// Construcor that selects the specific type.
    /// </summary>
    /// <param name="t1"></param>
    public OneOf(T1 t1)
    {
        Item1 = t1.ThrowIfNull();
        ItemIndex = 1;

        SelectedType = typeof(T1);
        HashCode = System.HashCode.Combine(SelectedType, t1);
    }

    /// <summary>
    /// Construcor that selects the specific type.
    /// </summary>
    /// <param name="t2"></param>
    public OneOf(T2 t2)
    {
        Item2 = t2.ThrowIfNull();
        ItemIndex = 2;

        SelectedType = typeof(T2);
        HashCode = System.HashCode.Combine(SelectedType, t2);
    }

    public static implicit operator OneOf<T1, T2>(T1 value) => new(value);

    public static implicit operator OneOf<T1, T2>(T2 value) => new(value);

    /// <summary>
    /// Executes either onT1 or onT2 depending on Value is of type T1 or T2.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public TResult Either<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2)
    {
        return ItemIndex switch
        {
            1 => Item1 is T1 t1
                    ? onT1(t1)
                    : throw new ArgumentException($"{typeof(T1).Name}"),
            2 => Item2 is T2 t2
                    ? onT2(t2)
                    : throw new ArgumentException($"{typeof(T2).Name}"),
            _ => throw new ArgumentException($"ItemIndex = {ItemIndex}")
        };
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj) => Equals(obj as OneOf<T1, T2>);

    /// <inheritdoc/>
    public bool Equals(OneOf<T1, T2>? other)
    {
        if (null == other || ItemIndex != other.ItemIndex) return false;

        return ItemIndex switch
        {
            1 => Item1.EqualsNullable(other.Item1),
            2 => Item2.EqualsNullable(other.Item2),
            _ => false
        };
    }

    /// <inheritdoc/>
    public override int GetHashCode() => HashCode;

    protected int HashCode { get; set; }

    /// <summary>
    /// Invokes action if Value is of type T1.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Invoke(Action<T1> action)
    {
        if (TryGet(out T1? value))
        {
            action(value!);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes action if Value is of type T2.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Invoke(Action<T2> action)
    {
        if (TryGet(out T2? value))
        {
            action(value!);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes an action only if the selected type matches the specific type.
    /// </summary>
    /// <param name="onT1">Is called if the selected type is of type T1.</param>
    /// <param name="onT2">Is called if the selected type is of type T2.</param>
    /// <returns></returns>
    public bool Invoke(
        Action<T1> onT1,
        Action<T2> onT2)
    {
        return Invoke(onT1) || Invoke(onT2);
    }

    /// <summary>
    /// Returns true is the selected type if of type T.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public bool IsOfType<T>() => SelectedType == typeof(T);

    /// <summary>
    /// One of the selectable alternative.
    /// </summary>
    protected T1? Item1 { get; }

    /// <summary>
    /// One of the selectable alternative.
    /// </summary>
    protected T2? Item2 { get; }

    /// <summary>
    /// Returns the index of the selected item.
    /// 0: nothing selected
    /// 1: Item1 selected
    /// 2: Item2 selected
    /// </summary>
    public int ItemIndex { get; protected set; }

    public virtual bool TryGet<T>(out T? value)
    {
        switch(ItemIndex)
        {
            case 1:
                if (Item1 is T t1Value)
                {
                    value = t1Value;
                    return true;
                }
                break;
            case 2:
                if (Item2 is T t2Value)
                {
                    value = t2Value;
                    return true;
                }
                break;
        }

        value = default;
        return false;
    }

    public Type? SelectedType { get; protected set; }

    public override string ToString() => SelectedType!.ToString();
}

/// <summary>
/// This class represent disjunct alternatives of types. Types are mutually exclusive. Just one of the types is set.
/// </summary>
/// <typeparam name="T1"></typeparam>
/// <typeparam name="T2"></typeparam>
/// <typeparam name="T3"></typeparam>
public class OneOf<T1, T2, T3> 
    : OneOf<T1, T2>
    , IEquatable<OneOf<T1, T2, T3>>
{
    protected OneOf()
    {
    }

    /// <inheritdoc/>
    public OneOf(T1 t1) : base(t1)
    {
    }

    /// <inheritdoc/>
    public OneOf(T2 t2) : base(t2)
    {
    }

    /// <summary>
    /// Construcor that selects the specific type.
    /// </summary>
    /// <param name="t3"></param>
    public OneOf(T3 t3)
    {
        Item3 = t3.ThrowIfNull();
        ItemIndex = 3;

        SelectedType = typeof(T3);
        HashCode = System.HashCode.Combine(SelectedType, t3);
    }

    public static implicit operator OneOf<T1, T2, T3>(T1 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3>(T2 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3>(T3 value) => new(value);

    /// <summary>
    /// Executes either onT1 or onT2 depending on Value is of type T1, T2 or T3.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public TResult Either<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2,
        Func<T3, TResult> onT3)
    {
        if(3 == ItemIndex)
        {
            if (Item3 is not T3 t3) throw new ArgumentException($"{typeof(T3).Name}");

            return onT3(t3);
        }
        return Either(onT1, onT2);
    }

    public override bool Equals(object? obj) => Equals(obj as OneOf<T1, T2, T3>);

    public bool Equals(OneOf<T1, T2, T3>? other)
    {
        if (base.Equals(other)) return true;

        if (null == other || ItemIndex != other.ItemIndex) return false;

        return Item3.EqualsNullable(other.Item3);
    }

    public override int GetHashCode() => HashCode;

    /// <summary>
    /// Invokes action if Value is of type T3.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Invoke(Action<T3> action)
    {
        if (TryGet(out T3? value))
        {
            action(value!);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes the the action that matches the type of <see cref="Value"/>.
    /// </summary>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <returns></returns>
    public bool Invoke(
        Action<T1> onT1,
        Action<T2> onT2,
        Action<T3> onT3)
    {
        return Invoke(onT1, onT2) || Invoke(onT3);
    }

    /// <summary>
    /// One of the selectable alternative.
    /// </summary>
    protected T3? Item3 { get; }

    public override bool TryGet<T>(out T? value) where T : default
    {
        if (3 == ItemIndex)
        {
            if (Item3 is T t)
            {
                value = t;
                return true;
            }
            value = default;
            return false;
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
public class OneOf<T1, T2, T3, T4>
    : OneOf<T1, T2, T3>
    , IEquatable<OneOf<T1, T2, T3, T4>>
{
    protected OneOf()
    {
    }

    /// <inheritdoc/>
    public OneOf(T1 t1) : base(t1)
    {
    }

    /// <inheritdoc/>
    public OneOf(T2 t2) : base(t2)
    {
    }

    /// <inheritdoc/>
    public OneOf(T3 t3) : base(t3)
    {
    }

    /// <summary>
    /// Construcor that selects the specific type.
    /// </summary>
    /// <param name="t4"></param>
    public OneOf(T4 t4)
    {
        Item4 = t4.ThrowIfNull();
        ItemIndex = 4;

        SelectedType = typeof(T4);
        HashCode = System.HashCode.Combine(SelectedType, t4);
    }

    public static implicit operator OneOf<T1, T2, T3, T4>(T1 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4>(T2 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4>(T3 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4>(T4 value) => new(value);

    /// <summary>
    /// Executes either onT1 or onT2 depending on Value is of type T1, T2, T3 or T4.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException">Throw an exception if it is not of any available type</exception>
    public TResult Either<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2,
        Func<T3, TResult> onT3,
        Func<T4, TResult> onT4)
    {
        if (4 == ItemIndex)
        {
            if (Item4 is not T4 t4) throw new ArgumentException($"{typeof(T4).Name}");

            return onT4(t4);
        }
        return Either(onT1, onT2, onT3);
    }

    public override bool Equals(object? obj) => Equals(obj as OneOf<T1, T2, T3, T4>);

    public bool Equals(OneOf<T1, T2, T3, T4>? other)
    {
        if (base.Equals(other)) return true;

        if (null == other || ItemIndex != other.ItemIndex) return false;

        return Item4.EqualsNullable(other.Item4);
    }

    public override int GetHashCode() => HashCode;

    /// <summary>
    /// Invokes action if Value is of type T4.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Invoke(Action<T4> action)
    {
        if (TryGet(out T4? value))
        {
            action(value!);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes the the action that matches the type of <see cref="Value"/>.
    /// </summary>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <returns></returns>
    public bool Invoke(
        Action<T1> onT1,
        Action<T2> onT2,
        Action<T3> onT3,
        Action<T4> onT4)
    {
        return Invoke(onT1, onT2, onT3) || Invoke(onT4);
    }

    /// <summary>
    /// One of the selectable alternative.
    /// </summary>
    protected T4? Item4 { get; }

    public override bool TryGet<T>(out T? value) where T : default
    {
        if(4 == ItemIndex)
        {
            if (Item4 is T t)
            {
                value = t;
                return true;
            }
            value = default;
            return false;
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
public class OneOf<T1, T2, T3, T4, T5>
    : OneOf<T1, T2, T3, T4>
    , IEquatable<OneOf<T1, T2, T3, T4, T5>>
{
    protected OneOf()
    {
    }

    /// <inheritdoc/>
    public OneOf(T1 t1) : base(t1)
    {
    }

    /// <inheritdoc/>
    public OneOf(T2 t2) : base(t2)
    {
    }

    /// <inheritdoc/>
    public OneOf(T3 t3) : base(t3)
    {
    }

    /// <inheritdoc/>
    public OneOf(T4 t4) : base(t4)
    {
    }

    /// <summary>
    /// Construcor that selects the specific type.
    /// </summary>
    /// <param name="t5"></param>
    public OneOf(T5 t5)
    {
        Item5 = t5.ThrowIfNull();
        ItemIndex = 5;

        SelectedType = typeof(T5);
        HashCode = System.HashCode.Combine(SelectedType, t5);
    }

    public static implicit operator OneOf<T1, T2, T3, T4, T5>(T1 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5>(T2 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5>(T3 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5>(T4 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5>(T5 value) => new(value);

    /// <summary>
    /// Executes either onT1 or onT2 depending on Value is of type T1, T2, T3, T4 or T5.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <param name="onT5"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public TResult Either<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2,
        Func<T3, TResult> onT3,
        Func<T4, TResult> onT4,
        Func<T5, TResult> onT5)
    {
        if (5 == ItemIndex)
        {
            if (Item5 is not T5 t5) throw new ArgumentException($"{typeof(T5).Name}");

            return onT5(t5);
        }
        return Either(onT1, onT2, onT3, onT4);
    }

    public override bool Equals(object? obj) => Equals(obj as OneOf<T1, T2, T3, T4, T5>);

    public bool Equals(OneOf<T1, T2, T3, T4, T5>? other)
    {
        if (base.Equals(other)) return true;

        if (null == other || ItemIndex != other.ItemIndex) return false;

        return Item5.EqualsNullable(other.Item5);
    }

    public override int GetHashCode() => HashCode;

    /// <summary>
    /// Invokes action if Value is of type T5.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Invoke(Action<T5> action)
    {
        if (TryGet(out T5? value))
        {
            action(value!);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes the the action that matches the type of <see cref="Value"/>.
    /// </summary>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <param name="onT5"></param>
    /// <returns></returns>
    public bool Invoke(
        Action<T1> onT1,
        Action<T2> onT2,
        Action<T3> onT3,
        Action<T4> onT4,
        Action<T5> onT5)
    {
        return Invoke(onT1, onT2, onT3, onT4) || Invoke(onT5);
    }

    /// <summary>
    /// One of the selectable alternative.
    /// </summary>
    protected T5? Item5 { get; }

    public override bool TryGet<T>(out T? value) where T : default
    {
        if (5 == ItemIndex)
        {
            if (Item5 is T t)
            {
                value = t;
                return true;
            }
            value = default;
            return false;
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
/// <typeparam name="T6"></typeparam>
public class OneOf<T1, T2, T3, T4, T5, T6>
    : OneOf<T1, T2, T3, T4, T5>
    , IEquatable<OneOf<T1, T2, T3, T4, T5, T6>>
{
    protected OneOf()
    {
    }

    /// <inheritdoc/>
    public OneOf(T1 t1) : base(t1)
    {
    }

    /// <inheritdoc/>
    public OneOf(T2 t2) : base(t2)
    {
    }

    /// <inheritdoc/>
    public OneOf(T3 t3) : base(t3)
    {
    }

    /// <inheritdoc/>
    public OneOf(T4 t4) : base(t4)
    {
    }

    /// <inheritdoc/>
    public OneOf(T5 t5) : base(t5)
    {
    }

    /// <summary>
    /// Construcor that selects the specific type.
    /// </summary>
    /// <param name="t6"></param>
    public OneOf(T6 t6)
    {
        Item6 = t6.ThrowIfNull();
        ItemIndex = 6;

        SelectedType = typeof(T6);
        HashCode = System.HashCode.Combine(SelectedType, t6);
    }

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6>(T1 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6>(T2 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6>(T3 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6>(T4 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6>(T5 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6>(T6 value) => new(value);

    /// <summary>
    /// Executes one of the methods depending on the selected value (type).
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <param name="onT5"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public TResult Either<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2,
        Func<T3, TResult> onT3,
        Func<T4, TResult> onT4,
        Func<T5, TResult> onT5,
        Func<T6, TResult> onT6)
    {
        if (6 == ItemIndex)
        {
            if (Item6 is not T6 t6) throw new ArgumentException($"{typeof(T6).Name}");

            return onT6(t6);
        }
        return Either(onT1, onT2, onT3, onT4, onT5);
    }

    public override bool Equals(object? obj) => Equals(obj as OneOf<T1, T2, T3, T4, T5, T6>);

    public bool Equals(OneOf<T1, T2, T3, T4, T5, T6>? other)
    {
        if (base.Equals(other)) return true;

        if (null == other || ItemIndex != other.ItemIndex) return false;

        return Item6.EqualsNullable(other.Item6);
    }

    public override int GetHashCode() => HashCode;

    /// <summary>
    /// Invokes action if Value is of type T5.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Invoke(Action<T6> action)
    {
        if (TryGet(out T6? value))
        {
            action(value!);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes the the action that matches the type of <see cref="Value"/>.
    /// </summary>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <param name="onT5"></param>
    /// <returns></returns>
    public bool Invoke(
        Action<T1> onT1,
        Action<T2> onT2,
        Action<T3> onT3,
        Action<T4> onT4,
        Action<T5> onT5,
        Action<T6> onT6)
    {
        return Invoke(onT1, onT2, onT3, onT4, onT5) || Invoke(onT6);
    }

    /// <summary>
    /// One of the selectable alternative.
    /// </summary>
    protected T6? Item6 { get; }

    public override bool TryGet<T>(out T? value) where T : default
    {
        if (6 == ItemIndex)
        {
            if (Item6 is T t)
            {
                value = t;
                return true;
            }
            value = default;
            return false;
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
/// <typeparam name="T6"></typeparam>
/// <typeparam name="T7"></typeparam>
public class OneOf<T1, T2, T3, T4, T5, T6, T7>
    : OneOf<T1, T2, T3, T4, T5, T6>
    , IEquatable<OneOf<T1, T2, T3, T4, T5, T6, T7>>
{
    protected OneOf()
    {
    }

    /// <inheritdoc/>
    public OneOf(T1 t1) : base(t1)
    {
    }

    /// <inheritdoc/>
    public OneOf(T2 t2) : base(t2)
    {
    }

    /// <inheritdoc/>
    public OneOf(T3 t3) : base(t3)
    {
    }

    /// <inheritdoc/>
    public OneOf(T4 t4) : base(t4)
    {
    }

    /// <inheritdoc/>
    public OneOf(T5 t5) : base(t5)
    {
    }

    /// <inheritdoc/>
    public OneOf(T6 t6) : base(t6)
    {
    }

    /// <summary>
    /// Construcor that selects the specific type.
    /// </summary>
    /// <param name="t7"></param>
    public OneOf(T7 t7)
    {
        Item7 = t7.ThrowIfNull();
        ItemIndex = 7;

        SelectedType = typeof(T7);
        HashCode = System.HashCode.Combine(SelectedType, t7);
    }

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7>(T1 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7>(T2 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7>(T3 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7>(T4 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7>(T5 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7>(T6 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7>(T7 value) => new(value);

    /// <summary>
    /// Executes one of the methods depending on the selected value (type).
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <param name="onT5"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public TResult Either<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2,
        Func<T3, TResult> onT3,
        Func<T4, TResult> onT4,
        Func<T5, TResult> onT5,
        Func<T6, TResult> onT6,
        Func<T7, TResult> onT7)
    {
        if (7 == ItemIndex)
        {
            if (Item7 is not T7 t7) throw new ArgumentException($"{typeof(T7).Name}");

            return onT7(t7);
        }
        return Either(onT1, onT2, onT3, onT4, onT5, onT6);
    }

    public override bool Equals(object? obj) => Equals(obj as OneOf<T1, T2, T3, T4, T5, T6, T7>);

    public bool Equals(OneOf<T1, T2, T3, T4, T5, T6, T7>? other)
    {
        if (base.Equals(other)) return true;

        if (null == other || ItemIndex != other.ItemIndex) return false;

        return Item7.EqualsNullable(other.Item7);
    }

    public override int GetHashCode() => HashCode;

    /// <summary>
    /// Invokes action if Value is of type T5.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Invoke(Action<T7> action)
    {
        if (TryGet(out T7? value))
        {
            action(value!);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes the the action that matches the type of <see cref="Value"/>.
    /// </summary>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <param name="onT5"></param>
    /// <param name="onT6"></param>
    /// <param name="onT7"></param>
    /// <returns></returns>
    public bool Invoke(
        Action<T1> onT1,
        Action<T2> onT2,
        Action<T3> onT3,
        Action<T4> onT4,
        Action<T5> onT5,
        Action<T6> onT6,
        Action<T7> onT7)
    {
        return Invoke(onT1, onT2, onT3, onT4, onT5, onT6) || Invoke(onT7);
    }

    /// <summary>
    /// One of the selectable alternative.
    /// </summary>
    protected T7? Item7 { get; }

    public override bool TryGet<T>(out T? value) where T : default
    {
        if (7 == ItemIndex)
        {
            if (Item7 is T t)
            {
                value = t;
                return true;
            }
            value = default;
            return false;
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
/// <typeparam name="T6"></typeparam>
/// <typeparam name="T7"></typeparam>
/// <typeparam name="T8"></typeparam>
public class OneOf<T1, T2, T3, T4, T5, T6, T7, T8>
    : OneOf<T1, T2, T3, T4, T5, T6, T7>
    , IEquatable<OneOf<T1, T2, T3, T4, T5, T6, T7, T8>>
{
    protected OneOf()
    {
    }

    /// <inheritdoc/>
    public OneOf(T1 t1) : base(t1)
    {
    }

    /// <inheritdoc/>
    public OneOf(T2 t2) : base(t2)
    {
    }

    /// <inheritdoc/>
    public OneOf(T3 t3) : base(t3)
    {
    }

    /// <inheritdoc/>
    public OneOf(T4 t4) : base(t4)
    {
    }

    /// <inheritdoc/>
    public OneOf(T5 t5) : base(t5)
    {
    }

    /// <inheritdoc/>
    public OneOf(T6 t6) : base(t6)
    {
    }

    /// <inheritdoc/>
    public OneOf(T7 t7) : base(t7)
    {
    }

    /// <summary>
    /// Construcor that selects the specific type.
    /// </summary>
    /// <param name="t8"></param>
    public OneOf(T8 t8)
    {
        Item8 = t8.ThrowIfNull();
        ItemIndex = 8;

        SelectedType = typeof(T8);
        HashCode = System.HashCode.Combine(SelectedType, t8);
    }

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8>(T1 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8>(T2 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8>(T3 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8>(T4 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8>(T5 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8>(T6 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8>(T7 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8>(T8 value) => new(value);

    /// <summary>
    /// Executes one of the methods depending on the selected value (type).
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <param name="onT5"></param>
    /// <param name="onT6"></param>
    /// <param name="onT7"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public TResult Either<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2,
        Func<T3, TResult> onT3,
        Func<T4, TResult> onT4,
        Func<T5, TResult> onT5,
        Func<T6, TResult> onT6,
        Func<T7, TResult> onT7,
        Func<T8, TResult> onT8)
    {
        if (8 == ItemIndex)
        {
            if (Item8 is not T8 t8) throw new ArgumentException($"{typeof(T8).Name}");

            return onT8(t8);
        }
        return Either(onT1, onT2, onT3, onT4, onT5, onT6, onT7);
    }

    public override bool Equals(object? obj) => Equals(obj as OneOf<T1, T2, T3, T4, T5, T6, T7, T8>);

    public bool Equals(OneOf<T1, T2, T3, T4, T5, T6, T7, T8>? other)
    {
        if (base.Equals(other)) return true;

        if (null == other || ItemIndex != other.ItemIndex) return false;

        return Item8.EqualsNullable(other.Item8);
    }

    public override int GetHashCode() => HashCode;

    /// <summary>
    /// Invokes action if Value is of type T8.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Invoke(Action<T8> action)
    {
        if (TryGet(out T8? value))
        {
            action(value!);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes the the action that matches the type of <see cref="Value"/>.
    /// </summary>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <param name="onT5"></param>
    /// <param name="onT6"></param>
    /// <param name="onT7"></param>
    /// <returns></returns>
    public bool Invoke(
        Action<T1> onT1,
        Action<T2> onT2,
        Action<T3> onT3,
        Action<T4> onT4,
        Action<T5> onT5,
        Action<T6> onT6,
        Action<T7> onT7,
        Action<T8> onT8)
    {
        return Invoke(onT1, onT2, onT3, onT4, onT5, onT6, onT7) || Invoke(onT8);
    }

    /// <summary>
    /// One of the selectable alternative.
    /// </summary>
    protected T8? Item8 { get; }

    public override bool TryGet<T>(out T? value) where T : default
    {
        if (8 == ItemIndex)
        {
            if (Item8 is T t)
            {
                value = t;
                return true;
            }
            value = default;
            return false;
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
/// <typeparam name="T6"></typeparam>
/// <typeparam name="T7"></typeparam>
/// <typeparam name="T8"></typeparam>
/// <typeparam name="T9"></typeparam>
public class OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    : OneOf<T1, T2, T3, T4, T5, T6, T7, T8>
    , IEquatable<OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9>>
{
    protected OneOf()
    {
    }

    /// <inheritdoc/>
    public OneOf(T1 t1) : base(t1)
    {
    }

    /// <inheritdoc/>
    public OneOf(T2 t2) : base(t2)
    {
    }

    /// <inheritdoc/>
    public OneOf(T3 t3) : base(t3)
    {
    }

    /// <inheritdoc/>
    public OneOf(T4 t4) : base(t4)
    {
    }

    /// <inheritdoc/>
    public OneOf(T5 t5) : base(t5)
    {
    }

    /// <inheritdoc/>
    public OneOf(T6 t6) : base(t6)
    {
    }

    /// <inheritdoc/>
    public OneOf(T7 t7) : base(t7)
    {
    }

    /// <inheritdoc/>
    public OneOf(T8 t8) : base(t8)
    {
    }

    /// <summary>
    /// Construcor that selects the specific type.
    /// </summary>
    /// <param name="t9"></param>
    public OneOf(T9 t9)
    {
        Item9 = t9.ThrowIfNull();
        ItemIndex = 9;

        SelectedType = typeof(T8);
        HashCode = System.HashCode.Combine(SelectedType, t9);
    }

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T2 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T3 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T4 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T5 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T6 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T7 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T8 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T9 value) => new(value);

    /// <summary>
    /// Executes one of the methods depending on the selected value (type).
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <param name="onT5"></param>
    /// <param name="onT6"></param>
    /// <param name="onT7"></param>
    /// <param name="onT8"></param>
    /// <param name="onT9"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public TResult Either<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2,
        Func<T3, TResult> onT3,
        Func<T4, TResult> onT4,
        Func<T5, TResult> onT5,
        Func<T6, TResult> onT6,
        Func<T7, TResult> onT7,
        Func<T8, TResult> onT8,
        Func<T9, TResult> onT9)
    {
        if (9 == ItemIndex)
        {
            if (Item9 is not T9 t9) throw new ArgumentException($"{typeof(T9).Name}");

            return onT9(t9);
        }
        return Either(onT1, onT2, onT3, onT4, onT5, onT6, onT7, onT8);
    }

    public override bool Equals(object? obj) => Equals(obj as OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9>);

    public bool Equals(OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9>? other)
    {
        if (base.Equals(other)) return true;

        if (null == other || ItemIndex != other.ItemIndex) return false;

        return Item9.EqualsNullable(other.Item9);
    }

    public override int GetHashCode() => HashCode;

    /// <summary>
    /// Invokes action if Value is of type T9.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Invoke(Action<T9> action)
    {
        if (TryGet(out T9? value))
        {
            action(value!);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes the the action that matches the type of <see cref="Value"/>.
    /// </summary>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <param name="onT5"></param>
    /// <param name="onT6"></param>
    /// <param name="onT7"></param>
    /// <param name="onT8"></param>
    /// <param name="onT9"></param>
    /// <returns></returns>
    public bool Invoke(
        Action<T1> onT1,
        Action<T2> onT2,
        Action<T3> onT3,
        Action<T4> onT4,
        Action<T5> onT5,
        Action<T6> onT6,
        Action<T7> onT7,
        Action<T8> onT8,
        Action<T9> onT9)
    {
        return Invoke(onT1, onT2, onT3, onT4, onT5, onT6, onT7, onT8) || Invoke(onT9);
    }

    /// <summary>
    /// One of the selectable alternative.
    /// </summary>
    protected T9? Item9 { get; }

    public override bool TryGet<T>(out T? value) where T : default
    {
        if (9 == ItemIndex)
        {
            if (Item9 is T t)
            {
                value = t;
                return true;
            }
            value = default;
            return false;
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
/// <typeparam name="T6"></typeparam>
/// <typeparam name="T7"></typeparam>
/// <typeparam name="T8"></typeparam>
/// <typeparam name="T9"></typeparam>
public class OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
    : OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9>
    , IEquatable<OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>>
{
    protected OneOf()
    {
    }

    /// <inheritdoc/>
    public OneOf(T1 t1) : base(t1)
    {
    }

    /// <inheritdoc/>
    public OneOf(T2 t2) : base(t2)
    {
    }

    /// <inheritdoc/>
    public OneOf(T3 t3) : base(t3)
    {
    }

    /// <inheritdoc/>
    public OneOf(T4 t4) : base(t4)
    {
    }

    /// <inheritdoc/>
    public OneOf(T5 t5) : base(t5)
    {
    }

    /// <inheritdoc/>
    public OneOf(T6 t6) : base(t6)
    {
    }

    /// <inheritdoc/>
    public OneOf(T7 t7) : base(t7)
    {
    }

    /// <inheritdoc/>
    public OneOf(T8 t8) : base(t8)
    {
    }

    /// <inheritdoc/>
    public OneOf(T9 t9) : base(t9)
    {
    }

    /// <summary>
    /// Construcor that selects the specific type.
    /// </summary>
    /// <param name="t10"></param>
    public OneOf(T10 t10)
    {
        Item10 = t10.ThrowIfNull();
        ItemIndex = 10;

        SelectedType = typeof(T8);
        HashCode = System.HashCode.Combine(SelectedType, t10);
    }

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T2 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T3 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T4 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T5 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T6 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T7 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T8 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T9 value) => new(value);

    public static implicit operator OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T10 value) => new(value);

    /// <summary>
    /// Executes one of the methods depending on the selected value (type).
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <param name="onT5"></param>
    /// <param name="onT6"></param>
    /// <param name="onT7"></param>
    /// <param name="onT8"></param>
    /// <param name="onT9"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public TResult Either<TResult>(
        Func<T1, TResult> onT1,
        Func<T2, TResult> onT2,
        Func<T3, TResult> onT3,
        Func<T4, TResult> onT4,
        Func<T5, TResult> onT5,
        Func<T6, TResult> onT6,
        Func<T7, TResult> onT7,
        Func<T8, TResult> onT8,
        Func<T9, TResult> onT9,
        Func<T10, TResult> onT10)
    {
        if (9 == ItemIndex)
        {
            if (Item10 is not T10 t10) throw new ArgumentException($"{typeof(T10).Name}");

            return onT10(t10);
        }
        return Either(onT1, onT2, onT3, onT4, onT5, onT6, onT7, onT8, onT9);
    }

    public override bool Equals(object? obj) => Equals(obj as OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>);

    public bool Equals(OneOf<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>? other)
    {
        if (base.Equals(other)) return true;

        if (null == other || ItemIndex != other.ItemIndex) return false;

        return Item10.EqualsNullable(other.Item10);
    }

    public override int GetHashCode() => HashCode;

    /// <summary>
    /// Invokes action if Value is of type T10.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public bool Invoke(Action<T10> action)
    {
        if (TryGet(out T10? value))
        {
            action(value!);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Invokes the the action that matches the type of <see cref="Value"/>.
    /// </summary>
    /// <param name="onT1"></param>
    /// <param name="onT2"></param>
    /// <param name="onT3"></param>
    /// <param name="onT4"></param>
    /// <param name="onT5"></param>
    /// <param name="onT6"></param>
    /// <param name="onT7"></param>
    /// <param name="onT8"></param>
    /// <param name="onT9"></param>
    /// <param name="onT10"></param>
    /// <returns></returns>
    public bool Invoke(
        Action<T1> onT1,
        Action<T2> onT2,
        Action<T3> onT3,
        Action<T4> onT4,
        Action<T5> onT5,
        Action<T6> onT6,
        Action<T7> onT7,
        Action<T8> onT8,
        Action<T9> onT9,
        Action<T10> onT10)
    {
        return Invoke(onT1, onT2, onT3, onT4, onT5, onT6, onT7, onT8, onT9) || Invoke(onT10);
    }

    /// <summary>
    /// One of the selectable alternative.
    /// </summary>
    protected T10? Item10 { get; }

    public override bool TryGet<T>(out T? value) where T : default
    {
        if (10 == ItemIndex)
        {
            if (Item10 is T t)
            {
                value = t;
                return true;
            }
            value = default;
            return false;
        }

        return base.TryGet(out value);
    }
}
