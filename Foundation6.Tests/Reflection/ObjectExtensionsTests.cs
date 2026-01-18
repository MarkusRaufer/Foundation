using NUnit.Framework;
using Shouldly;
using System;
using System.Linq;

namespace Foundation.Reflection;

[TestFixture]
public class ObjectExtensionsTests
{
    private record Person(string FirstName, string LastName, DateOnly DateOfBirth);
    [Test]
    public void TryGetValue_Should_ReturnKeyValues_When_UsingARecordType()
    {
        // Arrange
        var person = new Person("John", "Doe", new DateOnly(1970, 1, 2));

        // Act
        var keyValues = person.ToKeyValues().ToDictionary(x => x.Key, x => x.Value);

        // Assert
        keyValues.Count.ShouldBe(3);
        
        keyValues.TryGetValue(nameof(Person.FirstName), out var firstName).ShouldBeTrue();
        firstName.ShouldBe(person.FirstName);

        keyValues.TryGetValue(nameof(Person.LastName), out var lastName).ShouldBeTrue();
        lastName.ShouldBe(person.LastName);

        keyValues.TryGetValue(nameof(Person.DateOfBirth), out var dateOfBirth).ShouldBeTrue();
        dateOfBirth.ShouldBe(person.DateOfBirth);
    }
}
