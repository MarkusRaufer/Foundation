using System.Runtime.CompilerServices;

namespace Foundation;

public static class GuidExtensions
{
    public static ref Guid ThrowIfEmpty(this ref Guid guid, [CallerArgumentExpression("guid")] string argumentName = "")
    {
        if(Guid.Empty == guid) throw new ArgumentNullException(argumentName);
        return ref guid;
    }
}
