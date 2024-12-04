using FluentAssertions;

namespace Foundation.DesignPatterns.ChainOfResponsibility;

public class ChainHandlerTests
{
    [Fact]
    public void New_Should_ReturnAChainHandler_When_TryHandleIsAFuncThatReturnsAnOption()
    {
        var sut = ChainHandler.New<int, int>(x => Option.Some(x));
    }
    [Fact]
    public void Handle_Should_ReturnSome5_When_HandleExpects5()
    {
        var sut = ChainHandler.New<int, int>(x => x == 5 ? Option.Some(x) : Option.None<int>());
        sut.Handle(5).TryGet(out var value).Should().BeTrue();
        value.Should().Be(5);
    }

    [Fact]
    public void Handle_Should_ReturnSome5_When_HandleExpects5_AndSuccessorHandlesIt()
    {
        var successor = ChainHandler.New<int, int>(x => x == 5 ? Option.Some(x) : Option.None<int>());
        var sut = ChainHandler.New<int>(x => x == 2 ? Option.Some(x) : Option.None<int>(), successor);

        sut.Handle(5).TryGet(out var value).Should().BeTrue();
        value.Should().Be(5);
    }
}
