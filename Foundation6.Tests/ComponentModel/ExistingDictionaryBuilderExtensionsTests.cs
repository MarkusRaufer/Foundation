using Foundation.Collections.Generic;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.ComponentModel;

[TestFixture]
public class ExistingDictionaryBuilderExtensionsTests
{
    [Test]
    public void ChangeWith_Should_AddAKeyValue_When_KeyValueNotExists()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var birthdayProperty = "Birthday";

        var firstName = "Peter";
        var lastName = "Pan";
        
        var dictionary = new Dictionary<string, object>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
        };

        IDictionary<string, EventActionValue<object>>? changes = null;

        var birthday = new DateOnly(1962, 3, 4);

        // Act
        var sut = dictionary.ChangeWith(birthdayProperty, birthday).Build(x => changes = x);

        // Assert
        sut.ShouldNotBeNull();
        ReferenceEquals(sut, dictionary).ShouldBeTrue();
        sut.Count.ShouldBe(3);
        {
            var value = sut[firstNameProperty];
            value.ShouldBe(firstName);
        }
        {
            var value = sut[lastNameProperty];
            value.ShouldBe(lastName);
        }
        {
            var value = sut[birthdayProperty];
            value.ShouldBe(birthday);
        }

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(1);
        var change = changes.First();
        change.Key.ShouldBe(birthdayProperty);
        change.Value.Action.ShouldBe(EventAction.Add);
        change.Value.Value.ShouldBe(birthday);
    }

    [Test]
    public void ChangeWith_Should_UpdateKeyValues_When_KeyValuesExist()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var birthdayProperty = "Birthday";

        var firstName = "Peter";
        var lastName = "Pan";
        var newFirstName = "John";
        var newLastName = "Doe";
        var birthday = new DateOnly(1962, 3, 4);

        var dictionary = new Dictionary<string, object>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
            { birthdayProperty, birthday },
        };

        IDictionary<string, EventActionValue<object>>? changes = null;


        // Act
        var sut = dictionary.ChangeWith(firstNameProperty, newFirstName)
                            .And(lastNameProperty, newLastName)                
                            .Build(x => changes = x);

        // Assert
        sut.ShouldNotBeNull();
        ReferenceEquals(sut, dictionary).ShouldBeTrue();
        sut.Count.ShouldBe(3);
        {
            var value = sut[firstNameProperty];
            value.ShouldBe(newFirstName);
        }
        {
            var value = sut[lastNameProperty];
            value.ShouldBe(newLastName);
        }
        {
            var value = sut[birthdayProperty];
            value.ShouldBe(birthday);
        }

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(2);
        {
            var change = changes[firstNameProperty];
            change.Action.ShouldBe(EventAction.Update);
            change.Value.ShouldBe(newFirstName);
        }
        {
            var change = changes[lastNameProperty];
            change.Action.ShouldBe(EventAction.Update);
            change.Value.ShouldBe(newLastName);
        }
    }

    [Test]
    public void ChangeWith_Should_AddAKeyValueAndUpdate2KeyValue_When_1DoesNotExist2ExistButOnly1HasDifferentValue()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var birthdayProperty = "Birthday";

        var firstName = "Peter";
        var lastName = "Pan";
        var newFirstName = "John";
        var birthday = new DateOnly(1962, 3, 4);

        var dictionary = new Dictionary<string, object>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
        };

        IDictionary<string, EventActionValue<object>>? changes = null;


        // Act
        var sut = dictionary.ChangeWith(firstNameProperty, newFirstName)
                            .And(lastNameProperty, lastName)
                            .And(birthdayProperty, birthday)
                            .Build(x => changes = x);

        // Assert
        sut.ShouldNotBeNull();
        ReferenceEquals(sut, dictionary).ShouldBeTrue();
        sut.Count.ShouldBe(3);
        {
            var value = sut[firstNameProperty];
            value.ShouldBe(newFirstName);
        }
        {
            var value = sut[lastNameProperty];
            value.ShouldBe(lastName);
        }
        {
            var value = sut[birthdayProperty];
            value.ShouldBe(birthday);
        }

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(2);
        {
            var change = changes[firstNameProperty];
            change.Action.ShouldBe(EventAction.Update);
            change.Value.ShouldBe(newFirstName);
        }
        {
            var change = changes[birthdayProperty];
            change.Action.ShouldBe(EventAction.Add);
            change.Value.ShouldBe(birthday);
        }
    }

    [Test]
    public void ChangeWith_Should_ChangeKeyValueAndRemoveKey_When_1KeyIsNew1KeyWithNewValueExists()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var birthdayProperty = "Birthday";

        var firstName = "Peter";
        var lastName = "Pan";
        var newFirstName = "John";
        var birthday = new DateOnly(1962, 3, 4);

        var dictionary = new Dictionary<string, object>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
            { birthdayProperty, birthday },
        };

        IDictionary<string, EventActionValue<object>>? changes = null;


        // Act
        var sut = dictionary.ChangeWith(firstNameProperty, newFirstName)
                            .AndRemove(lastNameProperty)
                            .Build(x => changes = x);

        // Assert
        sut.ShouldNotBeNull();
        ReferenceEquals(sut, dictionary).ShouldBeTrue();
        sut.Count.ShouldBe(2);
        {
            var value = sut[firstNameProperty];
            value.ShouldBe(newFirstName);
        }
        {
            var value = sut[birthdayProperty];
            value.ShouldBe(birthday);
        }

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(2);
        {
            var change = changes[firstNameProperty];
            change.Action.ShouldBe(EventAction.Update);
            change.Value.ShouldBe(newFirstName);
        }
        {
            var change = changes[lastNameProperty];
            change.Action.ShouldBe(EventAction.Remove);
            change.Value.ShouldBe(lastName);
        }
    }

    [Test]
    public void NewWith_Should_AddAKeyValue_When_KeyValueNotExists()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var birthdayProperty = "Birthday";

        var firstName = "Peter";
        var lastName = "Pan";

        var dictionary = new Dictionary<string, object>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
        };

        IDictionary<string, EventActionValue<object>>? changes = null;

        var birthday = new DateOnly(1962, 3, 4);

        // Act
        var sut = dictionary.NewWith(birthdayProperty, birthday).Build(x => changes = x);

        // Assert
        sut.ShouldNotBeNull();
        ReferenceEquals(sut, dictionary).ShouldBeFalse();
        sut.Count.ShouldBe(3);
        {
            var value = sut[firstNameProperty];
            value.ShouldBe(firstName);
        }
        {
            var value = sut[lastNameProperty];
            value.ShouldBe(lastName);
        }
        {
            var value = sut[birthdayProperty];
            value.ShouldBe(birthday);
        }

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(1);
        var change = changes.First();
        change.Key.ShouldBe(birthdayProperty);
        change.Value.Action.ShouldBe(EventAction.Add);
        change.Value.Value.ShouldBe(birthday);
    }

    [Test]
    public void NewWith_Should_UpdateKeyValues_When_KeyValuesExist()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var birthdayProperty = "Birthday";

        var firstName = "Peter";
        var lastName = "Pan";
        var newFirstName = "John";
        var newLastName = "Doe";
        var birthday = new DateOnly(1962, 3, 4);

        var dictionary = new Dictionary<string, object>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
            { birthdayProperty, birthday },
        };

        IDictionary<string, EventActionValue<object>>? changes = null;


        // Act
        var sut = dictionary.NewWith(firstNameProperty, newFirstName)
                            .And(lastNameProperty, newLastName)
                            .Build(x => changes = x);

        // Assert
        sut.ShouldNotBeNull();
        ReferenceEquals(sut, dictionary).ShouldBeFalse();
        sut.Count.ShouldBe(3);
        {
            var value = sut[firstNameProperty];
            value.ShouldBe(newFirstName);
        }
        {
            var value = sut[lastNameProperty];
            value.ShouldBe(newLastName);
        }
        {
            var value = sut[birthdayProperty];
            value.ShouldBe(birthday);
        }

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(2);
        {
            var change = changes[firstNameProperty];
            change.Action.ShouldBe(EventAction.Update);
            change.Value.ShouldBe(newFirstName);
        }
        {
            var change = changes[lastNameProperty];
            change.Action.ShouldBe(EventAction.Update);
            change.Value.ShouldBe(newLastName);
        }
    }

    [Test]
    public void NewWith_Should_AddAKeyValueAndUpdate2KeyValue_When_1DoesNotExist2ExistButOnly1HasDifferentValue()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var birthdayProperty = "Birthday";

        var firstName = "Peter";
        var lastName = "Pan";
        var newFirstName = "John";
        var birthday = new DateOnly(1962, 3, 4);

        var dictionary = new Dictionary<string, object>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
        };

        IDictionary<string, EventActionValue<object>>? changes = null;


        // Act
        var sut = dictionary.NewWith(firstNameProperty, newFirstName)
                            .And(lastNameProperty, lastName)
                            .And(birthdayProperty, birthday)
                            .Build(x => changes = x);

        // Assert
        sut.ShouldNotBeNull();
        ReferenceEquals(sut, dictionary).ShouldBeFalse();
        sut.Count.ShouldBe(3);
        {
            var value = sut[firstNameProperty];
            value.ShouldBe(newFirstName);
        }
        {
            var value = sut[lastNameProperty];
            value.ShouldBe(lastName);
        }
        {
            var value = sut[birthdayProperty];
            value.ShouldBe(birthday);
        }

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(2);
        {
            var change = changes[firstNameProperty];
            change.Action.ShouldBe(EventAction.Update);
            change.Value.ShouldBe(newFirstName);
        }
        {
            var change = changes[birthdayProperty];
            change.Action.ShouldBe(EventAction.Add);
            change.Value.ShouldBe(birthday);
        }
    }

    [Test]
    public void NewWith_Should_ChangeKeyValueAndRemoveKey_When_1KeyIsNew1KeyExistsWithNewValue()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var birthdayProperty = "Birthday";

        var firstName = "Peter";
        var lastName = "Pan";
        var newFirstName = "John";
        var birthday = new DateOnly(1962, 3, 4);

        var dictionary = new Dictionary<string, object>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
            { birthdayProperty, birthday },
        };

        IDictionary<string, EventActionValue<object>>? changes = null;


        // Act
        var sut = dictionary.NewWith(firstNameProperty, newFirstName)
                            .AndRemove(lastNameProperty)
                            .Build(x => changes = x);

        // Assert
        sut.ShouldNotBeNull();
        ReferenceEquals(sut, dictionary).ShouldBeFalse();
        sut.Count.ShouldBe(2);
        {
            var value = sut[firstNameProperty];
            value.ShouldBe(newFirstName);
        }
        {
            var value = sut[birthdayProperty];
            value.ShouldBe(birthday);
        }

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(2);
        {
            var change = changes[firstNameProperty];
            change.Action.ShouldBe(EventAction.Update);
            change.Value.ShouldBe(newFirstName);
        }
        {
            var change = changes[lastNameProperty];
            change.Action.ShouldBe(EventAction.Remove);
            change.Value.ShouldBe(lastName);
        }
    }
    [Test]
    public void RemoveNewWith_Should_RemoveAKey_When_KeyExists()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var birthdayProperty = "Birthday";

        var firstName = "Peter";
        var lastName = "Pan";
        var birthday = new DateOnly(1962, 3, 4);

        var dictionary = new Dictionary<string, object>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
            { birthdayProperty, birthday },
        };

        IDictionary<string, EventActionValue<object>>? changes = null;


        // Act
        var sut = dictionary.RemoveNewWith(birthdayProperty).Build(x => changes = x);

        // Assert
        sut.ShouldNotBeNull();
        ReferenceEquals(sut, dictionary).ShouldBeFalse();
        sut.Count.ShouldBe(2);
        {
            var value = sut[firstNameProperty];
            value.ShouldBe(firstName);
        }
        {
            var value = sut[lastNameProperty];
            value.ShouldBe(lastName);
        }

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(1);
        var change = changes.First();
        change.Key.ShouldBe(birthdayProperty);
        change.Value.Action.ShouldBe(EventAction.Remove);
        change.Value.Value.ShouldBe(birthday);
    }

    [Test]
    public void RemoveNewWith_Should_UpdateKeyValues_When_KeyValuesExist()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var birthdayProperty = "Birthday";

        var firstName = "Peter";
        var lastName = "Pan";
        var newLastName = "Doe";
        var birthday = new DateOnly(1962, 3, 4);

        var dictionary = new Dictionary<string, object>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
            { birthdayProperty, birthday },
        };

        IDictionary<string, EventActionValue<object>>? changes = null;


        // Act
        var sut = dictionary.RemoveNewWith(firstNameProperty)
                            .And(lastNameProperty, newLastName)
                            .Build(x => changes = x);

        // Assert
        sut.ShouldNotBeNull();
        ReferenceEquals(sut, dictionary).ShouldBeFalse();
        sut.Count.ShouldBe(2);
        {
            var value = sut[lastNameProperty];
            value.ShouldBe(newLastName);
        }
        {
            var value = sut[birthdayProperty];
            value.ShouldBe(birthday);
        }

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(2);

        {
            var change = changes[firstNameProperty];
            change.Action.ShouldBe(EventAction.Remove);
            change.Value.ShouldBe(firstName);
        }
        {
            var change = changes[lastNameProperty];
            change.Action.ShouldBe(EventAction.Update);
            change.Value.ShouldBe(newLastName);
        }
    }

    [Test]
    public void RemoveWith_Should_RemoveAKey_When_KeyExists()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var birthdayProperty = "Birthday";

        var firstName = "Peter";
        var lastName = "Pan";
        var birthday = new DateOnly(1962, 3, 4);

        var dictionary = new Dictionary<string, object>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
            { birthdayProperty, birthday },
        };

        IDictionary<string, EventActionValue<object>>? changes = null;


        // Act
        var sut = dictionary.RemoveWith(birthdayProperty).Build(x => changes = x);

        // Assert
        sut.ShouldNotBeNull();
        ReferenceEquals(sut, dictionary).ShouldBeTrue();
        sut.Count.ShouldBe(2);
        {
            var value = sut[firstNameProperty];
            value.ShouldBe(firstName);
        }
        {
            var value = sut[lastNameProperty];
            value.ShouldBe(lastName);
        }

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(1);
        var change = changes.First();
        change.Key.ShouldBe(birthdayProperty);
        change.Value.Action.ShouldBe(EventAction.Remove);
        change.Value.Value.ShouldBe(birthday);
    }

    [Test]
    public void RemoveWith_Should_UpdateKeyValues_When_KeyValuesExist()
    {
        // Arrange
        var firstNameProperty = "FirstName";
        var lastNameProperty = "LastName";
        var birthdayProperty = "Birthday";

        var firstName = "Peter";
        var lastName = "Pan";
        var newLastName = "Doe";
        var birthday = new DateOnly(1962, 3, 4);

        var dictionary = new Dictionary<string, object>
        {
            { firstNameProperty, firstName },
            { lastNameProperty, lastName },
            { birthdayProperty, birthday },
        };

        IDictionary<string, EventActionValue<object>>? changes = null;


        // Act
        var sut = dictionary.RemoveWith(firstNameProperty)
                            .And(lastNameProperty, newLastName)
                            .Build(x => changes = x);

        // Assert
        sut.ShouldNotBeNull();
        ReferenceEquals(sut, dictionary).ShouldBeTrue();
        sut.Count.ShouldBe(2);
        {
            var value = sut[lastNameProperty];
            value.ShouldBe(newLastName);
        }
        {
            var value = sut[birthdayProperty];
            value.ShouldBe(birthday);
        }

        changes.ShouldNotBeNull();
        changes.Count.ShouldBe(2);

        {
            var change = changes[firstNameProperty];
            change.Action.ShouldBe(EventAction.Remove);
            change.Value.ShouldBe(firstName);
        }
        {
            var change = changes[lastNameProperty];
            change.Action.ShouldBe(EventAction.Update);
            change.Value.ShouldBe(newLastName);
        }
    }
}
