namespace Foundation;

public static class Countable
{
    public static Countable<T> New<T>(T value, bool hashCodeIncludesCount) => new(value, 0, hashCodeIncludesCount);

    public static Countable<T> New<T>(T value, int count, bool hashCodeIncludesCount) => new(value, count, hashCodeIncludesCount);
}

public sealed class Countable<T> : IEquatable<Countable<T>>
{
    public Countable(T? value, bool hashCodeIncludesCount) : this(value, 0, hashCodeIncludesCount)
    {
    }

    public Countable(T? value, int count, bool hashCodeIncludesCount)
	{
		Value = value;
		Count = count;
        HashCodeIncludesCount = hashCodeIncludesCount;
    }

	public int Count { get; private set; }

	public void Dec() => Count--;

    public void Inc() => Count++;

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

    public T? Value { get; }

	public bool ValueEquals(Countable<T>? other)
	{
		return other is not null && Value.EqualsNullable(other.Value);
	}
}
