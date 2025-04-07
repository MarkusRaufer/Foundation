using NUnit.Framework;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Foundation.Timers;

[TestFixture]
public class CountdownTimerTests
{
    [Test]
    public void IntervalElapsed_Should_BeCalledOnce_When_Subscribed()
    {
        var sut = new CountdownTimer(duration: TimeSpan.FromSeconds(2), interval: TimeSpan.FromSeconds(1));

        var onElapsedCalled = 0;
        
        void onElapsed(TimeSpan span)
        {
            onElapsedCalled++;
        }

        var zero = TimeSpan.Zero;

        sut.IntervalElapsed.Subscribe(onElapsed);
        sut.Start();
        
        while (!sut.HasReachedZero)
        {
            Task.Delay(TimeSpan.FromSeconds(1));
        }
        onElapsedCalled.ShouldBe(1);
    }

    [Test]
    public void ZeroReached_Should_BeCalledOnce_When_Subscribed()
    {
        var sut = new CountdownTimer(duration: TimeSpan.FromSeconds(1), interval: TimeSpan.FromSeconds(1));

        var onElapsedCalled = 0;

        void onIntervalElapsed(TimeSpan span)
        {
            onElapsedCalled++;
        }

        var onZeroReachedCalled = 0;

        void onZeroReached()
        {
            onZeroReachedCalled++;
        }

        var zero = TimeSpan.Zero;

        sut.IntervalElapsed.Subscribe(onIntervalElapsed);
        sut.ZeroReached.Subscribe(onZeroReached);

        sut.Start();

        while (!sut.HasReachedZero)
        {
            Task.Delay(TimeSpan.FromSeconds(1));
        }
        onElapsedCalled.ShouldBe(1);
        onZeroReachedCalled.ShouldBe(1);
    }
}
