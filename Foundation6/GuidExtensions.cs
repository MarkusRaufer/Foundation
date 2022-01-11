namespace Foundation;

using System.Diagnostics.CodeAnalysis;

public static class GuidExtensions
{
    public static ref Guid ThrowIfEmpty(this ref Guid guid, [DisallowNull] string argumentName)
    {
        if(Guid.Empty == guid) throw new ArgumentNullException(argumentName);
        return ref guid;
    }
}
