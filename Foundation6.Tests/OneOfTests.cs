using FluentAssertions;
using NUnit.Framework;
using System;

namespace Foundation
{
    [TestFixture]
    public class OneOfTests
    {
        [Test]
        public void CastOperator_Should_CreateOneOfObject_When_AssignedInteger()
        {
            var expected = 5;
            OneOf<int, string> sut = expected;

            TestMethod(5);

            sut.TryGet<string>(out var _).Should().BeFalse();
            sut.TryGet<int>(out var value).Should().BeTrue();
            value.Should().Be(expected);
        }

        [Test]
        public void CastOperator_Should_CreateOneOfObject_When_AssignedString()
        {
            var expected = "5";

            OneOf<int, string> sut = expected;

            TestMethod(expected);

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

                if (sut.Item1 is int item1) Assert.AreEqual(expected, item1);
                else Assert.Fail("Item1 has no value");

                Assert.IsNull(sut.Item2);
                
            }
            {
                var expected = "12";

                var sut = new OneOf<int, string>(expected);

                if (sut.Item2 is string item2) Assert.AreEqual(expected, item2);
                else Assert.Fail("Item1 has no value");
            }
        }

        [Test]
        public void SelectedType_Should_ReturnInt32Type_When_Using2TypeArguments_And_Item1Type_IsInt32()
        {
            var expected = 12;

            var sut = new OneOf<int, string>(expected);
            
            Assert.AreEqual(expected.GetType(), sut.SelectedType);
        }

        [Test]
        public void SelectedType_Should_ReturnStringType_When_Using2TypeArguments_And_Item2Type_IsString()
        {
            var expected = "12";

            var sut = new OneOf<int, string>(expected);

            Assert.AreEqual(expected.GetType(), sut.SelectedType);
        }

        [Test]
        public void SelectedType_Should_ReturnStringType_When_Using4TypeArguments_And_Item1Type_IsString()
        {
            var expected = "12";

            var sut = new OneOf<int, string, DateTime, double>(expected);

            Assert.AreEqual(expected.GetType(), sut.SelectedType);
        }

        [Test]
        public void TryGet_Should_ReturnFalse_When_TypeIsNotTheMatchingType()
        {
            var expected = 12;

            var sut = new OneOf<int, double>(expected);

            Assert.IsFalse(sut.TryGet(out double _));
            Assert.IsFalse(sut.TryGet(out string? _));
        }

        [Test]
        public void TryGet_Should_ReturnTrue_When_TypeIsTheMatchingType()
        {
            {
                var expected = 12;

                var sut = new OneOf<int, double>(expected);

                Assert.IsTrue(sut.TryGet(out int intValue));
                Assert.AreEqual(expected, intValue);
            }
            {
                var expected = 12.3;

                var sut = new OneOf<int, double>(expected);

                Assert.IsTrue(sut.TryGet(out double doubleValue));
                Assert.AreEqual(expected, doubleValue);

            }
            {
                var expected = "myValue";

                var sut = new OneOf<int, string, double>(expected);

                Assert.IsTrue(sut.TryGet(out string? stringValue));
                Assert.AreEqual(expected, stringValue);
            }
            {
                var expected = "myValue";

                var sut = new OneOf<int, string, DateTime, double>(expected);

                Assert.IsTrue(sut.TryGet(out string? stringValue));
                Assert.AreEqual(expected, stringValue);
            }
        }

        private void TestMethod(OneOf<int, string> oneOf)
        {

        }
    }
}
