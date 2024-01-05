using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation;

public class ByteStringHelper
{
    public static ByteString Concat(IEnumerable<ByteString> byteStrings)
    {
        var length = byteStrings.Select(x => x.Length).Sum();
        var bytes = new byte[length];

        var index = 0;
        foreach (var byteString in byteStrings)
        {
            var target = new Span<byte>(bytes, index, byteString.Length);
            byteString.AsSpan().CopyTo(target);
            index += byteString.Length;
        }

        return new ByteString(bytes);
    }
}
