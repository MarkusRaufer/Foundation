namespace Foundation.TestUtil.Runtime.Serialization;

using System.Runtime.Serialization.Formatters.Binary;

public static class Serializer
{
#pragma warning disable SYSLIB0011 // Type or member is obsolete
    public static void Serialize(this Stream stream, object graph)
    {
        var formatter = new BinaryFormatter();

        formatter.Serialize(stream, graph);
    }

    public static object Deserialize(this Stream stream)
    {
        if(0 != stream.Position) stream.Position = 0;

        var formatter = new BinaryFormatter();

        return formatter.Deserialize(stream);
    }
#pragma warning restore SYSLIB0011 // Type or member is obsolete
}
