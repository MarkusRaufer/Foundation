namespace Foundation.Buffers;

public static class StringExtensions
{
    public static CharSplitEnumerator SplitIntoSpans(this string str, char separator)
    {
        return new CharSplitEnumerator(str.AsSpan(), separator);
    }
    public static LineSplitEnumerator SplitLines(this string str)
    {
        // LineSplitEnumerator is a struct so there is no allocation here
        return new LineSplitEnumerator(str.AsSpan());
    }
}

