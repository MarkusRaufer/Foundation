using Foundation.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation;

public static class ByteStringExtensions
{
    public static ByteString Concat(this ByteString byteString, IEnumerable<ByteString> byteStrings)
    {
        return ByteStringHelper.Concat(byteStrings.InsertAt(byteString, 0));
    }
}
