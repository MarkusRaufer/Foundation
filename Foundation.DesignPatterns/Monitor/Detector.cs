namespace Foundation.DesignPatterns.Monitor;

public class Detector<TDelegate> : IDetector<TDelegate>
    where TDelegate : Delegate
{
    public TDelegate? Monitor { get; set; }
}
