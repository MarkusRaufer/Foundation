using Foundation.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.ComponentModel;

[TestFixture]
public class EnumerableExtensionsTests
{
    [Test]
    public void ToUpdateEvents_Should_Have1AddEvent_When_KeyDoesNotExist()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var dateOfBirthProperty = "DateOfBirth";

        var firstName = "Peter";
        var lastName = "Pan";
        
        var source = new Dictionary<string, object?>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
        };

        var dateOfBirth = new DateOnly(1962, 3, 4);
        var newValues = new Dictionary<string, object?>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
            { dateOfBirthProperty, dateOfBirth },
        };

        // Act
        var events = source.ToUpdateEvents(newValues).ToDictionary(x => x.Key, x => x.Value);

        // Assert
        events.Count.ShouldBe(1);
        events.TryGetValue(dateOfBirthProperty, out var actionValue).ShouldBeTrue();
        actionValue.Action.ShouldBe(EventAction.Add);
        actionValue.Value.ShouldBe(dateOfBirth);
    }

    [Test]
    public void ToUpdateEvents_Should_Have2UpdateEvents_When_2ExistingKeysHaveNewValuesExist()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var dateOfBirthProperty = "DateOfBirth";

        var firstName = "Peter";
        var lastName = "Pan";
        var newFirstName = "John";
        var newLastName = "Doe";
        var dateOfBirth = new DateOnly(1962, 3, 4);

        var dictionary = new Dictionary<string, object?>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
            { dateOfBirthProperty, dateOfBirth },
        };

        var newValues = new Dictionary<string, object?>
        {
            { firstNameProperty, newFirstName },
            { lastNameProperty, newLastName },
            { dateOfBirthProperty, dateOfBirth },
        };


        // Act
        var events = dictionary.ToUpdateEvents(newValues).ToDictionary(x => x.Key, x => x.Value);

        // Assert
        events.Count.ShouldBe(2);
        {
            events.TryGetValue(firstNameProperty, out var actionValue).ShouldBeTrue();
            actionValue.Action.ShouldBe(EventAction.Update);
            actionValue.Value.ShouldBe(newFirstName);
        }
        {
            events.TryGetValue(lastNameProperty, out var actionValue).ShouldBeTrue();
            actionValue.Action.ShouldBe(EventAction.Update);
            actionValue.Value.ShouldBe(newLastName);
        }
    }

    [Test]
    public void ToUpdateEvents_Should_1AddAnd1UpdateEvent_When_1KeyDoesNotExistAnd1KeyHasNewValue()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var dateOfBirthProperty = "DateOfBirth";

        var firstName = "Peter";
        var lastName = "Pan";
        var newFirstName = "John";
        var dateOfBirth = new DateOnly(1962, 3, 4);

        var dictionary = new Dictionary<string, object?>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
        };

        var newValues = new Dictionary<string, object?>
        {
            { firstNameProperty, newFirstName },
            { lastNameProperty, lastName },
            { dateOfBirthProperty, dateOfBirth },
        };

        // Act
        var events = dictionary.ToUpdateEvents(newValues).ToDictionary(x => x.Key, x => x.Value);

        // Assert
        // Assert
        events.Count.ShouldBe(2);
        {
            events.TryGetValue(firstNameProperty, out var actionValue).ShouldBeTrue();
            actionValue.Action.ShouldBe(EventAction.Update);
            actionValue.Value.ShouldBe(newFirstName);
        }
        {
            events.TryGetValue(dateOfBirthProperty, out var actionValue).ShouldBeTrue();
            actionValue.Action.ShouldBe(EventAction.Add);
            actionValue.Value.ShouldBe(dateOfBirth);
        }
    }

    [Test]
    public void ToUpdateEvents_Should_Have1UpdateAnd1RemoveEvent_When_1KeyHasNewValueAnd1KeyDoesNotExistInNewValues()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var dateOfBirthProperty = "DateOfBirth";

        var firstName = "Peter";
        var lastName = "Pan";
        var newFirstName = "John";
        var dateOfBirth = new DateOnly(1962, 3, 4);

        var dictionary = new Dictionary<string, object?>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
            { dateOfBirthProperty, dateOfBirth },
        };

        var newValues = new Dictionary<string, object?>
        {
            { firstNameProperty, newFirstName },
            { dateOfBirthProperty, dateOfBirth },
        };

        // Act
        var events = dictionary.ToUpdateEvents(newValues).ToDictionary(x => x.Key, x => x.Value);

        // Assert
        // Assert
        events.Count.ShouldBe(2);
        {
            events.TryGetValue(firstNameProperty, out var actionValue).ShouldBeTrue();
            actionValue.Action.ShouldBe(EventAction.Update);
            actionValue.Value.ShouldBe(newFirstName);
        }
        {
            events.TryGetValue(lastNameProperty, out var actionValue).ShouldBeTrue();
            actionValue.Action.ShouldBe(EventAction.Remove);
            actionValue.Value.ShouldBe(lastName);
        }
    }

    [Test]
    public void ToUpdateEvents_Should_Have1AddAnd1RemoveEvent_When_1KeyHasNewValueAnd1KeyDoesNotExist()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var dateOfBirthProperty = "DateOfBirth";

        var firstName = "Peter";
        var lastName = "Pan";
        var newFirstName = "John";
        var dateOfBirth = new DateOnly(1962, 3, 4);

        var dictionary = new Dictionary<string, object?>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
            { dateOfBirthProperty, dateOfBirth },
        };

        var newValues = new Dictionary<string, object?>
        {
            { firstNameProperty, newFirstName },
            { dateOfBirthProperty, dateOfBirth },
        };

        // Act
        var events = dictionary.ToUpdateEvents(newValues).ToDictionary(x => x.Key, x => x.Value);

        // Assert
        // Assert
        events.Count.ShouldBe(2);
        {
            events.TryGetValue(firstNameProperty, out var actionValue).ShouldBeTrue();
            actionValue.Action.ShouldBe(EventAction.Update);
            actionValue.Value.ShouldBe(newFirstName);
        }
        {
            events.TryGetValue(lastNameProperty, out var actionValue).ShouldBeTrue();
            actionValue.Action.ShouldBe(EventAction.Remove);
            actionValue.Value.ShouldBe(lastName);
        }
    }

    [Test]
    public void RemoveNewWith_Should_UpdateKeyValues_When_KeyValuesExist()
    {
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var dateOfBirthProperty = "DateOfBirth";

        var firstName = "Peter";
        var lastName = "Pan";
        var dateOfBirth = new DateOnly(1962, 3, 4);

        var dictionary = new Dictionary<string, object?>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
            { dateOfBirthProperty, dateOfBirth },
        };

        var newValues = new Dictionary<string, object?>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
            { dateOfBirthProperty, dateOfBirth },
        };

        // Act
        var events = dictionary.ToUpdateEvents(newValues).ToDictionary(x => x.Key, x => x.Value);

        // Assert
        // Assert
        events.Count.ShouldBe(0);
    }
}
