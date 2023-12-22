namespace Foundation;

public static class ByteStringComparer
{
    private class DefaultByteStringComparer : IComparer<ByteString>
    {
        public int Compare(ByteString? x, ByteString? y)
        {
            if (ReferenceEquals(x, y)) return 0;

            if (x is null) return y is null ? 0 : -1;
            if (y is null) return 1;

            var lhs = x.AsSpan();
            var rhs = y.AsSpan();
            
            return lhs.SequenceCompareTo(rhs);
        }
    }

    private class NullByteStringComparer : IComparer<ByteString>
    {
        public int Compare(ByteString? x, ByteString? y)
        {
            if (ReferenceEquals(x, y)) return 0;

            if (x is null) return y is null ? 0 : 1;
            if (y is null) return -1;

            var lhs = x.AsSpan();
            var rhs = y.AsSpan();

            return lhs.SequenceCompareTo(rhs);
        }
    }

    /// <summary>
    /// null is considered. A ByteString is null means smaller.
    /// </summary>
    public static IComparer<ByteString> Default => new DefaultByteStringComparer();

    /// <summary>
    /// null is considered. A ByteString is null means greater. In a list including null values the null values appear at the end. 
    /// Means you can stop iterating the list on the first null ByteString.
    /// </summary>
    public static IComparer<ByteString> NullIsGreater => new NullByteStringComparer();
}
