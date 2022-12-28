namespace Foundation;

/// <summary>
/// Can have three states: <see cref="State"/> can be Some(true), Some(false) or None./>.
/// </summary>
public readonly struct TriState : IEquatable<TriState>
{
    public TriState(bool isTrue)
    {
        State = Option.Some(isTrue);
    }

    public static bool operator ==(TriState left, TriState right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TriState left, TriState right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj) => obj is TriState other && Equals(other);

    public bool Equals(TriState other)
    {
        if (State.IsNone) return other.State.IsNone;

        return other.State.IsSome && State.Equals(other.State);
    }

    public override int GetHashCode()
    {
        if(State.IsNone) return 0;
        
        return State.OrThrow() ? 2 : 1;
    }
        
    public Option<bool> State { get; }

    public override string ToString() => $"{State}";

    public bool TryGet(out bool state)
    {
        if(State.IsNone)
        {
            state = default;
            return false;
        }
        state = State.OrThrow();
        return true;
    }

    public static TriState ToState(bool state)
    {
        return new TriState(state);
    }

    public static TriState<TState1, TState2> ToState<TState1, TState2>(TState1 state)
    {
        return new TriState<TState1, TState2>(state);
    }

    public static TriState<TState1, TState2> ToState<TState1, TState2>(TState2 state)
    {
        return new TriState<TState1, TState2>(state);
    }
}

/// <summary>
/// Can have three states: <see cref="NoState"/>, <typeparamref name="TState1"/> and <typeparamref name="TState2"/>.
/// </summary>
/// <typeparam name="TState1"></typeparam>
/// <typeparam name="TState2"></typeparam>
public readonly struct TriState<TState1, TState2> : IEquatable<TriState<TState1, TState2>>
{
    public TriState(TState1 state1)
    {
        state1.ThrowIfNull();

        State = Option.Some(new OneOf<TState1, TState2>(state1));
    }

    public TriState(TState2 state2)
    {
        state2.ThrowIfNull();

        State = Option.Some(new OneOf<TState1, TState2>(state2));

    }

    public static bool operator ==(TriState<TState1, TState2> left, TriState<TState1, TState2> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(TriState<TState1, TState2> left, TriState<TState1, TState2> right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj) => obj is TriState<TState1, TState2> other && Equals(other);

    public bool Equals(TriState<TState1, TState2> other)
    {
        return State.Equals(other.State);
    }

    public override int GetHashCode()
    {
        if (State.IsNone) return 0;

        return State.OrThrow().GetHashCode();
    }

    public void Invoke(
        Action<TState1>? state1 = null, 
        Action<TState2>? state2 = null, 
        Action? none = null)
    {
        if (State.IsNone)
        {
            none?.Invoke();
            return;
        }
        var oneOf = State.OrThrow();

        oneOf.Invoke(s1 => state1?.Invoke(s1));
        oneOf.Invoke(s2 => state2?.Invoke(s2));
    }

    public TResult Either<TResult>(
        Func<TState1, TResult> state1, 
        Func<TState2, TResult> state2,
        Func<TResult> none)
    {
        if(State.IsNone) return none();

        return State.OrThrow().Either(state1, state2);
    }

    public Option<OneOf<TState1, TState2>> State { get; }

    public override string ToString()
    {
        if (State.IsNone) return $"{State}";

        return State.OrThrow()
                    .Either(state1 => $"{state1}", state2 => $"{state2}");
    }

    public bool TryGet(out TState1? state) => State.OrThrow().TryGet(out state);

    public bool TryGet(out TState2? state) => State.OrThrow().TryGet(out state);
}