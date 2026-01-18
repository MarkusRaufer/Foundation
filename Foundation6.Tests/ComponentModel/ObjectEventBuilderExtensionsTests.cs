using Foundation.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.ComponentModel;

[TestFixture]
public class ObjectEventBuilderExtensionsTests
{
    private class Person
    {
        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public string FirstName { get; }

        public string LastName { get; }

        public DateOnly DateOfBirth { get; set; }
    }

    private record Pet(string Name, DateOnly Birthday);

    [Test]
    public void CreateUpdateEvents_Should_ThrowException_When_ClassWithImmutablePropertyIsUsed()
    {
        // Arrange
        var firstName = "Peter";
        var lastName = "Pan";
        var person = new Person(firstName, lastName) { DateOfBirth = new DateOnly(1961, 2, 3) };

        var newDateOfBirth = new DateOnly(1962, 3, 4);
        var newLastName = "Doe";

        // Act
        var events = person.CreateUpdateEventsWith(x => x.LastName, newLastName)
                                  .And(x => x.DateOfBirth, newDateOfBirth)
                                  .Build(ignorePropertiesWithNonPublicSetter: true)
                                  .ToArray();

        // Assert
        var ev = events.Single();
        ev.Key.ShouldBe(nameof(Person.DateOfBirth));
        ev.Value.Action.ShouldBe(EventAction.Update);
        ev.Value.Value.ShouldBe(newDateOfBirth);

    }

    [Test]
    public void CreateUpdateEvents_Should_ThrowException_When_NonExistingPropertyIsUsedAndIgnoreInvalidPropertiesIsFalse()
    {
        // Arrange
        var firstName = "Peter";
        var lastName = "Pan";
        var person = new Person(firstName, lastName) { DateOfBirth = new DateOnly(1961, 2, 3) };

        var newBirthday = new DateOnly(1962, 3, 4);
        var newLastName = "Doe";

        // Act
        var ex = Should.Throw<ArgumentException>(() =>
                            person.CreateUpdateEventsWith(x => x.LastName, newLastName)
                                  .And([new KeyValuePair<string, object?>("invalidProperty", "value")])
                                  .Build(ignoreInvalidProperties: false)
                                  .ToArray()
                       );

        // Assert
        ex.ShouldNotBeNull();
    }

    [Test]
    public void CreateUpdateEventsWith_Should_Have1Event_When_1PropertyWasSelected()
    {
        // Arrange
        var firstName = "Peter";
        var lastName = "Pan";
        var person = new Person(firstName, lastName) { DateOfBirth = new DateOnly(1961, 2, 3) };

        var newDateOfBirth = new DateOnly(1962, 3, 4);

        // Act
        var events = person.CreateUpdateEventsWith(x => x.DateOfBirth, newDateOfBirth).Build()
                           .ToDictionary(x => x.Key, x => x.Value);

        // Assert

        events.Count.ShouldBe(1);

        events.TryGetValue(nameof(Person.DateOfBirth), out var actionValue).ShouldBeTrue();
        actionValue.Action.ShouldBe(EventAction.Update);
        actionValue.Value.ShouldBe(newDateOfBirth);
    }

    [Test]
    public void CreateUpdateEventsWith_Should_ChangeProperty_When_RecordWithReadOnlyPropertyIsUsed()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var person = new Person(firstName, lastName) { DateOfBirth = new DateOnly(1961, 2, 3) };

        var newLastName = "Lennon";
        var newDateOfBirth = new DateOnly(1962, 3, 4);

        // Act
        var events = person.CreateUpdateEventsWith(x => x.LastName, newLastName)
                    .And(x => x.DateOfBirth, newDateOfBirth)
                    .Build()
                    .ToDictionary(x => x.Key, x => x.Value);

        // Assert
        events.Count.ShouldBe(2);
        {
            events.TryGetValue(nameof(Person.LastName), out var actionValue).ShouldBeTrue();
            actionValue.Action.ShouldBe(EventAction.Update);
            actionValue.Value.ShouldBe(newLastName);
        }
        {
            events.TryGetValue(nameof(Person.DateOfBirth), out var actionValue).ShouldBeTrue();
            actionValue.Action.ShouldBe(EventAction.Update);
            actionValue.Value.ShouldBe(newDateOfBirth);
        }
    }
}
