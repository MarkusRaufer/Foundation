using System.Reflection;

namespace Foundation.IO
{
    public static class ObjectExtensions
    {
        public static byte[] ToByteArray(this object obj)
        {
            obj.ThrowIfNull(nameof(obj));

            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            writer.WriteObject(obj);
            return stream.ToArray();
        }

        public static byte[] ToByteArray(this object obj, IEnumerable<string> memberNames)
        {
            obj.ThrowIfNull(nameof(obj));

            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            writer.WriteObject(obj, memberNames);
            return stream.ToArray();
        }

        public static byte[] ToByteArray(this object obj, IEnumerable<MemberInfo> members)
        {
            obj.ThrowIfNull(nameof(obj));

            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            writer.WriteObject(obj, members);
            return stream.ToArray();
        }
    }
}
