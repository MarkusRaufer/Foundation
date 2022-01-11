namespace Foundation.Collections.Generic;

using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[TestFixture]
public class InvasivePredicatesTests
{
    [Test]
    public void PredicateCount_Should_ReturnTheNumberOfPredicates_When_AddedAListOfPredicates()
    {
        var predicates = new Func<int, bool>[] { n => n == 2, n => n == 4 };
        var sut = new InvasivePredicates<int>(predicates);
        
        Assert.AreEqual(predicates.Length, sut.PredicateCount);
    }

    [Test]
    public void Check_Should_ReturnNone_When_AllPredicatesWhereTrue()
    {
        var predicates = new Func<int, bool>[] { n => n == 2, n => n == 4 };
        var sut = new InvasivePredicates<int>(predicates);

        var numbers = Enumerable.Range(1, 4).ToArray();

        {
            var number = numbers[0];
            var result = sut.Check(number);
            var x = new TriState(false).Equals(result);
            Assert.AreEqual(new TriState(false), result);
            Assert.AreEqual(predicates.Length, sut.PredicateCount);
        }
        {
            var number = numbers[1];
            var result = sut.Check(number);
            Assert.AreEqual(new TriState(true), result);
            Assert.AreEqual(predicates.Length - 1, sut.PredicateCount);

        }
        {
            var number = numbers[2];
            var result = sut.Check(number);
            Assert.AreEqual(new TriState(false), result);
            Assert.AreEqual(predicates.Length - 1, sut.PredicateCount);

        }
        {
            var number = numbers[3];
            var result = sut.Check(number);
            Assert.AreEqual(new TriState(true), result);
            Assert.AreEqual(0, sut.PredicateCount);
        }
        {
            var number = numbers[3];
            var result = sut.Check(number);
            Assert.AreEqual(new TriState(), result);
            Assert.AreEqual(0, sut.PredicateCount);
        }

        Assert.AreEqual(0, sut.PredicateCount);
    }

    [Test]
    public void Check_Should_ReturnSomeFalse_When_NotAllPredicatesWhereTrue()
    {
        var predicates = new Func<int, bool>[] { n => n == 2, n => n == 4 };
        var sut = InvasivePredicates.New(predicates);

        var numbers = Enumerable.Range(1, 3).ToArray();

        {
            var number = numbers[0];
            var result = sut.Check(number);
            Assert.AreEqual(new TriState(false), result);
            Assert.AreEqual(predicates.Length, sut.PredicateCount);
        }
        {
            var number = numbers[1];
            var result = sut.Check(number);
            Assert.AreEqual(new TriState(true), result);
            Assert.AreEqual(predicates.Length - 1, sut.PredicateCount);
        }

        {
            var number = numbers[2];
            var result = sut.Check(number);
            Assert.AreEqual(new TriState(false), result);
            Assert.AreEqual(predicates.Length - 1, sut.PredicateCount);
        }
    }
}

