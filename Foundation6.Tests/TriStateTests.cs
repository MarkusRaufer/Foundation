using NUnit.Framework;

namespace Foundation
{
    [TestFixture]
    public class TriStateTests
    {
        [Test]
        public void Ctor_Should_IsFalseTrue_When_CtorArgumentIsFalse()
        {
            var sut = new TriState(false);
            Assert.IsTrue(sut.IsFalse);
            Assert.IsFalse(sut.IsTrue);
            Assert.IsFalse(sut.IsNone);
        }

        [Test]
        public void Ctor_Should_IsNoneTrue_When_NoCtorArgumentIsUsed()
        {
            var sut = new TriState();
            Assert.IsTrue(sut.IsNone);
            Assert.IsFalse(sut.IsTrue);
            Assert.IsFalse(sut.IsFalse);
        }

        [Test]
        public void Ctor_Should_IsTrueTrue_When_CtorArgumentIsTrue()
        {
            var sut = new TriState(true);
            Assert.IsTrue(sut.IsTrue);
            Assert.IsFalse(sut.IsFalse);
            Assert.IsFalse(sut.IsNone);
        }

        [Test]
        public void Equals_Should_ReturnFalse_When_BothAreDifferent()
        {
            {
                var sut1 = new TriState();
                var sut2 = new TriState(false);

                Assert.IsFalse(sut1.Equals(sut2));
                Assert.IsTrue(sut1 != sut2);
            }
            {
                var sut1 = new TriState(false);
                var sut2 = new TriState();

                Assert.IsFalse(sut1.Equals(sut2));
                Assert.IsTrue(sut1 != sut2);
            }
            {
                var sut1 = new TriState();
                var sut2 = new TriState(true);

                Assert.IsFalse(sut1.Equals(sut2));
                Assert.IsTrue(sut1 != sut2);
            }
            {
                var sut1 = new TriState(true);
                var sut2 = new TriState();

                Assert.IsFalse(sut1.Equals(sut2));
                Assert.IsTrue(sut1 != sut2);
            }
            {
                var sut1 = new TriState(false);
                var sut2 = new TriState(true);

                Assert.IsFalse(sut1.Equals(sut2));
                Assert.IsTrue(sut1 != sut2);
            }
            {
                var sut1 = new TriState(true);
                var sut2 = new TriState(false);

                Assert.IsFalse(sut1.Equals(sut2));
                Assert.IsTrue(sut1 != sut2);
            }
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_BothIsFalse()
        {
            var sut1 = new TriState(false); 
            var sut2 = new TriState(false);

            Assert.IsTrue(sut1.Equals(sut2));
            Assert.IsTrue(sut1 == sut2);
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_BothIsNone()
        {
            var sut1 = new TriState();
            var sut2 = new TriState();

            Assert.IsTrue(sut1.Equals(sut2));
            Assert.IsTrue(sut1 == sut2);
        }

        [Test]
        public void Equals_Should_ReturnTrue_When_BothIsTrue()
        {
            var sut1 = new TriState(true);
            var sut2 = new TriState(true);

            Assert.IsTrue(sut1.Equals(sut2));
            Assert.IsTrue(sut1 == sut2);
        }
    }
}
