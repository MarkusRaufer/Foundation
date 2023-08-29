using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Collections.Generic;

[TestFixture]
public class SortedLamdaSearchSetTests
{
    [Test]
    public void FindAll_Should_ReturnValues_When_Using_LambdaExpression()
    {
        LambdaExpression lambda = (int x) => x > 2 && x < 6;
        var numbers = Enumerable.Range(1, 10);

        var sut = new SortedLambdaSearchSet<int>(numbers);

        var result = sut.FindAll(lambda).ToArray();
        Assert.AreEqual(3, result.Length);
        Assert.AreEqual(3, result[0]);
        Assert.AreEqual(4, result[1]);
        Assert.AreEqual(5, result[2]);
    }
}
