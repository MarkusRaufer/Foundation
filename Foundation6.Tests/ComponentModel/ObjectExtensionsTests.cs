using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.ComponentModel;

[TestFixture]
public class ObjectExtensionsTests
{
    private class Person
    {
        private string _lastName;

        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            _lastName = lastName;
        }

        public string FirstName { get; }

        public string LastName => _lastName;

        public DateOnly DateOfBirth { get; set; }
    }

    [Test]
    public void ToMemberUpdateEvents_Should_Return2Events_When_2DifferentPropertiesAnd1FieldAreUsedAndConsiderPropertySetterIsTrue()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var person = new Person(firstName, lastName) { DateOfBirth = new DateOnly(1961, 2, 3) };

        var firstNameChange = new KeyValuePair<string, object?>(nameof(Person.FirstName), "Edgar");
        var lastNameChange = new KeyValuePair<string, object?>("_lastName", "Poe");
        var dateOfBirthChange = new KeyValuePair<string, object?>(nameof(Person.DateOfBirth), new DateOnly(1972, 3, 4));

        // Act
        var events = person.ToMemberUpdateEvents([firstNameChange, lastNameChange, dateOfBirthChange],
                                                 hasPublicPropertySetter: true)
                           .ToDictionary(x => x.Key, x => x.Value);

        // Assert
        events.Count.ShouldBe(2);
        {
            events.TryGetValue(lastNameChange.Key, out var actionValue).ShouldBeTrue();
            actionValue.Action.ShouldBe(EventAction.Update);
            actionValue.Value.ShouldBe(lastNameChange.Value);

        }
        {
            events.TryGetValue(dateOfBirthChange.Key, out var actionValue).ShouldBeTrue();
            actionValue.Action.ShouldBe(EventAction.Update);
            actionValue.Value.ShouldBe(dateOfBirthChange.Value);
        }
    }

    [Test]
    public void ToPropertyUpdateEvents_Should_ReturnOneEvent_When_OneDifferentPropertyIsUsedAndConsiderPropertySetterIsFalse()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var person = new Person(firstName, lastName) { DateOfBirth = new DateOnly(1961, 2, 3) };
        var change = new KeyValuePair<string, object?>(nameof(Person.LastName), "Lennon");

        // Act
        var events = person.ToPropertyUpdateEvents([change]).ToDictionary(x => x.Key, x => x.Value);

        // Assert
        events.Count.ShouldBe(1);
        events.TryGetValue(change.Key, out var actionValue).ShouldBeTrue();
        actionValue.Action.ShouldBe(EventAction.Update);
        actionValue.Value.ShouldBe(change.Value);
    }

    [Test]
    public void ToPropertyUpdateEvents_Should_ReturnOneEvent_When_ThreeDifferentPropertiesAreUsedAndConsiderPropertySetterIsTrue()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var person = new Person(firstName, lastName) { DateOfBirth = new DateOnly(1961, 2, 3) };

        var firstNameChange = new KeyValuePair<string, object?>(nameof(Person.FirstName), "Edgar");
        var lastNameChange = new KeyValuePair<string, object?>(nameof(Person.LastName), "Poe");
        var dateOfBirthChange = new KeyValuePair<string, object?>(nameof(Person.DateOfBirth), new DateOnly(1972, 3, 4));

        // Act
        var events = person.ToPropertyUpdateEvents([firstNameChange, lastNameChange, dateOfBirthChange],
                                           ignorePropertiesWithNonPublicSetter: true)
                           .ToDictionary(x => x.Key, x => x.Value);

        // Assert
        events.Count.ShouldBe(1);
        events.TryGetValue(dateOfBirthChange.Key, out var actionValue).ShouldBeTrue();
        actionValue.Action.ShouldBe(EventAction.Update);
        actionValue.Value.ShouldBe(dateOfBirthChange.Value);
    }
}
