using Foundation.Reflection;
using System.Linq.Expressions;
using System.Reflection;

namespace Foundation.IO;

public static class BinarySerializer
{
    public static BinarySerializer<T> New<T>(
        Func<IDictionary<string, object>, T> factory,
        params Expression<Func<T, object>>[] members)
    {
        return new (factory, new MemberInfoCash<T>(members));
    }
}

public class BinarySerializer<T>
{
    private readonly Func<IDictionary<string, object>, T> _factory;
    private readonly BinaryObjectReader<T> _reader;
    private readonly BinaryObjectWriter<T> _writer;

    public BinarySerializer(Func<IDictionary<string, object>, T> factory)
        : this(factory, new MemberInfoCash<T>(typeof(T).GetProperties()))
    {
    }

    public BinarySerializer(
        Func<IDictionary<string, object>, T> factory,
        MemberInfo[] members) : this(factory, new MemberInfoCash<T>(members))
    {
    }

    public BinarySerializer(
        Func<IDictionary<string, object>, T> factory,
        string[] memberNames) : this(factory, new MemberInfoCash<T>(memberNames))
    {
    }

    internal BinarySerializer(
        Func<IDictionary<string, object>, T> factory,
        MemberInfoCash<T> memberCash)
    {
        _factory = factory.ThrowIfNull();
        
        memberCash.ThrowIfNull();

        _reader = new BinaryObjectReader<T>(memberCash);
        _writer = new BinaryObjectWriter<T>(memberCash);
    }

    /// <summary>
    /// Deserializes an object using values from <paramref name="stream"/>. If stream does not contain values an exception is thrown.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public T Deserialize(Stream stream)
    {
        stream.ThrowIfNull();

        return _factory(DeserializeKeyValues(stream));
    }

    public IDictionary<string, object> DeserializeKeyValues(Stream stream)
    {
        stream.ThrowIfNull();

        var keyValues = _reader.ReadKeyValues(stream);

        return keyValues.ToDictionary(kv => kv.Key, kv => kv.Value);
    }


    public void Serialize(T obj, Stream stream)
    {
        obj.ThrowIfNull();
        stream.ThrowIfNull();

        _writer.WriteObject(stream, obj);
    }
}
