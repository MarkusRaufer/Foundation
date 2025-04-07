using System.Timers;
using Timer = System.Timers.Timer;

namespace Foundation.Timers;

public class CountdownTimer
{
    public Event<Action<TimeSpan>> IntervalElapsed = new();
    public Event<Action> ZeroReached = new();

    private readonly Timer _timer;

    public CountdownTimer(TimeSpan duration, TimeSpan interval)
    {
        Duration = duration;
        RemainingDuration = duration;
        Interval = interval;

        _timer = new Timer(interval.TotalMilliseconds);
        _timer.Elapsed += OnTimerIntervalElapsed;
        _timer.AutoReset = true;
        _timer.Enabled = true;

        HasReachedZero = false;
    }

    public CountdownTimer(Timer timer, TimeSpan duration)
    {
        _timer = timer.ThrowIfNull();
        Duration = duration;
        RemainingDuration = duration;

        _timer.Elapsed += OnTimerIntervalElapsed;
        _timer.AutoReset = true;
        _timer.Enabled = true;

        Interval = TimeSpan.FromMilliseconds(_timer.Interval);

        HasReachedZero = false;
    }

    public TimeSpan Duration { get; }

    public bool HasReachedZero { get; private set; }

    public TimeSpan RemainingDuration { get; private set; }

    public TimeSpan Interval { get; }

    private void OnTimerIntervalElapsed(object? sender, ElapsedEventArgs e)
    {
        RemainingDuration = RemainingDuration.Subtract(Interval);
        
        if (RemainingDuration.Ticks <= 0)
        {
            _timer.Stop();
            RemainingDuration = TimeSpan.Zero;
            HasReachedZero = true;
            ZeroReached.Publish();
        }

        IntervalElapsed.Publish(RemainingDuration);
    }

    public void Start() => _timer.Start();

    public void Stop() => _timer.Stop();
}
