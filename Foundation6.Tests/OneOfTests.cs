using FluentAssertions;
using NUnit.Framework;
using System;

namespace Foundation
{
    [TestFixture]
    public class OneOfTests
    {
        [Test]
        public void CastOperator_Should_Compilable_When_PassingIntegerToMethod()
        {
            TestMethod(5);
        }

        [Test]
        public void CastOperator_Should_Compilable_When_PassingStringToMethod()
        {
            TestMethod("test");
        }

        [Test]
        public void CastOperator_Should_CreateOneOfObject_When_AssignedInteger()
        {
            var expected = 5;
            OneOf<int, string> sut = expected;

            sut.TryGet<string>(out var _).Should().BeFalse();
            sut.TryGet<int>(out var value).Should().BeTrue();
            value.Should().Be(expected);
        }

        [Test]
        public void CastOperator_Should_CreateOneOfObject_When_AssignedString()
        {
            var expected = "5";

            OneOf<int, string> sut = expected;

            sut.TryGet<int>(out var _).Should().BeFalse();
            sut.TryGet<string>(out var value).Should().BeTrue();
            value.Should().Be(expected);
        }

        [Test]
        public void Either_Should_ReturnTheResultOfTheMatchingFunc_When_CalledRightFunc()
        {
            {
                var expected = 12;

                var sut = new OneOf<int, double>(expected);

                var result = sut.Either((int i) => i, (double _) => 20);

                Assert.AreEqual(expected, result);
            }
            {
                var expected = 12.3;

                var sut = new OneOf<int, double>(expected);

                var result = sut.Either(_ => 20.0, (double d) => d);

                Assert.AreEqual(expected, result);
            }
            {
                var expected = "myValue";

                var sut = new OneOf<int, string, double>(expected);

                var result = sut.Either((int _) => "int", (string s) => s, (double _) => "double");

                Assert.AreEqual(expected, result);
            }
        }

        [Test]
        public void Invoke_Should_CallTheMatchingAction_When_TypeMatches()
        {
            {
                var expected = 12;

                var sut = new OneOf<int, double>(expected);
                var value = 0;

                Assert.True(sut.Invoke((int i) => value = i));
                Assert.False(sut.Invoke((double _) => value = 20));

                Assert.AreEqual(expected, value);
            }
            {
                var expected = 12.3;

                var sut = new OneOf<int, double>(expected);
                var value = 0D;

                Assert.False(sut.Invoke((int _) => value = 20D));
                Assert.True(sut.Invoke((double d) => value = d));

                Assert.AreEqual(expected, value);
            }
            {
                var expected = "myValue";

                var sut = new OneOf<int, string, double>(expected);
                var value = "";

                Assert.False(sut.Invoke((int _) => value = "int"));
                Assert.True(sut.Invoke((string s) => value = s));
                Assert.False(sut.Invoke((double _) => value = "double"));

                Assert.AreEqual(expected, value);
            }
        }


        [Test]
        public void Invoke_Should_ExecuteMatchingAction_When_UsingMultipleActions()
        {
            var expected = 12;

            var sut = new OneOf<int, double>(expected);

            var value = 0;

            sut.Invoke((int i) => value = i, (double _) => value = 20);

            Assert.AreEqual(expected, value);
        }

        [Test]
        public void IsOfType_Should_ExecuteMatchingAction_When_UsingMultipleActions()
        {
            var expected = "myValue";

            var sut = new OneOf<int, string>(expected);

            var isInt = sut.IsOfType<int>();
            var isDouble = sut.IsOfType<double>();
            var isString = sut.IsOfType<string>();

            isInt.Should().BeFalse();
            isDouble.Should().BeFalse();
            isString.Should().BeTrue();
        }

        [Test]
        public void Item1_Should_ReturnSome_When_TypeIsTheFirstTypeArgument()
        {
            {
                var expected = 12;

                var sut = new OneOf<int, string>(expected);

                sut.TryGet(out int number).Should().BeTrue();
                number.Should().Be(expected);
            }
            {
                var expected = "12";

                var sut = new OneOf<int, string>(expected);

                sut.TryGet(out string? str).Should().BeTrue();
                str.Should().Be(expected);
            }
        }

        [Test]
        public void SelectedType_Should_ReturnInt32Type_When_Using2TypeArguments_And_Item1Type_IsInt32()
        {
            var expected = 12;

            var sut = new OneOf<int, string>(expected);

            sut.SelectedType.Should().Be(expected.GetType());
        }

        [Test]
        public void SelectedType_Should_ReturnStringType_When_Using2TypeArguments_And_Item2Type_IsString()
        {
            var expected = "12";

            var sut = new OneOf<int, string>(expected);

            sut.SelectedType.Should().Be(expected.GetType());
        }

        [Test]
        public void SelectedType_Should_ReturnStringType_When_Using4TypeArguments_And_Item1Type_IsString()
        {
            var expected = "12";

            var sut = new OneOf<int, string, DateTime, double>(expected);

            sut.SelectedType.Should().Be(expected.GetType());
        }

        [Test]
        public void TryGet_Should_ReturnFalse_When_TypeIsNotTheMatchingType()
        {
            var expected = 12;

            var sut = new OneOf<int, double>(expected);

            sut.TryGet(out double _).Should().BeFalse();
            sut.TryGet(out string? _).Should().BeFalse();
        }

        [Test]
        public void TryGet_Should_ReturnTrue_When_TypeIsTheMatchingType()
        {
            {
                var expected = 12;

                var sut = new OneOf<int, double>(expected);

                sut.TryGet(out int value).Should().BeTrue();
                value.Should().Be(expected);
            }
            {
                var expected = 12.3;

                var sut = new OneOf<int, double>(expected);

                sut.TryGet(out double value).Should().BeTrue();
                value.Should().Be(expected);

            }
            {
                var expected = "myValue";

                var sut = new OneOf<int, string, double>(expected);

                sut.TryGet(out string? value).Should().BeTrue();
                value.Should().Be(expected);
            }
            {
                var expected = "myValue";

                var sut = new OneOf<int, string, DateTime, double>(expected);

                sut.TryGet(out string? value).Should().BeTrue();
                value.Should().Be(expected);
            }
        }

        private void TestMethod(OneOf<int, string> oneOf)
        {

        }
    }
}
