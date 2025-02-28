using System.Reflection;

namespace Foundation;
public class StringToTypeConverter
{
    private readonly Assembly[] _assemblies;


    public StringToTypeConverter() : this(Assembly.GetExecutingAssembly().Location)
    {
    }

    public StringToTypeConverter(string assemblyLocation) : this(GetAssemblies(assemblyLocation))
    {
    }

    public StringToTypeConverter(IEnumerable<Assembly> assemblies)
    {
        _assemblies = assemblies.ToArray();   
    }

    private static IEnumerable<Assembly> GetAssemblies(string location)
    {
        var dir = Path.GetDirectoryName(location);
        if (dir is null) yield break;

        var dlls = Directory.GetFiles(dir, "*.dll");
        foreach (var dll in dlls)
        {
            yield return Assembly.LoadFile(dll);
        }
    }

    public Type? GetType(string name)
    {
        var type = Type.GetType(name);
        if (type is null)
        {
            var span = name.AsSpan();
            var index = span.IndexOf('+');
            var typeName = span[(index + 1)..].ToString();

            type = _assemblies.SelectMany(x => x.GetTypes()).Where(x => x.Name == typeName).FirstOrDefault();
        }
        return type;
    }
}
