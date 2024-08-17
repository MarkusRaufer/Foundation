using FluentAssertions;
using NUnit.Framework;
using System;

namespace Foundation;

[TestFixture]
public class Iso8601PeriodTests
{
    [Test]
    public void TryParse_Should_Returns_1Day_When_StringIs_1Day()
    {
        var str = "PT1D";

        Iso8601Period.TryParse(str, out var result).Should().BeTrue();

        var expected = TimeSpan.FromDays(1);
        result.Should().Be(expected);
    }

    [Test]
    public void TryParse_Should_Returns_12Days_When_StringIs_12Days()
    {
        var str = "PT12D";

        Iso8601Period.TryParse(str, out var result).Should().BeTrue();

        var expected = TimeSpan.FromDays(12);
        result.Should().Be(expected);
    }

    [Test]
    public void TryParse_Should_Returns_10Days_23Hours_34Minutes_45Seconds_56MilliSeconds_When_StringIs_10Days_23Hours_34Minutes_45Seconds_56MilliSeconds()
    {
        var str = "P10D23H34M45S56F";

        Iso8601Period.TryParse(str, out var result).Should().BeTrue();

        var expected = TimeSpan.FromDays(10)
                               .Add(TimeSpan.FromHours(23))
                               .Add(TimeSpan.FromMinutes(34))
                               .Add(TimeSpan.FromSeconds(45))
                               .Add(TimeSpan.FromMilliseconds(56));
        
        result.Should().Be(expected);
    }

    [Test]
    public void TryParse_Should_Returns_Negative_10Days_23Hours_34Minutes_45Seconds_56MilliSeconds_When_StringIs_10Days_23Hours_34Minutes_45Seconds_56MilliSeconds()
    {
        var str = "-P10D23H34M45S56F";

        Iso8601Period.TryParse(str, out var result).Should().BeTrue();

        var expected = TimeSpan.Zero
                               .Subtract(TimeSpan.FromDays(10))
                               .Subtract(TimeSpan.FromHours(23))
                               .Subtract(TimeSpan.FromMinutes(34))
                               .Subtract(TimeSpan.FromSeconds(45))
                               .Subtract(TimeSpan.FromMilliseconds(56));

        result.Should().Be(expected);
    }

    [Test]
    public void TryParse_Should_Returns_1Hour_When_StringIs_1Hour()
    {
        var str = "PT1H";

        Iso8601Period.TryParse(str, out var result).Should().BeTrue();

        var expected = TimeSpan.FromHours(1);
        result.Should().Be(expected);
    }

    [Test]
    public void TryParse_Should_Returns_12Hour_When_StringIs_12Hour()
    {
        var str = "PT12H";

        Iso8601Period.TryParse(str, out var result).Should().BeTrue();

        var expected = TimeSpan.FromHours(12);
        result.Should().Be(expected);
    }

    [Test]
    public void TryParse_Should_Returns_1Hour23Minutes45Seconds67MilliSeconds_When_StringIs_1Hour23Minutes45Seconds67MilliSeconds()
    {
        var str = "PT1H23M45S67F";

        Iso8601Period.TryParse(str, out var result).Should().BeTrue();

        var expected = TimeSpan.FromHours(1).Add(TimeSpan.FromMinutes(23)).Add(TimeSpan.FromSeconds(45)).Add(TimeSpan.FromMilliseconds(67));
        result.Should().Be(expected);
    }

    [Test]
    public void TryParse_Should_Returns_1MilliSecond_When_StringIs_1MilliSecond()
    {
        var str = "PT1F";

        Iso8601Period.TryParse(str, out var result).Should().BeTrue();

        var expected = TimeSpan.FromMilliseconds(1);
        result.Should().Be(expected);
    }

    [Test]
    public void TryParse_Should_Returns_23MilliSecond_When_StringIs_23MilliSecond()
    {
        var str = "PT23F";

        Iso8601Period.TryParse(str, out var result).Should().BeTrue();

        var expected = TimeSpan.FromMilliseconds(23);
        result.Should().Be(expected);
    }

    [Test]
    public void TryParse_Should_Returns_1Minute_When_StringIs_1Minute()
    {
        var str = "PT1M";

        Iso8601Period.TryParse(str, out var result).Should().BeTrue();

        var expected = TimeSpan.FromMinutes(1);
        result.Should().Be(expected);
    }

    [Test]
    public void TryParse_Should_Returns_23Minute_When_StringIs_23Minute()
    {
        var str = "PT23M";

        Iso8601Period.TryParse(str, out var result).Should().BeTrue();

        var expected = TimeSpan.FromMinutes(23);
        result.Should().Be(expected);
    }

    [Test]
    public void TryParse_Should_Returns_1Minute23Seconds45MilliSeconds_When_StringIs_1Minute23Seconds45MilliSeconds()
    {
        var str = "PT1M23S45F";

        Iso8601Period.TryParse(str, out var result).Should().BeTrue();

        var expected = TimeSpan.FromMinutes(1).Add(TimeSpan.FromSeconds(23)).Add(TimeSpan.FromMilliseconds(45));
        result.Should().Be(expected);
    }

    [Test]
    public void TryParse_Should_Returns_1Second_When_StringIs_1Second()
    {
        var str = "PT1S";

        Iso8601Period.TryParse(str, out var result).Should().BeTrue();

        var expected = TimeSpan.FromSeconds(1);
        result.Should().Be(expected);
    }

    [Test]
    public void TryParse_Should_Returns_23Second_When_StringIs_23Second()
    {
        var str = "PT23S";

        Iso8601Period.TryParse(str, out var result).Should().BeTrue();

        var expected = TimeSpan.FromSeconds(23);
        result.Should().Be(expected);
    }

    [Test]
    public void TryParse_Should_Returns_1SecondAnd234MilliSeconds_When_StringIs_1SecondAnd234MilliSeconds()
    {
        var str = "PT1S234F";

        Iso8601Period.TryParse(str, out var result).Should().BeTrue();

        var expected = TimeSpan.FromSeconds(1).Add(TimeSpan.FromMilliseconds(234));
        result.Should().Be(expected);
    }

}
