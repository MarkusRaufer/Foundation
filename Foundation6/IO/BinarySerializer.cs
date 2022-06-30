using Foundation.Collections.Generic;
using Foundation.Linq.Expressions;
using Foundation.Reflection;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.IO;

public static class BinarySerializer
{
    public static BinarySerializer<T> New<T>(Func<IEnumerable<KeyValuePair<string, object>>, T> factory)
        => new(factory);

    public static BinarySerializer<T> New<T>(
        IEnumerable<string> memberNames, 
        Func<IEnumerable<KeyValuePair<string, object>>, T> factory)
        => new (memberNames, factory);

    public static BinarySerializer<T> New<T>(
        Func<IEnumerable<KeyValuePair<string, object>>, T> factory,
        params Expression<Func<T, object>>[] members)
    {
        if (0 == members.Length) throw new ArgumentOutOfRangeException(nameof(members));

        var memberInfos = members.Cast<LambdaExpression>()
                                 .Select(GetMemberFromLambda);

        return new(memberInfos.Select(m => m.Name), factory);
    }

    #region private members
    private static MemberInfo GetMemberFromLambda(LambdaExpression lambda)
    {
        var member = lambda.Body switch
        {
            MemberExpression me => me,
            UnaryExpression ue => (MemberExpression)ue.Operand,
            _ => throw new InvalidArgumentException($"{lambda.Body}")
        };

        return member.Member switch
        {
            FieldInfo => member.Member,
            PropertyInfo => member.Member,
            _ => throw new InvalidArgumentException($"{member.Member}")
        };
    }
    #endregion private members
}

public class BinarySerializer<T>
{
    private readonly Func<IEnumerable<KeyValuePair<string, object>>, T> _factory;
    private readonly ICollection<string>? _memberNames;
    private ICollection<MemberInfo>? _memberCache;

    public BinarySerializer(Func<IEnumerable<KeyValuePair<string, object>>, T> factory)
    {
        _factory = factory.ThrowIfNull();
    }

    public BinarySerializer(
        IEnumerable<string> memberNames,
        Func<IEnumerable<KeyValuePair<string, object>>, T> factory) : this(factory)
    {
        _memberNames = memberNames.ThrowIfEmpty().ToArray();
    }

    private static KeyValuePair<string, object> CreateKeyValuePair((MemberInfo, object) tuple)
        => new (tuple.Item1.Name, tuple.Item2);

    /// <summary>
    /// Deserializes an object using values from <paramref name="stream"/>. If stream does not contain values an exception is thrown.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public object? Deserialze(Stream stream)
    {
        stream.ThrowIfNull();

        stream.Position = 0L;
        var reader = new BinaryReader(stream);

        var memberValues = reader.ReadFromMembers(GetMembers()).ToArray();
        if (0 == memberValues.Length) return null;

        return _factory(memberValues.Select(CreateKeyValuePair));
    }

    /// <summary>
    /// Deserializes an object using values from <paramref name="stream"/>. If stream does not contain values an exception is thrown.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="stream"></param>
    /// <returns></returns>
    public Opt<T> Deserialze<T>(Stream stream)
    {
        return Deserialze(stream) is T t ? Opt.Some(t) : Opt.None<T>();
    }

    public void Deserialze(T obj, Stream stream)
    {
        obj.ThrowIfNull();
        stream.ThrowIfNull();

        stream.Position = 0L;
        var reader = new BinaryReader(stream);
        reader.ReadToObject(obj, GetMembers());
    }

    public IEnumerable<(MemberInfo, object)> DeserializeObjectMembers(Stream stream)
    {
        stream.ThrowIfNull();
        stream.Position = 0L;
        var reader = new BinaryReader(stream);
        return reader.ReadFromMembers(GetMembers());
    }

    /// <summary>
    /// Filters fields and properties only.
    /// </summary>
    /// <param name="memberInfos"></param>
    /// <returns></returns>
    private static IEnumerable<MemberInfo> FilterMutableMembers(IEnumerable<MemberInfo> memberInfos)
    {
        return memberInfos.Where(mi =>
        {
            return mi switch
            {
                FieldInfo fi => !fi.IsInitOnly,
                PropertyInfo pi => pi.CanWrite,
                _ => false
            };
        });
    }

    private IEnumerable<MemberInfo> GetMembers()
    {
        if (null != _memberCache) return _memberCache;

        var members = FilterMutableMembers(typeof(T).GetMembers());

        if(null != _memberNames && 0 < _memberNames.Count)
        {
            members = members.Where(member => _memberNames.Contains(member.Name));
        }

        _memberCache = members.ToArray();
        return _memberCache;
    }

    public void Serialze(object obj, Stream stream)
    {
        if (null == obj) throw new ArgumentNullException(nameof(obj));
        if (null == stream) throw new ArgumentNullException(nameof(stream));

        var writer = new BinaryWriter(stream);
        var members = GetMembers();

        writer.WriteObject(obj, members);
    }
}
