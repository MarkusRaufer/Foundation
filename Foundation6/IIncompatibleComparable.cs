namespace Foundation;

public interface IIncompatibleComparable
{
    /// <summary>
    /// This is the return value if a comparable is incompatible. The value can be -1 or 1. 
    /// Means that in a sorted list the comparable appears at the beginning or at the end of the list. 
    /// </summary>
    int CompareValueIfIncompatible { get; }
}

