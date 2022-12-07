using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation;

[TestFixture]
public class ResultTests
{
    [Test]
    public void Error_Should_IsOkFalse_When_UsedWithError()
    {
        var exception = new ArgumentException("error");
        var sut = Result.Error(exception);
        Assert.False(sut.IsOk);
        Assert.AreEqual(exception, sut.Error);
    }

    [Test]
    public void Error_Should_IsOkFalse_When_GenericUsedWithError()
    {
        var error = "error";
        var sut = Result.Error<int, string>(error);
        Assert.False(sut.IsOk);
        Assert.AreEqual(error, sut.Error);
    }

    [Test]
    public void Ok_Should_IsOkTrue_When_UsedWithoutValue()
    {
        var sut = Result.Ok();
        Assert.True(sut.IsOk);
    }

    [Test]
    public void Ok_Should_IsOkTrue_When_UsedWithValue()
    {
        var value = 5;
        var sut = Result.Ok<int, string>(value);
        Assert.True(sut.IsOk);
        Assert.AreEqual(value, sut.Ok);
    }
}
