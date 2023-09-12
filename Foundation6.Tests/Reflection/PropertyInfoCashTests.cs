using NUnit.Framework;
using System;
using System.Linq;

namespace Foundation.Reflection;

[TestFixture]
public class PropertyInfoCashTests
{
    private class Person
    {
        public Person(string firstName, string lastName, DateOnly birthDay)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDay = birthDay;
        }

        public DateOnly BirthDay { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    [Test]
    public void GetMembers_Should_Return2Properties_When_Using_TypeForCtor_And_MemberNames()
    {
        var sut = new PropertyInfoCash(typeof(Person), new[] { nameof(Person.LastName), nameof(Person.BirthDay) });

        var members = sut.GetMembers().ToArray();

        Assert.AreEqual(2, members.Length);
        Assert.IsTrue(members.Any(x => x.Name == nameof(Person.BirthDay)));
        Assert.IsTrue(members.Any(x => x.Name == nameof(Person.LastName)));
    }

    [Test]
    public void GetMembers_Should_Return2Properties_When_Using_GenericTypeArgument_And_MemberNames()
    {
        var sut = new PropertyInfoCash<Person>(new[] { nameof(Person.LastName), nameof(Person.BirthDay) });

        var members = sut.GetMembers().ToArray();

        Assert.AreEqual(2, members.Length);
        Assert.IsTrue(members.Any(x => x.Name == nameof(Person.BirthDay)));
        Assert.IsTrue(members.Any(x => x.Name == nameof(Person.LastName)));
    }

    [Test]
    public void GetMembers_Should_ReturnAllProperties_When_Using_TypeForCtor()
    {
        var sut = new PropertyInfoCash(typeof(Person));

        var members = sut.GetMembers().ToArray();
        Assert.AreEqual(typeof(Person).GetProperties().Length, members.Length);

        Assert.IsTrue(members.Any(x => x.Name == nameof(Person.BirthDay)));
        Assert.IsTrue(members.Any(x => x.Name == nameof(Person.FirstName)));
        Assert.IsTrue(members.Any(x => x.Name == nameof(Person.LastName)));
    }    

    [Test]
    public void GetMembers_Should_ReturnAllProperties_When_Using_GenericTypeArgument()
    {
        var sut = new PropertyInfoCash<Person>();

        var members = sut.GetMembers().ToArray();
        Assert.AreEqual(typeof(Person).GetProperties().Length, members.Length);

        Assert.IsTrue(members.Any(x => x.Name == nameof(Person.BirthDay)));
        Assert.IsTrue(members.Any(x => x.Name == nameof(Person.FirstName)));
        Assert.IsTrue(members.Any(x => x.Name == nameof(Person.LastName)));
    }
}
