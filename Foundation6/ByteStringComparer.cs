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

            if (x.Length < y.Length) return -1;
            if (x.Length > y.Length) return 1;

            for (var i = 0; i < y.Length; i++)
            {
                var lhs = x[i];
                var rhs = y[i];
                if (lhs < rhs) return -1;
                if (lhs > rhs) return 1;
            }

            return 0;
        }
    }

    private class NullByteStringComparer : IComparer<ByteString>
    {
        public int Compare(ByteString? x, ByteString? y)
        {
            if (ReferenceEquals(x, y)) return 0;

            if (x is null) return y is null ? 0 : 1;
            if (y is null) return -1;

            if (x.Length < y.Length) return -1;
            if (x.Length > y.Length) return 1;

            for (var i = 0; i < y.Length; i++)
            {
                var lhs = x[i];
                var rhs = y[i];
                if (lhs < rhs) return -1;
                if (lhs > rhs) return 1;
            }

            return 0;
        }
    }

    public static IComparer<ByteString> Default => new DefaultByteStringComparer();
    public static IComparer<ByteString> NullValuesAreGreatest => new NullByteStringComparer();
}

