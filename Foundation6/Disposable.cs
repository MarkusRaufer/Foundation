namespace Foundation;

public sealed class Disposable : IDisposable
{
    private readonly Action[] _disposeActions;
    private bool _disposed;

    public Disposable(params Action[] disposeActions)
    {
        _disposeActions = disposeActions;
    }

    ~Disposable()
    {
        Dispose();
    }

    public void Dispose()
    {
        if (_disposed) return;

        _disposed = true;
        foreach (var action in _disposeActions)
            action();

        GC.SuppressFinalize(this);
    }
}
