using FluentAssertions;
using NUnit.Framework;

namespace Foundation;

[TestFixture]
public class FriendTests
{
    private class MyClass;

    private class MySubClass : MyClass;

    [Test]
    public void FriendValue_Instance_Should_HaveChangedValue_When_Changer_IsFriendOfValue()
    {
        // Arrange
        var myClass = new MyClass();
        var initialValue = $"friend of {nameof(MyClass)}";
        var changedValue = $"changed by {nameof(MyClass)}";
        var sut = FriendValue.New(myClass, initialValue);

        // Act
        sut[myClass] = changedValue;

        // ClassicAssert
        sut.Value.Should().Be(changedValue);
    }

    [Test]
    public void FriendValue_Instance_Should_NotChangeValue_When_Changer_IsNotFriendOfValue()
    {
        // Arrange
        var friend = new MyClass();
        var noFriend = new MyClass();

        var initialValue = $"friend of {nameof(MyClass)}";
        var changedValue = $"changed by another instance of {nameof(MyClass)}";

        var sut = FriendValue.New(friend, initialValue);

        // Act
        sut[noFriend] = changedValue;

        // ClassicAssert
        sut.Value.Should().Be(initialValue);
    }

    [Test]
    public void FriendValue_FriendType_Should_HaveChangedValue_When_Changer_IsSameFriendInstanceOfValue()
    {
        // Arrange
        var myClass = new MyClass();
        var initialValue = $"friend of {nameof(myClass)}";
        var changedValue = $"changed by {nameof(myClass)}";
        var sut = new FriendValue<MyClass, string>(initialValue);

        // Act
        sut[myClass] = changedValue;

        // ClassicAssert
        sut.Value.Should().Be(changedValue);
    }

    [Test]
    public void FriendValue_FriendType_Should_HaveChangedValue_When_Changer_IsSameFriendTypeOfValue()
    {
        // Arrange
        var myClass = new MyClass();
        var myClass2 = new MyClass();
        var initialValue = $"friend of {nameof(myClass)}";
        var changedValue = $"changed by {nameof(myClass2)}";
        var sut = new FriendValue<MyClass, string>(initialValue);

        // Act
        sut[myClass2] = changedValue;

        // ClassicAssert
        sut.Value.Should().Be(changedValue);
    }

    [Test]
    public void FriendValue_FriendType_Should_HaveChangedValue_When_Changer_IsSubclassOfFriendOfValue()
    {
        // Arrange
        var myClass = new MyClass();
        var mySubClass = new MySubClass();
        var initialValue = $"friend of {nameof(myClass)}";
        var changedValue = $"changed by {nameof(mySubClass)}";
        var sut = new FriendValue<MyClass, string>(initialValue);

        // Act
        sut[mySubClass] = changedValue;

        // ClassicAssert
        sut.Value.Should().Be(changedValue);
    }
}
