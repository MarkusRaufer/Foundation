namespace Foundation.Collections.Generic
{
    public static class HashChainHelper
    {
        public static bool IsConsistent<T>(IEnumerable<T> elements, Func<T, Option<int>> getPrevElementHash)
            where T : notnull
        {
            return IsConsistent(elements, elem => elem.GetHashCode(), getPrevElementHash);
        }

        public static bool IsConsistent<T>(
            IEnumerable<T> elements, 
            Func<T, int> getHash, 
            Func<T, Option<int>> getPrevElementHash)
            where T : notnull
        {
            return IsConsistent<T, int>(elements, getHash, getPrevElementHash);
        }

        public static bool IsConsistent<T, THash>(
            IEnumerable<T> elems, 
            Func<T, THash> getElementHash, 
            Func<T, Option<THash>> getPrevElementHash)
            where T : notnull
            where THash : notnull
        {
            var prevHash = Option.None<THash>();

            foreach (var elem in elems)
            {
                if (elem is null) return false;

                var currentElemHash = getElementHash(elem);
                if(currentElemHash is null) return false;

                var currentElemPrevHash = getPrevElementHash(elem);

                if (prevHash.IsNone)
                {
                    if(currentElemPrevHash.IsSome) return false;

                    prevHash = Option.Some(currentElemHash);
                    continue;
                }

                if (currentElemPrevHash.IsNone) return false;

                if (currentElemPrevHash != prevHash) return false;

                prevHash = currentElemHash;
            }

            return true;
        }
    }
}
