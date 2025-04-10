﻿using FluentAssertions;
using NUnit.Framework;
using System;

namespace Foundation;

[TestFixture]
public class ResultTests
{
    [Test]
    public void FromOption_Should_ReturnErrorResult_When_TypeIsInvalid()
    {
        object value = 5;

        var error = new Error("test error", "this is a test error");
        var sut = Result.FromOption(() => Option.MaybeOfType<string>(value), () => error);

        sut.IsOk.Should().BeFalse();
        sut.TryGetError(out var err).Should().BeTrue();
        err.Should().Be(error);
    }

    [Test]
    public void FromOption_Should_ReturnOkResult_When_TypeIsValid()
    {
        object value = 5;

        var sut = Result.FromOption(() => Option.MaybeOfType<int>(value));

        sut.IsOk.Should().BeTrue();
        sut.TryGetOk(out int ok).Should().BeTrue();
        ok.Should().Be((int)value);
    }

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
    public void Maybe_Should_ReturnErrorResult_When_TypeIsNotOfTypeInt()
    {
        object value = 5;

        var sut = Result.Maybe<string>(value);

        sut.IsOk.Should().BeFalse();
    }

    [Test]
    public void Maybe_Should_ReturnErrorResult_When_ResultHasOneGenericParameterAndOneMethodArgument()
    {
        // Arrange
        object value = null!;

        // Act
        var sut = Result.Maybe<string>(value);

        // ClassicAssert
        sut.IsOk.Should().BeFalse();
    }

    [Test]
    public void Maybe_Should_ReturnErrorResult_When_ResultHasOneGenericParameterAndTwoMethodArgument()
    {
        // Arrange
        object value = null!;
        var exceptionMessage = "test";

        // Act
        var sut = Result.Maybe<string>(value, () => new ArgumentException(exceptionMessage));

        // ClassicAssert
        sut.IsOk.Should().BeFalse();
    }

    [Test]
    public void Maybe_Should_ReturnOkResult_When_ResultHasOneGenericParameterAndTypeIsInt()
    {
        object value = 5;

        var sut = Result.Maybe<int>(value);

        sut.IsOk.Should().BeTrue();
        sut.TryGetOk(out int ok).Should().BeTrue();
        ok.Should().Be((int)value);
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

        Assert.True(sut.TryGetError(out ArgumentException? error));
        Assert.AreEqual(exception, error);
    }

    [Test]
    public void TryGetOk_Should_ReturnAValue_When_IsOKIsTrue()
    {
        var value = 5;
        var sut = Result.Ok(value);

        Assert.IsTrue(sut.TryGetOk(out int ok));
        Assert.AreEqual(value, ok);

        var user = Result.Ok("bob");
        var pwd = Result.Ok("secret");

        var isAuthorized = user.TryGetOk(out var usr)
                        && pwd.TryGetOk(out var pw)
                        && login(usr, pw);
        
        bool login(string username, string password)
        {
            return username == "bob" && password == "secret";
        }
    }
}
