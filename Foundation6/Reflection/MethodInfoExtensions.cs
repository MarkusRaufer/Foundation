using System.Reflection;
using System.Runtime.CompilerServices;

namespace Foundation.Reflection;

public static class MethodInfoExtensions
{
    private static readonly Type ExtensionAttributeType = typeof(ExtensionAttribute);
    
    public static bool IsExtensionMethod(this MethodInfo methodInfo) => methodInfo.CustomAttributes.Any(x => x.AttributeType == ExtensionAttributeType);
}
