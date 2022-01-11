using NUnit.Framework;
using System;

namespace Foundation
{
    [TestFixture]
    public class EventTests
    {
        private class MyTest
        {
            public void Func() { }
        }

        [Test]
        public void Dispose_ShouldUnsubscribeCollectedSubscriptions_When_Out_Of_Scope()
        {
            var sut = new Event<Action>();

            {
                var myTest = new MyTest();
                using (var disposable = sut.Subscribe(myTest.Func))
                {
                }
            }

            Assert.AreEqual(0, sut.SubscribtionCount);
        }

        [Test]
        public void Invoke_ShouldCallAllSubscriptions_When_Multiple_Actions_Subscribed()
        {
            var executed1 = false;
            void func1()
            {
                executed1 = true;
            }

            var executed2 = false;
            void func2()
            {
                executed2 = true;
            }
            
            var sut = new Event<Action>();

            sut.Subscribe(func1);
            sut.Subscribe(func2);

            sut.Publish();

            Assert.IsTrue(executed1);
            Assert.IsTrue(executed2);
        }

        [Test]
        public void Subscribe_ShouldHaveOneSubscription_When_Subscribed_Same_Action()
        {
            void func()
            {
            }

            var sut = new Event<Action>();

            sut.Subscribe(func);
            sut.Subscribe(func);

            Assert.AreEqual(1, sut.SubscribtionCount);
        }

        [Test]
        public void Subscribe_ShouldUnsubscribeAllSubscriptions_When_Called_Dispose()
        {
            void func1() {}
            void func2() {}

            var sut = new Event<Action>();
            {

                var disposable1 = sut.Subscribe(func1);
                var disposable2 = sut.Subscribe(func2);

                disposable1.Dispose();
                disposable2.Dispose();

                Assert.AreEqual(0, sut.SubscribtionCount);
            }
            {
                using (var disposable1 = sut.Subscribe(func1))
                using (var disposable2 = sut.Subscribe(func2)) 
                {
                }
            }
            Assert.AreEqual(0, sut.SubscribtionCount);
        }

        [Test]
        public void Subscribe_ShouldHaveMultipleSubscriptions_When_Different_Actions_Subscribed()
        {
            var sut = new Event<Action>();

            void func1()
            {
            }
            sut.Subscribe(func1);

            void func2()
            {
            }
            sut.Subscribe(func2);

            Assert.AreEqual(2, sut.SubscribtionCount);
        }
    }
}
