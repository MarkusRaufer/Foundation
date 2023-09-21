using Foundation.Collections.Generic;
using Foundation.Reflection;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.IO;

public static class BinaryObjectReader
{
    public static BinaryObjectReader<T> New<T>(params Expression<Func<T, object>>[] members)
    {
        if (0 == members.Length) throw new ArgumentOutOfRangeException(nameof(members));

        return new(new MemberInfoCash<T>(members));
    }
}

public class BinaryObjectReader<T>
{
     private readonly MemberInfoCash<T> _memberCash;


    public BinaryObjectReader() : this(new MemberInfoCash<T>(typeof(T).GetProperties()))
    {
    }

    public BinaryObjectReader(MemberInfo[] members) 
        : this(new MemberInfoCash<T>(members))
    {
        members.ThrowIfNull();
    }

    public BinaryObjectReader(string[] memberNames)
        : this(new MemberInfoCash<T>(memberNames))
    {
    }

    public BinaryObjectReader(MemberInfoCash<T> memberCash)
    {
        _memberCash = memberCash.ThrowIfNull();
    }

    public IEnumerable<KeyValuePair<string, object>> ReadKeyValues(Stream stream)
    {
        return ReadMembers(stream).Select(tuple => Pair.New(tuple.member.Name, tuple.value));
    }
    public IEnumerable<(MemberInfo member, object value)> ReadMembers(Stream stream)
    {
        stream.ThrowIfNull();

        var reader = new BinaryReader(stream);
        return reader.ReadFromMembers(_memberCash.GetMembers());
    }

    public IEnumerable<(PropertyInfo property, object value)> ReadProperties(Stream stream)
    {
        stream.ThrowIfNull();

        var reader = new BinaryReader(stream);
        return reader.ReadFromProperties(_memberCash.GetMembers().OfType<PropertyInfo>());
    }
}
