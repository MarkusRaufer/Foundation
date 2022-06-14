namespace Foundation
{
    public static class MinMaxExtensions
    {
        public static System.Range ToRange(this MinMax<int> minMax)
        {
            return new System.Range(new Index(minMax.Min), new Index(minMax.Max));
        }
    }
}
