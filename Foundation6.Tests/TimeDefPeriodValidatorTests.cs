using NUnit.Framework;
using System;

namespace Foundation;

[TestFixture]
public class TimeDefPeriodValidatorTests
{
    [Test]
    public void Validate_Should_ReturnFalse_When_TimeDef_Is_DateOnly()
    {
        var sut = new TimeDefPeriodValidator();

        var dateSpan = new TimeDef.DateSpan(new(2015, 6, 1), new(2022, 6, 30));

        var start = new DateTime(2015, 6, 1);
        var end = new DateTime(2015, 8, 1);

        Assert.IsFalse(sut.IsValid(dateSpan, Period.New(start, end)));
    }

    [Test]
    public void Validate_Should_ReturnFalse_When_TimeDef_In_Hour()
    {
        var sut = new TimeDefPeriodValidator();

        var hours = new TimeDef.Hours(8);

        var start = new DateTime(2015, 6, 1, 7, 0, 0);
        var end = new DateTime(2015, 6, 1, 18, 0, 0);

        Assert.IsFalse(sut.IsValid(hours, Period.New(start, end)));
    }

    [Test]
    public void Validate_Should_ReturnFalse_When_TimeDef_In_Minute()
    {
        var sut = new TimeDefPeriodValidator();

        var minute = new TimeDef.Minutes(30);

        var start = new DateTime(2015, 6, 1, 7, 0, 0);
        var end = new DateTime(2015, 6, 1, 7, 30, 0);

        Assert.IsTrue(sut.IsValid(minute, Period.New(start, end)));
    }

    [Test]
    public void Validate_Should_ReturnTrue_When_Comples_TimeDef()
    {
        var sut = new TimeDefPeriodValidator();

        var year = new TimeDef.Year(new[] { 2015 });
        var day = new TimeDef.Day(new[] { 1, 15 });
        var weekday = new TimeDef.Weekday(new[] { DayOfWeek.Monday, DayOfWeek.Wednesday });
        var timedef = new TimeDef.And(day, new TimeDef.And(weekday, year));

        var start = new DateTime(2015, 6, 1);
        var end = new DateTime(2018, 7, 1);

        Assert.IsTrue(sut.IsValid(timedef, Period.New(start, end)));
    }

    [Test]
    public void Validate_Should_ReturnTrue_When_TimeDef_Is_DateOnly()
    {
        var sut = new TimeDefPeriodValidator();

        var dateSpan = new TimeDef.DateSpan(From: new (2015, 6, 1), To: new (2015, 8, 1));

        var start = new DateTime(2015, 1, 1);
        var end = new DateTime(2015, 12, 31);

        Assert.IsTrue(sut.IsValid(dateSpan, Period.New(start, end)));
    }
    
    [Test]
    public void Validate_Should_ReturnTrue_When_TimeDef_In_Hour()
    {
        var sut = new TimeDefPeriodValidator();

        var hour = new TimeDef.Hour(new [] { 8, 12 });

        var start = new DateTime(2015, 6, 1, 7, 0, 0);
        var end = new DateTime(2015, 6, 1, 18, 0, 0);

        Assert.IsTrue(sut.IsValid(hour, Period.New(start, end)));
    }

    [Test]
    public void Validate_Should_ReturnTrue_When_TimeDef_In_Minute()
    {
        var sut = new TimeDefPeriodValidator();

        var minute = new TimeDef.Minute(new [] { 30, 45 });

        var start = new DateTime(2015, 6, 1, 7, 0, 0);
        var end = new DateTime(2015, 6, 1, 8, 0, 0);

        Assert.IsTrue(sut.IsValid(minute, Period.New(start, end)));
    }

    [Test]
    public void Validate_Should_ReturnTrue_When_TimeDef_In_Month()
    {
        var sut = new TimeDefPeriodValidator();

        var month = new TimeDef.Month(new [] { Month.Jun, Month.Aug });

        var start = new DateTime(2015, 6, 1);
        var end = new DateTime(2015, 10, 1);

        Assert.IsTrue(sut.IsValid(month, Period.New(start, end)));
    }

    [Test]
    public void Validate_Should_ReturnTrue_When_TimeDef_In_Weekday()
    {
        var sut = new TimeDefPeriodValidator();

        var weekday = new TimeDef.Weekday(new [] { DayOfWeek.Monday, DayOfWeek.Wednesday });

        var start = new DateTime(2015, 6, 1);
        var end = new DateTime(2015, 6, 7);

        Assert.IsTrue(sut.IsValid(weekday, Period.New(start, end)));
    }

    [Test]
    public void Validate_Should_ReturnTrue_When_TimeDef_In_Year()
    {
        var sut = new TimeDefPeriodValidator();

        var year = new TimeDef.Year(new [] { 2015, 2017, 2018 });

        var start = new DateTime(2015, 6, 1);
        var end = new DateTime(2018, 7, 1);

        Assert.IsTrue(sut.IsValid(year, Period.New(start, end)));
    }


}
