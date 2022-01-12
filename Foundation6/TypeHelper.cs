namespace Foundation;

public static class TypeHelper
{
    /// <summary>
    /// returns the type from the short name. e.g. bool, int.
    /// </summary>
    /// <param name="shortTypeName"></param>
    /// <returns></returns>
    public static Type? GetPrimitveType(string shortTypeName)
    {
        var fullName = GetPrimitiveTypeFullName(shortTypeName);
        if (null == fullName) return null;

        return Type.GetType(fullName);
    }

    /// <summary>
    /// returns the fullname of the type. e.g. int => System.Int32.
    /// </summary>
    /// <param name="shortTypeName">e.g. bool, byte int, long, ...</param>
    /// <returns></returns>
    public static string? GetPrimitiveTypeFullName(string shortTypeName)
    {
        return shortTypeName switch
        {
            "bool" => typeof(Boolean).FullName,
            "byte" => typeof(Byte).FullName,
            "char" => typeof(Char).FullName,
            "double" => typeof(Double).FullName,
            "float" => typeof(Single).FullName,
            "int" => typeof(Int32).FullName,
            "long" => typeof(Int64).FullName,
            "sbyte" => typeof(SByte).FullName,
            "short" => typeof(Int16).FullName,
            "uint" => typeof(UInt32).FullName,
            "ulong" => typeof (UInt64).FullName,
            "ushort" => typeof(UInt16).FullName,
            _ => null,
        };
    }

    public static IEnumerable<string> GetPrimitiveTypeShortNames()
    {
        yield return "bool";
        yield return "byte";
        yield return "char";
        yield return "double";
        yield return "float";
        yield return "int";
        yield return "long";
        yield return "sbyte";
        yield return "short";
        yield return "uint";
        yield return "ulong";
        yield return "ushort";
    }

    /// <summary>
    /// returns the type from the short name. e.g. int, string.
    /// </summary>
    /// <param name="shortTypeName"></param>
    /// <returns></returns>
    public static Type? GetScalarType(string shortTypeName)
    {
        var fullName = GetScalarTypeFullName(shortTypeName);
        if (null == fullName) return null;

        return Type.GetType(fullName);
    }

    /// <summary>
    /// returns the fullname of the type. e.g. int => System.Int32.
    /// </summary>
    /// <param name="shortTypeName">e.g. DateTime, decimal, string.</param>
    /// <returns></returns>
    public static string? GetScalarTypeFullName(string shortTypeName)
    {
        var fullName = GetPrimitiveTypeFullName(shortTypeName);
        if(null != fullName) return fullName;

        return shortTypeName switch
        {
            "DateTime" => typeof(DateTime).FullName,
            "decimal" => typeof(Decimal).FullName,
            "string" => typeof(String).FullName,
            _ => null,
        };
    }

    public static IEnumerable<string> GetScalarTypeShortNames()
    {
        foreach(var typeName in GetPrimitiveTypeShortNames())
            yield return typeName;

        yield return "DateTime";
        yield return "decimal";
        yield return "string";
    }

    public static IEnumerable<Type> PrimitiveArrayTypes()
    {
        yield return typeof(Boolean[]);
        yield return typeof(Byte[]);
        yield return typeof(Char[]);
        yield return typeof(Double[]);
        yield return typeof(Int16[]);
        yield return typeof(Int32[]);
        yield return typeof(Int64[]);
        yield return typeof(SByte[]);
        yield return typeof(Single[]);
        yield return typeof(UInt16[]);
        yield return typeof(UInt32[]);
        yield return typeof(UInt64[]);
    }

    public static IEnumerable<Type> PrimitiveEnumerableTypes()
    {
        yield return typeof(IEnumerable<Boolean>);
        yield return typeof(IEnumerable<Byte>);
        yield return typeof(IEnumerable<Char>);
        yield return typeof(IEnumerable<Double>);
        yield return typeof(IEnumerable<Int16>);
        yield return typeof(IEnumerable<Int32>);
        yield return typeof(IEnumerable<Int64>);
        yield return typeof(IEnumerable<SByte>);
        yield return typeof(IEnumerable<Single>);
        yield return typeof(IEnumerable<UInt16>);
        yield return typeof(IEnumerable<UInt32>);
        yield return typeof(IEnumerable<UInt64>);
    }

    public static IEnumerable<Type> PrimitiveTypes()
    {
        yield return typeof(Boolean);
        yield return typeof(Byte);
        yield return typeof(Char);
        yield return typeof(Double);
        yield return typeof(Int16);
        yield return typeof(Int32);
        yield return typeof(Int64);
        yield return typeof(SByte);
        yield return typeof(Single);
        yield return typeof(UInt16);
        yield return typeof(UInt32);
        yield return typeof(UInt64);
    }

    public static IEnumerable<Type> ScalarArrayTypes()
    {
        foreach (var primitive in PrimitiveArrayTypes())
        {
            yield return primitive;
        }
        yield return typeof(DateTime[]);
        yield return typeof(decimal[]);
        yield return typeof(string[]);
    }

    public static IEnumerable<Type> ScalarEnumerableTypes()
    {
        foreach (var primitive in PrimitiveEnumerableTypes())
        {
            yield return primitive;
        }
        yield return typeof(IEnumerable<DateTime>);
        yield return typeof(IEnumerable<decimal>);
        yield return typeof(IEnumerable<string>);
    }

    public static IEnumerable<Type> ScalarTypes()
    {
        foreach (var primitive in PrimitiveTypes())
        {
            yield return primitive;
        }

        yield return typeof(DateTime);
        yield return typeof(decimal);
        yield return typeof(string);
    }
}

