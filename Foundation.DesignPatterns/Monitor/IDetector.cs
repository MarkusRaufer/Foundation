namespace Foundation.DesignPatterns.Monitor;

/// <summary>
/// A detector can be monitored by somebody.
/// The detector calls the monitors delegate (Monitor) to inform the monitor on detection.
/// </summary>
/// <typeparam name="TDelegate"></typeparam>
public interface IDetector<TDelegate> where TDelegate : Delegate
{
    /// <summary>
    /// This delegate is called on the monitor.
    /// </summary>
    TDelegate? Monitor { get; set; }
}
