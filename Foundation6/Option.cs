using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace Foundation;

public static class Option
{
    public static Option<T> Maybe<T>(T? value) => new(value);

    public static Option<T> None<T>() => Option<T>.None;

    public static Option<T> Some<T>(T value) => null != value ? new(value) : throw new ArgumentNullException(nameof(value));
}


[Serializable]
public readonly struct Option<T>
    : IEquatable<Option<T>>
    , ISerializable
{
    private readonly T? _value;

    public Option(T? value)
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

        if (!IsSome || info.GetValue(nameof(T), typeof(T)) is not T value)
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
        if (!IsSome) return !other.IsSome;
        if(!other.IsSome) return false;

        return GetHashCode() == other.GetHashCode() 
            && _value!.Equals(other._value);
    }

    public override int GetHashCode()
    {
        return IsSome ? _value!.GetHashCode() : 0;
    }

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        info.AddValue(nameof(IsSome), IsSome);
        if(IsSome) info.AddValue(nameof(_value), _value);
    }

    public bool IsNone => !IsSome;

    public bool IsSome { get; }

    public override string ToString() => IsSome ? $"Some({_value})" : "None";

    /// <summary>
    /// If returning true it has a value otherwise it returns false.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool TryGet([NotNullWhen(true)] out T? value)
    {
        value = _value;

        return IsSome;
    }

    internal static readonly Option<T> None = new();
}