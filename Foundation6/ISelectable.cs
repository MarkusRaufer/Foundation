namespace Foundation;

public interface ISelectable
{
    event Action<ISelectable> SelectionChanged;
    bool IsSelected { get; set; }
}

