namespace Foundation.Buffers;

public readonly ref struct LineSplitEntry
{
    public LineSplitEntry(ReadOnlySpan<char> line, ReadOnlySpan<char> separator)
    {
        Line = line;
        Separator = separator;
    }

    public ReadOnlySpan<char> Line { get; }
    public ReadOnlySpan<char> Separator { get; }

    public void Deconstruct(out ReadOnlySpan<char> line, out ReadOnlySpan<char> separator)
    {
        line = Line;
        separator = Separator;
    }

    // This method allow to implicitly cast the type into a ReadOnlySpan<char>, so you can write the following code
    // foreach (ReadOnlySpan<char> entry in str.SplitLines())
    public static implicit operator ReadOnlySpan<char>(LineSplitEntry entry) => entry.Line;
}

