using FluentAssertions;
using NUnit.Framework;
using System;

namespace Foundation;

[TestFixture]
public class QuantityTests
{
    [Test]
    public void Parse_Should_ReturnQuantity_When_NoGenericArgumentIsUsed()
    {
        //Arrange
        var quantity = new Quantity("kg", 123.45M);
        var str = quantity.ToString();

        //Act
        var sut = Quantity.Parse(str);

        //Assert
        sut.Should().Be(quantity);
    }

    [Test]
    public void Parse_Should_ReturnQuantityWithValueOfTypeGuid_When_GenericArgumentIsGuid()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var quantity = new Quantity<Guid>(nameof(Guid), guid);
        var str = quantity.ToString();

        //Act
        var sut = Quantity.Parse<Guid>(str);

        //Assert
        sut.Should().Be(quantity);
    }

    [Test]
    public void Parse_Should_ReturnQuantityWithValueOfTypeInt_When_GenericArgumentIsInt()
    {
        //Arrange
        var quantity = new Quantity<int>("pcs", 123);
        var str = quantity.ToString();

        //Act
        var sut = Quantity.Parse<int>(str);

        //Assert
        sut.Should().Be(quantity);
    }

    [Test]
    public void TryParse_Should_ReturnTrue_When_NoGenericArgumentIsUsed()
    {
        //Arrange
        var quantity = new Quantity("kg", 123.45M);
        var str = quantity.ToString();

        //Act
        var success = Quantity.TryParse(str, out var sut);

        //Assert
        success.Should().BeTrue();
        sut.Should().Be(quantity);
    }

    [Test]
    public void TryParse_Should_ReturnQuantityWithValueOfTypeGuid_When_GenericArgumentIsGuid()
    {
        //Arrange
        var guid = Guid.NewGuid();
        var quantity = new Quantity<Guid>(nameof(Guid), guid);
        var str = quantity.ToString();

        //Act
        var success = Quantity.TryParse<Guid>(str, out var sut);

        //Assert
        success.Should().BeTrue();
        sut.Should().Be(quantity);
    }

    [Test]
    public void TryParse_Should_ReturnQuantityWithValueOfTypeInt_When_GenericArgumentIsInt()
    {
        //Arrange
        var quantity = new Quantity<int>("pcs", 123);
        var str = quantity.ToString();

        //Act
        var success = Quantity.TryParse<int>(str, out var sut);

        //Assert
        success.Should().BeTrue();
        sut.Should().Be(quantity);
    }
}
