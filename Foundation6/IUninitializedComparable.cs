namespace Foundation;

public interface IUninitializedComparable
{
    /// <summary>
    /// This is the return value if a comparable is uninitialized. Default of Microsoft is -1;
    /// Means that in a sorted list the comparable appears at the beginning or at the end of the list. 
    /// </summary>
    int CompareValueIfEmpty { get; }
}

