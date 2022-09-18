namespace Foundation.IO;

public static class TempFile
{
    public static string? GetRandomName() => Path.GetFileNameWithoutExtension(Path.GetTempFileName());
}
