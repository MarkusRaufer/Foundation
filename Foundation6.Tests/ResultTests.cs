using NUnit.Framework;
using System;

namespace Foundation;

[TestFixture]
public class ResultTests
{
    [Test]
    public void IsOk_Should_ReturnFalse_When_ResultContainsAnError()
    {
        var sut = Result.Error(new ArgumentException());

        Assert.False(sut.IsOk);
    }

    [Test]
    public void IsOk_Should_ReturnTrue_When_ResultIsOk()
    {
        var sut = Result.Ok();

        Assert.True(sut.IsOk);
    }

    [Test]
    public void OkOrThrow_Should_ReturnAValue_When_IsOKIsTrue()
    { 
        var value = 5;
        var sut = Result.Ok<int>(value);

        Assert.IsTrue(sut.TryGetOk(out int ok));
        Assert.AreEqual(value, ok);
    }

    [Test]
    public void OkOrThrow_Should_ThrowAnException_When_IsOKIsFalse()
    {
        var sut = Result.Error<int, Exception>(new ArgumentOutOfRangeException());

        Assert.Throws<ArgumentOutOfRangeException>(() => sut.OkOrThrow());
    }

    [Test]
    public void Ok_Should_IsOkTrue_When_UsedWithValue()
    {
        var value = 5;
        var sut = Result.Ok<int, string>(value);

        Assert.True(sut.IsOk);
        Assert.AreEqual(value, sut.ToOk());
    }

    [Test]
    public void ToError_Should_ReturnTrue_When_ResultContainsAnError()
    {
        var exception = new ArgumentException("error");
        var sut = Result.Error(exception);

        Assert.AreEqual(exception, sut.ToError());
    }

    [Test]
    public void ToError_Should_ThrowException_When_ResultContainsNoError()
    {
        var sut = Result.Ok();

        Assert.Throws<ArgumentException>(() => sut.ToError());
    }

    [Test]
    public void TryGetError_Should_ReturnFalse_When_ResultContainsNoError()
    {
        var sut = Result.Ok();

        Assert.False(sut.TryGetError(out Error? _));
    }

    [Test]
    public void TryGetError_Should_ReturnTrue_When_ResultContainsAnError()
    {
        var exception = new ArgumentException("error");
        var sut = Result.Error(exception);

        Assert.True(sut.TryGetError(out Error? error));
        Assert.AreEqual(exception, error);
    }
}
