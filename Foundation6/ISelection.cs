namespace Foundation;

/// <summary>
/// Pattern for selections.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface ISelection<T>
{
    IEnumerable<T> SelectedValues { get; }
}

