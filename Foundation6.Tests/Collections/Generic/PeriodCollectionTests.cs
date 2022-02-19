using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Collections.Generic
{
    [TestFixture]
    public class PeriodCollectionTests
    {
        [Test]
        public void Add_Should_Have3PeriodsInSortedOrder_When_Added3Periods()
        {
            var sut = new PeriodCollection();
            var start = new DateTime(2022, 1, 1, 12, 0, 0);
            var end = start + TimeSpan.FromDays(1);

            var period1 = Period.New(start + TimeSpan.FromDays(1), start + TimeSpan.FromDays(2));
            var period2 = Period.New(start, end + TimeSpan.FromDays(1));
            var period3 = Period.New(start, end);

            sut.Add(period1);
            sut.Add(period2);
            sut.Add(period3);

            var periods = sut.ToArray();
            Assert.AreEqual(3, periods.Length);
            Assert.AreEqual(period3, periods[0]);
            Assert.AreEqual(period2, periods[1]);
            Assert.AreEqual(period1, periods[2]);
        }
    }
}
