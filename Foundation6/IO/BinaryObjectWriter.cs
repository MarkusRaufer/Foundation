using Foundation.Reflection;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.IO;

public class BinaryObjectWriter
{
    private readonly MemberInfoCash _memberCash;

    public BinaryObjectWriter(Type type) : this(new MemberInfoCash(type))
    {
    }

    public BinaryObjectWriter(Type type, MemberInfo[] members) : this(new MemberInfoCash(type, members))
    {
    }

    public BinaryObjectWriter(Type type, string[] memberNames) : this(new MemberInfoCash(type, memberNames))
    {
    }

    public BinaryObjectWriter(MemberInfoCash memberCash)
    {
        _memberCash = memberCash.ThrowIfNull();
    }

    public void WriteObject<T>(Stream stream, T obj)
    {
        if (null == stream) throw new ArgumentNullException(nameof(stream));
        if (null == obj) throw new ArgumentNullException(nameof(obj));

        var writer = new BinaryWriter(stream);
        var members = _memberCash.GetMembers();

        writer.WriteObject(obj, members);
    }

    public static BinaryObjectWriter<T> New<T>(params Expression<Func<T, object>>[] members)
    {
        return new(new MemberInfoCash<T>(members));
    }
}

public class BinaryObjectWriter<T>
{
    private readonly MemberInfoCash<T> _memberCash;

    public BinaryObjectWriter() : this(new MemberInfoCash<T>(typeof(T).GetProperties()))
    {
    }

    public BinaryObjectWriter(MemberInfo[] members) : this(new MemberInfoCash<T>(members))
    {
    }

    public BinaryObjectWriter(string[] memberNames) : this(new MemberInfoCash<T>(memberNames))
    {
    }

    public BinaryObjectWriter(MemberInfoCash<T> memberCash)
    {
        _memberCash = memberCash.ThrowIfNull();
    }

    public void WriteObject(Stream stream, T obj)
    {
        if (null == stream) throw new ArgumentNullException(nameof(stream));
        if (null == obj) throw new ArgumentNullException(nameof(obj));

        var writer = new BinaryWriter(stream);
        var members = _memberCash.GetMembers();

        writer.WriteObject(obj, members);
    }
}
