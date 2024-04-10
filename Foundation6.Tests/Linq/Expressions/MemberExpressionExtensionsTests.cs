using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Foundation.Linq.Expressions;

[TestFixture]
public class MemberExpressionExtensionsTests
{
    [Test]
    public void DistinctMembers_Should_Return4Members_When_MemberListContains2EqualMembers()
    {
        var dt = new DateTime(2020, 1, 2);

        var members = new List<MemberExpression>();

        {
            var parameter = Expression.Parameter(typeof(DateTime), "dt1");
            var me = Expression.Property(parameter, "Day");
            members.Add(me);
        }
        {
            var parameter = Expression.Parameter(typeof(DateTime), "dt2");
            var me = Expression.Property(parameter, "Day");
            members.Add(me);
        }
        {
            var parameter = Expression.Parameter(typeof(DateTime), "dt1");
            var me = Expression.Property(parameter, "Year");
            members.Add(me);
        }
        {
            var parameter = Expression.Parameter(typeof(DateTime), "dt1");
            var me = Expression.Property(parameter, "Day");
            members.Add(me);
        }
        {
            var parameter = Expression.Parameter(typeof(DateTime), "dt2");
            var me = Expression.Property(parameter, "Year");
            members.Add(me);
        }
        {
            var parameter = Expression.Parameter(typeof(DateTime), "dt2");
            var me = Expression.Property(parameter, "Day");
            members.Add(me);
        }


        var distinctMembers = members.DistinctMembers().ToArray();
        distinctMembers.Length.Should().Be(4);
    }
}
