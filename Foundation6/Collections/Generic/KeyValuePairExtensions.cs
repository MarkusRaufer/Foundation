namespace Foundation.Collections.Generic
{
    public static class KeyValuePairExtensions
    {
        public static KeyValuePair<TKey, TValue> ThrowIfEmpty<TKey, TValue>(this KeyValuePair<TKey, TValue> pair, string argumentName)
        {
            return null != pair.Key ? pair : throw new ArgumentNullException(argumentName);
        }
    }
}
