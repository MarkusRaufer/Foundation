﻿using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Foundation
{
    [TestFixture]
    public class EventTests
    {
        private class MyTest : IDisposable
        {
            public MyTest() : this(Countable.New("Func", 0))
            {
            }
            public MyTest(Countable<string> countable)
            {
                FuncCount = countable;
            }

            public Countable<string> FuncCount { get; }

            public void Func()
            {
                FuncCount.Inc();
                IsCalledFunc1 = true;
            }

            public void Func2(IEnumerable<string> names)
            { 
                IsCalledFunc2 = true;
            }

            public void Dispose()
            {
                
            }

            public bool IsCalledFunc1 { get; set; }
            public bool IsCalledFunc2 { get; set; }

        }

        [Test]
        public void Dispose_Should_UnsubscribeCollectedSubscriptions_When_Out_Of_Scope()
        {
            var sut = new Event<Action>();

            {
                var myTest = new MyTest();
                using (var disposable = sut.Subscribe(myTest.Func))
                {
                }
            }

            Assert.AreEqual(0, sut.SubscriptionCount);
        }

        [Test]
        public void Invoke_Should_CallAllSubscriptions_When_Multiple_Actions_Subscribed()
        {
            var executed1 = false;
            void func1() => executed1 = true;

            var executed2 = false;
            void func2() => executed2 = true;

            // leaving the scope unregisters all subscriptions automatically.
            using var sut = new Event<Action>();

            sut.Subscribe(func1);
            sut.Subscribe(func2);

            sut.Publish();

            Assert.IsTrue(executed1);
            Assert.IsTrue(executed2);
        }

        [Test]
        public void Publish_Should_CallSubscription_When_DelegateWithEnumerableAsSingleParameterIsUsed()
        {
            // Arrange

            using var sut = new Event<Action>();

            var countable = Countable.New("Func", 0);

            var mt1 = new MyTest(countable);
            sut.Subscribe(mt1.Func);
            {
                using var mt2 = new MyTest(countable);
                using var subscription = sut.Subscribe(mt2.Func);
            }

            GC.Collect();

            // Act
            sut.Publish();

            // ClassicAssert
            countable.Count.Should().Be(1);
        }

        [Test]
        public void Subscribe_Should_HaveTwoSubscription_When_Subscribed_Same_Action()
        {
            void func()
            {
            }

            using var sut = new Event<Action>();

            sut.Subscribe(func);
            sut.Subscribe(func);

            sut.SubscriptionCount.Should().Be(2);
        }

        [Test]
        public void Subscribe_Should_UnsubscribeAllSubscriptions_When_Called_Dispose()
        {
            void func1() {}
            void func2() {}

            using var sut = new Event<Action>();
            {

                var disposable1 = sut.Subscribe(func1);
                var disposable2 = sut.Subscribe(func2);

                disposable1.Dispose();
                disposable2.Dispose();

                Assert.AreEqual(0, sut.SubscriptionCount);
            }
            {
                using (var disposable1 = sut.Subscribe(func1))
                using (var disposable2 = sut.Subscribe(func2)) 
                {
                }
            }
            Assert.AreEqual(0, sut.SubscriptionCount);
        }

        [Test]
        public void Subscribe_Should_HaveMultipleSubscriptions_When_Different_Actions_Subscribed()
        {
            // Arrange
            var sut = new Event<Action>();

            void func1() { }
            void func2() { }

            // Act
            sut.Subscribe(func1);
            sut.Subscribe(func2);

            // ClassicAssert
            sut.SubscriptionCount.Should().Be(2);
        }

        [Test]
        public void Subscribe_Should_HaveSingleSubscriptions_When_2SubscribedAndDisposeOfOneSubscriberIsCalled()
        {
            // Arrange
            var sut = new Event<Action>();

            void func1() { }
            void func2() { }

            // Act
            sut.Subscribe(func1);
            var disposable = sut.Subscribe(func2);

            //this unsubscribes the delegate
            disposable.Dispose(); 

            // ClassicAssert
            sut.SubscriptionCount.Should().Be(1);
        }
    }
}
