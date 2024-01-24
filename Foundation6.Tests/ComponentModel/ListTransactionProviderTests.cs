using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.ComponentModel;

[TestFixture]
public class ListTransactionProviderTests
{
    [Test]
    public void Test()
    {
        var numbers = new List<int> { 1, 2, 3 };

        var sut = ListTransactionProvider.New(numbers);
        sut.Should().NotBeNull();

        using var transaction = sut.BeginTransaction();
        foreach(var number in sut)
        {
            if (2 == number) sut.Add(4);
        }
        transaction.Commit();

        var expected = new List<int> { 1, 2, 3, 4 };
        
        numbers.Should().BeEquivalentTo(expected);
    }
}
