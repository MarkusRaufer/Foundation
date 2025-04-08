using NUnit.Framework;

namespace Foundation;

[TestFixture]
public class TriStateTests
{
    [Test]
    public void Ctor_Should_ReturnStateFalse_When_CtorArgument_IsFalse()
    {
        var sut = new TriState(false);
        Assert.IsFalse(sut.State.OrThrow());
    }

    [Test]
    public void Ctor_Should_ReturnState2_When_GenericVersion_And_CtorArgument_Is2()
    {
        var sut = new TriState<int, string>(2);

        Assert.IsTrue(sut.TryGet(out int state));
        Assert.AreEqual(2, state);
    }

    [Test]
    public void Ctor_Should_ReturnStateString_When_GenericVersion_And_CtorArgument_IsString()
    {
        var expectedState = "test";
        var sut = new TriState<int, string>(expectedState);

        Assert.IsTrue(sut.TryGet(out string? state));
        Assert.AreEqual(expectedState, state);
    }

    [Test]
    public void Ctor_Should_HasStateIsNone_When_DefaultIsUsed()
    {
        var sut = new TriState();
        Assert.IsTrue(sut.State.IsNone);
    }

    [Test]
    public void Ctor_Should_HasStateIsNone_When_GenericVersion_And_DefaultIsUsed()
    {
        var sut = new TriState<int, string>();
        Assert.IsTrue(sut.State.IsNone);
    }

    [Test]
    public void Ctor_Should_HasStateTrue_When_CtorArgumentIsTrue()
    {
        var sut = new TriState(true);
        Assert.IsTrue(sut.State.OrThrow());
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_BothAreDifferent()
    {
        {
            var sut1 = new TriState();
            var sut2 = new TriState(false);

            Assert.IsFalse(sut1.Equals(sut2));
            Assert.IsFalse(sut2.Equals(sut1));

            Assert.IsTrue(sut1 != sut2);
            Assert.IsTrue(sut2 != sut1);
        }
        {
            var sut1 = new TriState();
            var sut2 = new TriState(true);

            Assert.IsFalse(sut1.Equals(sut2));
            Assert.IsFalse(sut2.Equals(sut1));

            Assert.IsTrue(sut1 != sut2);
            Assert.IsTrue(sut2 != sut1);
        }
        {
            var sut1 = new TriState(false);
            var sut2 = new TriState(true);

            Assert.IsFalse(sut1.Equals(sut2));
            Assert.IsFalse(sut2.Equals(sut1));

            Assert.IsTrue(sut1 != sut2);
            Assert.IsTrue(sut2 != sut1);
        }
    }

    [Test]
    public void Equals_Should_ReturnFalse_When_GenericVersion_And_BothAreDifferent()
    {
        {
            var sut1 = new TriState<int, string>();
            var sut2 = new TriState<int, string>(2);

            Assert.IsFalse(sut1.Equals(sut2));
            Assert.IsFalse(sut2.Equals(sut1));

            Assert.IsTrue(sut1 != sut2);
            Assert.IsTrue(sut2 != sut1);
        }
        {
            var sut1 = new TriState<int, string>();
            var sut2 = new TriState<int, string>("test");

            Assert.IsFalse(sut1.Equals(sut2));
            Assert.IsFalse(sut2.Equals(sut1));

            Assert.IsTrue(sut1 != sut2);
            Assert.IsTrue(sut2 != sut1);
        }
        {
            var sut1 = new TriState<int, string>(2);
            var sut2 = new TriState<int, string>("test");

            Assert.IsFalse(sut1.Equals(sut2));
            Assert.IsFalse(sut2.Equals(sut1));

            Assert.IsTrue(sut1 != sut2);
            Assert.IsTrue(sut2 != sut1);
        }
        {
            var sut1 = new TriState<int, string>(2);
            var sut2 = new TriState<int, string>(3);

            Assert.IsFalse(sut1.Equals(sut2));
            Assert.IsFalse(sut2.Equals(sut1));

            Assert.IsTrue(sut1 != sut2);
            Assert.IsTrue(sut2 != sut1);
        }
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_Both_State_IsFalse()
    {
        var sut1 = new TriState(false); 
        var sut2 = new TriState(false);

        Assert.IsTrue(sut1.Equals(sut2));
        Assert.IsTrue(sut2.Equals(sut1));

        Assert.IsTrue(sut1 == sut2);
        Assert.IsTrue(sut2 == sut1);
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_Both_State_IsNone()
    {
        var sut1 = new TriState();
        var sut2 = new TriState();

        Assert.IsTrue(sut1.Equals(sut2));
        Assert.IsTrue(sut2.Equals(sut1));

        Assert.IsTrue(sut1 == sut2);
        Assert.IsTrue(sut2 == sut1);
    }

    [Test]
    public void Equals_Should_ReturnTrue_When_Both_State_IsTrue()
    {
        var sut1 = new TriState(true);
        var sut2 = new TriState(true);

        Assert.IsTrue(sut1.Equals(sut2));
        Assert.IsTrue(sut2.Equals(sut1));

        Assert.IsTrue(sut1 == sut2);
        Assert.IsTrue(sut2 == sut1);
    }

    [Test]
    public void Equals_Should_ReturnInt32_When_GenericVersion_And_BothStatesAreSameInt32()
    {
        var sut1 = new TriState<int, string>(2);
        var sut2 = new TriState<int, string>(2);

        Assert.IsTrue(sut1.Equals(sut2));
        Assert.IsTrue(sut2.Equals(sut1));

        Assert.IsTrue(sut1 == sut2);
        Assert.IsTrue(sut2 == sut1);
    }

    [Test]
    public void Equals_Should_ReturnString_When_GenericVersion_And_BothStatesAreSameString()
    {
        var sut1 = new TriState<int, string>("test");
        var sut2 = new TriState<int, string>("test");

        Assert.IsTrue(sut1.Equals(sut2));
        Assert.IsTrue(sut2.Equals(sut1));

        Assert.IsTrue(sut1 == sut2);
        Assert.IsTrue(sut2 == sut1);
    }

    [Test]
    public void Invoke_Should_InvoceState1_When_GenericVersion_And_State1Used()
    {
        var expectedState = 2;
        var sut = new TriState<int, string>(expectedState);

        var value = 0;
        sut.Invoke(onState1: state => value = state);

        Assert.AreEqual(expectedState, value);
    }

    [Test]
    public void Invoke_Should_InvoceNoneState_When_GenericVersion_And_NoState()
    {
        var sut = new TriState<int, string>();

        var value = false;
        sut.Invoke(none: () => value = true);

        Assert.AreEqual(true, value);
    }

    [Test]
    public void Invoke_Should_InvoceState2_When_GenericVersion_And_State2Used()
    {
        var expectedState = "test";
        var sut = new TriState<int, string>(expectedState);

        var value = "";
        sut.Invoke(onState2: state => value = state);

        Assert.AreEqual(expectedState, value);
    }

    [Test]
    public void Match_Should_ReturnsValue_When_GenericVersion_And_NoneUsed()
    {
        var sut = new TriState<int, string>();

        var value = sut.Either(onState1: state => state,
                              onState2: state => int.Parse(state),
                              none: () => -1);

        Assert.AreEqual(-1, value);
    }

    [Test]
    public void Match_Should_ReturnsValue_When_GenericVersion_And_State1IsUsed()
    {
        var expectedState = 2;
        var sut = new TriState<int, string>(expectedState);

        var value = sut.Either(onState1: state => state,
                              onState2: state => int.Parse(state),
                              none: () => -1);

        Assert.AreEqual(expectedState, value);
    }

    [Test]
    public void Match_Should_ReturnsValue_When_GenericVersion_And_State2IsUsed()
    {
        var sut = new TriState<int, string>("5");

        var value = sut.Either(onState1: state => state,
                              onState2: state => int.Parse(state),
                              none: () => -1);

        Assert.AreEqual(5, value);
    }

    [Test]
    public void OnState1_Should_ReturnsState1_When_GenericVersion_And_State1IsUsed()
    {
        var expectedState = 2;
        var sut = new TriState<int, string>(expectedState);

        var value = sut.OnState1(state => state + 10).OrThrow();

        Assert.AreEqual(12, value);
    }

    [Test]
    public void OnState2_Should_ReturnsState2_When_GenericVersion_And_State2IsUsed()
    {
        var sut = new TriState<int, string>("hello");

        var value = sut.OnState2(state => state + " world").OrThrow();

        Assert.AreEqual("hello world", value);
    }
}
