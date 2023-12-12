namespace Foundation;

public static class SpanExtensions
{
    public static bool IsEmptyOrWhiteSpace(this ReadOnlySpan<char> span)
    {
        return span.IsEmpty || span.IsWhiteSpace();
    }
}
