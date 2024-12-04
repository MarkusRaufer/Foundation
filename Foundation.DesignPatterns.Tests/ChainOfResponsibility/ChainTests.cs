using FluentAssertions;

namespace Foundation.DesignPatterns.ChainOfResponsibility;

public class ChainTests
{
    [Fact]
    public void Create_Should_ReturnChainHandler_When_ArgumentListNotEmpty()
    {
        // Arrange
        Option<int> handle1(int x) => x == 1 ? Option.Some(x) : Option.None<int>();
        Func<int, Option<int>> handle2 = x => x == 2 ? Option.Some(x) : Option.None<int>();
        Option<int> handle3(int x) => x == 3 ? Option.Some(x) : Option.None<int>();
        Func<int, Option<int>> handle4 = x => x == 4 ? Option.Some(x) : Option.None<int>();

        // Act
        var chain = Chain.Create<int>(handle1, handle2, handle3, handle4);

        // Assert
        chain.Should().NotBeNull();
    }

    [Fact]
    public void Handle_Should_ReturnSome4_When_InIs4()
    {
        // Arrange
        Option<int> handle1(int x) => x == 1 ? Option.Some(x) : Option.None<int>();
        Func<int, Option<int>> handle2 = x => x == 2 ? Option.Some(x) : Option.None<int>();
        Option<int> handle3(int x) => x == 3 ? Option.Some(x) : Option.None<int>();
        Func<int, Option<int>> handle4 = x => x == 4 ? Option.Some(x) : Option.None<int>();
        var chain = Chain.Create<int>(handle1, handle2, handle3, handle4);

        // Act
        var result = chain!.Handle(4);

        // Assert
        result.TryGet(out int value).Should().BeTrue();
        value.Should().Be(4);
    }

    [Fact]
    public void Handle_Should_ReturnNone_When_InIsNotHandled()
    {
        // Arrange
        Option<int> handle1(int x)         => x == 1 ? Option.Some(x) : Option.None<int>();
        Func<int, Option<int>> handle2 = x => x == 2 ? Option.Some(x) : Option.None<int>();
        Option<int> handle3(int x)         => x == 3 ? Option.Some(x) : Option.None<int>();
        Func<int, Option<int>> handle4 = x => x == 4 ? Option.Some(x) : Option.None<int>();

        var chain = Chain.Create<int>(handle1, handle2, handle3, handle4);

        // Act
        var result = chain!.Handle(5);

        // Assert
        result.IsNone.Should().BeTrue();
    }
}
