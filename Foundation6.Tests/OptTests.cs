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
                Opt<int> sut = expected;
                Assert.IsTrue(sut.IsSome);
                Assert.AreEqual(expected, sut.ValueOrThrow());
            }
            {
                const string expected = "test";
                Opt<string> sut = expected;
                Assert.IsTrue(sut.IsSome);
                Assert.AreEqual(expected, sut.ValueOrThrow());
            }
            {
                var sut = Opt.Maybe<string>(null);
                Assert.IsTrue(sut.IsNone);
            }
        }

        [Test]
        public void Equals_Should_Return_True_When_2_Untyped_Objects_Are_Equal()
        {
            const int expected = 3;
            object optional1 = Opt.Some(expected);
            object optional2 = Opt.Some(expected);
            Assert.IsTrue(optional1.Equals(optional2));
        }

        [Test]
        public void Equals_operator()
        {
            {
                const int expected = 3;
                var optional1 = Opt.Some(expected);
                var optional2 = Opt.Some(expected);
                Assert.IsTrue(optional1 == optional2);
            }

            {
                var optional1 = Opt.Some(3);
                var optional2 = Opt.Some(4);
                Assert.IsTrue(optional1 != optional2);
            }
        }

        [Test]
        public void Equals_Should_Return_True_When_2_Typed_Objects_Are_Equal()
        {
            const int expected = 3;
            var optional1 = Opt.Some(expected);
            var optional2 = Opt.Some(expected);
            Assert.IsTrue(optional1.Equals(optional2));
        }

        [Test]
        public void Is_Should_Return_Type_When_True()
        {
            var expected = 2;
            var sut = Opt.Some(expected);
            Assert.IsTrue(sut.Is(out int n));
            Assert.AreEqual(expected, n);
        }

        [Test]
        public void None_Should_Be_IsNone()
        {
            var sut = Opt.None<int>();
            Assert.IsTrue(sut.IsNone);
            Assert.IsFalse(sut.IsSome);
        }

        [Test]
        public void Uninitialized()
        {
            {
                //value type
                var sut = new Opt<int>();
                Assert.IsTrue(sut.IsNone);
                Assert.IsFalse(sut.IsSome);
            }
            {
                //reference type
                var sut = new Opt<string>();
                Assert.IsTrue(sut.IsNone);
                Assert.IsFalse(sut.IsSome);
            }
        }

        [Test]
        public void ValueOrThrow_Should_Return_A_Value_When_IsSome()
        {
            const int expected = 3;
            var sut = Opt.Some(expected);
            Assert.IsTrue(sut.IsSome);
            Assert.AreEqual(expected, sut.ValueOrThrow());
        }


        [Test]
        public void ValueOrThrow_Should_Throw_An_Exception_When_IsNone()
        {
            var sut = Opt.None<int>();
            Assert.Throws<NullReferenceException>(() => sut.ValueOrThrow());
        }
    }
}
