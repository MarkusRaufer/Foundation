using System.Diagnostics;
using System.Reflection;

namespace Foundation.Reflection;

public static class AssemblyExtensions
{
    public static string? GetAssemblyFileVersion(this Assembly assembly)
    {
        return FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
    }

    public static Result<string, FileNotFoundException> GetAssemblyDirectory(this Assembly assembly)
    {
        var directory = Path.GetDirectoryName(assembly.Location);
        return null != directory
            ? Result.Ok<string, FileNotFoundException>(directory)
            : Result.Error<string, FileNotFoundException>(new FileNotFoundException(assembly.Location));
    }

    public static string? GetAssemblyProduct(this Assembly assembly)
    {
        return FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
    }

    public static string? GetAssemblyVersion(this Assembly assembly)
    {
        return assembly.GetName().Version?.ToString();
    }
}
