namespace Foundation;

public static class Countable
{
	public static Countable<T> New<T>(T value, int count = 0) => new(value, count);
}

public sealed class Countable<T> : IEquatable<Countable<T>>
{
	public Countable(T value, int count = 0)
	{
		Value = value.ThrowIfNull();
		Count = count;
	}

	public int Count { get; private set; }

	public void Dec() => Count--;

    public void Inc() => Count++;

	public override bool Equals(object? obj) => Equals(obj as Countable<T>);

	public bool Equals(Countable<T>? other)
	{
		return other is not null
			&& Value.EqualsNullable(other.Value)
			&& Count == other.Count;
	}

	public override int GetHashCode() => System.HashCode.Combine(Value, Count);

    public int GetValueHashCode() => Value.GetNullableHashCode();

    public T Value { get; }

	public bool ValueEquals(Countable<T>? other)
	{
		return other is not null && Value.EqualsNullable(other.Value);
	}
}
