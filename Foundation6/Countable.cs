namespace Foundation;

public static class Countable
{
    public static Countable<T> New<T>(T value) => new(value, 1, false);

    public static Countable<T> New<T>(T value, int count) => new(value, count, false);

    public static Countable<T> New<T>(T value, bool hashCodeIncludesCount) => new(value, 1, hashCodeIncludesCount);

    public static Countable<T> New<T>(T value, int count, bool hashCodeIncludesCount) => new(value, count, hashCodeIncludesCount);
}

public sealed class Countable<T> : IEquatable<Countable<T>>
{
	public Countable(T? value, int count, bool hashCodeIncludesCount)
	{
		Value = value;
		Count = count;
		HashCodeIncludesCount = hashCodeIncludesCount;
	}

    public static implicit operator T?(Countable<T> countable)
    {
        return countable.Value;
    }

    public static bool operator ==(Countable<T> lhs, Countable<T> rhs)
	{
		return lhs.Equals(rhs);
	}

    public static bool operator ==(Countable<T> lhs, T? rhs)
    {
        return lhs.Value.EqualsNullable(rhs);
    }

    public static bool operator ==(T? lhs, Countable<T> rhs)
    {
        return lhs.EqualsNullable(rhs.Value);
    }

    public static bool operator !=(Countable<T> lhs, Countable<T> rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator !=(Countable<T> lhs, T? rhs)
    {
        return !(lhs == rhs);
    }

    public static bool operator !=(T? lhs, Countable<T> rhs)
    {
        return !(lhs == rhs);
    }

    public int Count { get; private set; }

    public void Dec()
    {
        if (0 == Count) return;
        
        Count--;
    }


	public override bool Equals(object? obj) => Equals(obj as Countable<T>);

	public bool Equals(Countable<T>? other)
	{
		if (other is null) return false;
		if (!Value.EqualsNullable(other.Value)) return false;
		return HashCodeIncludesCount ? Count == other.Count : true;
	}

	public override int GetHashCode()
	{
		if(HashCodeIncludesCount) return Value is null ? System.HashCode.Combine(0, Count) : System.HashCode.Combine(Value, Count);

		return Value.GetNullableHashCode();
	}

    public int GetValueHashCode() => Value.GetNullableHashCode();

	public bool HashCodeIncludesCount { get; }

    public void Inc() => Count++;

    public T? Value { get; }

	public bool ValueEquals(Countable<T>? other)
	{
		return other is not null && Value.EqualsNullable(other.Value);
	}
}
