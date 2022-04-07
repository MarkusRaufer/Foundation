namespace Foundation.Collections.Generic
{
    public static class HashChainHelper
    {
        public static bool IsConsistant<T>(IEnumerable<HashChainElement<T>> elems)
            where T : notnull
        {
            return IsConsistant(elems, elem => elem.PreviousElementHash);
        }

        /// <summary>
        /// Checks the consistancy of hash chain elements.
        /// </summary>
        /// <typeparam name="T">T must contain the hash code of the previous element. This hash code must not be 0 except for the first element.</typeparam>
        /// <param name="elems">elements of a hash chain</param>
        /// <param name="getPrevElementHash">returns the hash code of the previous element of a hash chain.</param>
        /// <returns></returns>
        public static bool IsConsistant<T>(IEnumerable<T> elems, Func<T, int> getPrevElementHash)
            where T : notnull
        {
            var  prevHash = 0;

            foreach (var elem in elems)
            {
                if(elem is null) return false;

                var currentElemHash = elem.GetHashCode();
                var currentElemPrevHash = getPrevElementHash(elem);
                if (0 == prevHash)
                {
                    if (0 == currentElemHash || 0 != currentElemPrevHash) return false;

                    prevHash = currentElemHash;
                    continue;
                }

                if (currentElemPrevHash != prevHash) return false;

                prevHash = currentElemHash;
            }

            return true;
        }
    }
}
