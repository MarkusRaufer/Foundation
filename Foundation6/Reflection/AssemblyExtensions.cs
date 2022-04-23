using System.Reflection;

namespace Foundation.Reflection;

public static class AssemblyExtensions
{
    public static string? GetAssemblyFileVersion(this Assembly assembly)
    {
        var attributes = assembly.GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
        if (attributes.Length == 1)
        {
            if(attributes[0] is AssemblyFileVersionAttribute attribute)
                return attribute.Version;
        }
        return null;
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
        var attributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
        if (attributes.Length == 1)
        {
            if(attributes[0] is AssemblyProductAttribute attribute)
                return attribute.Product;
        }
        return null;
    }

    public static string? GetAssemblyVersion(this Assembly assembly)
    {
        return assembly.GetName().Version?.ToString();
    }
}
