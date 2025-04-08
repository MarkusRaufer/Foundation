using FluentAssertions;
using NUnit.Framework;
using System;

namespace Foundation;

[TestFixture]
public class SpanExtensionsTests
{
    [Test]
    public void IsEmptyOrWhiteSpace_Should_ReturnFalse_When_String_IsNotEmpty_And_NotWhiteSpaceOnly()
    {
        var str = "test";
        var span = str.AsSpan();
        span.IsEmptyOrWhiteSpace().Should().BeFalse();
    }

    [Test]
    public void IsEmptyOrWhiteSpace_Should_ReturnTrue_When_StringIsEmpty()
    {
        var str = string.Empty;
        var span = str.AsSpan();
        span.IsEmptyOrWhiteSpace().Should().BeTrue();
    }

    [Test]
    public void IsEmptyOrWhiteSpace_Should_ReturnTrue_When_StringIsWhiteSpace()
    {
        var str = "    ";
        var span = str.AsSpan();
        span.IsEmptyOrWhiteSpace().Should().BeTrue();
    }
}
