using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation
{
    [TestFixture]
    public class EitherTests
    {
        [Test]
        public void GetAlternative_Should_ReturnTrue_When_TypeIsPartOfTheAlternativeArguments()
        {
            {
                int expected = 12;
                var sut = new Either<int, string>(expected);

                var alternative = sut.GetAlternatives<int>().Single();
                Assert.AreEqual(expected, alternative);

                Assert.IsFalse(sut.GetAlternatives<string>().Any());
            }
            {
                var expected = "12";
                var sut = new Either<int, string>(expected);

                var alternative = sut.GetAlternatives<string>().Single();
                Assert.AreEqual(expected, alternative);

                Assert.IsFalse(sut.GetAlternatives<int>().Any());
            }
        }

        [Test]
        public void Is_Should_ReturnTrue_When_TypeIsPartOfTheAlternativeArguments()
        {
            {
                var sut = new Either<int, double>(12);
                Assert.IsTrue(sut.Is<int>());
                Assert.IsFalse(sut.Is<double>());
                Assert.IsFalse(sut.Is<string>());
            }
            {
                var sut = new Either<int, double>(12.3);
                Assert.IsTrue(sut.Is<double>());
                Assert.IsFalse(sut.Is<int>());
                Assert.IsFalse(sut.Is<string>());
            }
        }
    }
}
