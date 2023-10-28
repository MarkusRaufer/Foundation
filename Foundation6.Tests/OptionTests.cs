using FluentAssertions;
using NUnit.Framework;
using System;

namespace Foundation
{
    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void Cast_operator()
        {
            {
                const int expected = 3;
                Option<int> sut = expected;

                Assert.IsTrue(sut.IsSome);
                Assert.AreEqual(expected, sut.OrThrow());
            }
            {
                const string expected = "test";
                Option<string> sut = expected;

                Assert.IsTrue(sut.IsSome);
                Assert.AreEqual(expected, sut.OrThrow());
            }
            {
                var sut = Option.Maybe<string>(null);
                Assert.IsTrue(sut.IsNone);
            }
        }

        [Test]
        public void Ctor_Should_IsNone_When_Uninitialized()
        {
            {
                //value type
                var sut = new Option<int>();

                Assert.IsTrue(sut.IsNone);
                Assert.IsFalse(sut.IsSome);
            }
            {
                //reference type
                var sut = new Option<string>();

                Assert.IsTrue(sut.IsNone);
                Assert.IsFalse(sut.IsSome);
            }
        }

        [Test]
        public void Equals_BothSidesOption_Should_ReturnFalse_When_2_Untyped_Objects_Are_Equal()
        {

            object optional1 = Option.Some(3);
            object optional2 = Option.Some(4);

            optional1.Equals(optional2).Should().BeFalse();
        }

        [Test]
        public void Equals_BothSidesOption_Should_ReturnTrue_When_2_Typed_Objects_Are_Equal()
        {
            const int expected = 3;
            var optional1 = Option.Some(expected);
            var optional2 = Option.Some(expected);

            optional1.Equals(optional2).Should().BeTrue();
        }

        [Test]
        public void Equals_operator_BothSidesOption_Should_ReturnFalse_When_ValuesAreNotEqual()
        {
            var optional1 = Option.Some(3);
            var optional2 = Option.Some(4);

            optional1.Equals(optional2).Should().BeFalse();
        }

        [Test]
        public void Equals_operator_BothSidesOption_Should_ReturnTrue_When_ValuesAreEqual()
        {
            const int expected = 3;

            var optional1 = Option.Some(expected);
            var optional2 = Option.Some(expected);

            optional1.Equals(optional2).Should().BeTrue();
        }

        [Test]
        public void Equals_operator_OneSideOption_Should_ReturnFalse_When_ValuesAreNotEqual()
        {
            var optional = Option.Some(3);
            var value = 4;

            var eq = optional == value;

            eq.Should().BeFalse();

            eq = value == optional;

            eq.Should().BeFalse();
        }

        [Test]
        public void Equals_operator_OneSideOption_Should_ReturnTrue_When_ValuesAreEqual()
        {
            var value = 3;
            var optional = Option.Some(value);

            var eq = optional == value;

            eq.Should().BeTrue();

            eq = value == optional;

            eq.Should().BeTrue();
        }

        [Test]
        public void None_Should_Be_IsNone_When_CalledNone()
        {
            var sut = Option.None<int>();

            Assert.IsTrue(sut.IsNone);
            Assert.IsFalse(sut.IsSome);
        }

        [Test]
        public void OrThrow_Should_Return_A_Value_When_IsSome()
        {
            const int expected = 3;
            var sut = Option.Some(expected);

            Assert.IsTrue(sut.IsSome);
            Assert.AreEqual(expected, sut.OrThrow());
        }

        [Test]
        public void OrThrow_Should_Throw_An_Exception_When_IsNone()
        {
            var sut = Option.None<int>();

            Assert.Throws<NullReferenceException>(() => sut.OrThrow());
        }

        [Test]
        public void TryGet_Should_Return_Type_When_True()
        {
            var expected = 2;
            var sut = Option.Some(expected);

            Assert.IsTrue(sut.TryGet(out int n));
            Assert.AreEqual(expected, n);
        }
    }
}
