namespace Foundation.ComponentModel
{
    public static class OrdinalTuple
    {
        public static OrdinalTuple<TValue> New<TValue>(int index, TValue value) => new (index, value);
        public static OrdinalTuple<TIndex, TValue> New<TIndex, TValue>(TIndex index, TValue value) where TIndex : notnull 
            => new(index, value);
    }

    public record struct OrdinalTuple<TValue>(int Index, TValue Value);

    public record struct OrdinalTuple<TIndex, TValue>(TIndex Index, TValue Value) where TIndex : notnull;
}
