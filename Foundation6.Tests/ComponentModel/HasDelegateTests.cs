using NUnit.Framework;
using Shouldly;
using System;

namespace Foundation.ComponentModel;

[TestFixture]

public class HasDelegateTests
{
    private record SetState(Delegate Delegate) : IHasDelegate;

    [Test]
    public void DynamicInvoke_Should_CallDelegate_When_Invoked()
    {
        // Arrange
        var str = "";

        void setValue(string value)
        {
            str = value;
        }

        var sut = new SetState(setValue);

        // Act
        sut.Delegate.DynamicInvoke("test");

        // Assert
        str.ShouldBe("test");
    }
}
