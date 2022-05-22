namespace Foundation
{
    public static class RangeExtensions
    {
        /// <summary>
        /// Checks if value is within Start and End. Index from end are not supported.
        /// </summary>
        /// <param name="range"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInRange(this System.Range range, int value)
        {
            if (range.Start.Value > value) return false;

            if (range.End.IsFromEnd) 
                return 0 == range.End.Value;

            return range.End.Value >= value;
        }
    }
}
