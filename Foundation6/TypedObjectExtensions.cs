namespace Foundation
{
    public static class TypedObjectExtensions
    {
        public static bool IsNull<T>(this T? obj) => obj == null;
    }
}
