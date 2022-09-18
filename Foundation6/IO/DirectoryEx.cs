namespace Foundation.IO;

public static class DirectoryEx
{
    public static void CreateIfNotExists(string path)
    {
        if (Directory.Exists(path)) return;
        Directory.CreateDirectory(path);
    }
}
