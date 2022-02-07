using NUnit.Framework;

namespace Foundation
{
    [TestFixture]
    public class OneOfTests
    {
        [Test]
        public void Item2_Should_ReturnSome_When_TypeIsTheScondTypeArgument()
        {
            {
                int expected = 12;
                var sut = new OneOf<int, string>(expected);

                Assert.IsTrue(sut.Item1.IsSome);
                Assert.AreEqual(expected, sut.Item1.ValueOrThrow());

                Assert.IsFalse(sut.Item2.IsSome);
            }
            {
                var expected = "12";
                var sut = new OneOf<int, string>(expected);

                Assert.IsFalse(sut.Item1.IsSome);

                Assert.IsTrue(sut.Item2.IsSome);
                Assert.AreEqual(expected, sut.Item2.ValueOrThrow());
            }
        }

        [Test]
        public void Item1_Should_ReturnSome_When_TypeIsTheFirstTypeArgument()
        {
            {
                int expected = 12;
                var sut = new OneOf<int, string>(expected);

                Assert.IsTrue(sut.Item1.IsSome);
                Assert.AreEqual(expected, sut.Item1.ValueOrThrow());

                Assert.IsFalse(sut.Item2.IsSome);
            }
            {
                var expected = "12";
                var sut = new OneOf<int, string>(expected);

                Assert.IsFalse(sut.Item1.IsSome);

                Assert.IsTrue(sut.Item2.IsSome);
                Assert.AreEqual(expected, sut.Item2.ValueOrThrow());
            }
        }

        [Test]
        public void OrdinalIndex_Should_ReturnTheOrdinalPosition_When_Created()
        {
            {
                var expected = 12;
                var sut = new OneOf<int, double>(expected);
                Assert.AreEqual(1, sut.OrdinalIndex);
            }
            {
                var expected = 12.3;
                var sut = new OneOf<int, double>(expected);
                Assert.AreEqual(2, sut.OrdinalIndex);
            }
            {
                var expected = "myValue";
                var sut = new OneOf<int, string, double>(expected);
                Assert.AreEqual(2, sut.OrdinalIndex);
            }
            {
                var expected = "myValue";
                var sut = new OneOf<int, double, string>(expected);
                Assert.AreEqual(3, sut.OrdinalIndex);
            }
        }

        [Test]
        public void Try_Should_ReturnFalse_When_TypeIsNotTheMatchingType()
        {
            var expected = 12;
            var sut = new OneOf<int, double>(expected);
            Assert.IsFalse(sut.Try(out double _));
            Assert.IsFalse(sut.Try(out string _));
        }

        [Test]
        public void Try_Should_ReturnTrue_When_TypeIsTheMatchingType()
        {
            {
                var expected = 12;
                var sut = new OneOf<int, double>(expected);
                Assert.IsTrue(sut.Try(out int intValue));
                Assert.AreEqual(expected, intValue);
            }
            {
                var expected = 12.3;
                var sut = new OneOf<int, double>(expected);
                Assert.IsTrue(sut.Try(out double doubleValue));
                Assert.AreEqual(expected, doubleValue);

            }
            {
                var expected = "myValue";
                var sut = new OneOf<int, string, double>(expected);
                Assert.IsTrue(sut.Try(out string? stringValue));
                Assert.AreEqual(expected, stringValue);
            }
        }
    }
}
