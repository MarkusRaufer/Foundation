namespace Foundation;

using System.Runtime.Serialization;

public static class Option
{
    public static Option<T> Maybe<T>(T? value) => (null == value) ? None<T>() : Some(value);

    public static Option<T> None<T>() => Option<T>.None;

    public static Option<T> Some<T>(T value) => null != value ? new Option<T>(value) : throw new ArgumentNullException(nameof(value));
}

/// <summary>
/// A result that can have a value.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
public readonly struct Option<T>
    : IEquatable<Option<T>>
    , ISerializable
{
    private readonly T? _value;

    internal Option(T? value)
    {
        IsSome = value is not null;
        _value = value;
    }

    public Option(SerializationInfo info, StreamingContext context)
    {
        if (info.GetValue(nameof(IsSome), typeof(bool)) is not bool isSome)
        {
            IsSome = false;
            _value = default;
            return;
        }

        IsSome = isSome;

        if (!IsSome || info.GetValue(nameof(Value), typeof(T)) is not T value)
        {
            IsSome = false;
            _value = default;
            return;
        }
        _value = value;
    }

    public static implicit operator Option<T>(T obj) => Option.Maybe(obj);

    public static bool operator ==(Option<T> lhs, Option<T> rhs) => lhs.Equals(rhs);

    public static bool operator !=(Option<T> lhs, Option<T> rhs) => !(lhs == rhs);

    public override bool Equals(object? obj) => obj is Option<T> other && Equals(other);

    public bool Equals(Option<T> other)
    {
        if (IsNone) return other.IsNone;
        return other.IsSome && _value!.Equals(other.Value);
    }

    public override int GetHashCode()
    {
        if (IsNone) return 0;
        return _value!.GetHashCode();
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(IsSome), IsSome);
        info.AddValue(nameof(Value), _value);
    }

    public bool TryGet(out T? value)
    {
        value = _value;

        return IsSome;
    }

    public bool IsNone => !IsSome;

    public bool IsSome { get; }

    public override string ToString() => IsSome ? $"Some({_value})" : "None";

    internal static readonly Option<T> None = new();

    internal T Value
    {
        get
        {
            if (IsNone) throw new NullReferenceException(nameof(Value));
            return _value!;
        }
    }

    internal object ValueAsObject
    {
        get
        {
            if (IsNone) throw new NullReferenceException(nameof(ValueAsObject));
            return _value!;
        }
    }
}
