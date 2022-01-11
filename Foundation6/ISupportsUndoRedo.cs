namespace Foundation;

public interface ISupportsUndoRedo
{
    bool CanRedo { get; }
    bool CanUndo { get; }
    void Redo();
    void Undo();
}

