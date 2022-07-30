using NUnit.Framework;
using System;

namespace Foundation
{
    [TestFixture]
    public class OptTests
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
        public void Equals_Should_Return_True_When_2_Untyped_Objects_Are_Equal()
        {
            const int expected = 3;

            object optional1 = Option.Some(expected);
            object optional2 = Option.Some(expected);

            Assert.IsTrue(optional1.Equals(optional2));
        }

        [Test]
        public void Equals_operator()
        {
            {
                const int expected = 3;

                var optional1 = Option.Some(expected);
                var optional2 = Option.Some(expected);

                Assert.IsTrue(optional1 == optional2);
            }

            {
                var optional1 = Option.Some(3);
                var optional2 = Option.Some(4);
                Assert.IsTrue(optional1 != optional2);
            }
        }

        [Test]
        public void Equals_Should_Return_True_When_2_Typed_Objects_Are_Equal()
        {
            const int expected = 3;
            var optional1 = Option.Some(expected);
            var optional2 = Option.Some(expected);
            Assert.IsTrue(optional1.Equals(optional2));
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
