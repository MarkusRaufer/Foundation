using NUnit.Framework;
using Shouldly;
using System;
using System.Threading.Tasks;

namespace Foundation.Timers;

[TestFixture]
public class CountdownTimerTests
{
    [Test]
    public void IntervalElapsed_Should_BeCalledOnce_When_Subscribed()
    {
        using var sut = new CountdownTimer(duration: TimeSpan.FromSeconds(2), interval: TimeSpan.FromSeconds(1));

        var onElapsedCalled = 0;
        
        void onElapsed(TimeSpan span)
        {
            onElapsedCalled++;
        }

        var zero = TimeSpan.Zero;

        sut.IntervalElapsed.Subscribe(onElapsed);
        sut.Start();
       
        var times = 0;
        while (times < 3)
        {
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            times++;
        }
        
        
        onElapsedCalled.ShouldBe(2);
    }

    [Test]
    public void ZeroReached_Should_BeCalledOnce_When_Subscribed()
    {
        using var sut = new CountdownTimer(duration: TimeSpan.FromSeconds(1), interval: TimeSpan.FromSeconds(1));

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

        var times = 0;
        while (times < 2)
        {
            Task.Delay(TimeSpan.FromSeconds(1)).Wait();
            times++;
        }

        onElapsedCalled.ShouldBe(1);
        onZeroReachedCalled.ShouldBe(1);
    }
}
