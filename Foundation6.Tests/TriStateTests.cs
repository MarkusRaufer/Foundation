using NUnit.Framework;

namespace Foundation
{
    [TestFixture]
    public class TriStateTests
    {
        [Test]
        public void Ctor_Should_HasStateFalse_When_CtorArgument_IsFalse()
        {
            var sut = new TriState(false);
            Assert.IsFalse(sut.State.OrThrow());
        }

        [Test]
        public void Ctor_Should_HasStateIsNone_When_NoCtorArgumentIsUsed()
        {
            var sut = new TriState();
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
    }
}
