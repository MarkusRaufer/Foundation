namespace Foundation.DesignPatterns.Monitor;

/// <summary>
/// The monitor gets informed by detectors.
/// </summary>
/// <typeparam name="TDelegate"></typeparam>
public interface IMonitor<TDelegate> where TDelegate : Delegate
{
    void Attach(IDetector<TDelegate> detector);
    void Detach(IDetector<TDelegate> detector);
}
