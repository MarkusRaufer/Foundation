using System.Reflection;

namespace Foundation.Reflection;

public static class AssemblyEx
{
    /// <summary>
    /// Returns the directory with the executing assembly.
    /// </summary>
    /// <returns></returns>
    public static string? GetExecutionDirectory()
    {
        return GetExecutionDirectory(Assembly.GetExecutingAssembly());
    }

    /// <summary>
    /// Returns the directory of the assembly.
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    public static string? GetExecutionDirectory(Assembly assembly)
    {
        assembly.ThrowIfNull();
        return Path.GetDirectoryName(assembly.Location);
    }
}
