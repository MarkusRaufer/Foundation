using NUnit.Framework;
using System;

namespace Foundation
{
    [TestFixture]
    public class OneOfTests
    {
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
        public void Item1_Should_ReturnSome_When_TypeIsTheFirstTypeArgument()
        {
            {
                var expected = 12;

                var sut = new OneOf<int, string>(expected);

                Assert.IsTrue(sut.Item1.IsSome);
                Assert.AreEqual(expected, sut.Item1.OrThrow());

                Assert.IsFalse(sut.Item2.IsSome);
            }
            {
                var expected = "12";

                var sut = new OneOf<int, string>(expected);

                Assert.IsFalse(sut.Item1.IsSome);

                Assert.IsTrue(sut.Item2.IsSome);
                Assert.AreEqual(expected, sut.Item2.OrThrow());
            }
        }

        [Test]
        public void Item2_Should_ReturnSome_When_TypeIsTheScondTypeArgument()
        {
            {
                var expected = 12;

                var sut = new OneOf<int, string>(expected);

                Assert.True(sut.Item1.IsSome);
                Assert.AreEqual(expected, sut.Item1.OrThrow());

                Assert.False(sut.Item2.IsSome);
            }
            {
                var expected = "12";

                var sut = new OneOf<int, string>(expected);

                Assert.False(sut.Item1.IsSome);

                Assert.True(sut.Item2.IsSome);
                Assert.AreEqual(expected, sut.Item2.OrThrow());
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
            Assert.IsFalse(sut.TryGet(out string _));
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
    }
}
