using System.Timers;
using Timer = System.Timers.Timer;

namespace Foundation.Timers;

/// <summary>
/// Counts a duration of time down.
/// </summary>
public sealed class CountdownTimer : IDisposable
{
    private readonly Timer _timer;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="duration">The duration of the count down (e.g. 10 seconds).</param>
    /// <param name="interval">The interval of the count down (e.g. 1 second).</param>
    public CountdownTimer(TimeSpan duration, TimeSpan interval)
    {
        Duration = duration;
        RemainingDuration = duration;
        Interval = interval;

        _timer = new Timer(interval.TotalMilliseconds);
        _timer.Elapsed += OnTimerIntervalElapsed;
    }

    public CountdownTimer(Timer timer, TimeSpan duration)
    {
        _timer = timer.ThrowIfNull();
        Duration = duration;
        RemainingDuration = duration;

        _timer.Elapsed += OnTimerIntervalElapsed;

        Interval = TimeSpan.FromMilliseconds(_timer.Interval);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Stop();

        _timer.Dispose();
        IntervalElapsed.Dispose();
        ZeroReached.Dispose();
    }

    /// <summary>
    /// The duration of the count down.
    /// </summary>
    public TimeSpan Duration { get; }

    /// <summary>
    /// Event which is called on each elapsed interval.
    /// </summary>
    public Event<Action<TimeSpan>> IntervalElapsed { get; } = new();

    /// <summary>
    /// The remaining duration which will be decreased on every elapsed interval.
    /// </summary>
    public TimeSpan RemainingDuration { get; private set; }

    /// <summary>
    /// The interval of the count down.
    /// </summary>
    public TimeSpan Interval { get; }

    private void OnTimerIntervalElapsed(object? sender, ElapsedEventArgs e)
    {
        RemainingDuration = RemainingDuration.Subtract(Interval);

        var zeroReached = RemainingDuration.Ticks <= 0;

        if (zeroReached)
        {
            Stop();
            RemainingDuration = TimeSpan.Zero;
        }

        IntervalElapsed.Publish(RemainingDuration);
        if (zeroReached) ZeroReached.Publish();
    }

    /// <summary>
    /// Starts the timer in another thread.
    /// </summary>
    public void Start()
    {
        _timer.AutoReset = true;
        _timer.Enabled = true;
        _timer.Start();
    }

    /// <summary>
    /// Stops the timer.
    /// </summary>
    public void Stop()
    {
        _timer.Stop();
        _timer.AutoReset = false;
        _timer.Enabled = false;
    }

    /// <summary>
    /// Event is called once when the count down reached zero.
    /// </summary>
    public Event<Action> ZeroReached { get; } = new();
}
